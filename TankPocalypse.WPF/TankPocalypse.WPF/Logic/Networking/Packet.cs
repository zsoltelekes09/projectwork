// <copyright file="Packet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;

    /// <summary>
    /// Basic Packet structure.
    /// </summary>
    public class Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// Empty packet with only its type.
        /// </summary>
        /// <param name="type">type.</param>
        public Packet(PacketType type)
        {
            this.Type = type;
            this.Payload = Array.Empty<byte>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// </summary>
        public Packet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// Convert byte to Entity properties.
        /// </summary>
        /// <param name="bytes">bytes.</param>
        public Packet(byte[] bytes)
        {
            int i = 0;
            this.Type = (PacketType)BitConverter.ToUInt32(bytes, 0);
            i += sizeof(PacketType);
            this.Payloadlength = BitConverter.ToInt32(bytes, i);
            i += sizeof(int);
            this.Payload = bytes.Skip(i).ToArray();
        }

        /// <summary>
        /// Gets or sets the type of a packet.
        /// </summary>
        public PacketType Type { get; set; }

        /// <summary>
        /// Gets or sets of sets the Payload of the packet.
        /// </summary>
        public byte[] Payload { get; set; }

        /// <summary>
        /// Gets or sets payloadlegth of the packet.
        /// </summary>
        public int Payloadlength { get; set; }

        /// <summary>
        /// Serialize Tank to byte.
        /// </summary>
        /// <param name="t">Tank.</param>
        /// <returns>byte.</returns>
        public static byte[] Serialize(Tank t)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(t.Id);
                    writer.Write(t.Position.X);
                    writer.Write(t.Position.Y);
                    writer.Write(t.TurretIdx);
                    writer.Write(t.BodyIdx);
                }

                return m.ToArray();
            }
        }

        /// <summary>
        /// Serialize Network tank.
        /// </summary>
        /// <param name="t">EnemyNetworkPackage.</param>
        /// <returns>byte array.</returns>
        public static byte[] SerializeNetworkTank(EnemyNetworkPackage t)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(t.ID);
                    writer.Write(t.HP);
                }

                return m.ToArray();
            }
        }

        /// <summary>
        /// Desserialize Tank.
        /// </summary>
        /// <param name="data">byte[].</param>
        /// <returns>Tank.</returns>
        public static Tank Desserialize(byte[] data)
        {
            Tank result = new Tank();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    result.Id = reader.ReadInt32();
                    result.Position = new VectorExtend(reader.ReadSingle(), reader.ReadSingle());
                    result.TurretIdx = reader.ReadInt32();
                    result.BodyIdx = reader.ReadInt32();
                }
            }

            return result;
        }

        /// <summary>
        /// Create Byte from network tanks list.
        /// </summary>
        /// <param name="tanks">tanks.</param>
        /// <returns>byte array.</returns>
        public static byte[] CreateByteFromNetworkTankList(List<EnemyNetworkPackage> tanks)
        {
            byte[] tankbytes = new byte[tanks.Count * 10];
            int counter = 0;
            foreach (var item in tanks)
            {
                foreach (var juke in SerializeNetworkTank(item))
                {
                    tankbytes[counter] = juke;
                    counter++;
                }
            }

            return tankbytes;
        }

        /// <summary>
        /// Create list from byte array.
        /// </summary>
        /// <param name="byteData">bytearray.</param>
        /// <param name="numberofBytesPerEntity">number of bytes.</param>
        /// <returns>List of tanks.</returns>
        public static List<Tank> CreateListFromByte(byte[] byteData, int numberofBytesPerEntity)
        {
            List<Tank> newList = new List<Tank>();
            for (int i = 0; i < byteData.Length - 20; i += 20)
            {
                byte[] tempArray = Extension.SubArray<byte>(byteData, i, numberofBytesPerEntity);
                newList.Add(Desserialize(tempArray));
            }

            return newList;
        }

        /// <summary>
        /// Create byte Array from properties.
        /// </summary>
        /// <returns>byte array.</returns>
        public byte[] GetBytes()
        {
            int ptSize = sizeof(PacketType);

            // Join the Packet data
            int i = 0;
            int payloadlength;
            byte[] bytes = new byte[ptSize + sizeof(int) + this.Payload.Length];

            BitConverter.GetBytes((uint)this.Type).CopyTo(bytes, i);
            i += ptSize;
            i += sizeof(int);
            this.Payload.CopyTo(bytes, i);
            i += this.Payload.Length;
            payloadlength = this.Payload.Length;
            BitConverter.GetBytes(payloadlength).CopyTo(bytes, ptSize);
            return bytes;
        }

        /// <summary>
        /// Write data to networkstream.
        /// </summary>
        /// <param name="stream">networkstream.</param>
        public void Send(NetworkStream stream)
        {
            byte[] bytes = this.GetBytes();

            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Creates byte from list.
        /// </summary>
        /// <param name="tanks">tanks.</param>
        /// <param name="numberofTanks">numberoftanks.</param>
        /// <returns>byte array.</returns>
        public byte[] CreateByteFromList(List<Tank> tanks, int numberofTanks)
        {
            byte[] tankbytes = new byte[numberofTanks * 20];
            int counter = sizeof(PacketType);
            BitConverter.GetBytes((uint)this.Type).CopyTo(tankbytes, 0);
            foreach (var item in tanks)
            {
                foreach (var juke in Serialize(item))
                {
                    tankbytes[counter] = juke;
                    counter++;
                }
            }

            return tankbytes;
        }
    }
}
