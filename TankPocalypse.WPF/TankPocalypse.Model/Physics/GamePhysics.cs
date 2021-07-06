// <copyright file="GamePhysics.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.Physics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Model.World;

    /// <summary>
    /// This is the physics class. It handles every dynamic and static collision between every elements of the game.
    /// </summary>
    public class GamePhysics : IGamePhysics
    {
        private IGameModel gameModel;
        private IGameWorld gameWorld;

        private List<HitLine> hitLines = new List<HitLine>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePhysics"/> class.
        /// </summary>
        /// <param name="gameModel">GameModel entity.</param>
        /// <param name="gameWorld">GameWorld entity.</param>
        public GamePhysics(IGameModel gameModel, IGameWorld gameWorld)
        {
            this.gameModel = gameModel;
            this.gameWorld = gameWorld;
        }

        /// <summary>
        /// Sets up the static world collision walls.
        /// </summary>
        public void SetupWorldBounds()
        {
            HitLine topHitLine = new HitLine(new Vector2(0, 0), new Vector2(this.gameWorld.GridWidth * 50, 0), 3);
            this.hitLines.Add(topHitLine);
            HitLine bottomHitLine = new HitLine(new Vector2(0, this.gameWorld.GridHeight * 50), new Vector2(this.gameWorld.GridWidth * 50, this.gameWorld.GridHeight * 50), 3);
            this.hitLines.Add(bottomHitLine);
            HitLine leftHitLine = new HitLine(new Vector2(0, 0), new Vector2(0, this.gameWorld.GridHeight * 50), 3);
            this.hitLines.Add(leftHitLine);
            HitLine rightHitLine = new HitLine(new Vector2(this.gameWorld.GridWidth * 50, 0), new Vector2(this.gameWorld.GridWidth * 50, this.gameWorld.GridHeight * 50), 3);
            this.hitLines.Add(rightHitLine);

            for (int y = 0; y < this.gameWorld.GridHeight; y++)
            {
                for (int x = 0; x < this.gameWorld.GridWidth; x++)
                {
                    if (!(this.gameWorld.WorldGrid[y, x].NodeType == 0 || this.gameWorld.WorldGrid[y, x].NodeType == 8 || this.gameWorld.WorldGrid[y, x].NodeType == 9))
                    {
                        this.AddUnwalkableNode(this.gameWorld.WorldGrid[y, x]);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new collision wall to the physics database.
        /// </summary>
        /// <param name="hl">Collision wall.</param>
        public void AddHitLine(HitLine hl)
        {
            if (!this.hitLines.Where(x => x.StartPos == hl.StartPos && x.EndPos == hl.EndPos).Any())
            {
                this.hitLines.Add(hl);
            }
        }

        /// <summary>
        /// Creates collision walls arround the node.
        /// </summary>
        /// <param name="node">Unwalkable node.</param>
        public void AddUnwalkableNode(Node node)
        {
            float posX = node.GridIdx.X;
            float posY = node.GridIdx.Y;

            HitLine topHitLine = new HitLine(new Vector2(posX * 50, posY * 50), new Vector2((posX * 50) + 50, posY * 50), 3);
            this.hitLines.Add(topHitLine);
            HitLine bottomHitLine = new HitLine(new Vector2(posX * 50, (posY * 50) + 50), new Vector2((posX * 50) + 50, (posY * 50) + 50), 3);
            this.hitLines.Add(bottomHitLine);
            HitLine leftHitLine = new HitLine(new Vector2(posX * 50, posY * 50), new Vector2(posX * 50, (posY * 50) + 50), 3);
            this.hitLines.Add(leftHitLine);
            HitLine rightHitLine = new HitLine(new Vector2((posX * 50) + 50, posY * 50), new Vector2((posX * 50) + 50, (posY * 50) + 50), 3);
            this.hitLines.Add(rightHitLine);
        }

        /// <summary>
        /// Calculates physics from the actual state of gamemodel.
        /// </summary>
        public void UpdatePhysics() // STATIC OVERLAPPING
        {
            foreach (var selfVehicle in this.gameModel.Vehicles)
            {
                // *************** STATIC COLLISION WITH WORLD ******************** //
                foreach (var targetLine in this.hitLines)
                {
                    var fLineX1 = targetLine.EndX - targetLine.StartX;
                    var fLineY1 = targetLine.EndY - targetLine.StartY;

                    var fLineX2 = selfVehicle.Px - targetLine.StartX;
                    var fLineY2 = selfVehicle.Py - targetLine.StartY;

                    var fEdgeLength = (fLineX1 * fLineX1) + (fLineY1 * fLineY1);

                    var t = Math.Max(0, Math.Min(fEdgeLength, (fLineX1 * fLineX2) + (fLineY1 * fLineY2))) / fEdgeLength;

                    var fClosestPointX = targetLine.StartX + (t * fLineX1);
                    var fClosestPointY = targetLine.StartY + (t * fLineY1);

                    var fDistance = Convert.ToSingle(Math.Sqrt(((selfVehicle.Px - fClosestPointX) * (selfVehicle.Px - fClosestPointX)) + ((selfVehicle.Py - fClosestPointY) * (selfVehicle.Py - fClosestPointY))));

                    if (fDistance <= (selfVehicle.Radius + targetLine.Radius))
                    {
                        var fOverlap = 1.0f * (fDistance - selfVehicle.Radius - targetLine.Radius);

                        var newPosX = selfVehicle.Px - (fOverlap * (selfVehicle.Px - fClosestPointX) / fDistance);
                        var newPosY = selfVehicle.Py - (fOverlap * (selfVehicle.Py - fClosestPointY) / fDistance);
                        selfVehicle.Position = new Vector2(newPosX, newPosY);
                    }
                }

                // *************** STATIC COLLISION WITH OTHER VEHICLES ************ //
                foreach (var otherVehicle in this.gameModel.Vehicles)
                {
                    if (!selfVehicle.Equals(otherVehicle))
                    {
                        if (DoCirclesOverlap(selfVehicle, otherVehicle))
                        {
                            var dist = Convert.ToSingle(Math.Sqrt(Math.Pow(selfVehicle.Px - otherVehicle.Px, 2) + Math.Pow(selfVehicle.Py - otherVehicle.Py, 2)));

                            float selfOverlapRate;
                            float targetOverlapRate;

                            if ((selfVehicle.Behavior == VehicleBehavior.Waiting && otherVehicle.Behavior == VehicleBehavior.Waiting) || (selfVehicle.Behavior != VehicleBehavior.Waiting && otherVehicle.Behavior == VehicleBehavior.Waiting))
                            {
                                selfOverlapRate = 0.01f;
                                targetOverlapRate = 0.99f;
                            }
                            else if (otherVehicle.Behavior == VehicleBehavior.Dead)
                            {
                                selfOverlapRate = 0.9f;
                                targetOverlapRate = 0.1f;
                            }
                            else
                            {
                                selfOverlapRate = 0.5f;
                                targetOverlapRate = 0.5f;
                            }

                            var selfOverlap = selfOverlapRate * (dist - selfVehicle.Radius - otherVehicle.Radius);
                            var targetOverlap = targetOverlapRate * (dist - selfVehicle.Radius - otherVehicle.Radius);

                            var newPosX = selfVehicle.Px - (selfOverlap * (selfVehicle.Px - otherVehicle.Px) / dist);
                            var newPosY = selfVehicle.Py - (selfOverlap * (selfVehicle.Py - otherVehicle.Py) / dist);
                            selfVehicle.Position = new Vector2(newPosX, newPosY);

                            newPosX = otherVehicle.Px + (targetOverlap * (selfVehicle.Px - otherVehicle.Px) / dist);
                            newPosY = otherVehicle.Py + (targetOverlap * (selfVehicle.Py - otherVehicle.Py) / dist);
                            otherVehicle.Position = new Vector2(newPosX, newPosY);
                        }
                    }
                }
            }
        }

        private static bool DoCirclesOverlap(IVehicle oneVehicle, IVehicle otherVehicle)
        {
            return Math.Pow(oneVehicle.Position.X - otherVehicle.Position.X, 2) + Math.Pow(oneVehicle.Position.Y - otherVehicle.Position.Y, 2) <= Math.Pow(oneVehicle.Radius + otherVehicle.Radius, 2);
        }
    }
}
