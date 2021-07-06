// <copyright file="GameModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

[assembly: System.CLSCompliant(false)]

namespace TankPocalypse.Model
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Windows;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Model.Units;
    using TankPocalypse.Model.World;

    /// <summary>
    /// This is the game model class. It handels all the necessary data of the gamestate.
    /// </summary>
    public class GameModel : IGameModel
    {
        private IGameWorld gameWorld;
        private IFlowField flowField;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameModel"/> class.
        /// </summary>
        /// <param name="gw">GameWorld entity.</param>
        /// <param name="flowField">FlowField entity.</param>
        public GameModel(IGameWorld gw, IFlowField flowField)
        {
            this.Paused = true;
            this.gameWorld = gw;
            this.flowField = flowField;
        }

        /// <summary>
        /// Unit created event fires when a unit has been created.
        /// </summary>
        public event Action<IVehicle> UnitCreated;

        /// <summary>
        /// Gets the team identifier.
        /// </summary>
        public byte TeamID { get; private set; }

        /// <summary>
        /// Gets the vehicles list.
        /// </summary>
        public List<IVehicle> Vehicles { get; private set; } = new List<IVehicle>();

        /// <summary>
        /// Gets or sets the onscreen objects list.
        /// </summary>
        public List<IReorderable> OnScreenObjects { get; set; }

        /// <summary>
        /// Gets or sets the onscreen vehicles.
        /// </summary>
        public List<IReorderable> OnScreenVehicles { get; set; }

        /// <summary>
        /// Gets or sets the onscreen clickable objects from team0.
        /// </summary>
        public List<IReorderable> OnScreenClickableObjectsTeam0 { get; set; } = new List<IReorderable>();

        /// <summary>
        /// Gets or sets the onscreen clickable objects from team1.
        /// </summary>
        public List<IReorderable> OnScreenClickableObjectsTeam1 { get; set; } = new List<IReorderable>();

        /// <summary>
        /// Gets or sets the team0 vehicles list.
        /// </summary>
        public List<IVehicle> VehiclesTeam0 { get; set; } = new List<IVehicle>();

        /// <summary>
        /// Gets or sets the team1 vehicles list.
        /// </summary>
        public List<IVehicle> VehiclesTeam1 { get; set; } = new List<IVehicle>();

        /// <summary>
        /// Gets or sets the revealed nodes list.
        /// </summary>
        public List<Node> RevealedNodes { get; set; } = new List<Node>();

        /// <summary>
        /// Gets or sets the onscreen clickable objects.
        /// </summary>
        public List<IClickableObject> OnScreenClickableObjects { get; set; } = new List<IClickableObject>();

        /// <summary>
        /// Gets or sets the team vehicles list.
        /// </summary>
        public List<IVehicle> TeamVehicles { get; set; } = new List<IVehicle>();

        /// <summary>
        /// Gets or sets the enemyvehciles list.
        /// </summary>
        public List<IVehicle> EnemyVehicles { get; set; } = new List<IVehicle>();

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
        /// Gets the team vehicle init count.
        /// </summary>
        public int SelfVehicleInitCount
        {
            get { return this.TeamID == 0 ? this.Team0InitCount : this.Team1InitCount; }
        }

        /// <summary>
        /// Gets the enemy vehicle init count.
        /// </summary>
        public int EnemyVehicleInitCount
        {
            get { return this.TeamID == 0 ? this.Team1InitCount : this.Team0InitCount; }
        }

        /// <summary>
        /// Gets or sets the outcome.
        /// </summary>
        public byte OutcomeCase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game is paused or not.
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// Gets or sets the session time.
        /// </summary>
        public int SessionTime { get; set; } = 180;

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
        public void SetTeamID(byte id)
        {
            this.TeamID = id;

            if (id == 0)
            {
                this.TeamVehicles = this.VehiclesTeam0;
                this.EnemyVehicles = this.VehiclesTeam1;
            }
            else
            {
                this.TeamVehicles = this.VehiclesTeam1;
                this.EnemyVehicles = this.VehiclesTeam0;
            }
        }

        /// <summary>
        /// Creates a unit.
        /// </summary>
        /// <param name="uniqueId">Units id.</param>
        /// <param name="locOnMap">Units location on map.</param>
        /// <param name="radius">Units size radius.</param>
        /// <param name="team">Units team id.</param>
        /// <returns>A new IVehicle object.</returns>
        public IVehicle CreateUnit(byte uniqueId, Vector2 locOnMap, float radius, byte team)
        {
            IVehicle v = new Vehicle(uniqueId, locOnMap, radius, this.gameWorld, this.flowField, this.Vehicles, team);
            if (team == 0)
            {
                this.VehiclesTeam0.Add(v);
            }
            else
            {
                this.VehiclesTeam1.Add(v);
            }

            v.UnitKilled += this.RemoveDeadUnitFromList;
            this.Vehicles.Add(v);

            this.UnitCreated?.Invoke(v);

            return v;
        }

        /// <summary>
        /// This method creates the given count of vehicles on the map.
        /// </summary>
        /// <param name="count">Vehicle count to create. Only even values should be used.</param>
        public void CreateVehicles(byte count)
        {
            int team0Count = count / 2;
            int team1Count = team0Count;

            this.Team0InitCount = team0Count;
            this.Team1InitCount = team1Count;

            byte uniqueCounter = 0;
            int x = 0;
            int y = 0;

            // Place Team0 units;
            while (team0Count != 0 && y < this.gameWorld.GridHeight)
            {
                if (this.gameWorld.WorldGrid[y, x].NodeType == 8)
                {
                    var centerLoc = this.gameWorld.WorldGrid[y, x].WorldPosition;
                    this.CreateUnit(uniqueCounter++, centerLoc, 13, 0);
                    team0Count--;
                }

                x++;
                if (x >= this.gameWorld.GridWidth)
                {
                    x = 0;
                    y++;
                }
            }

            x = this.gameWorld.GridWidth - 1;
            y = this.gameWorld.GridHeight - 1;

            // Place Team1 units;
            while (team1Count != 0 && y >= 0)
            {
                if (this.gameWorld.WorldGrid[y, x].NodeType == 9)
                {
                    var centerLoc = this.gameWorld.WorldGrid[y, x].WorldPosition;
                    this.CreateUnit(uniqueCounter++, centerLoc, 13, 1);
                    team1Count--;
                }

                x--;
                if (x < 0)
                {
                    x = this.gameWorld.GridWidth - 1;
                    y--;
                }
            }
        }

        private void RemoveDeadUnitFromList(IVehicle vehicle)
        {
            if (vehicle.TeamId == 0 && this.VehiclesTeam0.Contains(vehicle))
            {
                this.VehiclesTeam0.Remove(vehicle);
            }
            else if (this.VehiclesTeam1.Contains(vehicle))
            {
                this.VehiclesTeam1.Remove(vehicle);
            }
        }
    }
}
