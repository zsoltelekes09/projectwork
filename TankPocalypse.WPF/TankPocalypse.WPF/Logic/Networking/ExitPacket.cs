// <copyright file="ExitPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    /// <summary>
    /// Exit packet structure.
    /// </summary>
    public class ExitPacket : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExitPacket"/> class.
        /// With Exit type packetType.
        /// </summary>
        public ExitPacket()
            : base(PacketType.Exit)
        {
            this.Payload = System.Array.Empty<byte>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitPacket"/> class.
        /// </summary>
        /// <param name="bytes">bytes.</param>
        public ExitPacket(byte[] bytes)
            : base(PacketType.Exit)
        {
            this.Payload = bytes;
        }
    }
}
