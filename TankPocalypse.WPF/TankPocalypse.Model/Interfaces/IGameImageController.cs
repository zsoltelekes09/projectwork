// <copyright file="IGameImageController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.Interfaces
{
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Game image controller interface.
    /// </summary>
    public interface IGameImageController
    {
        /// <summary>
        /// Gets the node textures dicitonary.
        /// </summary>
        public Dictionary<int, BitmapImage> NodeTextures { get; }

        /// <summary>
        /// Gets the object textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> ObjectTextures { get; }

        /// <summary>
        /// Gets the debug textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> DebugTextures { get; }

        /// <summary>
        /// Gets the tank team0 body textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> TankBodyTextures { get; }

        /// <summary>
        /// Gets the tank team0 turret textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> TankTurretTextures { get; }

        /// <summary>
        /// Gets the tank team1 body textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> Team1BodyTextures { get; }

        /// <summary>
        /// Gets the tank team1 turret textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> Team1TurretTextures { get; }

        /// <summary>
        /// Gets the shooting animation textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> Shooting { get; }

        /// <summary>
        /// Gets the healthbar textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> HealthBarTextures { get; }

        /// <summary>
        /// Gets the other textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> OtherTextures { get; }

        /// <summary>
        /// Gets the UI textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> UITextures { get; }

        /// <summary>
        /// Gets the pause screen backgorund.
        /// </summary>
        public BitmapImage PauseBackgound { get; }

        /// <summary>
        /// Gets explosion textures.
        /// </summary>
        public Dictionary<int, BitmapImage> ExplosionTextures { get; }

        /// <summary>
        /// Gets flame textures.
        /// </summary>
        public Dictionary<int, BitmapImage> FlameTextures { get; }

        /// <summary>
        /// Clears every texture reference.
        /// </summary>
        public void DeleteEverything();
    }
}
