// <copyright file="PingPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Networking
{
    /// <summary>
    /// Ping packet class.
    /// </summary>
    public class PingPacket : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingPacket"/> class.
        /// Set PacketType to Ping,and an empty Payload.
        /// </summary>
        public PingPacket()
            : base(PacketType.Ping)
        {
            this.Payload = System.Array.Empty<byte>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PingPacket"/> class.
        /// Set PacketType to Ping and Payload to receive bytes.
        /// </summary>
        /// <param name="bytes">byte array.</param>
        public PingPacket(byte[] bytes)
            : base(PacketType.Ping)
        {
            this.Payload = bytes;
        }
    }
}
