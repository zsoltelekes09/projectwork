// <copyright file="SaveFilePacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Newtonsoft.Json;
    using TankPocalypse.Logic.Interfaces;

    /// <summary>
    /// SaveFile packet structure.
    /// </summary>
    [DataContract]
    public class SaveFilePacket : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFilePacket"/> class.
        /// Create SaveFilePacket.
        /// </summary>
        /// <param name="sg">sg.</param>
        public SaveFilePacket(IUISavedGame sg)
            : base(PacketType.SaveFile)
        {
            if (sg != null)
            {
                this.SaveId = sg.SaveId;
                this.SaveName = sg.SaveName;
                this.SaveFileXDoc = sg.SaveFileXDoc;
                this.Payload = Serializer(this);
            }
            else
            {
                this.SaveId = 0;
                this.SaveName = "null";
                this.SaveFileXDoc = new XDocument();
                this.Payload = Serializer(this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFilePacket"/> class.
        /// Empty constructor.
        /// </summary>
        [JsonConstructor]
        public SaveFilePacket()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFilePacket"/> class.
        /// Convert byte to SaveFile Props.
        /// </summary>
        /// <param name="bytes">bytes.</param>
        public SaveFilePacket(byte[] bytes)
            : base(PacketType.SaveFile)
        {
            this.Payload = bytes;
            var s = SaveFilePacket.Deserializer(bytes);
            this.SaveId = s.SaveId;
            this.SaveName = s.SaveName;
            this.SaveFileXDoc = s.SaveFileXDoc;
        }

        /// <summary>
        /// Gets or sets saveId.
        /// </summary>
        [DataMember]
        public byte SaveId { get; set; }

        /// <summary>
        /// Gets or sets SaveName.
        /// </summary>
        [DataMember]
        public string SaveName { get; set; }

        /// <summary>
        /// Gets or sets SaveFileXDoc.
        /// </summary>
        [DataMember]
        public XDocument SaveFileXDoc { get; set; }

        /// <summary>
        /// Serializing SaveFilePacket.
        /// </summary>
        /// <param name="savepacket">savepacket.</param>
        /// <returns>byte array.</returns>
        public static byte[] Serializer(SaveFilePacket savepacket)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            byte[] val = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(savepacket, settings));

            return val;
        }

        /// <summary>
        /// Deserializing SaveFilePacket.
        /// </summary>
        /// <param name="bytes">byte array..</param>
        /// <returns>SaveFilePacket.</returns>
        public static SaveFilePacket Deserializer(byte[] bytes)
        {
            var stringData = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var ret = JsonConvert.DeserializeObject<SaveFilePacket>(stringData, settings);
            return ret;
        }
    }
}
