// <copyright file="PingPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    /// <summary>
    /// PingPacket.
    /// </summary>
    public class PingPacket : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingPacket"/> class.
        /// Empty packet with Ping type.
        /// </summary>
        public PingPacket()
            : base(PacketType.Ping)
        {
            this.Payload = System.Array.Empty<byte>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PingPacket"/> class.
        /// </summary>
        /// <param name="bytes">byte array.</param>
        public PingPacket(byte[] bytes)
            : base(PacketType.Ping)
        {
            this.Payload = bytes;
        }
    }
}
