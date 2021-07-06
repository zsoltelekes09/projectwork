// <copyright file="ITcpGameServer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.WPF.Logic;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Skeleton of the TcpGameServer.
    /// </summary>
    public interface ITcpGameServer
    {
        /// <summary>
        /// Gets or sets a value indicating whether running value.
        /// </summary>
        public bool Running { get; set; }

        /// <summary>
        /// Gets or sets tcpclient.
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether clientIsReady.
        /// </summary>
        public bool ClientIsReady { get; set; }

        /// <summary>
        /// Gets or sets networkStream.
        /// </summary>
        public NetworkStream Stream { get; set; }

        /// <summary>
        /// Gets or sets listener.
        /// </summary>
        public TcpListener Listener { get; set; }

        /// <summary>
        /// Gets or sets globalprofiletcp.
        /// </summary>
        public IUIProfile Globalprofiletcp { get; set; }

        /// <summary>
        /// Gets or sets globalmapdata.
        /// </summary>
        public IUIMap Globalmapdata { get; set; }

        /// <summary>
        /// Gets or sets port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets gamecontrol.
        /// </summary>
        public IGameControl Gamecontrol { get; set; }

        /// <summary>
        /// Run Server.
        /// </summary>
        public void Run();

        /// <summary>
        /// Exit packet send.
        /// </summary>
        public void ExitPacketSend();

        /// <summary>
        /// Sending Game over packet.
        /// </summary>
        public void SendGameOverPacket();

        /// <summary>
        /// Send Pause packet.
        /// </summary>
        public void StartPausePacket();

        /// <summary>
        /// Send SaveFile packet.
        /// </summary>
        public void SaveFileSend();

        /// <summary>
        /// UnitcountSend.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">parameter eventargs.</param>
        public void UnitCountSend(object sender, EventArgs e);

        /// <summary>
        /// Send Start game packet.
        /// </summary>
        public void StartGamePacket();
    }
}
