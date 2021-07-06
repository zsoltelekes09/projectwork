// <copyright file="GameWorld.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.World
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Numerics;
    using TankPocalypse.Model.Interfaces;

    /// <summary>
    /// This is the gameworld class. It stores a collection of nodes which builds up the game world.
    /// </summary>
    public class GameWorld : IGameWorld
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameWorld"/> class.
        /// </summary>
        public GameWorld()
        {
        }

        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int GridWidth { get; private set; }

        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int GridHeight { get; private set; }

        /// <summary>
        /// Gets the Grid collection.
        /// </summary>
        public Node[,] WorldGrid { get; private set; }

        /// <summary>
        /// Initialises the game world.
        /// </summary>
        /// <param name="mapData">Map data from file.</param>
        public void SetupGrid(List<string> mapData)
        {
            this.GridHeight = int.Parse(mapData[0]);
            this.GridWidth = int.Parse(mapData[1]);
            this.WorldGrid = new Node[this.GridHeight, this.GridWidth];

            for (int y = 0; y < this.GridHeight; y++)
            {
                string mapColumn = mapData[y + 2];
                for (int x = 0; x < this.GridWidth; x++)
                {
                    this.WorldGrid[y, x] = new Node(x, y, char.GetNumericValue(mapColumn[x]));
                }
            }
        }

        /// <summary>
        /// Returns the grid height and width.
        /// </summary>
        /// <returns>New point, which represents the height and width of the grid.</returns>
        public Point GetWorldDimensions()
        {
            return new Point(this.GridWidth, this.GridHeight);
        }

        /// <summary>
        /// Returns a node from the grid by a world location parameter.
        /// </summary>
        /// <param name="worldLoc">Location to search for...</param>
        /// <returns>Node from the grid.</returns>
        public Node GetNodeByWorldCoordinates(Vector2 worldLoc)
        {
            int x = (int)worldLoc.X / 50;
            int y = (int)worldLoc.Y / 50;
            return this.WorldGrid[y, x];
        }

        private static List<string> ReadTextFile(string path)
        {
            List<string> lines = new List<string>();
            StreamReader sr = new StreamReader(path);
            while (!sr.EndOfStream)
            {
                lines.Add(sr.ReadLine());
            }

            sr.Close();
            return lines;
        }
    }
}
