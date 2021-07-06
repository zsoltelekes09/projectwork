// <copyright file="IUIProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Interfaces
{
    /// <summary>
    /// UIProfile interface.
    /// </summary>
    public interface IUIProfile
    {
        /// <summary>
        /// Gets the killed unit count.
        /// </summary>
        public int UnitsKilled { get; }

        /// <summary>
        /// Gets the lost unit count.
        /// </summary>
        public int UnitsLost { get; }

        /// <summary>
        /// Gets the win count.
        /// </summary>
        public int GamesWon { get; }

        /// <summary>
        /// Gets the lose count.
        /// </summary>
        public int GamesLost { get; }

        /// <summary>
        /// Gets the profile name.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets the UserId.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets the profiel scores.
        /// </summary>
        public int Scores { get; }
    }
}
