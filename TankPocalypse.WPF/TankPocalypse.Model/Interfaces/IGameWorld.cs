// <copyright file="IGameWorld.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.Interfaces
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Numerics;
    using TankPocalypse.Model.World;

    /// <summary>
    /// Gameworld interface.
    /// </summary>
    public interface IGameWorld
    {
        /// <summary>
        /// Gets the Grid collection.
        /// </summary>
        public Node[,] WorldGrid { get; }

        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int GridWidth { get; }

        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int GridHeight { get; }

        /// <summary>
        /// Returns the grid height and width.
        /// </summary>
        /// <returns>New point, which represents the height and width of the grid.</returns>
        public Point GetWorldDimensions();

        /// <summary>
        /// Returns a node from the grid by a world location parameter.
        /// </summary>
        /// <param name="worldLoc">Location to search for...</param>
        /// <returns>Node from the grid.</returns>
        public Node GetNodeByWorldCoordinates(Vector2 worldLoc);

        /// <summary>
        /// Initialises the game world.
        /// </summary>
        /// <param name="mapData">Map data from file.</param>
        public void SetupGrid(List<string> mapData);
    }
}
