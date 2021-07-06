// <copyright file="IGameRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Repository.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// GameRepository interface.
    /// </summary>
    public interface IGameRepository
    {
        /// <summary>
        /// Returns every stored map in a list.
        /// </summary>
        /// <returns>IMap list.</returns>
        public List<IMap> GetAllMaps();

        /// <summary>
        /// Returns every stored profile in a list.
        /// </summary>
        /// <returns>IProfile list.</returns>
        public List<IProfile> GetAllProfiles();

        /// <summary>
        /// Updates the profile statistics.
        /// </summary>
        /// <param name="id">Profile id.</param>
        /// <param name="userName">Profile name.</param>
        /// <param name="didWin">Bool, which represents the outcome of the game.</param>
        /// <param name="killedUnits">Killed unit count.</param>
        /// <param name="lostUnits">Lost unit count.</param>
        public void UpdateProfile(int id, string userName, bool didWin, int killedUnits, int lostUnits);

        /// <summary>
        /// Deletes the profile by id.
        /// </summary>
        /// <param name="id">Profile id.</param>
        public void DeleteProfile(int id);

        /// <summary>
        /// Adds a new profile to the repository.
        /// </summary>
        /// <param name="profil">New profile.</param>
        public void AddProfile(IProfile profil);

        /// <summary>
        /// Loads the given saved game.
        /// </summary>
        /// <param name="savedGame">SavedGame entity.</param>
        public void LoadSaved(ISavedGame savedGame);

        /// <summary>
        /// Saves the current state of the game.
        /// </summary>
        public void SaveGame();

        /// <summary>
        /// Returns every saved game files in a list.
        /// </summary>
        /// <returns>List of ISavedGame entities.</returns>
        public List<ISavedGame> GetSavedGames();
    }
}
