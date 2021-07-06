// <copyright file="TcpGameClient.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Sockets;
    using System.Windows;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.Logic.UIClasses;
    using TankPocalypse.WPF.Interfaces;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Communication over Tcp protocol as a client.
    /// </summary>
    public class TcpGameClient : ITcpGameClient
    {
        /// <summary>
        /// Stopwatch.
        /// </summary>
        private readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// port.
        /// </summary>
        private readonly int port;

        /// <summary>
        /// Mainvm.
        /// </summary>
        private MainViewModel mainvm;

        /// <summary>
        /// LobbyView.
        /// </summary>
        private LobbyViewModel lobbyview;

        /// <summary>
        /// ServerAddress.
        /// </summary>
        private string serverAddress;

        /// <summary>
        /// Stream.
        /// </summary>
        private NetworkStream stream;

        /// <summary>
        /// UserMapDataPacket.
        /// </summary>
        private UserMapDataPacket userMapData;

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpGameClient"/> class.
        /// Create Tcpclient entity.
        /// </summary>
        /// <param name="ip">ip.</param>
        /// <param name="port">port.</param>
        public TcpGameClient(string ip, int port)
        {
            this.Client = new TcpClient();
            this.Running = false;
            this.port = port;
            this.serverAddress = ip;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpGameClient"/> class.
        /// constructor.
        /// </summary>
        /// <param name="mainVM">mainvm.</param>
        public TcpGameClient(MainViewModel mainVM)
        {
            this.Running = false;
            this.Client = new TcpClient();
            this.mainvm = mainVM;
        }

        /// <inheritdoc/>
        public TcpClient Client { get; set; }

        /// <inheritdoc/>
        public IGameControl Gamecontrol { get; set; }

        /// <inheritdoc/>
        public bool Running { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether amiReady.
        /// </summary>
        public bool AmIReady { get; set; }

        /// <summary>
        /// Gets or sets Globalprofiletcp.
        /// </summary>
        public IUIProfile Globalprofiletcp { get; set; }

        /// <summary>
        /// Reading until the whole array is read.
        /// </summary>
        /// <param name="stream">stream.</param>
        /// <param name="data">data.</param>
        public static void ReadWholeArray(Stream stream, byte[] data)
        {
            int offset = 0;
            int remaining = data.Length;
            while (remaining > 0)
            {
                int read = stream.Read(data, offset, remaining);
                if (read <= 0)
                {
                    throw new EndOfStreamException();
                }

                remaining -= read;
                offset += read;
            }
        }

        /// <summary>
        /// Closing connections and stream.
        /// </summary>
        public void CloseConnection()
        {
            this.stream.Close();
            this.Client.Close();
            GC.Collect();
        }

        /// <summary>
        /// Set up Lobby.
        /// </summary>
        /// <param name="lb">lobby.</param>
        public void SetLobby(LobbyViewModel lb)
        {
            this.lobbyview = lb;
        }

        /// <summary>
        /// connecting to a remote server.
        /// </summary>
        /// <param name="ip">IP address.</param>
        public void Connect(string ip)
        {
            this.serverAddress = ip;
            try
            {
                this.Client.Connect(this.serverAddress, 15000);
            }
            catch (SocketException)
            {
            }

            if (this.Client.Connected)
            {
                this.Running = true;
                this.stream = this.Client.GetStream();
            }
        }

        /// <summary>
        /// Run.
        /// </summary>
        public void Run()
        {
            if (this.Running)
            {
                UsernamePacket username = new UsernamePacket(this.Globalprofiletcp.UserName);
                username.Send(this.stream);
            }

            while (this.Running)
            {
                if (IsDisconnected(this.Client))
                {
                    this.HandleIncomingPackets();
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        this.mainvm.MainMenuCommand.Execute(null);
                    });
                    this.stream.Close();
                    this.Client.Close();
                    this.Running = false;
                }
            }
        }

        /// <summary>
        /// Sending Ping Packet.
        /// </summary>
        /// <returns>long.</returns>
        public long PingSend()
        {
            this.stopwatch.Reset();
            PingPacket ping = new PingPacket();
            this.stopwatch.Start();
            ping.Send(this.stream);

            byte[] type = new byte[4];
            this.stream.Read(type, 0, 4);
            PacketType p = (PacketType)(PacketType)BitConverter.ToUInt32(type, 0);
            byte[] length = new byte[4];
            this.stream.Read(length, 0, 4);
            int packetsize = BitConverter.ToInt32(length, 0);

            byte[] packet = new byte[packetsize];
            if (packetsize > 0)
            {
                this.stream.Read(packet, 0, packet.Length);
            }

            if (p == PacketType.Ping)
            {
                PingPacket pingresponse = new PingPacket(packet);
                this.stopwatch.Stop();
            }

            return this.stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Sending Exit packet.
        /// </summary>
        public void ExitPacketSend()
        {
            ExitPacket exitpacket = new ExitPacket();
            exitpacket.Send(this.stream);
        }

        /// <summary>
        /// Sending Ready packet.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">eventargs.</param>
        public void SendReadyPacket(object sender, EventArgs e)
        {
            if (this.AmIReady != true)
            {
                Packet p = new Packet();
                p.Type = PacketType.UserIsReady;
                p.Payload = new byte[1];
                p.Payload[0] = Convert.ToByte(1);
                p.Send(this.stream);
                this.AmIReady = true;
            }
            else
            {
                Packet p = new Packet();
                p.Type = PacketType.UserIsReady;
                p.Payload = new byte[1];
                p.Payload[0] = Convert.ToByte(0);
                p.Send(this.stream);
                this.AmIReady = false;
            }
        }

        /// <summary>
        /// Handling Usermapdata.
        /// </summary>
        /// <param name="packet">packet.</param>
        public void ProcessUserMapData(byte[] packet)
        {
            this.userMapData = new UserMapDataPacket(packet);

            UIProfile profile = new UIProfile(this.userMapData.UserName, 1, 5, 3, 3, 2, 6);
            if (this.userMapData.MapInfo != null)
            {
                this.mainvm.GlobalMap = this.userMapData.MapInfo;
            }

            this.mainvm.NetworkProfile = profile;
        }

        /// <summary>
        /// Check if connection lost or something wrong with it.
        /// </summary>
        /// <param name="client">Tcpclient.</param>
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
        /// Handling incoming packets.
        /// </summary>
        private void HandleIncomingPackets()
        {
            try
            {
                // Check for new incomding messages
                if (this.Client.Available > 0 && this.Client.Connected)
                {
                    byte[] type = new byte[4];
                    this.stream.Read(type, 0, 4);
                    PacketType p = (PacketType)(PacketType)BitConverter.ToUInt32(type, 0);
                    byte[] length = new byte[4];
                    this.stream.Read(length, 0, 4);
                    int packetsize = BitConverter.ToInt32(length, 0);
                    byte[] packet = new byte[packetsize];
                    if (packetsize > 0)
                    {
                        if (p != PacketType.SaveFile)
                        {
                            this.stream.Read(packet, 0, packetsize);
                        }
                        else
                        {
                            ReadWholeArray(this.stream, packet);
                        }
                    }

                    switch (p)
                    {
                        case PacketType.UsernameMapData:

                            this.ProcessUserMapData(packet);
                            break;
                        case PacketType.UnitCount:
                            UnitCountPacket up = new UnitCountPacket(packet);
                            this.mainvm.GlobalUnitCount = Convert.ToByte(up.UnitCounter);
                            break;
                        case PacketType.CanStartGame:
                            this.mainvm.GameViewCommand.Execute(null);
                            break;
                        case PacketType.Pause:
                            this.Gamecontrol.PauseGame();
                            break;
                        case PacketType.GameEnded:
                            this.Gamecontrol.SetEnd();
                            break;
                        case PacketType.ServerPackage:
                            break;
                        case PacketType.Ping:
                            PingPacket ping = new PingPacket();
                            ping.Send(this.stream);
                            break;
                        case PacketType.SaveFile:
                            SaveFilePacket svp = new SaveFilePacket(packet);
                            SaveFileHelper svHelper = new SaveFileHelper(svp.SaveId, svp.SaveName, svp.SaveFileXDoc);
                            if (svHelper.SaveName != "null")
                            {
                                this.mainvm.GlobalSaveFile = svHelper;
                            }
                            else
                            {
                                this.mainvm.GlobalSaveFile = null;
                            }

                            break;
                        case PacketType.Exit:

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                this.AmIReady = false;
                                this.mainvm.LobbyMenuCommand.Execute(null);
                            });
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.mainvm.MainMenuCommand.Execute(null);
                });
            }
        }
    }
}
