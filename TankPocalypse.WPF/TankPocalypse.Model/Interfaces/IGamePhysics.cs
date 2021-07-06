// <copyright file="IGamePhysics.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.Interfaces
{
    using System.Collections.Generic;
    using TankPocalypse.Model.Physics;
    using TankPocalypse.Model.World;

    /// <summary>
    /// Gamephysics interface.
    /// </summary>
    public interface IGamePhysics
    {
        /// <summary>
        /// Sets up the static world collision walls.
        /// </summary>
        public void SetupWorldBounds();

        /// <summary>
        /// Adds a new collision wall to the physics database.
        /// </summary>
        /// <param name="hl">Collision wall.</param>
        public void AddHitLine(HitLine hl);

        /// <summary>
        /// Creates collision walls arround the node.
        /// </summary>
        /// <param name="node">Unwalkable node.</param>
        public void AddUnwalkableNode(Node node);

        /// <summary>
        /// Calculates physics from the actual state of gamemodel.
        /// </summary>
        public void UpdatePhysics();
    }
}
