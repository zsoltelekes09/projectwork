// <copyright file="GameLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

[assembly: System.CLSCompliant(false)]

namespace TankPocalypse.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using GalaSoft.MvvmLight.Ioc;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.Logic.Networking;
    using TankPocalypse.Logic.UIClasses;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Model.World;
    using TankPocalypse.Repository;
    using TankPocalypse.Repository.Classes;
    using TankPocalypse.Repository.Interfaces;
    using Timer = System.Timers.Timer;

    /// <summary>
    /// GameLogic class. Keeps up the life cycle of the game.
    /// </summary>
    public class GameLogic : IGameLogic
    {
        // ******** S E R V E R - C L I E N T - P R O P S ******** //
        private bool appIsServer;
        private ClientGamePacket clientbackupPackage;

        private UdpClient udpClient;
        private IUIProfile globalProfile;
        private UdpClient udpServer;
        private byte[] receivedUdpByte;
        private IPEndPoint ep;
        private IPEndPoint remoteEp;
        private ServerGamePacket serverbackupPackage;
        private ClientGamePacket cp = new ClientGamePacket();
        private ServerGamePacket sp = new ServerGamePacket();
        private readonly object tickLockerObject = new object();
        private static object networkLock = new object();
        private IGameModel gameModel;
        private IGameRepository gameRepository;
        private IGameWorld gameWorld;
        private IGameInputController gameInput;
        private IGamePhysics gamePhysics;
        private IFlowField flowField;
        private Timer sessionTimer;
        private Timer gameTimer;
        private bool gameOver;
        private List<Tuple<int, int>> revealRadius;
        private int teamIdxStart;
        private int teamIdxEnd;
        private int netIdxStart;
        private int netIdxEnd;
        private int team0BaseCounter;
        private int team1BaseCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameLogic"/> class.
        /// </summary>
        public GameLogic()
        {
            this.gameRepository = new GameRepository();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameLogic"/> class.
        /// </summary>
        /// <param name="gameWorld">GameWorld entiry.</param>
        /// <param name="gameRepository">GameRepository entity.</param>
        /// <param name="gameModel">GameModel entity.</param>
        /// <param name="gameInputController">GameInputController entity.</param>
        /// <param name="gamePhysics">GamePhysics entity.</param>
        /// <param name="flowField">FlowField entity.</param>
        [PreferredConstructor]
        public GameLogic(IGameWorld gameWorld, IGameRepository gameRepository, IGameModel gameModel, IGameInputController gameInputController, IGamePhysics gamePhysics, IFlowField flowField)
        {
            this.gameModel = gameModel;
            this.gameRepository = gameRepository;
            this.gameWorld = gameWorld;
            this.gameInput = gameInputController;
            this.gamePhysics = gamePhysics;
            this.flowField = flowField;

            this.revealRadius = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(0, 0),
                new Tuple<int, int>(0, 1),
                new Tuple<int, int>(0, 2),
                new Tuple<int, int>(0, 3),
                new Tuple<int, int>(0, -1),
                new Tuple<int, int>(0, -2),
                new Tuple<int, int>(0, -3),

                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(1, 2),
                new Tuple<int, int>(1, 3),
                new Tuple<int, int>(1, 0),
                new Tuple<int, int>(1, -1),
                new Tuple<int, int>(1, -2),
                new Tuple<int, int>(1, -3),
                new Tuple<int, int>(2, 2),
                new Tuple<int, int>(2, 1),
                new Tuple<int, int>(2, 0),
                new Tuple<int, int>(2, -1),
                new Tuple<int, int>(2, -2),
                new Tuple<int, int>(3, 1),
                new Tuple<int, int>(3, 0),
                new Tuple<int, int>(3, -1),

                new Tuple<int, int>(-1, 1),
                new Tuple<int, int>(-1, 2),
                new Tuple<int, int>(-1, 3),
                new Tuple<int, int>(-1, 0),
                new Tuple<int, int>(-1, -1),
                new Tuple<int, int>(-1, -2),
                new Tuple<int, int>(-1, -3),
                new Tuple<int, int>(-2, 2),
                new Tuple<int, int>(-2, 1),
                new Tuple<int, int>(-2, 0),
                new Tuple<int, int>(-2, -1),
                new Tuple<int, int>(-2, -2),
                new Tuple<int, int>(-3, 1),
                new Tuple<int, int>(-3, 0),
                new Tuple<int, int>(-3, -1),
            };
        }

        private event Action UpdateUIEvent;

        private event Action GameEnded;

        /// <summary>
        /// Gets or sets a value indicating whether UdpRunning.
        /// </summary>
        public bool UdpRunning { get; set; }

        /// <inheritdoc/>
        public bool ConnectionLost { get; set; }

        /// <inheritdoc/>
        public bool GameOver
        {
            get { return this.gameOver; }
            set { this.gameOver = value; }
        }

        /// <inheritdoc/>
        public bool IsGamePaused
        {
            get { return this.gameModel.Paused; }
        }

        /// <summary>
        /// Starts udp client.
        /// </summary>
        /// <param name="ip">ip.</param>
        public void StartUdpClient(string ip)
        {
            this.udpClient = new UdpClient();
            this.ep = new IPEndPoint(IPAddress.Parse(ip), 11000);
            this.udpClient.Connect(this.ep);
            this.UdpRunning = true;
            var t = new Task(
                () =>
            {
                while (this.UdpRunning)
                {
                    this.receivedUdpByte = this.udpClient.Receive(ref this.ep);
                    byte[] type = new byte[4];
                    type = Extension.SubArray(this.receivedUdpByte, 0, 4);
                    PacketType p = (PacketType)(PacketType)BitConverter.ToUInt32(type, 0);
                    byte[] length = Extension.SubArray(this.receivedUdpByte, 4, 8);
                    int packetsize = BitConverter.ToInt32(length, 0);
                    byte[] packet = Extension.SubArray(this.receivedUdpByte, 8, packetsize);
                    if (p == PacketType.ServerPackage)
                    {
                        lock (networkLock)
                        {
                            if (this.gameModel != null)
                            {
                                this.sp = new ServerGamePacket(packet);
                                this.serverbackupPackage = this.sp;
                            }
                        }
                    }
                }

                this.udpClient.Close();
                this.udpClient.Dispose();
                this.udpClient = null;
            },
                TaskCreationOptions.LongRunning);
            t.Start();
        }

        /// <summary>
        /// Stops udp client.
        /// </summary>
        public void StopUdpClient()
        {
            lock (networkLock)
            {
                this.UdpRunning = false;
            }
        }

        /// <summary>
        /// stops udp server.
        /// </summary>
        public void StopUdpServer()
        {
            lock (networkLock)
            {
                this.UdpRunning = false;
            }
        }

        /// <summary>
        /// Starts upd server.
        /// </summary>
        public void StartUdpServer()
        {
            if (this.udpServer == null)
            {
                this.udpServer = new UdpClient(11000);
                this.remoteEp = new IPEndPoint(IPAddress.Any, 11000);
                this.UdpRunning = true;
            }

            var t = new Task(
                () =>
            {
                while (this.UdpRunning)
                {
                    try
                    {
                        this.receivedUdpByte = this.udpServer.Receive(ref this.remoteEp);
                    }
                    catch (Exception)
                    {
                        this.UdpRunning = false;
                        this.ConnectionLost = true;
                        this.StopGameTimer();
                        this.sessionTimer.Stop();
                        this.udpServer.Close();
                        this.udpServer.Dispose();
                        this.udpServer = null;
                        this.GameEnded?.Invoke();
                    }

                    byte[] type = new byte[4];
                    type = Extension.SubArray(this.receivedUdpByte, 0, 4);
                    PacketType p = (PacketType)(PacketType)BitConverter.ToUInt32(type, 0);
                    byte[] length = Extension.SubArray(this.receivedUdpByte, 4, 8);
                    int packetsize = BitConverter.ToInt32(length, 0);
                    byte[] packet = Extension.SubArray(this.receivedUdpByte, 8, packetsize);

                    if (p == PacketType.ClientPackage)
                    {
                        lock (networkLock)
                        {
                            if (this.gameModel != null)
                            {
                                // EXC.
                                this.cp = new ClientGamePacket(packet, this.gameModel.Vehicles.Count / 2);
                                this.clientbackupPackage = this.cp;
                            }
                        }
                    }
                }

                Debug.WriteLine(" UDP SERVER EXITED SUCCESFULLY ");
                this.udpServer.Close();
                this.udpServer.Dispose();
                this.udpServer = null;
            },
                TaskCreationOptions.LongRunning);
            t.Start();
        }

        // **************** START NEW GAME WITH VM OPTIONS ***************** \\

        /// <inheritdoc/>
        public void StartNewGame(IUIMap mapToLoad, byte unitCount)
        {
            this.gameWorld.SetupGrid(mapToLoad?.MapData);
            this.gamePhysics.SetupWorldBounds();
            this.flowField.SetupFlowField();
            this.gameModel.CreateVehicles(unitCount);
            this.SetupIndexesForNetwork();
            this.CalculateRevealedNodes();

            this.gameTimer = new Timer
            {
                Interval = 15,  // 15 default
                AutoReset = true,
            };
            this.gameTimer.Elapsed += this.UpdateEveryTick;

            if (this.appIsServer)
            {
                List<IVehicle> owntank = this.gameModel.Vehicles.GetRange(this.gameModel.Vehicles.Count / 2, this.gameModel.Vehicles.Count / 2);
                List<Tank> tankTypeOwnTank = new List<Tank>();
                foreach (var item in owntank)
                {
                    tankTypeOwnTank.Add(new Tank(item.UniqueId, new VectorExtend(item.Px, item.Py), item.TurretIdx, item.BodyIdx, item.ShootIdx));
                }

                List<IVehicle> networkVehicle = this.gameModel.Vehicles.GetRange(0, this.gameModel.Vehicles.Count / 2);
                List<EnemyNetworkPackage> enemyNetworkData = new List<EnemyNetworkPackage>();

                foreach (var item in networkVehicle)
                {
                    enemyNetworkData.Add(new EnemyNetworkPackage(item.UniqueId, item.Health));
                }

                this.clientbackupPackage = new ClientGamePacket(tankTypeOwnTank, enemyNetworkData);
                this.cp = this.clientbackupPackage;
            }
            else
            {
                List<IVehicle> owntank = this.gameModel.Vehicles.GetRange(0, this.gameModel.Vehicles.Count / 2);
                List<Tank> tankTypeOwnTank1 = new List<Tank>();
                foreach (var item in owntank)
                {
                    tankTypeOwnTank1.Add(new Tank(item.UniqueId, new VectorExtend(item.Px, item.Py), item.TurretIdx, item.BodyIdx, item.ShootIdx));
                }

                List<IVehicle> networkVehicle1 = this.gameModel.Vehicles.GetRange(this.gameModel.Vehicles.Count / 2, this.gameModel.Vehicles.Count / 2);
                List<EnemyNetworkPackage> enemyNetworkData1 = new List<EnemyNetworkPackage>();

                foreach (var item in networkVehicle1)
                {
                    enemyNetworkData1.Add(new EnemyNetworkPackage(item.UniqueId, item.Health));
                }

                this.serverbackupPackage = new ServerGamePacket(tankTypeOwnTank1, enemyNetworkData1, this.gameModel.Team0BasePercent, this.gameModel.Team1BasePercent, (short)this.gameModel.SessionTime);
                this.sp = this.serverbackupPackage;
            }
        }

        // **************** LOAD SAVED GAME WITH VM OPTIONS ***************** \\

        /// <inheritdoc/>
        public void LoadGame(IUISavedGame uiSavedGame)
        {
            ISavedGame savedGame = ConvertUISavedGameToSavedGame(uiSavedGame);
            this.gameRepository.LoadSaved(savedGame);
            this.gamePhysics.SetupWorldBounds();
            this.flowField.SetupFlowField();
            this.SetupIndexesForNetwork();
            this.CalculateRevealedNodes();
            this.gameTimer = new Timer
            {
                Interval = 15,  // 15 default
                AutoReset = true,
            };
            this.gameTimer.Elapsed += this.UpdateEveryTick;

            if (this.appIsServer)
            {
                List<IVehicle> owntank = this.gameModel.Vehicles.GetRange(this.gameModel.Vehicles.Count / 2, this.gameModel.Vehicles.Count / 2);
                List<Tank> tankTypeOwnTank = new List<Tank>();
                foreach (var item in owntank)
                {
                    tankTypeOwnTank.Add(new Tank(item.UniqueId, new VectorExtend(item.Px, item.Py), item.TurretIdx, item.BodyIdx, item.ShootIdx));
                }

                List<IVehicle> networkVehicle = this.gameModel.Vehicles.GetRange(0, this.gameModel.Vehicles.Count / 2);
                List<EnemyNetworkPackage> enemyNetworkData = new List<EnemyNetworkPackage>();

                foreach (var item in networkVehicle)
                {
                    enemyNetworkData.Add(new EnemyNetworkPackage(item.UniqueId, item.Health));
                }

                this.clientbackupPackage = new ClientGamePacket(tankTypeOwnTank, enemyNetworkData);
                this.cp = this.clientbackupPackage;
            }
            else
            {
                List<IVehicle> owntank = this.gameModel.Vehicles.GetRange(0, this.gameModel.Vehicles.Count / 2);
                List<Tank> tankTypeOwnTank1 = new List<Tank>();
                foreach (var item in owntank)
                {
                    tankTypeOwnTank1.Add(new Tank(item.UniqueId, new VectorExtend(item.Px, item.Py), item.TurretIdx, item.BodyIdx, item.ShootIdx));
                }

                List<IVehicle> networkVehicle1 = this.gameModel.Vehicles.GetRange(this.gameModel.Vehicles.Count / 2, this.gameModel.Vehicles.Count / 2);
                List<EnemyNetworkPackage> enemyNetworkData1 = new List<EnemyNetworkPackage>();

                foreach (var item in networkVehicle1)
                {
                    enemyNetworkData1.Add(new EnemyNetworkPackage(item.UniqueId, item.Health));
                }

                this.serverbackupPackage = new ServerGamePacket(tankTypeOwnTank1, enemyNetworkData1, this.gameModel.Team0BasePercent, this.gameModel.Team1BasePercent, (short)this.gameModel.SessionTime);
                this.sp = this.serverbackupPackage;
            }
        }

        // *************** L O A D - S A V E - M E T H O D S ***************** //

        /// <inheritdoc/>
        public List<IUISavedGame> GetAllSavedGames()
        {
            List<IUISavedGame> uiSavedGames = new List<IUISavedGame>();
            var repoSaves = this.gameRepository.GetSavedGames();
            foreach (ISavedGame save in repoSaves)
            {
                uiSavedGames.Add(ConvertSavedGameToUISavedGame(save));
            }

            return uiSavedGames;
        }

        /// <inheritdoc/>
        public void SetGlobalProfile(IUIProfile globProfile)
        {
            this.globalProfile = globProfile;
        }

        /// <inheritdoc/>
        public ObservableCollection<IUIProfile> GetAllProfiles()
        {
            ObservableCollection<IUIProfile> uiProfileList = new ObservableCollection<IUIProfile>();
            foreach (IProfile profile in this.gameRepository.GetAllProfiles())
            {
                uiProfileList.Add(UIProfile.ConvertProfileToUIProfile(profile));
            }

            return uiProfileList;
        }

        /// <inheritdoc/>
        public void DeleteProfile(int id)
        {
            this.gameRepository.DeleteProfile(id);
        }

        /// <inheritdoc/>
        public void AddNewProfile(IUIProfile newProfile)
        {
            this.gameRepository.AddProfile(UIProfile.ConvertUIProfileToProfile(newProfile));
        }

        // *************** M A P - R E P O - M E T H O D S ***************** //

        /// <inheritdoc/>
        public List<IUIMap> GetAllMaps()
        {
            List<IUIMap> uiMapList = new List<IUIMap>();
            foreach (IMap map in this.gameRepository.GetAllMaps())
            {
                uiMapList.Add(new UIMap(map.MapName, map.MapData));
            }

            return uiMapList;
        }

        // *************** G A M E - M E T H O D S ****************** //

        /// <inheritdoc/>
        public void StartGame()
        {
            this.gameTimer.Start();
        }

        /// <inheritdoc/>
        public void SubscribeInvalidateMethod(Action method)
        {
            this.UpdateUIEvent += method;
        }

        /// <inheritdoc/>
        public void SubscribeToGameEndedEvent(Action method)
        {
            this.GameEnded += method;
        }

        /// <summary>
        /// SaveGame.
        /// </summary>
        public void SaveGame()
        {
            this.gameRepository.SaveGame();
        }

        /// <inheritdoc/>
        public void StartSessionTimer()
        {
            this.sessionTimer = new Timer
            {
                Interval = 1000,  // 15 default
                AutoReset = true,
            };

            this.sessionTimer.Elapsed += this.DecreaseTime;

            // this.gameModel.SessionTime = 180;
            this.sessionTimer.Start();
        }

        /// <inheritdoc/>
        public void PauseSessionTimer()
        {
            if (this.gameModel.SessionTime > 0)
            {
                if (!this.gameModel.Paused)
                {
                    this.sessionTimer.Start();
                }
                else
                {
                    this.sessionTimer.Stop();
                }
            }
        }

        /// <summary>
        /// SetTeamid.
        /// </summary>
        /// <param name="id">id.</param>
        public void SetTeamID(byte id)
        {
            this.gameModel.SetTeamID(id);
        }

        /// <inheritdoc/>
        public void SetAppAsServer()
        {
            this.appIsServer = true;
            this.gameModel.IsServer = true;
        }

        /// <inheritdoc/>
        public void StopGameTimer()
        {
            if (this.gameTimer != null)
            {
                this.gameTimer.Stop();
                if (this.appIsServer)
                {
                    this.ServerSendMethod();
                }
                else
                {
                    this.ClientSendMethod();
                }
            }
        }

        /// <inheritdoc/>
        public void DeleteEverything()
        {
            this.gameModel = null;
            this.gameRepository = null;
            this.gameWorld = null;
            this.gameInput = null;
            this.gamePhysics = null;
            this.flowField = null;
            this.gameTimer = null;
        }

        /// <inheritdoc/>
        public void PauseGame()
        {
            if (this.IsGamePaused)
            {
                this.gameModel.Paused = false;
                this.gameTimer.Start();
                this.UpdateUIEvent?.Invoke();
            }
            else
            {
                this.gameModel.Paused = true;
                this.UpdateUIEvent?.Invoke();
                this.gameTimer.Stop();
            }
        }

        private static ISavedGame ConvertUISavedGameToSavedGame(IUISavedGame uiSavedGame)
        {
            return new SavedGame(uiSavedGame.SaveId, uiSavedGame.SaveName, uiSavedGame.SaveFileXDoc);
        }

        private static IUISavedGame ConvertSavedGameToUISavedGame(ISavedGame savedGame)
        {
            return new UISavedGame(savedGame.SaveId, savedGame.SaveDescription, savedGame.SaveXDoc);
        }

        private void UpdateProfile()
        {
            byte oCase = this.gameModel.OutcomeCase;
            bool didWon = false;

            if (oCase != 2)
            {
                didWon = oCase == this.gameModel.TeamID;
            }

            this.gameRepository.UpdateProfile(this.globalProfile.UserId, this.globalProfile.UserName, didWon, this.gameModel.EnemyVehicleInitCount - this.gameModel.EnemyVehicles.Count, this.gameModel.SelfVehicleInitCount - this.gameModel.TeamVehicles.Count);
        }

        private void SetupIndexesForNetwork()
        {
            var vCount = this.gameModel.Vehicles.Count;

            if (this.appIsServer)
            {
                this.teamIdxStart = 0;
                this.teamIdxEnd = vCount / 2;

                this.netIdxStart = vCount / 2;
                this.netIdxEnd = vCount;
            }
            else
            {
                this.netIdxStart = 0;
                this.netIdxEnd = vCount / 2;

                this.teamIdxStart = vCount / 2;
                this.teamIdxEnd = vCount;
            }
        }

        private void UpdateEveryTick(object source, System.Timers.ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(this.tickLockerObject, 2))
            {
                this.gamePhysics.UpdatePhysics();
                this.gameInput.RefreshKeys();

                if (this.appIsServer)
                {
                    int counter = 0;
                    for (int i = this.netIdxStart; i < this.netIdxEnd; i++)
                    {
                        this.gameModel.Vehicles[i].UpdateFromNetwork(new Vector2(this.cp.Tanks[counter].Position.X, this.cp.Tanks[counter].Position.Y), Convert.ToInt16(this.cp.Tanks[counter].BodyIdx), Convert.ToInt16(this.cp.Tanks[counter].TurretIdx), this.cp.Tanks[counter].ShootIdx);
                        counter++;
                    }

                    int counterHP = 0;
                    for (int i = this.teamIdxStart; i < this.teamIdxEnd; i++)
                    {
                        this.gameModel.Vehicles[i].SetHpFromNetwork(Convert.ToByte(this.cp.NetworkTanks[counterHP].HP));
                        this.gameModel.Vehicles[i].ApplyBehaviors();
                        counterHP++;
                    }
                }
                else
                {
                    this.gameModel.SessionTime = this.sp.SessionTime;
                    this.gameModel.Team0BasePercent = (byte)this.sp.Team0Base;
                    this.gameModel.Team1BasePercent = (byte)this.sp.Team1Base;
                    int counter = 0;
                    for (int i = this.netIdxStart; i < this.netIdxEnd; i++)
                    {
                        this.gameModel.Vehicles[i].UpdateFromNetwork(new Vector2(this.sp.Tanks[counter].Position.X, this.sp.Tanks[counter].Position.Y), Convert.ToInt16(this.sp.Tanks[counter].BodyIdx), Convert.ToInt16(this.sp.Tanks[counter].TurretIdx), this.sp.Tanks[counter].ShootIdx);
                        counter++;
                    }

                    int counterHP = 0;
                    for (int i = this.teamIdxStart; i < this.teamIdxEnd; i++)
                    {
                        this.gameModel.Vehicles[i].SetHpFromNetwork(Convert.ToByte(this.sp.NetworkTanks[counterHP].HP));
                        this.gameModel.Vehicles[i].ApplyBehaviors();
                        counterHP++;
                    }
                }

                if (this.appIsServer)
                {
                    this.ServerSendMethod();
                    this.CalculateBaseCapture();
                }
                else
                {
                    this.ClientSendMethod();
                }

                this.CalculateRevealedNodes();
                this.UpdateUIEvent?.Invoke();

                if (this.appIsServer)
                {
                    this.gameOver = this.CheckIfGameEnded();
                    if (this.gameOver)
                    {
                        this.StopGameTimer();
                        this.sessionTimer.Stop();
                        this.gameModel.GameOver = true;
                        this.gameModel.OutcomeCase = this.CheckOutcome();
                        this.UpdateProfile();

                        this.GameEnded?.Invoke();
                        this.UpdateUIEvent?.Invoke();
                    }
                }
                else
                {
                    this.gameOver = this.CheckIfGameEnded();
                    if (this.gameOver)
                    {
                        this.StopGameTimer();
                        this.gameModel.GameOver = true;
                        this.gameModel.OutcomeCase = this.CheckOutcome();
                        this.UpdateProfile();

                        this.GameEnded?.Invoke();
                        this.UpdateUIEvent?.Invoke();
                    }
                }

                Monitor.Exit(this.tickLockerObject);
            }
        }

        private void ServerSendMethod()
        {
            List<IVehicle> owntank = this.gameModel.Vehicles.GetRange(0, this.gameModel.Vehicles.Count / 2);
            List<Tank> tankTypeOwnTank = new List<Tank>();
            foreach (var item in owntank)
            {
                tankTypeOwnTank.Add(new Tank(item.UniqueId, new VectorExtend(item.Px, item.Py), item.TurretIdx, item.BodyIdx, item.ShootIdx));
            }

            List<IVehicle> networkVehicle = this.gameModel.Vehicles.GetRange(this.gameModel.Vehicles.Count / 2, this.gameModel.Vehicles.Count / 2);
            List<EnemyNetworkPackage> enemyNetworkData = new List<EnemyNetworkPackage>();

            foreach (var item in networkVehicle)
            {
                enemyNetworkData.Add(new EnemyNetworkPackage(item.UniqueId, item.Health));
            }

            ServerGamePacket serverPackage = new ServerGamePacket(tankTypeOwnTank, enemyNetworkData, this.gameModel.Team0BasePercent, this.gameModel.Team1BasePercent, (short)this.gameModel.SessionTime);
            if (this.remoteEp.ToString() != "0.0.0.0:11000")
            {
                ServerGamePacket checkSp = new ServerGamePacket(serverPackage.Payload);
                serverPackage.Send(this.udpServer, this.remoteEp);
            }
        }

        private void ClientSendMethod()
        {
            List<IVehicle> owntank = this.gameModel.Vehicles.GetRange(this.gameModel.Vehicles.Count / 2, this.gameModel.Vehicles.Count / 2);
            List<Tank> tankTypeOwnTank = new List<Tank>();
            foreach (var item in owntank)
            {
                tankTypeOwnTank.Add(new Tank(item.UniqueId, new VectorExtend(item.Px, item.Py), item.TurretIdx, item.BodyIdx, item.ShootIdx));
            }

            List<IVehicle> networkVehicle = this.gameModel.Vehicles.GetRange(0, this.gameModel.Vehicles.Count / 2);
            List<EnemyNetworkPackage> enemyNetworkData = new List<EnemyNetworkPackage>();
            foreach (var item in networkVehicle)
            {
                enemyNetworkData.Add(new EnemyNetworkPackage(item.UniqueId, item.Health));
            }

            ClientGamePacket clientPackage = new ClientGamePacket(tankTypeOwnTank, enemyNetworkData);

            clientPackage.Send(this.udpClient);
        }

        private void DecreaseTime(object source, System.Timers.ElapsedEventArgs e)
        {
            this.gameModel.SessionTime--;

            if (this.gameModel.SessionTime == 0)
            {
                this.sessionTimer.Stop();
            }
        }

        private void CalculateRevealedNodes()
        {
            List<Node> currentVisibleNodes = new List<Node>();
            foreach (IVehicle item in this.gameModel.TeamVehicles)
            {
                var vehicleNodeIdx = item.GetNodeIdxVehicleIsOn();
                foreach (var nodeIdx in this.revealRadius)
                {
                    var combinedIdx = new Tuple<int, int>(vehicleNodeIdx.X + nodeIdx.Item1, vehicleNodeIdx.Y + nodeIdx.Item2);
                    if (this.IsNodeOnGrid(combinedIdx) && !currentVisibleNodes.Contains(this.gameWorld.WorldGrid[combinedIdx.Item2, combinedIdx.Item1]))
                    {
                        currentVisibleNodes.Add(this.gameWorld.WorldGrid[combinedIdx.Item2, combinedIdx.Item1]);
                    }
                }
            }

            var newUnrevealedNodes = this.gameModel.RevealedNodes.Except(currentVisibleNodes).ToList();
            foreach (Node item in newUnrevealedNodes)
            {
                item.IsRevealed = false;
            }

            var newRevealedNodes = currentVisibleNodes.Except(this.gameModel.RevealedNodes).ToList();
            foreach (Node item in newRevealedNodes)
            {
                item.IsRevealed = true;
            }

            this.gameModel.RevealedNodes = currentVisibleNodes;
        }

        private bool IsNodeOnGrid(Tuple<int, int> nodeIdx)
        {
            if (nodeIdx.Item1 >= 0 && nodeIdx.Item1 < this.gameWorld.GridWidth && nodeIdx.Item2 >= 0 && nodeIdx.Item2 < this.gameWorld.GridHeight)
            {
                return true;
            }

            return false;
        }

        private bool CheckIfGameEnded()
        {
            var team0Count = this.gameModel.VehiclesTeam0.Count;
            var team1Count = this.gameModel.VehiclesTeam1.Count;

            if (this.gameModel.SessionTime < 1 || team0Count < 1 || team1Count < 1 || this.gameModel.Team0BasePercent >= 100 || this.gameModel.Team1BasePercent >= 100)
            {
                return true;
            }

            return false;
        }

        private byte CheckOutcome()
        {
            if (this.gameModel.SessionTime > 0)
            {
                if (this.gameModel.Team1BasePercent >= 100 || this.gameModel.VehiclesTeam1.Count < 1)
                {
                    return 0;
                }

                if (this.gameModel.Team0BasePercent >= 100 || this.gameModel.VehiclesTeam0.Count < 1)
                {
                    return 1;
                }
            }
            else
            {
                if (this.gameModel.Team1BasePercent >= 100 || this.gameModel.VehiclesTeam1.Count < 1)
                {
                    return 0;
                }

                if (this.gameModel.Team0BasePercent >= 100 || this.gameModel.VehiclesTeam0.Count < 1)
                {
                    return 1;
                }
            }

            return 2;
        }

        private void CalculateBaseCapture()
        {
            bool team0IsCapturing = this.gameModel.VehiclesTeam0.Any(x =>
            {
                var idx = x.GetNodeIdxVehicleIsOn();
                return this.gameWorld.WorldGrid[idx.Y, idx.X].NodeType == 9;
            });

            bool team1IsCapturing = this.gameModel.VehiclesTeam1.Any(x =>
            {
                var idx = x.GetNodeIdxVehicleIsOn();
                return this.gameWorld.WorldGrid[idx.Y, idx.X].NodeType == 8;
            });

            if (team0IsCapturing)
            {
                if (!this.gameModel.VehiclesTeam1.Any(x =>
                {
                    var idx = x.GetNodeIdxVehicleIsOn();
                    return this.gameWorld.WorldGrid[idx.Y, idx.X].NodeType == 9;
                }))
                {
                    this.team1BaseCounter++;
                    if (this.team1BaseCounter % 25 == 0)
                    {
                        this.gameModel.Team1BasePercent++;
                    }
                }
            }
            else
            {
                this.team1BaseCounter = 0;
                this.gameModel.Team1BasePercent = 0;
            }

            if (team1IsCapturing)
            {
                if (!this.gameModel.VehiclesTeam0.Any(x =>
                {
                    var idx = x.GetNodeIdxVehicleIsOn();
                    return this.gameWorld.WorldGrid[idx.Y, idx.X].NodeType == 8;
                }))
                {
                    this.team0BaseCounter++;
                    if (this.team0BaseCounter % 25 == 0)
                    {
                        this.gameModel.Team0BasePercent++;
                    }
                }
            }
            else
            {
                this.team0BaseCounter = 0;
                this.gameModel.Team0BasePercent = 0;
            }
        }
    }
}
