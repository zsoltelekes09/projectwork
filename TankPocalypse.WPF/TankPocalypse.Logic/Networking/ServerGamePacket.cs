// <copyright file="ServerGamePacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Networking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Server Game Packet class.
    /// </summary>
    public class ServerGamePacket : Packet
    {
        /// <summary>
        /// number of tanks.
        /// </summary>
        private int numOfTanks;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerGamePacket"/> class.
        /// Set properties' values.
        /// </summary>
        /// <param name="tanks">own tanklist.</param>
        /// <param name="networkTanks"> enemy tanklist.</param>
        /// <param name="team0Base">team0 base percent.</param>
        /// <param name="team1Base">team1 base percent.</param>
        /// <param name="sessionTime">sessionTime.</param>
        public ServerGamePacket(List<Tank> tanks, List<EnemyNetworkPackage> networkTanks, int team0Base, int team1Base, short sessionTime)
            : base(PacketType.ServerPackage)
        {
            this.Tanks = tanks;
            this.NetworkTanks = networkTanks;
            this.Team0Base = team0Base;
            this.Team1Base = team1Base;
            this.SessionTime = sessionTime;
            this.Payload = this.SetServerGamePacketPayload();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerGamePacket"/> class.
        /// Set packettype to ServerPackage.
        /// </summary>
        public ServerGamePacket()
            : base(PacketType.ServerPackage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerGamePacket"/> class.
        /// Convert byte array to ServerGamepacket properties.
        /// </summary>
        /// <param name="bytes">bytes array.</param>
        public ServerGamePacket(byte[] bytes)
        {
            int i = 0;
            this.numOfTanks = Convert.ToInt32(bytes[0]);
            i += sizeof(byte);
            this.Payload = bytes.Skip(i).ToArray();
            this.ReadServerGamePacketPayload(this.numOfTanks);
        }

        /// <summary>
        /// Gets or sets tank list.
        /// </summary>
        public List<Tank> Tanks { get; set; }

        /// <summary>
        /// Gets or sets network tanks.
        /// </summary>
        public List<EnemyNetworkPackage> NetworkTanks { get; set; }

        /// <summary>
        /// Gets or sets team0 base percent.
        /// </summary>
        public int Team0Base { get; set; }

        /// <summary>
        /// Gets or sets team1 base percent.
        /// </summary>
        public int Team1Base { get; set; }

        /// <summary>
        /// Gets or sets session Time.
        /// </summary>
        public short SessionTime { get; set; }

        /// <summary>
        /// Create byte array from class properties.
        /// </summary>
        /// <returns>byte array.</returns>
        public byte[] SetServerGamePacketPayload()
        {
            byte[] payloadSend = new byte[sizeof(byte) + (this.Tanks.Count * 21) + (this.NetworkTanks.Count * 8) + sizeof(int) + sizeof(int) + sizeof(short)];
            int bytecounter = 0;
            BitConverter.GetBytes((byte)this.Tanks.Count).CopyTo(payloadSend, bytecounter);
            bytecounter++;
            foreach (var item in this.Tanks)
            {
                CreateByteFromList(this.Tanks).CopyTo(payloadSend, bytecounter);
            }

            byte[] testbyte = Extension.SubArray(payloadSend, 1, 21);
            Tank t = Desserialize(testbyte);
            bytecounter += this.Tanks.Count * 21;
            foreach (var item in this.NetworkTanks)
            {
                CreateByteFromNetworkTankList(this.NetworkTanks).CopyTo(payloadSend, bytecounter);
            }

            bytecounter += this.NetworkTanks.Count * 8;
            BitConverter.GetBytes(this.Team0Base).CopyTo(payloadSend, bytecounter);
            bytecounter += sizeof(int);
            BitConverter.GetBytes(this.Team1Base).CopyTo(payloadSend, bytecounter);
            bytecounter += sizeof(int);
            BitConverter.GetBytes(this.SessionTime).CopyTo(payloadSend, bytecounter);
            this.Payload = payloadSend;
            return payloadSend;
        }

        /// <summary>
        /// Reads Payload and convert byte subarrays to properties.
        /// </summary>
        /// <param name="numOfTanks">number of tanks.</param>
        public void ReadServerGamePacketPayload(int numOfTanks)
        {
            byte[] subarr = Extension.SubArray<byte>(this.Payload, 0, numOfTanks * 21);
            List<Tank> tanklist = CreateListFromByte(subarr, 21);
            this.Tanks = tanklist;
            byte[] subarrNetw = Extension.SubArray<byte>(this.Payload, numOfTanks * 21, numOfTanks * 8);
            List<EnemyNetworkPackage> enemytank = CreateListFromBytetwork(subarrNetw);
            this.NetworkTanks = enemytank;
            this.Team0Base = BitConverter.ToInt32(Extension.SubArray<byte>(this.Payload, (numOfTanks * 21) + (numOfTanks * 8), 4));
            this.Team1Base = BitConverter.ToInt32(Extension.SubArray<byte>(this.Payload, (numOfTanks * 21) + (numOfTanks * 8) + 4, 4));
            this.SessionTime = BitConverter.ToInt16(Extension.SubArray<byte>(this.Payload, (numOfTanks * 21) + (numOfTanks * 8) + 8, 2));
        }
    }
}
