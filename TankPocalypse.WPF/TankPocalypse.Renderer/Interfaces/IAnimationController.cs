// <copyright file="IAnimationController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Renderer.Interfaces
{
    using System.Windows.Media;
    using TankPocalypse.Model.Interfaces;

    /// <summary>
    /// Animation controller interface.
    /// </summary>
    public interface IAnimationController
    {
        /// <summary>
        /// Updates the animations when method is called.
        /// </summary>
        /// <param name="dg">Drawing handler for animations to be drawn with.</param>
        public void UpdateAnimations(ref DrawingGroup dg);

        /// <summary>
        /// Creates a new animation on vheicle, depending on the input identifier value.
        /// </summary>
        /// <param name="vehicle">Source vehicle.</param>
        /// <param name="identifier">Type of animation.</param>
        public void AddAnimationFromVehicle(IVehicle vehicle, string identifier);
    }
}
