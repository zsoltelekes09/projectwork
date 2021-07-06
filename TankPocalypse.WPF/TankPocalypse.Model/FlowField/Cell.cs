// <copyright file="Cell.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.FlowField
{
    using System.Numerics;

    /// <summary>
    /// Cell class represents a Node from the world, but is used for flowfield calculations.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="worldPos">World location of cell.</param>
        /// <param name="cellIndex">Index value of the cell.</param>
        public Cell(Vector2 worldPos, Vector2 cellIndex)
        {
            this.WorldPos = worldPos;
            this.CellIndex = cellIndex;
            this.Cost = 1;
            this.BestCost = ushort.MaxValue;
            this.BestDirection = Vector2.Zero;
            this.HasLesserCells = false;
        }

        /// <summary>
        /// Gets or sets the world location of the cell.
        /// </summary>
        public Vector2 WorldPos { get; set; }

        /// <summary>
        /// Gets or sets the cost of the cell.
        /// </summary>
        public ushort Cost { get; set; }

        /// <summary>
        /// Gets or sets the best cost of the cell.
        /// </summary>
        public ushort BestCost { get; set; }

        /// <summary>
        /// Gets or sets the best direction of the cell.
        /// </summary>
        public Vector2 BestDirection { get; set; }

        /// <summary>
        /// Gets the cell index inside flowfield gird.
        /// </summary>
        public Vector2 CellIndex { get; private set; }

        /// <summary>
        /// Gets an array of lesser cells.
        /// </summary>
        public LesserCell[] LesserCellBlock { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether cell has lesser cells or not.
        /// </summary>
        public bool HasLesserCells { get; set; }

        /// <summary>
        /// Generates lesser cells.
        /// </summary>
        /// <param name="start">Start idx.</param>
        /// <param name="end">End idx.</param>
        public void SetupLesserCell(Vector2 start, Vector2 end)
        {
            if (!this.HasLesserCells)
            {
                this.LesserCellBlock = new LesserCell[4];
                this.HasLesserCells = true;
            }

            var idxVector = this.CellIndex - ((start + end) / 2);

            if (idxVector.Y == 0.5f)
            {
                if (idxVector.X == 0.5f)
                {
                    this.LesserCellBlock[0] = new LesserCell(start, end, this);
                }
                else
                {
                    this.LesserCellBlock[1] = new LesserCell(start, end, this);
                }
            }
            else
            {
                if (idxVector.X == 0.5f)
                {
                    this.LesserCellBlock[2] = new LesserCell(start, end, this);
                }
                else
                {
                    this.LesserCellBlock[3] = new LesserCell(start, end, this);
                }
            }
        }
    }
}