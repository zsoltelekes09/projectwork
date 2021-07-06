// <copyright file="Vehicle.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.Units
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using TankPocalypse.Model.FlowField;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Model.World;
    using Point = System.Drawing.Point;

    /// <summary>
    /// This is the vehicle class.
    /// </summary>
    public class Vehicle : IVehicle, IReorderable, IClickableObject
    {
        private static readonly Random Rnd = new Random();
        private IGameWorld gameWorld;
        private IFlowField flowFieldController;

        private Vector2 flowFieldBestDirection;

        private List<IVehicle> vehicles;

        private Cell[,] flowField;

        private IVehicle lastHitFrom;

        private int oldShootIdx;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class.
        /// </summary>
        /// <param name="uniqueId">Unique id of vehicle.</param>
        /// <param name="position">World location.</param>
        /// <param name="radius">Radius of vehicle.</param>
        /// <param name="gw">GameWorld entity.</param>
        /// <param name="flowFieldController">FlowFieldController entity.</param>
        /// <param name="vehicles">List of all vehicles.</param>
        /// <param name="teamId">Vehicles team id.</param>
        public Vehicle(byte uniqueId, Vector2 position, float radius, IGameWorld gw, IFlowField flowFieldController, List<IVehicle> vehicles, byte teamId)
        {
            this.UniqueId = uniqueId;
            this.Position = position;
            this.Radius = radius;
            this.gameWorld = gw;
            this.flowFieldController = flowFieldController;
            this.vehicles = vehicles;
            this.TeamId = teamId;

            this.Initialise();
        }

        /// <summary>
        /// UnitKilled event. Fires, when the vehicle is killed.
        /// </summary>
        public event Action<IVehicle> UnitKilled;

        /// <summary>
        /// Create an animation event. Fires, when the unit shoots or blows up.
        /// </summary>
        public event Action<IVehicle, string> CreateAnimation;

        /// <summary>
        /// Gets or sets the vehicles porsition in world.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets the last correct position of the vehicle.
        /// </summary>
        public Vector2 LastPosition { get; private set; }

        /// <summary>
        /// Gets or sets the velocity of the vehicle.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Gets the acceleration of the vehicle.
        /// </summary>
        public Vector2 Acceleration { get; private set; }

        /// <summary>
        /// Gets the normalized heading vector of velocity.
        /// </summary>
        public Vector2 HeadingNormal => Vector2.Normalize(this.Velocity);

        /// <summary>
        /// Gets the maximum steering force value.
        /// </summary>
        public Vector2 SteerForce { get; private set; }

        /// <summary>
        /// Gets or sets the fixed heading normal value.
        /// </summary>
        public Vector2 HeadingNormalFixed { get; set; }

        /// <summary>
        /// Gets a horizontal default vector.
        /// </summary>
        public Vector2 HorizontalVector { get; private set; }

        /// <summary>
        /// Gets or sets the worldposition of the vehcile. This is an interface requirement.
        /// </summary>
        public Vector2 WorldPosition { get => this.Position; set => this.Position = this.WorldPosition; } // interface property

        /// <summary>
        /// Gets the direct location to seek.
        /// </summary>
        public Vector2 DirectLocation { get; private set; }

        /// <summary>
        /// Gets the radius of the vehicle.
        /// </summary>
        public float Radius { get; private set; }

        /// <summary>
        /// Gets the maximum speed value of the vehicle.
        /// </summary>
        public float MaxSpeed { get; private set; }

        /// <summary>
        /// Gets the maximum force of the vehicle.
        /// </summary>
        public float MaxForce { get; private set; }

        /// <summary>
        /// Gets or sets the top left screen position of the object.
        /// </summary>
        public Vector2 ObjectScreenStartPosition { get; set; }

        /// <summary>
        /// Gets or sets the bottom right screen position of the object.
        /// </summary>
        public Vector2 ObjectScreenEndPosition { get; set; }

        /// <summary>
        /// Gets the body index value of the vehicle.
        /// </summary>
        public int BodyIdx
        {
            get { return this.HeadingDirectionRounded; }
        }

        /// <summary>
        /// Gets the turret index value of the vehicle.
        /// </summary>
        public int TurretIdx
        {
            get { return this.TurretIndex; }
        }

        /// <summary>
        /// Gets or sets the vehicles position on the screen.
        /// </summary>
        public Vector2 ScreenPosition { get; set; }

        /// <summary>
        /// Gets the object type of the vehicle.
        /// </summary>
        public short GetObjectType
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets the X coordinate of position.
        /// </summary>
        public float Px => this.Position.X;

        /// <summary>
        /// Gets the Y coordinate of position.
        /// </summary>
        public float Py => this.Position.Y;

        /// <summary>
        /// Gets or sets the target vehicle.
        /// </summary>
        public IVehicle TargetVehicle { get; set; }

        /// <summary>
        /// Gets the teamID of the vehicle.
        /// </summary>
        public byte TeamId { get; private set; }

        /// <summary>
        /// Gets the health of the vehicle.
        /// </summary>
        public int Health { get; private set; }

        /// <summary>
        /// Gets the attack damage of the vehicle.
        /// </summary>
        public int AttackDamage { get; private set; }

        /// <summary>
        /// Gets the rounded heading direction.
        /// </summary>
        public int HeadingDirectionRounded { get; private set; }

        /// <summary>
        /// Gets the normalised turret direction.
        /// </summary>
        public Vector2 TurretDirectionNormal { get; private set; }

        /// <summary>
        /// Gets the maximum rotating speed of the turret.
        /// </summary>
        public int TurretMaxRotationSpeed { get; private set; }

        /// <summary>
        /// Gets the index of the turret.
        /// </summary>
        public int TurretIndex { get; private set; }

        /// <summary>
        /// Gets or sets the vehicles behavior.
        /// </summary>
        public VehicleBehavior Behavior { get; set; }

        /// <summary>
        /// Gets the vehicle movement behavior.
        /// </summary>
        public VehicleMovementBehavior MovementBehavior { get; private set; }

        /// <summary>
        /// Gets a value indicating whether flowfield route is requested or not.
        /// </summary>
        public bool IsFlowRouteRequested => this.flowField != null;

        /// <summary>
        /// Gets a value indicating whether turret is on target or not.
        /// </summary>
        public bool IsTurretOnTarget { get; private set; }

        /// <summary>
        /// Gets a value indicating whether vehicle is selected or not.
        /// </summary>
        public bool IsSelected { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse is over the vehicle or not.
        /// </summary>
        public bool IsMouseOver { get; set; }

        /// <summary>
        /// Gets a value indicating whether the vehicle is standing on a revealed node or not.
        /// </summary>
        public bool IsRevealed
        {
            get
            {
                var idx = this.GetNodeIdxVehicleIsOn();
                return this.gameWorld.WorldGrid[idx.Y, idx.X].IsRevealed;
            }
        }

        /// <summary>
        /// Gets a value indicating whether vehicle is reloaded after firing.
        /// </summary>
        public bool IsReloaded { get; private set; }

        /// <summary>
        /// Gets the value of realod tick.
        /// </summary>
        public int ReloadTick { get; private set; }

        /// <summary>
        /// Gets the uniqueID of the vehicle.
        /// </summary>
        public byte UniqueId { get; private set; }

        /// <summary>
        /// Gets the shooting animation index value.
        /// </summary>
        public byte ShootIdx { get; private set; }

        /// <summary>
        /// Gets the shooting animation stages of the vehicle.
        /// </summary>
        public string[] ShootingStages { get; private set; }

        /// <summary>
        /// Updates the texture indexes of the vehicle based on body and turret directions.
        /// </summary>
        public void CheckDirection()
        {
            Vector2 headingNorm = this.HeadingNormal;

            var degree = this.HorizontalVector.VectorAngle(headingNorm).ClampAngleToNearestTheta(5) * -1;

            if (degree < 0)
            {
                degree = 360 + degree;
            }

            if (degree != this.HeadingDirectionRounded)
            {
                this.HeadingDirectionRounded = degree;
            }

            var turretIndex = (int)this.TurretDirectionNormal.VectorAngle(this.HorizontalVector);

            turretIndex = turretIndex.ClampIntegerToNearestTheta(5);

            if (turretIndex < 0)
            {
                turretIndex = 360 + turretIndex;
            }

            this.TurretIndex = turretIndex;
        }

        // ************************************ F L O W F I E L D - F O L L O W I N G ***************************** //

        /// <summary>
        /// Flowfield following method.
        /// </summary>
        /// <returns>Velocity vector.</returns>
        public Vector2 FollowFlowField()
        {
            int x = (int)this.Position.X / 50;
            int y = (int)this.Position.Y / 50;
            Cell cell = this.flowField[y, x];

            if (cell.HasLesserCells)
            {
                int xRel = (int)this.Position.X % 50;
                int yRel = (int)this.Position.Y % 50;
                if (xRel < 25 && yRel < 25)
                {
                    // Case idx is 0.
                    if (cell.LesserCellBlock[0] != null)
                    {
                        this.flowFieldBestDirection = cell.LesserCellBlock[0].BestDirection;
                    }
                    else
                    {
                        this.flowFieldBestDirection = cell.BestDirection;
                    }
                }
                else if (xRel < 50 && yRel < 25)
                {
                    // Case idx is 1.
                    if (cell.LesserCellBlock[1] != null)
                    {
                        this.flowFieldBestDirection = cell.LesserCellBlock[1].BestDirection;
                    }
                    else
                    {
                        this.flowFieldBestDirection = cell.BestDirection;
                    }
                }
                else if (xRel < 25 && yRel < 50)
                {
                    // Case idx is 2.
                    if (cell.LesserCellBlock[2] != null)
                    {
                        this.flowFieldBestDirection = cell.LesserCellBlock[2].BestDirection;
                    }
                    else
                    {
                        this.flowFieldBestDirection = cell.BestDirection;
                    }
                }
                else
                {
                    // Case idx is 3.
                    if (cell.LesserCellBlock[3] != null)
                    {
                        this.flowFieldBestDirection = cell.LesserCellBlock[3].BestDirection;
                    }
                    else
                    {
                        this.flowFieldBestDirection = cell.BestDirection;
                    }
                }
            }
            else
            {
                this.flowFieldBestDirection = cell.BestDirection;
            }

            float dist = Vector2.Distance(this.Position, this.DirectLocation);

            if (cell.BestCost <= 2 && dist < 50)
            {
                return this.SeekLocation(this.DirectLocation);
            }
            else
            {
                return this.SeekLocation(this.flowFieldBestDirection + this.Position);
            }
        }

        // ******************************** A T T A C K - B E H A V I O R ************************** //

        /// <summary>
        /// Checks if end node is visible from start, which means there is a direct sight from start to end node without any blocking obstacles.
        /// </summary>
        /// <param name="start">Start node.</param>
        /// <param name="end">End node.</param>
        /// <returns>True if there is direct sight and false othervise.</returns>
        public bool CheckVisibility(Node start, Node end)
        {
            int x0 = start.GridIdx.X;
            int y0 = start.GridIdx.Y;
            int x1 = end.GridIdx.X;
            int y1 = end.GridIdx.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int x = x0;
            int y = y0;
            int n = 1 + dx + dy;
            int xInc = (x1 > x0) ? 1 : -1;
            int yInc = (y1 > y0) ? 1 : -1;
            int error = dx - dy;
            dx *= 2;
            dy *= 2;

            List<Node> visitedNodes = new List<Node>();

            for (; n > 0; --n)
            {
                visitedNodes.Add(this.gameWorld.WorldGrid[y, x]);

                if (error > 0)
                {
                    x += xInc;
                    error -= dy;
                }
                else if (error < 0)
                {
                    y += yInc;
                    error += dx;
                }
                else
                {
                    x += xInc;
                    y += yInc;
                    error -= dy;
                    error += dx;
                    --n;
                }
            }

            foreach (var tile in visitedNodes)
            {
                if (!tile.IsWalkable)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Attacking behavior method. If being called, the method applies an attacking behavior on vehicle.
        /// </summary>
        /// <returns>Velocity vector.</returns>
        public Vector2 AttackingBehavior()
        {
            if (this.TargetVehicle.Behavior == VehicleBehavior.Dead)
            {
                this.TargetVehicle = null;
                this.MovementBehavior = VehicleMovementBehavior.Standing;
                this.Behavior = VehicleBehavior.Waiting;
                return Vector2.Zero;
            }

            bool isTargetVisible = this.CheckVisibility(this.GetNodeByPos(this.Position), this.GetNodeByPos(this.TargetVehicle.Position));
            float distFromTarget = Vector2.Distance(this.Position, this.TargetVehicle.Position);

            if (isTargetVisible)
            {
                this.flowField = null;

                if (distFromTarget < 110)
                {
                    this.ShootTarget();
                }

                if (distFromTarget < 100)
                {
                    if (this.MovementBehavior != VehicleMovementBehavior.Standing)
                    {
                        this.MovementBehavior = VehicleMovementBehavior.Standing;
                    }
                }
                else
                {
                    if (this.MovementBehavior == VehicleMovementBehavior.Standing)
                    {
                        this.MovementBehavior = VehicleMovementBehavior.Following;
                    }

                    return this.SeekTargetLocation(this.TargetVehicle.Position);
                }
            }
            else
            {
                if (this.IsFlowRouteRequested)
                {
                    if (this.MovementBehavior == VehicleMovementBehavior.Standing)
                    {
                        this.Behavior = VehicleBehavior.Waiting;
                    }
                    else
                    {
                        return this.FollowFlowField();
                    }
                }
                else
                {
                    this.RequestFlowRouteToTarget(this.TargetVehicle.Position);
                    return this.FollowFlowField();
                }
            }

            return Vector2.Zero;
        }

        /// <summary>
        /// Shoot at target method.
        /// </summary>
        public void ShootTarget()
        {
            if (this.TargetVehicle.Behavior != VehicleBehavior.Dead)
            {
                this.UpdateTurret();
                if (this.IsTurretOnTarget)
                {
                    if (this.IsReloaded)
                    {
                        // this.ShootIdx = 0;
                        this.ShootIdx = 1;
                        this.TargetVehicle.DecreaseHealth(this);
                        this.ReloadTick = 0;
                        this.IsReloaded = false;

                        this.CreateAnimation?.Invoke(this, "shoot");
                    }
                }
            }
            else
            {
                this.TargetVehicle = null;
            }
        }

        // ******************************** W A I T I N G - B E H A V I O R ************************** //

        /// <summary>
        /// Waiting behavior. Applies a waiting behavior if method being called.
        /// </summary>
        public void WaitingBehavior()
        {
            var target = this.FindTarget(this.vehicles);
            if (target != null)
            {
                this.TargetVehicle = target;
                this.Behavior = VehicleBehavior.Attacking;
            }
        }

        /// <summary>
        /// Target finding method. Returns a target to attack if any found.
        /// </summary>
        /// <param name="otherVehicles">List of every vehicle ingame.</param>
        /// <returns>Target vehicle or null.</returns>
        public Vehicle FindTarget(List<IVehicle> otherVehicles)
        {
            float minDist = float.MaxValue;
            Vehicle minTarget = null;

            foreach (Vehicle otherVehicle in otherVehicles)
            {
                if (otherVehicle.TeamId != this.TeamId && otherVehicle.Behavior != VehicleBehavior.Dead)
                {
                    float dist = Vector2.Distance(this.Position, otherVehicle.Position);
                    if (dist < minDist && dist < 110 && this.CheckVisibility(this.GetNodeByPos(this.Position), this.GetNodeByPos(otherVehicle.Position)))
                    {
                        minDist = dist;
                        minTarget = otherVehicle;
                    }
                }
            }

            return minTarget;
        }

        // ******************************** M O V I N G - B E H A V I O R ************************** //

        /// <summary>
        /// Moving behavior method. Applies moving behavior on vehicle.
        /// </summary>
        /// <returns>Velocity vector.</returns>
        public Vector2 MovingBehavior()
        {
            if (this.TargetVehicle != null)
            {
                var dist = Vector2.Distance(this.TargetVehicle.Position, this.Position);
                if (dist < 110)
                {
                    this.ShootTarget();
                }
                else
                {
                    this.TargetVehicle = null;
                }
            }
            else
            {
                this.TargetVehicle = this.FindTarget(this.vehicles);
            }

            return this.FollowFlowField();
        }

        /// <summary>
        /// Requests new flowfield to follow.
        /// </summary>
        /// <param name="movePos">Target location to move.</param>
        public void RequestFlowRouteToTarget(Vector2 movePos)
        {
            this.DirectLocation = movePos;
            this.flowField = this.flowFieldController.RequestRoute(movePos);
            this.MovementBehavior = VehicleMovementBehavior.Following;
        }

        /// <summary>
        /// Sets the location where the vehicle would move.
        /// </summary>
        /// <param name="movePos">Target location.</param>
        /// <param name="flowField">Flowfield to follow.</param>
        /// <param name="multipleUnits">Bool parameter for multiple selected vehicles.</param>
        public void SetPositionToMove(Vector2 movePos, Cell[,] flowField, bool multipleUnits)
        {
            if (multipleUnits == true)
            {
                bool found = false;
                while (!found)
                {
                    Point idx = new Point(Rnd.Next(10, 50), Rnd.Next(10, 50));
                    int x = (int)(movePos.X + idx.X) / 50;
                    int y = (int)(movePos.Y + idx.Y) / 50;
                    if (x >= 0 && x < this.gameWorld.GridWidth && y >= 0 && y < this.gameWorld.GridHeight && this.gameWorld.WorldGrid[y, x].IsWalkable)
                    {
                        this.DirectLocation = new Vector2(movePos.X + idx.X, movePos.Y + idx.Y);
                        found = true;
                    }
                }
            }
            else
            {
                this.DirectLocation = movePos;
            }

            this.flowField = flowField;
            this.Behavior = VehicleBehavior.Moving;
            this.MovementBehavior = VehicleMovementBehavior.Moving;
        }

        // ***** SEEKING - ARRIVING - SLOWING ****** //

        /// <summary>
        /// Seeks the target location.
        /// </summary>
        /// <param name="target">Target location.</param>
        /// <returns>Velocity vector.</returns>
        public Vector2 SeekTargetLocation(Vector2 target)
        {
            Vector2 desired = Vector2.Subtract(target, this.Position);
            desired = Vector2.Normalize(desired) * this.MaxSpeed;

            Vector2 steer = Vector2.Subtract(desired, this.Velocity);
            if (steer.Length() > this.MaxForce)
            {
                steer = Vector2.Normalize(steer) * this.MaxForce;
            }

            return steer;
        }

        /// <summary>
        /// Seeks the target location. Includes the SlowingDown behavioral movement method.
        /// </summary>
        /// <param name="target">Target location.</param>
        /// <returns>Velocity vector.</returns>
        public Vector2 SeekLocation(Vector2 target)
        {
            if (this.MovementBehavior == VehicleMovementBehavior.Moving)
            {
                float dist = Vector2.Distance(this.Position, target);
                if (dist > 30)
                {
                    Vector2 desired = Vector2.Subtract(target, this.Position);
                    desired = Vector2.Normalize(desired) * this.MaxSpeed;

                    Vector2 steer = Vector2.Subtract(desired, this.Velocity);
                    if (steer.Length() > this.MaxForce)
                    {
                        steer = Vector2.Normalize(steer) * this.MaxForce;
                    }

                    return steer;
                }
                else
                {
                    this.MovementBehavior = VehicleMovementBehavior.Slowing;
                    return this.SlowDown(target);
                }
            }
            else
            {
                return this.SlowDown(target);
            }
        }

        /// <summary>
        /// The method applies a decelerating movement towards the target input.
        /// </summary>
        /// <param name="targetPos">Target location.</param>
        /// <returns>Velocity vector.</returns>
        public Vector2 SlowDown(Vector2 targetPos)
        {
            float dist = Vector2.Distance(this.Position, targetPos);

            if (dist < 1.5f)
            {
                this.DirectLocation = Vector2.Zero;
                this.MovementBehavior = VehicleMovementBehavior.Standing;
                if (this.Behavior != VehicleBehavior.Attacking)
                {
                    this.Behavior = VehicleBehavior.Waiting;
                }

                return Vector2.Zero;
            }

            float speedPercent = dist / 30 * this.MaxSpeed;

            Vector2 desired = Vector2.Subtract(targetPos, this.Position);
            desired = Vector2.Normalize(desired) * speedPercent;

            Vector2 steer = Vector2.Subtract(desired, this.Velocity);
            if (steer.Length() > this.MaxForce)
            {
                steer = Vector2.Normalize(steer) * this.MaxForce;
            }

            return steer;
        }

        // ******************************** U P D A T E - C I R C L E ************************** //

        /// <summary>
        /// This is the main network method, which updates the vehicles state from network data.
        /// </summary>
        /// <param name="netPos">Vehicle position from network.</param>
        /// <param name="netBodyIdx">Vehicle BodyTexture index from network.</param>
        /// <param name="netTurretIdx">Vehicle TurretTexture index from network.</param>
        /// <param name="shoot">Vehicles shooting stage index from network.</param>
        public void UpdateFromNetwork(Vector2 netPos, short netBodyIdx, short netTurretIdx, byte shoot)
        {
            this.Position = netPos;
            this.HeadingDirectionRounded = netBodyIdx;
            this.TurretIndex = netTurretIdx;
            this.ShootIdx = shoot;

            // ******* NEW WAY TO HANDLE SHOOT ANIMATIONS ******* \\
            if (this.ShootIdx != this.oldShootIdx)
            {
                this.oldShootIdx = this.ShootIdx;
                if (this.ShootIdx == 1)
                {
                    this.CreateAnimation?.Invoke(this, "shoot");
                }
            }
        }

        /// <summary>
        /// This is the main method which keeps the vehicle "alive" every tick.
        /// The method applies different kind of behaviors based on the vehicles actual behavioral state.
        /// </summary>
        public void ApplyBehaviors()
        {
            if (this.Health <= 0 && this.Behavior != VehicleBehavior.Dead)
            {
                this.Behavior = VehicleBehavior.Dead;
                this.MovementBehavior = VehicleMovementBehavior.Standing;
                if (this.IsSelected)
                {
                    this.SetDeselected();
                }

                this.UnitKilled?.Invoke(this);
                this.CreateAnimation?.Invoke(this, "flame");
                this.CreateAnimation?.Invoke(this, "explosion");
            }

            if (this.Behavior != VehicleBehavior.Dead)
            {
                Vector2 steerForce = new Vector2(0, 0);

                this.ReloadTurret();

                switch (this.Behavior)
                {
                    case VehicleBehavior.Attacking:

                        steerForce += this.AttackingBehavior();
                        break;
                    case VehicleBehavior.Waiting:

                        this.WaitingBehavior();
                        break;
                    case VehicleBehavior.Moving:

                        steerForce += this.MovingBehavior();
                        break;
                }

                this.ApplyForce(steerForce);
            }

            this.Update();
        }

        /// <summary>
        /// Sets the health value from network data.
        /// </summary>
        /// <param name="netHP">Health from network.</param>
        public void SetHpFromNetwork(byte netHP)
        {
            this.Health = netHP;
        }

        /// <summary>
        /// Updates the vehicle state.
        /// </summary>
        public void Update()
        {
            this.Velocity += this.Acceleration;

            if (this.Velocity.Length() > this.MaxSpeed)
            {
                this.Velocity = Vector2.Normalize(this.Velocity) * this.MaxSpeed;
            }

            this.HeadingNormalFixed = Vector2.Normalize(this.Velocity);

            if (this.MovementBehavior != VehicleMovementBehavior.Standing)
            {
                this.Position += this.Velocity;
                this.LastPosition = this.Position;
            }

            if (this.TargetVehicle == null)
            {
                this.UpdateTurret();
            }

            this.Acceleration *= 0;

            this.CheckDirection();
        }

        /// <summary>
        /// Adds force to the acceleration.
        /// </summary>
        /// <param name="force">Force to add.</param>
        public void ApplyForce(Vector2 force)
        {
            this.Acceleration += force;
        }

        /// <summary>
        /// Decreases the vehciles health.
        /// </summary>
        /// <param name="from">Enemy vehicle.</param>
        public void DecreaseHealth(IVehicle from)
        {
            this.lastHitFrom = from;
            this.Health -= from.AttackDamage;
            if (this.Health <= 0)
            {
                this.Behavior = VehicleBehavior.Dead;
                this.MovementBehavior = VehicleMovementBehavior.Standing;
                if (this.IsSelected)
                {
                    this.SetDeselected();
                }

                this.UnitKilled?.Invoke(this);

                this.CreateAnimation?.Invoke(this, "flame");
                this.CreateAnimation?.Invoke(this, "explosion");
            }
        }

        /// <summary>
        /// Sets the vehicle selected.
        /// </summary>
        public void SetSelected()
        {
            this.IsSelected = true;
        }

        /// <summary>
        /// Sets the vehicle deselected.
        /// </summary>
        public void SetDeselected()
        {
            this.IsSelected = false;
        }

        /// <summary>
        /// Gets the node index which the vehicles is currently on.
        /// </summary>
        /// <returns>Node index value.</returns>
        public Point GetNodeIdxVehicleIsOn()
        {
            int x = (int)this.Position.X / 50;
            int y = (int)this.Position.Y / 50;
            return new Point(x, y);
        }

        /// <summary>
        /// Sets the vehicles flowfield property value from the input.
        /// </summary>
        /// <param name="flowField">Input flowfield.</param>
        public void SetFlowField(Cell[,] flowField)
        {
            this.flowField = flowField;
        }

        // ***************** T U R R E T - C O N T R O L L E R ********* //

        /// <summary>
        /// Updates the turrets state.
        /// </summary>
        public void UpdateTurret()
        {
            if (this.TargetVehicle != null && Vector2.Distance(this.TargetVehicle.Position, this.Position) < 110)
            {
                Vector2 targetVector = Vector2.Subtract(this.TargetVehicle.Position, this.Position);
                var angle = this.TurretDirectionNormal.VectorAngle(targetVector);

                if (angle > 5f)
                {
                    this.IsTurretOnTarget = false;
                    angle = angle.ClampAngleToMaxValue(this.TurretMaxRotationSpeed);
                    this.TurretDirectionNormal = this.TurretDirectionNormal.RotateByAngle(angle);
                }
                else if (angle < -5f)
                {
                    angle = angle.ClampAngleToMaxValue(this.TurretMaxRotationSpeed);
                    this.IsTurretOnTarget = false;
                    this.TurretDirectionNormal = this.TurretDirectionNormal.RotateByAngle(angle);
                }
                else
                {
                    this.IsTurretOnTarget = true;

                    // DO NOTHING CUZ TURRERT IS ON TARGET.
                }
            }
            else
            {
                var angle = this.TurretDirectionNormal.VectorAngle(this.HeadingNormalFixed);
                if (angle > 0.2f)
                {
                    angle = angle.ClampAngleToMaxValue(this.TurretMaxRotationSpeed);
                    this.TurretDirectionNormal = this.TurretDirectionNormal.RotateByAngle(angle);
                }
                else if (angle < -0.2f)
                {
                    angle = angle.ClampAngleToMaxValue(this.TurretMaxRotationSpeed);
                    this.TurretDirectionNormal = this.TurretDirectionNormal.RotateByAngle(angle);
                }

                // else
                // {
                //    // DO NOTHING CUZ TURRERT IS ON TARGET.
                // }
            }
        }

        /// <summary>
        /// Loads saved properties. When this method is called, it sets every property from the input.
        /// </summary>
        /// <param name="posVector">Vehicle position.</param>
        /// <param name="heath">Vehicle health.</param>
        /// <param name="bodyIdx">BodyTexture index.</param>
        /// <param name="turretIdx">TurretTexture index.</param>
        /// <param name="velocity">Vehicles velocity.</param>
        /// <param name="turretNorm">Vehicles turret direction.</param>
        public void LoadFromSave(Vector2 posVector, byte heath, short bodyIdx, short turretIdx, Vector2 velocity, Vector2 turretNorm)
        {
            this.Position = posVector;
            this.Health = heath;
            if (this.Health <= 0)
            {
                this.Behavior = VehicleBehavior.Dead;
                this.MovementBehavior = VehicleMovementBehavior.Standing;
                this.UnitKilled?.Invoke(this);
            }

            this.HeadingDirectionRounded = bodyIdx;
            this.TurretIndex = turretIdx;
            this.Velocity = velocity;
            this.TurretDirectionNormal = turretNorm;
        }

        private Node GetNodeByPos(Vector2 pos)
        {
            int x = (int)pos.X / 50;
            int y = (int)pos.Y / 50;
            return this.gameWorld.WorldGrid[y, x];
        }

        /// <summary>
        /// Reloads the turret after shooting and sets turret animation index.
        /// </summary>
        private void ReloadTurret()
        {
            if (!this.IsReloaded)
            {
                this.ReloadTick++;
                /*
                if (this.ReloadTick < 22 && this.ReloadTick % 3 == 0)
                {
                    this.ShootIdx++;
                }
                else if (this.ReloadTick >= 22)
                {
                    this.ShootIdx = 0;
                }*/

                if (this.ReloadTick >= 20 && this.ShootIdx != 0)
                {
                    this.ShootIdx = 0;
                }

                if (this.ReloadTick == 60)
                {
                    this.IsReloaded = true;
                }
            }
        }

        private void Initialise()
        {
            this.AttackDamage = 10;
            this.Health = 100;
            this.Behavior = VehicleBehavior.Waiting;
            this.MovementBehavior = VehicleMovementBehavior.Standing;
            this.Velocity = new Vector2(0.01f, 0.01f);
            this.Acceleration = Vector2.Zero;
            this.SteerForce = Vector2.Zero;
            this.MaxSpeed = 0.8f * 1.2f;
            this.MaxForce = 0.03f * 1.2f;
            this.HeadingNormalFixed = new Vector2(1, 0);
            this.LastPosition = new Vector2(0, 0);
            this.HorizontalVector = new Vector2(1, 0);
            this.flowFieldBestDirection = Vector2.Zero;
            this.HeadingDirectionRounded = 0;
            this.TurretMaxRotationSpeed = 5;
            this.TurretDirectionNormal = new Vector2(1, 0);
            this.TurretIndex = 0;
            this.IsReloaded = true;
            this.ShootingStages = new[] { "2100", "3210", "4320", "5322", "4323", "3202", "2101" };
            this.ShootIdx = 0;
        }
    }
}
