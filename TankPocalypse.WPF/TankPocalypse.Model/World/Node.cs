// <copyright file="Node.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.World
{
    using System;
    using System.Drawing;
    using System.Numerics;
    using TankPocalypse.Model.Interfaces;

    /// <summary>
    /// The node class represents a tile on the grid.
    /// </summary>
    public class Node : IReorderable
    {
        // **** NodeTypes :: 0 = Grass ; 1 = Water ; 2 = Forest ; 8 = Team0Base ; 9 = Team1Base **** //

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="x">X index on the grid.</param>
        /// <param name="y">Y index on the grid.</param>
        /// <param name="nodeType">Type of the node.</param>
        public Node(int x, int y, double nodeType)
        {
            this.GetObjectType = 0;
            this.NodeType = Convert.ToInt16(nodeType);
            this.GridIdx = new Point(x, y);

            this.WorldPosition = new Vector2((x * 50) + 25, (y * 50) + 25);
            this.IsWalkable = this.NodeType == 0 || this.NodeType == 8 || this.NodeType == 9;
        }

        /// <summary>
        /// Gets the nodes index value on the grid.
        /// </summary>
        public Point GridIdx { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the node is visible or not.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the node is revealed or not.
        /// </summary>
        public bool IsRevealed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the node is walkable or not.
        /// </summary>
        public bool IsWalkable { get; set; }

        /// <summary>
        /// Gets the type of the node.
        /// </summary>
        public short NodeType { get; private set; }

        /// <summary>
        /// Gets or sets the world location of the node.
        /// </summary>
        public Vector2 WorldPosition { get; set; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        public short GetObjectType { get; private set; }

        /// <summary>
        /// Gets or sets the screen position coordinates of the node.
        /// </summary>
        public Vector2 ScreenPosition { get; set; }

        /// <summary>
        /// Gets or sets the starting location of the node drawing on the screen.
        /// </summary>
        public Vector2 ObjectScreenStartPosition { get; set; }

        /// <summary>
        /// Gets or sets the end location of the node drawing on the screen.
        /// </summary>
        public Vector2 ObjectScreenEndPosition { get; set; }

        /// <summary>
        /// Gets the identifier number, which represents to which team the node belongs to.
        /// </summary>
        public short TeamNodeId
        {
            get
            {
                return this.NodeType switch
                {
                    8 => 0,
                    9 => 1,
                    _ => -1
                };
            }
        }

        /// <summary>
        /// Gets or sets the center location of the node in the world.
        /// </summary>
        public Vector2 NodeCenterLocation { get; set; }
    }
}
