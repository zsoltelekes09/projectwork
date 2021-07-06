// <copyright file="IClickableObject.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.Interfaces
{
    using System.Numerics;

    /// <summary>
    /// This interface forces the implementer object to have clickable properties, which are necessary for mouse interaction.
    /// </summary>
    public interface IClickableObject
    {
        /// <summary>
        /// Gets or sets the top left corner of the object through the viewport.
        /// </summary>
        public Vector2 ObjectScreenStartPosition { get; set; }

        /// <summary>
        /// Gets or sets the bottom right corner of the object through the viewport.
        /// </summary>
        public Vector2 ObjectScreenEndPosition { get; set; }

        /// <summary>
        /// Gets or sets the position on screen.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets the teamID.
        /// </summary>
        public byte TeamId { get; }

        /// <summary>
        /// Sets the object selected.
        /// </summary>
        public void SetSelected();

        /// <summary>
        /// Sets the object deselected.
        /// </summary>
        public void SetDeselected();
    }
}
