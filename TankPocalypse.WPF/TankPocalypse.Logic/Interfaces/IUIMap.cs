// <copyright file="IUIMap.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// UIMap interface.
    /// </summary>
    public interface IUIMap
    {
        /// <summary>
        /// Gets the name of the map.
        /// </summary>
        public string MapName { get; }

        /// <summary>
        /// Gets the save data.
        /// </summary>
        public List<string> MapData { get; }
    }
}
