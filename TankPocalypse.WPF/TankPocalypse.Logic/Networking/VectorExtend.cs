// <copyright file="VectorExtend.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Networking
{
    using System;

    /// <summary>
    /// Extended Vector2 properties.
    /// </summary>
    [Serializable]
    public class VectorExtend
    {
        private float vx;
        private float vy;

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorExtend"/> class.
        /// Set Values of properties.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        public VectorExtend(float x, float y)
        {
            this.vx = x;
            this.vy = y;
        }

        /// <summary>
        /// Gets or sets x value.
        /// </summary>
        public float X
        {
            get { return this.vx; }
            set { this.vx = value; }
        }

        /// <summary>
        /// Gets or sets y value.
        /// </summary>
        public float Y
        {
            get { return this.vy; }
            set { this.vy = value; }
        }
    }
}
