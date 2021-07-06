// <copyright file="TcpGameServer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.Logic.UIClasses;
    using TankPocalypse.WPF.Interfaces;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Communication over Tcp protocol as a Server.
    /// </summary>
    public class TcpGameServer : IDisposable, ITcpGameServer
    {
        /// <summary>
        /// Mainvm.
        /// </summary>
        private readonly MainViewModel mainvm;

        private readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpGameServer"/> class.
        /// Setting up TcpListener and Port.
        /// </summary>
        /// <param name="port">port.</param>
        public TcpGameServer(int port)
        {
            this.Running = false;
            this.Port = port;
            this.Listener = new TcpListener(IPAddress.Any, port);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpGameServer"/> class.
        /// Set up mainvm.
        /// </summary>
        /// <param name="mainVM">MAINVM.</param>
        public TcpGameServer(MainViewModel mainVM)
        {
            this.Running = false;
            this.ClientIsReady = false;
            this.mainvm = mainVM;
        }

        /// <summary>
        /// Gets or sets a value indicating whether running.
        /// </summary>
        public bool Running { get; set; }

        /// <summary>
        /// Gets or sets a value of the listener.
        /// </summary>
        public TcpListener Listener { get; set; }

        /// <summary>
        /// Gets or sets Client.
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// Gets or sets a value of the gamecontrol.
        /// </summary>
        public IGameControl Gamecontrol { get; set; }

        /// <inheritdoc/>
        public bool ClientIsReady { get; set; }

        /// <summary>
        /// Gets or sets networkStream.
        /// </summary>
        public NetworkStream Stream { get; set; }

        /// <inheritdoc/>
        public IUIProfile Globalprofiletcp { get; set; }

        /// <inheritdoc/>
        public IUIMap Globalmapdata { get; set; }

        /// <summary>
        /// Gets or sets a value of the port.
        /// </summary>
        public int Port { get; set; }

        private LobbyViewModel Lobbyview { get; set; }

        /// <summary>
        /// Start Accepting Tcp cients.
        /// </summary>
        public void Run()
        {
            this.Running = true;
            while (this.Running)
            {
                this.HandleNewConnection();

                // System.Diagnostics.Debug.WriteLine("Thread:" + Thread.CurrentThread.ManagedThreadId);
                this.ConnenctionPacketControl();
            }
        }

        /// <summary>
        /// Closing all connetions and dispose them.
        /// </summary>
        public void CloseConnection()
        {
            // this.stream.Close();
            this.Client.Dispose();

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Connection handler.
        /// </summary>
        public void ConnenctionPacketControl()
        {
            while (this.Running)
            {
                if (IsDisconnected(this.Client))
                {
                    try
                    {
                        byte[] type = new byte[4];
                        this.Stream.Read(type, 0, 4);
                        PacketType p = (PacketType)(PacketType)BitConverter.ToUInt32(type, 0);
                        byte[] length = new byte[4];
                        this.Stream.Read(length, 0, 4);
                        int packetsize = BitConverter.ToInt32(length, 0);
                        byte[] packet = new byte[packetsize];
                        if (packetsize > 0)
                        {
                            this.Stream.Read(packet, 0, packet.Length);
                        }

                        switch (p)
                        {
                            case PacketType.Username:
                                this.ProcessUser(packet);
                                this.UserNameMapData();
                                UnitCountPacket up = new UnitCountPacket();
                                up.UnitCounter = this.mainvm.GlobalUnitCount;
                                BitConverter.GetBytes(up.UnitCounter).CopyTo(up.Payload, 0);
                                up.Send(this.Stream);
                                if (this.mainvm.GlobalSaveFile != null)
                                {
                                    this.SaveFileSend();
                                }

                                break;
                            case PacketType.UserIsReady:
                                ReadyPacket rp = new ReadyPacket(packet);
                                if (rp.Payload[0] == 1)
                                {
                                    this.mainvm.NetworkUserIsReady = true;
                                    this.mainvm.NetworkUserReadyText = "Ready";
                                    long x = this.PingSend();
                                    System.Diagnostics.Debug.WriteLine("PING: " + x + "ms");
                                }
                                else
                                {
                                    this.mainvm.NetworkUserIsReady = false;
                                    this.mainvm.NetworkUserReadyText = "Not Ready";
                                }

                                break;
                            case PacketType.UnitCount:
                                UnitCountPacket up2 = new UnitCountPacket();
                                up2.UnitCounter = this.mainvm.SelectedUnitCount;
                                BitConverter.GetBytes(up2.UnitCounter).CopyTo(up2.Payload, 0);
                                up2.Send(this.Stream);
                                break;
                            case PacketType.CanStartGame:

                                break;
                            case PacketType.Pause:
                                break;
                            case PacketType.ClientPackage:
                                break;
                            case PacketType.ServerPackage:
                                break;
                            case PacketType.Ping:

                                PingPacket pingmore = new PingPacket();

                                pingmore.Send(this.Stream);

                                break;
                            case PacketType.Exit:

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    this.mainvm.LobbyMenuCommand.Execute(null);
                                });

                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            this.mainvm.NetworkProfile = null;
                            this.mainvm.NetworkUserIsReady = false;
                            this.mainvm.NetworkUserReadyText = "Not Ready";
                            this.CloseConnection();
                            if (this.mainvm.SelectedViewModel is not MenuViewModel)
                            {
                                this.mainvm.LobbyMenuCommand.Execute(null);
                            }
                        });
                        this.Running = false;
                        break;
                    }
                }
                else
                {
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        this.mainvm.NetworkProfile = null;
                        this.mainvm.NetworkUserIsReady = false;
                        this.mainvm.NetworkUserReadyText = "Not Ready";
                        this.mainvm.LobbyMenuCommand.Execute(null);
                    });

                    break;
                }
            }
        }

        /// <summary>
        /// Sending Game is over packet.
        /// </summary>
        public void SendGameOverPacket()
        {
            Packet p = new Packet();
            p.Type = PacketType.GameEnded;
            p.Payload = Array.Empty<byte>();
            p.Send(this.Stream);
        }

        /// <summary>
        /// Sending SelectedUnit Count.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">eventarg.</param>
        public void UnitCountSend(object sender, EventArgs e)
        {
            if (this.Client != null && this.Client.Connected)
            {
                UnitCountPacket up = new UnitCountPacket();
                up.UnitCounter = this.mainvm.GlobalUnitCount;
                BitConverter.GetBytes(up.UnitCounter).CopyTo(up.Payload, 0);
                try
                {
                    up.Send(this.Stream);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Sending Start Game packet.
        /// </summary>
        public void StartGamePacket()
        {
            Packet startPacket = new Packet();
            startPacket.Payload = Array.Empty<byte>();
            startPacket.Type = PacketType.CanStartGame;
            startPacket.Send(this.Stream);
        }

        /// <summary>
        /// Sending Pause event trigger Packet.
        /// </summary>
        public void StartPausePacket()
        {
            Packet startPacket = new Packet();
            startPacket.Payload = Array.Empty<byte>();
            if (this.Gamecontrol.IsGameEnded)
            {
                startPacket.Type = PacketType.GameEnded;
            }
            else
            {
                startPacket.Type = PacketType.Pause;
            }

            if (!this.Gamecontrol.IsGameEnded)
            {
                startPacket.Send(this.Stream);
            }
        }

        /// <summary>
        /// Process ping.
        /// </summary>
        /// <param name="bytes">byte[].</param>
        public static void ProcessPing(byte[] bytes)
        {
            PingPacket pingTime = new PingPacket(bytes);
        }

        /// <summary>
        /// Sending Exit Packet.
        /// </summary>
        public void ExitPacketSend()
        {
            ExitPacket exitpacket = new ExitPacket();
            exitpacket.Send(this.Stream);
        }

        /// <summary>
        /// Sending Ping packet.
        /// </summary>
        /// <returns>long.</returns>
        public long PingSend()
        {
            this.stopwatch.Reset();
            PingPacket ping = new PingPacket();
            this.stopwatch.Start();
            ping.Send(this.Stream);

            byte[] type = new byte[4];
            this.Stream.Read(type, 0, 4);
            PacketType p = (PacketType)(PacketType)BitConverter.ToUInt32(type, 0);
            byte[] length = new byte[4];
            this.Stream.Read(length, 0, 4);
            int packetsize = BitConverter.ToInt32(length, 0);

            byte[] packet = new byte[packetsize];
            if (packetsize > 0)
            {
                this.Stream.Read(packet, 0, packet.Length);
            }

            if (p == PacketType.Ping)
            {
                PingPacket pingresponse = new PingPacket(packet);
                this.stopwatch.Stop();
            }

            return this.stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Processing user Data.
        /// </summary>
        /// <param name="packet">byte array.</param>
        public void ProcessUser(byte[] packet)
        {
            UsernamePacket usernamepacket = new UsernamePacket(packet);
            UIProfile profile = new UIProfile(usernamepacket.UserName, 1, 5, 3, 3, 2, 6);
            this.mainvm.NetworkProfile = profile;
        }

        /// <summary>
        /// Creating UserName and Map data packet.
        /// </summary>
        public void UserNameMapData()
        {
            IUIMap uimap = this.mainvm.GlobalMap;

            UserMapDataPacket usermapPacket = new UserMapDataPacket(this.mainvm.GlobalProfile.UserName, uimap);
            if (this.Client != null && this.Client.Connected)
            {
                usermapPacket.Send(this.Stream);
            }
        }

        /// <summary>
        /// Sending SaveFile packet.
        /// </summary>
        public void SaveFileSend()
        {
            IUISavedGame savefile = this.mainvm.GlobalSaveFile;
            SaveFilePacket sfp = new SaveFilePacket(savefile);
            if (this.Client != null && this.Client.Connected)
            {
                sfp.Send(this.Stream);
            }
        }

        /// <summary>
        /// Accepting new client to connect to the host.
        /// </summary>
        public void HandleNewConnection()
        {
            try
            {
                this.Listener.Start();
                this.Client = this.Listener.AcceptTcpClient();
                this.Client.NoDelay = true;
                this.Stream = this.Client.GetStream();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Check if remote host is connected or not.
        /// </summary>
        /// <param name="client">TcpClient.</param>
        /// <returns>bool.</returns>
        private static bool IsDisconnected(TcpClient client)
        {
            try
            {
                if (client != null && client.Client != null)
                {
                    if (!(client.Client.Poll(1, SelectMode.SelectRead) && client.Client.Available == 0))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (SocketException) { return false; }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// Dispose client.
        /// </summary>
        public void Dispose()
        {
            this.Stream.Close();
            this.Client.Dispose();
        }
    }
}