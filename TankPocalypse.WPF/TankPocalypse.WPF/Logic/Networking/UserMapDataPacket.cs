// <copyright file="UserMapDataPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Newtonsoft.Json;
    using TankPocalypse.Logic.Interfaces;

    /// <summary>
    /// Username and MapDAta packet class.
    /// </summary>
    [DataContract]
    public class UserMapDataPacket : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapDataPacket"/> class.
        /// Set up Properties with default values and passed values.
        /// </summary>
        /// <param name="username">username.</param>
        /// <param name="map">map.</param>
        public UserMapDataPacket(string username, IUIMap map)
            : base(PacketType.UsernameMapData)
        {
            this.UserName = username;
            this.MapInfo = map;
            this.Payload = Serializer(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapDataPacket"/> class.
        /// Empty Constructor.
        /// </summary>
        [JsonConstructor]
        public UserMapDataPacket()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapDataPacket"/> class.
        /// Convert bytes to UserMapDataPacket properties.
        /// </summary>
        /// <param name="bytes">byte array.</param>
        public UserMapDataPacket(byte[] bytes)
            : base(PacketType.UsernameMapData)
        {
            this.Payload = bytes;
            var s = UserMapDataPacket.Deserializer(bytes);
            this.MapInfo = s.MapInfo;
            this.UserName = s.UserName;
        }

        /// <summary>
        /// Gets or sets username.
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets Mapinfo.
        /// </summary>
        [DataMember]
        public IUIMap MapInfo { get; set; }

        /// <summary>
        /// Deserializing byte array to UserMapDataPacket.
        /// </summary>
        /// <param name="bytes">bytes.</param>
        /// <returns>UserMapDataPacket.</returns>
        public static UserMapDataPacket Deserializer(byte[] bytes)
        {
            string stringData = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            UserMapDataPacket ret;
            try
            {
                ret = JsonConvert.DeserializeObject<UserMapDataPacket>(stringData, settings);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// Serializing UserMapDataPacket to byte array.
        /// </summary>
        /// <param name="userpacket">userpacket.</param>
        /// <returns>byte array.</returns>
        public static byte[] Serializer(UserMapDataPacket userpacket)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            byte[] val = Encoding.Default.GetBytes(JsonConvert.SerializeObject(userpacket, settings));

            return val;
        }
    }
}
