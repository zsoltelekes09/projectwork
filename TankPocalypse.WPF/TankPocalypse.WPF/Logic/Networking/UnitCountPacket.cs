// <copyright file="UnitCountPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    using System;

    /// <summary>
    /// UnitCount structure.
    /// </summary>
    public class UnitCountPacket : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitCountPacket"/> class.
        /// Set up an empty payload with unitcount packet type.
        /// </summary>
        public UnitCountPacket()
            : base(PacketType.UnitCount)
        {
            this.Payload = new byte[4];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitCountPacket"/> class.
        /// Convert byte to UnitCountPacket properties.
        /// </summary>
        /// <param name="bytes">byte array.</param>
        public UnitCountPacket(byte[] bytes)
            : base(PacketType.UnitCount)
        {
            this.Payload = bytes;
            this.UnitCounter = BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Gets or sets unit counter.
        /// </summary>
        public int UnitCounter { get; set; }
    }
}
