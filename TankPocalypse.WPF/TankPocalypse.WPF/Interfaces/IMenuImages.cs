// <copyright file="IMenuImages.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Interfaces
{
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// MenuImages interface.
    /// </summary>
    public interface IMenuImages
    {
        /// <summary>
        /// Gets the background textures.
        /// </summary>
        public Dictionary<string, BitmapImage> Backgrounds { get; }

        /// <summary>
        /// Gets the button textures.
        /// </summary>
        public Dictionary<string, BitmapImage> Buttons { get; }
    }
}
