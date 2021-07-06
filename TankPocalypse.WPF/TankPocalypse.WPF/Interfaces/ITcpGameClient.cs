// <copyright file="ITcpGameClient.cs" company="PlaceholderCompany">
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
    using TankPocalypse.Logic.Networking;
    using TankPocalypse.WPF.Logic;

    /// <summary>
    /// TcpGameClient interface.
    /// </summary>
    public interface ITcpGameClient
    {
        /// <summary>
        /// Gets or sets tcpclient.
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// Gets or sets gamecontrol reference.
        /// </summary>
        public IGameControl Gamecontrol { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether running state.
        /// </summary>
        public bool Running { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ready state.
        /// </summary>
        public bool AmIReady { get; set; }

        /// <summary>
        /// Gets or sets or set globaltcpprofile.
        /// </summary>
        public IUIProfile Globalprofiletcp { get; set; }

        /// <summary>
        /// Close connection.
        /// </summary>
        public void CloseConnection();

        /// <summary>
        /// Connection ho remote host.
        /// </summary>
        /// <param name="ip">ip adsress.</param>
        public void Connect(string ip);

        /// <summary>
        /// Run Cient.
        /// </summary>
        public void Run();

        /// <summary>
        /// Sends exit packet.
        /// </summary>
        public void ExitPacketSend();

        /// <summary>
        /// Sends ready packet.
        /// </summary>
        /// <param name="sender">object.</param>
        /// <param name="e">eventargs.</param>
        public void SendReadyPacket(object sender, EventArgs e);
    }
}
