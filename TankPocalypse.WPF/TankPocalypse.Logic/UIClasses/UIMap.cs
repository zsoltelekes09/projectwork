// <copyright file="UIMap.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.UIClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TankPocalypse.Logic.Interfaces;

    /// <summary>
    /// UIMap class. Represents the Map class for the ui layer.
    /// </summary>
    public class UIMap : IUIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIMap"/> class.
        /// </summary>
        /// <param name="name">Name of the map.</param>
        /// <param name="mapData">Map data.</param>
        public UIMap(string name, List<string> mapData)
        {
            this.MapName = name;
            this.MapData = mapData;
        }

        /// <inheritdoc/>
        public string MapName { get; private set; }

        /// <inheritdoc/>
        public List<string> MapData { get; private set; }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is UIMap uim)
            {
                if (uim.MapName == this.MapName && uim.MapData.SequenceEqual(this.MapData))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
