// <copyright file="ISavedGame.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Repository.Interfaces
{
    using System.Xml.Linq;

    /// <summary>
    /// SavedGame interface.
    /// </summary>
    public interface ISavedGame
    {
        /// <summary>
        /// Gets the saves id.
        /// </summary>
        public byte SaveId { get; }

        /// <summary>
        /// Gets the saves description.
        /// </summary>
        public string SaveDescription { get; }

        /// <summary>
        /// Gets the save data.
        /// </summary>
        public XDocument SaveXDoc { get; }
    }
}
