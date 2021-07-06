// <copyright file="IUISavedGame.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Interfaces
{
    using System.Xml.Linq;

    /// <summary>
    /// UISavedGame interface.
    /// </summary>
    public interface IUISavedGame
    {
        /// <summary>
        /// Gets the save id.
        /// </summary>
        public byte SaveId { get; }

        /// <summary>
        /// Gets the save name.
        /// </summary>
        public string SaveName { get; }

        /// <summary>
        /// Gets the save data.
        /// </summary>
        public XDocument SaveFileXDoc { get; }
    }
}
