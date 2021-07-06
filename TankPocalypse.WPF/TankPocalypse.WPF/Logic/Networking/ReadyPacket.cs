// <copyright file="ReadyPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    /// <summary>
    /// Ready Packet.
    /// </summary>
    public class ReadyPacket : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadyPacket"/> class.
        /// </summary>
        public ReadyPacket()
            : base(PacketType.UserIsReady)
        {
            this.Payload = System.Array.Empty<byte>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadyPacket"/> class.
        /// </summary>
        /// <param name="bytes">bytes.</param>
        public ReadyPacket(byte[] bytes)
            : base(PacketType.UserIsReady)
        {
            this.Payload = bytes;
        }
    }
}
