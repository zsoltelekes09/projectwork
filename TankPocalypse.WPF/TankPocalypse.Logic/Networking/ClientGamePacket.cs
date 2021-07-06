// <copyright file="ClientGamePacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Networking
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Client game packet class.
    /// </summary>
    public class ClientGamePacket : Packet
        {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientGamePacket"/> class.
        /// Sets properties to be ready for network sending.
        /// </summary>
        /// <param name="tanks">list of tanks.</param>
        /// <param name="networkTanks">number of tanks.</param>
        public ClientGamePacket(List<Tank> tanks, List<EnemyNetworkPackage> networkTanks)
            : base(PacketType.ClientPackage)
        {
            this.Tanks = tanks;
            this.NetworkTanks = networkTanks;
            this.Payload = this.SetClientGamePacketPayload();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientGamePacket"/> class.
        /// Empty constructor.
        /// </summary>
        public ClientGamePacket()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientGamePacket"/> class.
        /// </summary>
        /// <param name="bytes">bytes.</param>
        /// <param name="numofTank">number of tanks.</param>
        public ClientGamePacket(byte[] bytes, int numofTank)
            : base(PacketType.ClientPackage)
        {
            int i = 0;

            this.Payload = bytes.Skip(i).ToArray();
            this.ReadClientGamePacketPayload(numofTank);
        }

        /// <summary>
        /// Gets or sets list of tanks.
        /// </summary>
        public List<Tank> Tanks { get; set; }

        /// <summary>
        /// Gets or sets list of network tanks.
        /// </summary>
        public List<EnemyNetworkPackage> NetworkTanks { get; set; }

        /// <summary>
        /// Sets payload from clientgamepacket properties.
        /// </summary>
        /// <returns>byte array.</returns>
        public byte[] SetClientGamePacketPayload()
        {
            byte[] payloadSend = new byte[(this.Tanks.Count * 21) + (this.NetworkTanks.Count * 10)];
            int bytecounter = 0;
            foreach (var item in this.Tanks)
            {
                CreateByteFromList(this.Tanks).CopyTo(payloadSend, 0);
            }

            bytecounter += this.Tanks.Count * 21;
            foreach (var item in this.NetworkTanks)
            {
                CreateByteFromNetworkTankList(this.NetworkTanks).CopyTo(payloadSend, bytecounter);
            }

            this.Payload = payloadSend;
            return payloadSend;
        }

        /// <summary>
        /// Sets properties by reading payload.
        /// </summary>
        /// <param name="numOfTanks">number of tanks.</param>
        public void ReadClientGamePacketPayload(int numOfTanks)
        {
            byte[] subarr = Extension.SubArray<byte>(this.Payload, 0, numOfTanks * 21);
            List<Tank> tanklist = CreateListFromByte(subarr, 21);
            this.Tanks = tanklist;
            byte[] subarrNetw = Extension.SubArray<byte>(this.Payload, numOfTanks * 21, numOfTanks * 8);
            List<EnemyNetworkPackage> enemytank = CreateListFromBytetwork(subarrNetw);
            this.NetworkTanks = enemytank;
        }
    }
}
