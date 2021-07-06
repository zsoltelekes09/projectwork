// <copyright file="UIMap.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    using System.Collections.Generic;
    using TankPocalypse.Logic.Interfaces;

    /// <summary>
    /// UIMAP structure for map sending.
    /// </summary>
    public class UIMap : IUIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIMap"/> class.
        /// Sets all properties of the UIMap.
        /// </summary>
        /// <param name="name">name.</param>
        /// <param name="mapData">mapContent.</param>
        public UIMap(string name, List<string> mapData)
        {
            this.MapName = name;
            this.MapData = mapData;
        }

        /// <inheritdoc/>
        public string MapName { get; set; }

        /// <inheritdoc/>
        public List<string> MapData { get; set; }
    }
}
