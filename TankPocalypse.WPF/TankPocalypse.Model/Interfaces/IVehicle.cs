// <copyright file="IVehicle.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.Interfaces
{
    using System;
    using System.Drawing;
    using System.Numerics;
    using TankPocalypse.Model.FlowField;

    /// <summary>
    /// Vehicle Behavior enum.
    /// </summary>
    public enum VehicleBehavior
    {
        /// <summary>
        /// Seeking state.
        /// </summary>
        Seeking,

        /// <summary>
        /// Attackign state.
        /// </summary>
        Attacking,

        /// <summary>
        /// Waiting state.
        /// </summary>
        Waiting,

        /// <summary>
        /// Moving state.
        /// </summary>
        Moving,

        /// <summary>
        /// Dead state.
        /// </summary>
        Dead,
    }

    /// <summary>
    /// Vehicle Movement behavior enum.
    /// </summary>
    public enum VehicleMovementBehavior
    {
        /// <summary>
        /// Standing state.
        /// </summary>
        Standing,

        /// <summary>
        /// Moving state.
        /// </summary>
        Moving,

        /// <summary>
        /// Slowing down state.
        /// </summary>
        Slowing,

        /// <summary>
        /// Following state.
        /// </summary>
        Following,
    }

    /// <summary>
    /// Vehicle interface.
    /// </summary>
    public interface IVehicle
    {
        /// <summary>
        /// UnitKilled event. Fires, when the vehicle is killed.
        /// </summary>
        public event Action<IVehicle> UnitKilled;

        /// <summary>
        /// Create an animation event. Fires, when the unit shoots or blows up.
        /// </summary>
        public event Action<IVehicle, string> CreateAnimation;

        /// <summary>
        /// Gets or sets the vehicles behavior.
        /// </summary>
        public VehicleBehavior Behavior { get; set; }

        /// <summary>
        /// Gets the vehicle movement behavior.
        /// </summary>
        public VehicleMovementBehavior MovementBehavior { get; }

        /// <summary>
        /// Gets the X coordinate of position.
        /// </summary>
        public float Px { get; }

        /// <summary>
        /// Gets the Y coordinate of position.
        /// </summary>
        public float Py { get; }

        /// <summary>
        /// Gets the radius of the vehicle.
        /// </summary>
        public float Radius { get; }

        /// <summary>
        /// Gets or sets the vehicles porsition in world.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the top left screen position of the object.
        /// </summary>
        public Vector2 ObjectScreenStartPosition { get; set; }

        /// <summary>
        /// Gets or sets the bottom right screen position of the object.
        /// </summary>
        public Vector2 ObjectScreenEndPosition { get; set; }

        /// <summary>
        /// Gets or sets the vehicles position on the screen.
        /// </summary>
        public Vector2 ScreenPosition { get; set; }

        /// <summary>
        /// Gets the normalised turret direction.
        /// </summary>
        public Vector2 TurretDirectionNormal { get; }

        /// <summary>
        /// Gets or sets the fixed heading normal value.
        /// </summary>
        public Vector2 HeadingNormalFixed { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the vehicle.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse is over the vehicle or not.
        /// </summary>
        public bool IsMouseOver { get; set; }

        /// <summary>
        /// Gets a value indicating whether vehicle is selected or not.
        /// </summary>
        public bool IsSelected { get; }

        /// <summary>
        /// Gets a value indicating whether the vehicle is standing on a revealed node or not.
        /// </summary>
        public bool IsRevealed { get; }

        /// <summary>
        /// Gets the body index value of the vehicle.
        /// </summary>
        public int BodyIdx { get; }

        /// <summary>
        /// Gets the turret index value of the vehicle.
        /// </summary>
        public int TurretIdx { get; }

        /// <summary>
        /// Gets the health of the vehicle.
        /// </summary>
        public int Health { get; }

        /// <summary>
        /// Gets the attack damage of the vehicle.
        /// </summary>
        public int AttackDamage { get; }

        /// <summary>
        /// Gets the teamID of the vehicle.
        /// </summary>
        public byte TeamId { get; }

        /// <summary>
        /// Gets the uniqueID of the vehicle.
        /// </summary>
        public byte UniqueId { get; }

        /// <summary>
        /// Gets the shooting animation index value.
        /// </summary>
        public byte ShootIdx { get; }

        /// <summary>
        /// Gets the shooting animation stages of the vehicle.
        /// </summary>
        public string[] ShootingStages { get; }

        /// <summary>
        /// Gets or sets the target vehicle.
        /// </summary>
        public IVehicle TargetVehicle { get; set; }

        /// <summary>
        /// Sets the vehicles flowfield property value from the input.
        /// </summary>
        /// <param name="flowField">Input flowfield.</param>
        public void SetFlowField(Cell[,] flowField);

        /// <summary>
        /// Gets the node index which the vehicles is currently on.
        /// </summary>
        /// <returns>Node index value.</returns>
        public Point GetNodeIdxVehicleIsOn();

        /// <summary>
        /// This is the main method which keeps the vehicle "alive" every tick.
        /// The method applies different kind of behaviors based on the vehicles actual behavioral state.
        /// </summary>
        public void ApplyBehaviors();

        /// <summary>
        /// This is the main network method, which updates the vehicles state from network data.
        /// </summary>
        /// <param name="netPos">Vehicle position from network.</param>
        /// <param name="netBodyIdx">Vehicle BodyTexture index from network.</param>
        /// <param name="netTurretIdx">Vehicle TurretTexture index from network.</param>
        /// <param name="shoot">Vehicles shooting stage index from network.</param>
        public void UpdateFromNetwork(Vector2 netPos, short netBodyIdx, short netTurretIdx, byte shoot);

        /// <summary>
        /// Sets the health value from network data.
        /// </summary>
        /// <param name="netHP">Health from network.</param>
        public void SetHpFromNetwork(byte netHP);

        /// <summary>
        /// Sets the location where the vehicle would move.
        /// </summary>
        /// <param name="movePos">Target location.</param>
        /// <param name="flowField">Flowfield to follow.</param>
        /// <param name="multipleUnits">Bool parameter for multiple selected vehicles.</param>
        public void SetPositionToMove(Vector2 movePos, Cell[,] flowField, bool multipleUnits);

        /// <summary>
        /// Decreases the vehciles health.
        /// </summary>
        /// <param name="from">Enemy vehicle.</param>
        public void DecreaseHealth(IVehicle from);

        /// <summary>
        /// Loads saved properties. When this method is called, it sets every property from the input.
        /// </summary>
        /// <param name="posVector">Vehicle position.</param>
        /// <param name="heath">Vehicle health.</param>
        /// <param name="bodyIdx">BodyTexture index.</param>
        /// <param name="turretIdx">TurretTexture index.</param>
        /// <param name="velocity">Vehicles velocity.</param>
        /// <param name="turretNorm">Vehicles turret direction.</param>
        public void LoadFromSave(Vector2 posVector, byte heath, short bodyIdx, short turretIdx, Vector2 velocity, Vector2 turretNorm);
    }
}
