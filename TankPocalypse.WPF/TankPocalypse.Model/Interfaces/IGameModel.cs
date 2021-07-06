// <copyright file="IGameModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Windows;
    using TankPocalypse.Model.World;

    /// <summary>
    /// GameModel interface.
    /// </summary>
    public interface IGameModel
    {
        /// <summary>
        /// Unit created event fires when a unit has been created.
        /// </summary>
        public event Action<IVehicle> UnitCreated;

        /// <summary>
        /// Gets the team identifier.
        /// </summary>
        public byte TeamID { get; }

        /// <summary>
        /// Gets or sets the onscreen objects list.
        /// </summary>
        public List<IReorderable> OnScreenObjects { get; set; }

        /// <summary>
        /// Gets or sets the onscreen vehicles.
        /// </summary>
        public List<IReorderable> OnScreenVehicles { get; set; }

        /// <summary>
        /// Gets or sets the onscreen clickable objects.
        /// </summary>
        public List<IClickableObject> OnScreenClickableObjects { get; set; }

        /// <summary>
        /// Gets the vehicles list.
        /// </summary>
        public List<IVehicle> Vehicles { get; }

        /// <summary>
        /// Gets or sets the onscreen clickable objects from team0.
        /// </summary>
        public List<IReorderable> OnScreenClickableObjectsTeam0 { get; set; }

        /// <summary>
        /// Gets or sets the onscreen clickable objects from team1.
        /// </summary>
        public List<IReorderable> OnScreenClickableObjectsTeam1 { get; set; }

        /// <summary>
        /// Gets or sets the team vehicles list.
        /// </summary>
        public List<IVehicle> TeamVehicles { get; set; }

        /// <summary>
        /// Gets or sets the enemyvehciles list.
        /// </summary>
        public List<IVehicle> EnemyVehicles { get; set; }

        /// <summary>
        /// Gets or sets the team0 vehicles list.
        /// </summary>
        public List<IVehicle> VehiclesTeam0 { get; set; }

        /// <summary>
        /// Gets or sets the team1 vehicles list.
        /// </summary>
        public List<IVehicle> VehiclesTeam1 { get; set; }

        /// <summary>
        /// Gets or sets the revealed nodes list.
        /// </summary>
        public List<Node> RevealedNodes { get; set; }

        /// <summary>
        /// Gets or sets the base capture percent of team 0.
        /// </summary>
        public byte Team0BasePercent { get; set; }

        /// <summary>
        /// Gets or sets the base capture percent of team 1.
        /// </summary>
        public byte Team1BasePercent { get; set; }

        /// <summary>
        /// Gets or sets the initial unit count of team 0.
        /// </summary>
        public int Team0InitCount { get; set; }

        /// <summary>
        /// Gets or sets the initial unit count of team 1.
        /// </summary>
        public int Team1InitCount { get; set; }

        /// <summary>
        /// Gets or sets the outcome.
        /// </summary>
        public byte OutcomeCase { get; set; }

        /// <summary>
        /// Gets the team vehicle init count.
        /// </summary>
        public int SelfVehicleInitCount { get; }

        /// <summary>
        /// Gets the enemy vehicle init count.
        /// </summary>
        public int EnemyVehicleInitCount { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the game is paused or not.
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// Gets or sets the session time.
        /// </summary>
        public int SessionTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game is orver or not.
        /// </summary>
        public bool GameOver { get; set; }

        /// <summary>
        /// Gets or sets the screen offset vector.
        /// </summary>
        public Vector2 ScreenOffset { get; set; }

/// <summary>
        /// Gets or sets a value indicating whether application is in server mode or not.
        /// </summary>
        public bool IsServer { get; set; }

        /// <summary>
        /// Gets or sets a ping value.
        /// </summary>
        public long Ping { get; set; }

        /// <summary>
        /// Gets or sets the start position of screen selection.
        /// </summary>
        public Point SelectStart { get; set; }

        /// <summary>
        /// Gets or sets the end position of screen selection.
        /// </summary>
        public Point SelectEnd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multiselection started or not.
        /// </summary>
        public bool Multiselect { get; set; }

        /// <summary>
        /// Gets the team id.
        /// </summary>
        /// <param name="id">Team id.</param>
        public void SetTeamID(byte id);

        /// <summary>
        /// This method creates the given count of vehicles on the map.
        /// </summary>
        /// <param name="count">Vehicle count to create. Only even values should be used.</param>
        public void CreateVehicles(byte count);

        /// <summary>
        /// Creates a unit.
        /// </summary>
        /// <param name="uniqueId">Units id.</param>
        /// <param name="locOnMap">Units location on map.</param>
        /// <param name="radius">Units size radius.</param>
        /// <param name="team">Units team id.</param>
        /// <returns>A new IVehicle object.</returns>
        public IVehicle CreateUnit(byte uniqueId, Vector2 locOnMap, float radius, byte team);
    }
}
