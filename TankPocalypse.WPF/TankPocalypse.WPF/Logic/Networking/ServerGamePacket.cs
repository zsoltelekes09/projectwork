// <copyright file="ServerGamePacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ServerGame Packet structure.
    /// </summary>
    public class ServerGamePacket : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerGamePacket"/> class.
        /// Setup Properties via constructor.
        /// </summary>
        /// <param name="tanks">tanks.</param>
        /// <param name="networkTanks">networkTanks.</param>
        /// <param name="team0Base">team0Base.</param>
        /// <param name="team1Base">team1Base.</param>
        /// <param name="sessionTime">sessionTime.</param>
        public ServerGamePacket(List<Tank> tanks, List<EnemyNetworkPackage> networkTanks, int team0Base, int team1Base, short sessionTime)
        {
            this.Tanks = tanks;
            this.NetworkTanks = networkTanks;
            this.Team0Base = team0Base;
            this.Team1Base = team1Base;
            this.SessionTime = sessionTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerGamePacket"/> class.
        /// Empty contructor.
        /// </summary>
        public ServerGamePacket()
            : base(PacketType.ServerPackage)
        {
        }

        /// <summary>
        /// Gets or sets of sets Tankist.
        /// </summary>
        public List<Tank> Tanks { get; set; }

        /// <summary>
        /// Gets or sets of sets NetworkTanks.
        /// </summary>
        public List<EnemyNetworkPackage> NetworkTanks { get; set; }

        /// <summary>
        /// Gets or sets of sets Tankist.
        /// </summary>
        public int Team0Base { get; set; }

        /// <summary>
        /// Gets or sets of sets Team1Base.
        /// </summary>
        public int Team1Base { get; set; }

        /// <summary>
        /// Gets or sets of sets SessionTime.
        /// </summary>
        public short SessionTime { get; set; }

        /// <summary>
        /// Create byte array from ServerGamepacket Payload.
        /// </summary>
        /// <returns>byte array.</returns>
        public byte[] SetServerGamePacketPayload()
        {
            byte[] payloadSend = new byte[(this.Tanks.Count * 20) + (this.Tanks.Count * sizeof(int)) + sizeof(int) + sizeof(short)];
            int bytecounter = 0;
            foreach (var item in this.Tanks)
            {
                this.CreateByteFromList(this.Tanks, this.Tanks.Count).CopyTo(payloadSend, 0);
            }

            bytecounter += this.Tanks.Count * 20;
            foreach (var item in this.NetworkTanks)
            {
                CreateByteFromNetworkTankList(this.NetworkTanks).CopyTo(payloadSend, bytecounter);
            }

            bytecounter += this.NetworkTanks.Count * 10;
            BitConverter.GetBytes(this.Team0Base).CopyTo(this.Payload, bytecounter);
            bytecounter += sizeof(int);
            BitConverter.GetBytes(this.Team1Base).CopyTo(this.Payload, bytecounter);
            bytecounter += sizeof(int);
            BitConverter.GetBytes(this.SessionTime).CopyTo(this.Payload, bytecounter);
            this.Payload = payloadSend;
            return payloadSend;
        }
    }
}
