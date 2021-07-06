// <copyright file="INetwork.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Interfaces
{
    /// <summary>
    /// Basic Network methods.
    /// </summary>
    public interface INetwork
    {
        /// <summary>
        /// Starts udp client.
        /// </summary>
        /// <param name="ip">ip.</param>
        public void StartUdpClient(string ip);

        /// <summary>
        /// Stops udp client.
        /// </summary>
        public void StopUdpClient();

        /// <summary>
        /// Starts udp server.
        /// </summary>
        public void StartUdpServer();

        /// <summary>
        /// Stops udp server.
        /// </summary>
        public void StopUdpServer();
    }
}
