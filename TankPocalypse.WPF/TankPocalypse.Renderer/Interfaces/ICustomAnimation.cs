// <copyright file="ICustomAnimation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Renderer.Interfaces
{
    using System.Windows.Media;
    using TankPocalypse.Model.Interfaces;

    /// <summary>
    /// Custom animation interface for animations.
    /// </summary>
    public interface ICustomAnimation
    {
        /// <summary>
        /// Gets a value indicating whether animation is finished or not.
        /// </summary>
        public bool IsFinished { get; }

        /// <summary>
        /// Increases animation tick.
        /// </summary>
        public void IncreaseTick();

        /// <summary>
        /// Creates a drawing from the animations actual state.
        /// </summary>
        /// <param name="dg">The animation adds its image to this drawing group.</param>
        /// <param name="imageController">The animation gets its images from the ImageController.</param>
        public void DrawAnimation(ref DrawingGroup dg, ref IGameImageController imageController);
    }
}
