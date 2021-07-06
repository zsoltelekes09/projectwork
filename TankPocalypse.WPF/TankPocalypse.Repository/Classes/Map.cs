// <copyright file="Map.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Repository.Classes
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using TankPocalypse.Repository.Interfaces;

    /// <summary>
    /// THis is the Map class, which represents the stored map data.
    /// </summary>
    public class Map : IMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="name">Map name.</param>
        /// <param name="mapData">Map data.</param>
        public Map(string name, List<string> mapData)
        {
            this.MapName = name;
            this.MapData = mapData;
        }

        /// <summary>
        /// Gets the name of the map.
        /// </summary>
        public string MapName { get; private set; }

        /// <summary>
        /// Gets the data of the map.
        /// </summary>
        public List<string> MapData { get; private set; }

        /// <summary>
        /// This static method creates a new Map entity from the given xml document.
        /// </summary>
        /// <param name="xMap">Xml data.</param>
        /// <returns>New IMap entity.</returns>
        public static IMap CreateMapFromXml(XDocument xMap)
        {
            string name = xMap?.Root?.Element("Name").Value;
            List<string> data = new List<string>();
            data.Add(xMap.Root?.Element("Data").Attribute("Height").Value);
            data.Add(xMap.Root?.Element("Data").Attribute("Width").Value);
            foreach (XElement row in xMap.Root?.Descendants("Row"))
            {
                data.Add(row.Value);
            }

            IMap newMap = new Map(name, data);
            return newMap;
        }
    }
}
