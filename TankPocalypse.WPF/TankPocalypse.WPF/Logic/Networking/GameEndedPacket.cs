// <copyright file="GameEndedPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    /// <summary>
    /// Game Ended packet.
    /// </summary>
    public class GameEndedPacket : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameEndedPacket"/> class.
        /// </summary>
        public GameEndedPacket()
            : base(PacketType.GameEnded)
        {
            this.Payload = System.Array.Empty<byte>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEndedPacket"/> class.
        /// </summary>
        /// <param name="bytes">bytes.</param>
        public GameEndedPacket(byte[] bytes)
            : base(PacketType.GameEnded)
        {
            this.Payload = bytes;
        }
    }
}
