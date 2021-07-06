// <copyright file="IReorderable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.Interfaces
{
    using System.Numerics;

    /// <summary>
    /// Reorderable interface for objects that are rendered in isometric space.
    /// </summary>
    public interface IReorderable
    {
        /// <summary>
        /// Gets or sets the world position of the object.
        /// </summary>
        public Vector2 WorldPosition { get; set; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        public short GetObjectType { get; }

        /// <summary>
        /// Gets or sets the top left corner of the object through the viewport.
        /// </summary>
        public Vector2 ObjectScreenStartPosition { get; set; }

        /// <summary>
        /// Gets or sets the bottom right corner of the object through the viewport.
        /// </summary>
        public Vector2 ObjectScreenEndPosition { get; set; }
    }
}
