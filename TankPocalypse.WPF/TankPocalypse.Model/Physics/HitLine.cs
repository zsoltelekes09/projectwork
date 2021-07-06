// <copyright file="HitLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.Physics
{
    using System.Numerics;

    /// <summary>
    /// This is the hitline class, which servers as a collision wall inside the game world.
    /// </summary>
    public class HitLine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HitLine"/> class.
        /// </summary>
        /// <param name="startPos">Start world location.</param>
        /// <param name="endPos">End world location.</param>
        /// <param name="radius">Thickness of wall.</param>
        public HitLine(Vector2 startPos, Vector2 endPos, int radius)
        {
            this.StartPos = startPos;
            this.EndPos = endPos;
            this.Radius = radius;
        }

        /// <summary>
        /// Gets the start world location of hitline.
        /// </summary>
        public Vector2 StartPos { get; }

        /// <summary>
        /// Gets the end world location of hitline.
        /// </summary>
        public Vector2 EndPos { get; }

        /// <summary>
        /// Gets the thickness of the hitline.
        /// </summary>
        public int Radius { get; }

        /// <summary>
        /// Gets the X coordinate of start location.
        /// </summary>
        public float StartX => this.StartPos.X;

        /// <summary>
        /// Gets the Y coordinate of start location.
        /// </summary>
        public float StartY => this.StartPos.Y;

        /// <summary>
        /// Gets the X coordinate of end location.
        /// </summary>
        public float EndX => this.EndPos.X;

        /// <summary>
        /// Gets the Y coordinate of end location.
        /// </summary>
        public float EndY => this.EndPos.Y;
    }
}
