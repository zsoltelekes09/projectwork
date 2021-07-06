// <copyright file="LesserCell.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.FlowField
{
    using System.Numerics;

    /// <summary>
    /// Lessercell is located inside a "Cell" entity.
    /// </summary>
    public class LesserCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LesserCell"/> class.
        /// </summary>
        /// <param name="start">Start direction.</param>
        /// <param name="end">End direction.</param>
        /// <param name="parentCell">Parent cell.</param>
        public LesserCell(Vector2 start, Vector2 end, Cell parentCell)
        {
            this.ParentCell = parentCell;
            this.BestDirection = (end - start) * 25;
        }

        /// <summary>
        /// Gets the parent cell.
        /// </summary>
        public Cell ParentCell { get; private set; }

        /// <summary>
        /// Gets the best direction to the next node.
        /// </summary>
        public Vector2 BestDirection { get; private set; }

        /// <summary>
        /// Gets the outward direction.
        /// </summary>
        public Vector2 OutwardDirection { get; private set; }

        /// <summary>
        /// Sets the best direction from input.
        /// </summary>
        /// <param name="inputDir">Input direction.</param>
        public void SetBestDirection(Vector2 inputDir)
        {
            if (this.OutwardDirection == Vector2.Zero)
            {
                this.BestDirection = inputDir;
            }
            else
            {
                this.BestDirection = this.OutwardDirection + (inputDir * 2);
            }
        }
    }
}
