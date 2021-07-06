// <copyright file="IGameLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// GameLogic interface.
    /// </summary>
    public interface IGameLogic : INetwork
    {
        /// <summary>
        /// Gets a value indicating whether game is paused or not.
        /// </summary>
        public bool IsGamePaused { get; }

        /// <summary>
        /// Gets or sets a value indicating whether connection is lost or not.
        /// </summary>
        public bool ConnectionLost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether game is over or not.
        /// </summary>
        public bool GameOver { get; set; }

        /// <summary>
        /// Stats a newgame.
        /// </summary>
        /// <param name="mapToLoad">Map data.</param>
        /// <param name="unitCount">Unit count.</param>
        public void StartNewGame(IUIMap mapToLoad, byte unitCount);

        /// <summary>
        /// Load the game.
        /// </summary>
        /// <param name="uiSavedGame">Load savedata.</param>
        public void LoadGame(IUISavedGame uiSavedGame);

        /// <summary>
        /// Start the game.
        /// </summary>
        public void StartGame();

        /// <summary>
        /// Sets the application to works as a server.
        /// </summary>
        public void SetAppAsServer();

        /// <summary>
        /// Subscribes to invalidate visuals event.
        /// </summary>
        /// <param name="method">Method to subscribe.</param>
        public void SubscribeInvalidateMethod(Action method);

        /// <summary>
        /// Sets team Id.
        /// </summary>
        /// <param name="id">Input Id.</param>
        public void SetTeamID(byte id);

        /// <summary>
        /// Stops game timer.
        /// </summary>
        public void StopGameTimer();

        /// <summary>
        /// Subscribes to game ended event.
        /// </summary>
        /// <param name="method">Method to subscribe.</param>
        public void SubscribeToGameEndedEvent(Action method);

        /// <summary>
        /// Gets all stored profiles from the repository.
        /// </summary>
        /// <returns>Observable collection of UIProfiles.</returns>
        public ObservableCollection<IUIProfile> GetAllProfiles();

        /// <summary>
        /// Deletes a profile from the repository via id.
        /// </summary>
        /// <param name="id">Profile id.</param>
        public void DeleteProfile(int id);

        /// <summary>
        /// Gets all stored map from the repository.
        /// </summary>
        /// <returns>List of UIMaps.</returns>
        public List<IUIMap> GetAllMaps();

        /// <summary>
        /// Gets all saved games from repository.
        /// </summary>
        /// <returns>List of UISavedGames.</returns>
        public List<IUISavedGame> GetAllSavedGames();

        /// <summary>
        /// Pauses the game.
        /// </summary>
        public void PauseGame();

        /// <summary>
        /// Savegame method.
        /// </summary>
        public void SaveGame();

        /// <summary>
        /// Starts the session timer.
        /// </summary>
        public void StartSessionTimer();

        /// <summary>
        /// Pauses the session timer.
        /// </summary>
        public void PauseSessionTimer();

        /// <summary>
        /// Adds a new profile to the repository.
        /// </summary>
        /// <param name="newProfile">Input UIProfile to add.</param>
        public void AddNewProfile(IUIProfile newProfile);

        /// <summary>
        /// Sets the global profile.
        /// </summary>
        /// <param name="globProfile">Input global profile.</param>
        public void SetGlobalProfile(IUIProfile globProfile);

        /// <summary>
        /// Cleares every connection with dependencies.
        /// </summary>
        public void DeleteEverything();
    }
}
