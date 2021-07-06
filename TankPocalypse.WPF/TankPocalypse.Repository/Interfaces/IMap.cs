// <copyright file="IMap.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Repository.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Map interface.
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Gets the name of the map.
        /// </summary>
        public string MapName { get; }

        /// <summary>
        /// Gets the data of the map.
        /// </summary>
        public List<string> MapData { get; }
    }
}
