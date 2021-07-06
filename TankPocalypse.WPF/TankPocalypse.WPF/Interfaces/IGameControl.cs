// <copyright file="IGameControl.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using TankPocalypse.Logic.Interfaces;

    /// <summary>
    /// GameControl interface.
    /// </summary>
    public interface IGameControl
    {
        /// <summary>
        /// Pause event.
        /// </summary>
        public event Action PauseEvent;

        /// <summary>
        /// Game has ended event. Used for signalling to the connected client.
        /// </summary>
        public event Action GameHasEndedNet;

        /// <summary>
        /// Gets or sets a value indicating whether game has ended or not.
        /// </summary>
        public bool IsGameEnded { get; set; }

        /// <summary>
        /// Gets a value indicating whether the connection is still up or not.
        /// </summary>
        public bool ConnectionLost { get; }

        /// <summary>
        /// Invalidates the visuals and refreshes render.
        /// </summary>
        public void InvalidateVisual();

        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <param name="mapToLoad">Map data.</param>
        /// <param name="teamId">Team id.</param>
        /// <param name="unitCount">Unit count.</param>
        /// <param name="globProfile">Used profile.</param>
        /// <param name="isServer">App is running in server mode or not.</param>
        /// <param name="ip">Connection ip address.</param>
        public void StartNewGame(IUIMap mapToLoad, byte teamId, byte unitCount, IUIProfile globProfile, bool isServer, string ip);

        /// <summary>
        /// Loads a previously saved game.
        /// </summary>
        /// <param name="saveFile">Save data.</param>
        /// <param name="teamId">Team id.</param>
        /// <param name="globProfile">Used profile.</param>
        /// <param name="isServer">App is running in server mode or not.</param>
        /// <param name="ip">Connection ip address.</param>
        public void LoadGame(IUISavedGame saveFile, byte teamId, IUIProfile globProfile, bool isServer, string ip);

        /// <summary>
        /// Sets the team id.
        /// </summary>
        /// <param name="teamID">Team id.</param>
        public void SetTeamID(byte teamID);

        /// <summary>
        /// Sets the game to an ended state.
        /// </summary>
        public void SetEnd();

        /// <summary>
        /// Cleares every dependency connection.
        /// </summary>
        public void DeleteEverything();

        /// <summary>
        /// Pauses the game.
        /// </summary>
        public void PauseGame();

        /// <summary>
        /// Saves the game.
        /// </summary>
        public void SaveGame();

        /// <summary>
        /// Gets all stored profiles.
        /// </summary>
        /// <returns>An observable collection of UIProfiles.</returns>
        public ObservableCollection<IUIProfile> GetProfiles();

        /// <summary>
        /// Deletes the given profile from the repository.
        /// </summary>
        /// <param name="selectedProfile">Profile to be deleted.</param>
        public void DeleteProfile(IUIProfile selectedProfile);

        /// <summary>
        /// Adds a new profile to the repository.
        /// </summary>
        /// <param name="newProfile">New profile to be added.</param>
        public void AddNewProfile(IUIProfile newProfile);

        /// <summary>
        /// Gets all the stored maps from repository.
        /// </summary>
        /// <returns>List of UIMaps.</returns>
        public List<IUIMap> GetAllMaps();

        /// <summary>
        /// Gets all the stored saves from the repository.
        /// </summary>
        /// <returns>List of UISavedGames.</returns>
        public List<IUISavedGame> GetAllSaves();
    }
}
