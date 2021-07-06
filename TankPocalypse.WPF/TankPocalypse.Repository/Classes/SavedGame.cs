// <copyright file="SavedGame.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Repository.Classes
{
    using System.Xml.Linq;
    using TankPocalypse.Repository.Interfaces;

    /// <summary>
    /// Savegame class. Stores every data necessary to load a saved game.
    /// </summary>
    public class SavedGame : ISavedGame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SavedGame"/> class.
        /// </summary>
        public SavedGame()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SavedGame"/> class.
        /// </summary>
        /// <param name="saveId">Save game id.</param>
        /// <param name="saveDescription">Description of save.</param>
        /// <param name="saveXDoc">Save data xml.</param>
        public SavedGame(byte saveId, string saveDescription, XDocument saveXDoc)
        {
            this.SaveId = saveId;
            this.SaveDescription = saveDescription;
            this.SaveXDoc = saveXDoc;
        }

        /// <summary>
        /// Gets the index of saved game.
        /// </summary>
        public byte SaveId { get; private init; }

        /// <summary>
        /// Gets the description of saved game.
        /// </summary>
        public string SaveDescription { get; private init; }

        /// <summary>
        /// Gets the save data.
        /// </summary>
        public XDocument SaveXDoc { get; private init; }

        /// <summary>
        /// Static method. Creates a SavedGame entity from the given xml data.
        /// </summary>
        /// <param name="savedGameXml">Xml data.</param>
        /// <returns>SavedGame entity.</returns>
        public static SavedGame CreateSavedGameFromXML(XDocument savedGameXml)
        {
            var savedGame = new SavedGame()
            {
                SaveId = 0,
                SaveDescription = savedGameXml?.Root.Element("Description")?.Value,
                SaveXDoc = savedGameXml,
            };

            return savedGame;
        }
    }
}
