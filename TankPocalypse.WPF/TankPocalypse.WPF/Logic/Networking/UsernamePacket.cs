// <copyright file="UsernamePacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Username packet class.
    /// </summary>
    [DataContract]
    public class UsernamePacket : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsernamePacket"/> class.
        /// </summary>
        /// <param name="username">username.</param>
        public UsernamePacket(string username)
            : base(PacketType.Username)
        {
            this.UserName = username;
            this.Payload = Serializer(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsernamePacket"/> class.
        /// Empty constructor.
        /// </summary>
        [JsonConstructor]
        public UsernamePacket()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsernamePacket"/> class.
        /// Convert bytes to UsernamePacket properties.
        /// </summary>
        /// <param name="bytes">byte array.</param>
        public UsernamePacket(byte[] bytes)
            : base(PacketType.Username)
        {
            this.Payload = bytes;
            var s = UsernamePacket.Deserializer(bytes);
            this.UserName = s.UserName;
        }

        /// <summary>
        /// Gets or sets username.
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Serializing UsernamePacket to byte array.
        /// </summary>
        /// <param name="userpacket">userpacket.</param>
        /// <returns>byte array.</returns>
        public static byte[] Serializer(UsernamePacket userpacket)
        {
            byte[] val = Encoding.Default.GetBytes(JsonConvert.SerializeObject(userpacket));
            return val;
        }

        /// <summary>
        /// Deserializing byte array to UsernamePacket.
        /// </summary>
        /// <param name="bytes">byte.</param>
        /// <returns>UsernamePacket.</returns>
        public static UsernamePacket Deserializer(byte[] bytes)
        {
            string stringData = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);

            var ret = JsonConvert.DeserializeObject<UsernamePacket>(stringData);
            return ret;
        }
    }
}
