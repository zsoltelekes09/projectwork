// <copyright file="MainViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.VM
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.Renderer.Interfaces;
    using TankPocalypse.WPF.Interfaces;
    using TankPocalypse.WPF.Logic;
    using TankPocalypse.WPF.Logic.Networking;

    /// <summary>
    /// This is the main viewmodel. Every usercontrol switching being handled here.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IMenuImages menuImages;
        private object selectedViewModel;
        private IUIProfile globalProfile;
        private IUIMap globalMap;
        private bool appIsServer;
        private IUISavedGame globalSaveFile;
        private byte globatTeamId;
        private byte globalUnitCount;
        private byte selectedUnitCount;

        private ITcpGameServer tcpGameServer;
        private ITcpGameClient tcpGameClient;
        private string ipAddress;
        private IUIProfile networkProfile;
        private bool networkUserIsReady;
        private string networkUserReadyText = "Not Ready";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            this.MenuImages = new MenuImages();
            this.SetupVM();
        }

        /// <summary>
        /// Gets the menu images library.
        /// </summary>
        public IMenuImages MenuImages
        {
            get { return this.menuImages; }
            private set { this.menuImages = value; }
        }

        /// <summary>
        /// Gets the profile menu command.
        /// </summary>
        public ICommand ProfileMenuCommand { get; private set; }

        /// <summary>
        /// Gets the main menu command.
        /// </summary>
        public ICommand MainMenuCommand { get; private set; }

        /// <summary>
        /// Gets the game view command.
        /// </summary>
        public ICommand GameViewCommand { get; private set; }

        /// <summary>
        /// Gets the lobby menu command.
        /// </summary>
        public ICommand LobbyMenuCommand { get; private set; }

        /// <summary>
        /// Gets the server menu command.
        /// </summary>
        public ICommand ServerMenuCommand { get; private set; }

        /// <summary>
        /// Gets the client menu command.
        /// </summary>
        public ICommand ClientMenuCommand { get; private set; }

        /// <summary>
        /// Gets the exit app command.
        /// </summary>
        public ICommand ExitApp { get; private set; }

        /// <summary>
        /// Gets the load menu command.
        /// </summary>
        public ICommand LoadViewCommand { get; private set; }

        /// <summary>
        /// Gets the new profile command.
        /// </summary>
        public ICommand NewProfileCommand { get; private set; }

        /// <summary>
        /// Gets the high score command.
        /// </summary>
        public ICommand HighScoreCommand { get; private set; }

        /// <summary>
        /// Gets or sets the tcp game server property.
        /// </summary>
        public ITcpGameServer TcpGameServer
        {
            get { return this.tcpGameServer; }
            set { this.tcpGameServer = value; }
        }

        /// <summary>
        /// Gets or sets the TcpGameClient property.
        /// </summary>
        public ITcpGameClient TcpGameClient
        {
            get { return this.tcpGameClient; }
            set { this.tcpGameClient = value; }
        }

        /// <summary>
        /// Gets or sets the ip address of the connection.
        /// </summary>
        public string IpAddress
        {
            get { return this.ipAddress; }
            set { this.ipAddress = value; }
        }

        /// <summary>
        /// Gets or sets the selected viewmodel.
        /// </summary>
        public object SelectedViewModel
        {
            get => this.selectedViewModel;
            set => this.Set(ref this.selectedViewModel, value);
        }

        /// <summary>
        /// Gets or sets the global profile.
        /// </summary>
        public IUIProfile GlobalProfile
        {
            get => this.globalProfile;
            set => this.Set(ref this.globalProfile, value);
        }

        /// <summary>
        /// Gets or sets the global map property.
        /// </summary>
        public IUIMap GlobalMap
        {
            get => this.globalMap;
            set => this.Set(ref this.globalMap, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether application is running in server mode or not.
        /// </summary>
        public bool AppIsServer
        {
            get => this.appIsServer;
            set => this.Set(ref this.appIsServer, value);
        }

        /// <summary>
        /// Gets or sets the network profile porperty.
        /// </summary>
        public IUIProfile NetworkProfile
        {
            get => this.networkProfile;
            set => this.Set(ref this.networkProfile, value);
        }

        /// <summary>
        /// Gets or sets the applications team id.
        /// </summary>
        public byte GlobatTeamId
        {
            get => this.globatTeamId;
            set => this.Set(ref this.globatTeamId, value);
        }

        /// <summary>
        /// Gets or sets the global unit count.
        /// </summary>
        public byte GlobalUnitCount
        {
            get => this.globalUnitCount;
            set => this.Set(ref this.globalUnitCount, value);
        }

        /// <summary>
        /// Gets or sets the global savefile.
        /// </summary>
        public IUISavedGame GlobalSaveFile
        {
            get => this.globalSaveFile;
            set => this.Set(ref this.globalSaveFile, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether network user is ready or not.
        /// </summary>
        public bool NetworkUserIsReady
        {
            get => this.networkUserIsReady;
            set => this.Set(ref this.networkUserIsReady, value);
        }

        /// <summary>
        /// Gets or sets the networ users ready text.
        /// </summary>
        public string NetworkUserReadyText
        {
            get => this.networkUserReadyText;
            set => this.Set(ref this.networkUserReadyText, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether a new profile is needed or not.
        /// </summary>
        public bool NewProfileNeeded { get; set; }

        /// <summary>
        /// Gets or sets the selected unit count.
        /// </summary>
        public byte SelectedUnitCount
        {
            get => this.selectedUnitCount;
            set => this.Set(ref this.selectedUnitCount, value);
        }

        /// <summary>
        /// Starts the game server.
        /// </summary>
        public void StartServer()
        {
            this.TcpGameServer = new TcpGameServer(this);
            this.TcpGameServer.Listener = new System.Net.Sockets.TcpListener(IPAddress.Any, 15000);
            this.TcpGameServer.Run();
        }

        /// <summary>
        /// Starts the game client.
        /// </summary>
        /// <param name="ip">Connection ip.</param>
        public void StartClient(string ip)
        {
            this.TcpGameClient.Globalprofiletcp = this.GlobalProfile;
            this.IpAddress = ip;
            this.TcpGameClient.Run();
        }

        private void SetupVM()
        {
            var tmpProfiles = new GameControl(true).GetProfiles();

            if (tmpProfiles.Count > 0)
            {
                this.GlobalProfile = tmpProfiles.FirstOrDefault();
            }
            else
            {
                this.NewProfileNeeded = true;
            }

            this.ClientMenuCommand = new RelayCommand(() => { this.SelectedViewModel = new ClientViewModel(this); });
            this.LoadViewCommand = new RelayCommand(() => { this.SelectedViewModel = new LoadViewModel(this); });
            this.NewProfileCommand = new RelayCommand(() => { this.SelectedViewModel = new NewProfileViewModel(this); });
            this.HighScoreCommand = new RelayCommand(() => { this.SelectedViewModel = new HighScoreViewModel(this); });
            this.ProfileMenuCommand = new RelayCommand(() => { this.SelectedViewModel = new ProfileViewModel(this); });
            this.ExitApp = new RelayCommand(() => { System.Windows.Application.Current.Shutdown(); });

            this.ServerMenuCommand = new RelayCommand(() =>
            {
                this.NetworkProfile = null;
                this.SelectedViewModel = new ServerViewModel(this);
            });

            this.GameViewCommand = new RelayCommand(() => { this.SelectedViewModel = new GameViewModel(this); });

            this.MainMenuCommand = new RelayCommand(() =>
            {
                if (this.AppIsServer)
                {
                    if (this.TcpGameServer != null)
                    {
                        this.TcpGameServer.Running = false;
                        this.TcpGameServer.Listener.Stop();
                        if (this.TcpGameServer.Client != null)
                        {
                            this.TcpGameServer.Client.Close();
                        }
                    }
                }
                else
                {
                    if (this.SelectedViewModel is not ClientViewModel)
                    {
                        if (this.TcpGameClient != null)
                        {
                            this.TcpGameClient.CloseConnection();
                        }
                    }
                }

                this.SelectedViewModel = new MenuViewModel(this);
                GC.Collect();
            });

            this.LobbyMenuCommand = new RelayCommand(() =>
            {
                if (this.SelectedViewModel is GameViewModel gvm)
                {
                    if (this.AppIsServer)
                    {
                        if (this.TcpGameServer.Client.Connected)
                        {
                            this.TcpGameServer.ExitPacketSend();
                            this.NetworkUserIsReady = false;
                            this.NetworkUserReadyText = "Not Ready";
                        }
                        else if (this.TcpGameServer != null && !this.TcpGameServer.Client.Connected)
                        {
                            this.TcpGameServer.Running = false;
                            this.TcpGameServer.Listener.Stop();

                            Task.Run(() =>
                            {
                                this.StartServer();
                            });
                        }
                    }
                    else
                    {
                        this.TcpGameClient.ExitPacketSend();
                        Thread.Sleep(100);
                    }

                    gvm.DeleteEverything();
                    this.SelectedViewModel = null;
                    TankIoc.Instance.Unregister<IGameLogic>();
                    TankIoc.Instance.Unregister<IGameRenderer>();
                    TankIoc.Instance.Unregister<IGameInputController>();
                    TankIoc.Instance.Reset();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    App.RegisterInstances();
                }
                else if (this.SelectedViewModel is LoadViewModel lvm)
                {
                    this.SelectedViewModel = null;
                }
                else
                {
                    this.SelectedViewModel = null;
                    GC.Collect();
                }

                this.SelectedViewModel = new LobbyViewModel(this);
            });

            if (this.NewProfileNeeded)
            {
                this.SelectedViewModel = new NewProfileViewModel(this);
            }
            else
            {
                this.SelectedViewModel = new MenuViewModel(this);
            }
        }
    }
}
