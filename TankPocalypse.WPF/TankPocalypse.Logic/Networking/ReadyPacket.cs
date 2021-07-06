// <copyright file="ReadyPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Networking
{
    /// <summary>
    /// Ready packet class.
    /// </summary>
    public class ReadyPacket : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadyPacket"/> class.
        /// Set PacketType to Ready,and an empty Payload.
        /// </summary>
        public ReadyPacket()
            : base(PacketType.UserIsReady)
        {
            this.Payload = System.Array.Empty<byte>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadyPacket"/> class.
        /// Set PacketType to Ready and Payload to receive bytes.
        /// </summary>
        /// <param name="bytes">byte array.</param>
        public ReadyPacket(byte[] bytes)
            : base(PacketType.UserIsReady)
        {
            this.Payload = bytes;
        }
    }
}
