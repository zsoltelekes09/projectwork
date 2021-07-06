// <copyright file="FlowFieldController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.FlowField
{
    using System.Collections.Generic;
    using System.Numerics;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Model.World;

    /// <summary>
    /// Flowfield class is the main pathfinder class.
    /// </summary>
    public class FlowFieldController : IFlowField
    {
        private IGameWorld gameWorld;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlowFieldController"/> class.
        /// </summary>
        /// <param name="wg">GameWorld entity.</param>
        public FlowFieldController(IGameWorld wg)
        {
            this.gameWorld = wg;
        }

        /// <summary>
        /// Gets the flowfield grid.
        /// </summary>
        public Cell[,] Grid { get; private set; }

        /// <summary>
        /// Gets the flowfield height.
        /// </summary>
        public int GridHeight { get; private set; }

        /// <summary>
        /// Gets the flowfield width.
        /// </summary>
        public int GridWidth { get; private set; }

        /// <summary>
        /// Gets the list of cardinal directions.
        /// </summary>
        public List<Vector2> CardinalDirections { get; private set; }

        /// <summary>
        /// Gets the list of all directions.
        /// </summary>
        public List<Vector2> AllDirections { get; private set; }

        /// <summary>
        /// Initialises the flowfield.
        /// </summary>
        public void SetupFlowField()
        {
            this.GridHeight = this.gameWorld.WorldGrid.GetLength(0);
            this.GridWidth = this.gameWorld.WorldGrid.GetLength(1);
            this.CardinalDirections = new List<Vector2>()
            {
                new Vector2(1, 0),
                new Vector2(-1, 0),
                new Vector2(0, -1),
                new Vector2(0, 1),
            };
            this.AllDirections = new List<Vector2>()
            {
                new Vector2(1, 0),
                new Vector2(-1, 0),
                new Vector2(0, -1),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(-1, -1),
                new Vector2(1, -1),
                new Vector2(-1, 1),
                new Vector2(0, 0),
            };

            this.GenerateGrid();
        }

        /// <summary>
        /// Creates the flowfield and returns it to the caller.
        /// </summary>
        /// <param name="destinationPos">Target destination.</param>
        /// <returns>Flowfield.</returns>
        public Cell[,] RequestRoute(Vector2 destinationPos)
        {
            int targetX = (int)destinationPos.X / 50;
            int targetY = (int)destinationPos.Y / 50;

            Cell[,] flowField = new Cell[this.GridHeight, this.GridWidth];
            for (int i = 0; i < this.GridHeight; i++)
            {
                for (int j = 0; j < this.GridWidth; j++)
                {
                    flowField[i, j] = new Cell(this.Grid[i, j].WorldPos, this.Grid[i, j].CellIndex) { Cost = this.Grid[i, j].Cost };
                }
            }

            this.CreateIntegrationField(targetX, targetY, ref flowField);
            this.CreateFlowField(ref flowField);

            return flowField;
        }

        /// <summary>
        /// Generates flowfield grid.
        /// </summary>
        private void GenerateGrid()
        {
            this.Grid = new Cell[this.GridHeight, this.GridWidth];
            for (int i = 0; i < this.GridHeight; i++)
            {
                for (int j = 0; j < this.GridWidth; j++)
                {
                    this.Grid[i, j] = new Cell(new Vector2(j * 50, i * 50), new Vector2(j, i));
                    if (!this.gameWorld.WorldGrid[i, j].IsWalkable)
                    {
                        this.Grid[i, j].Cost = ushort.MaxValue;
                    }
                }
            }
        }

        private void CreateIntegrationField(int targetX, int targetY, ref Cell[,] flowField)
        {
            Cell destinationCell = flowField[targetY, targetX];
            destinationCell.Cost = 0;
            destinationCell.BestCost = 0;

            Queue<Cell> toCheck = new Queue<Cell>();

            toCheck.Enqueue(destinationCell);

            while (toCheck.Count > 0)
            {
                Cell curCell = toCheck.Dequeue();
                List<Cell> curNeighbors = this.GetNeighborCellsReq(curCell, this.CardinalDirections, ref flowField);
                foreach (Cell curNeighbor in curNeighbors)
                {
                    if (curNeighbor.Cost == ushort.MaxValue)
                    {
                        continue;
                    }

                    if (curNeighbor.Cost + curCell.BestCost < curNeighbor.BestCost)
                    {
                        curNeighbor.BestCost = (ushort)(curNeighbor.Cost + curCell.BestCost);
                        toCheck.Enqueue(curNeighbor);
                    }
                }
            }
        }

        private void CreateFlowField(ref Cell[,] flowField)
        {
            foreach (Cell currentCell in flowField)
            {
                if (currentCell.Cost == ushort.MaxValue)
                {
                    continue;
                }

                List<Cell> curNeighbors = this.GetNeighborCellsReq(currentCell, this.AllDirections, ref flowField);

                int bestCost = currentCell.BestCost;

                foreach (Cell curNeighbor in curNeighbors)
                {
                    if (curNeighbor.BestCost < bestCost && this.CheckDirection(currentCell.CellIndex, curNeighbor.CellIndex, ref flowField))
                    {
                        bestCost = curNeighbor.BestCost;
                        currentCell.BestDirection = curNeighbor.WorldPos - currentCell.WorldPos;
                    }
                }
            }
        }

        private List<Cell> GetNeighborCellsReq(Cell currentCell, List<Vector2> directions, ref Cell[,] flowField)
        {
            List<Cell> neighborCells = new List<Cell>();
            foreach (Vector2 dir in directions)
            {
                Cell neighbor = this.GetCellAtRelativePosReq(currentCell.CellIndex, dir, ref flowField);
                if (neighbor != null)
                {
                    neighborCells.Add(neighbor);
                }
            }

            return neighborCells;
        }

        private Cell GetCellAtRelativePosReq(Vector2 originPos, Vector2 direction, ref Cell[,] flowField)
        {
            Vector2 finalPos = originPos + direction;

            if (finalPos.X < 0 || finalPos.X >= this.GridWidth || finalPos.Y < 0 || finalPos.Y >= this.GridHeight)
            {
                return null;
            }

            return flowField[(int)finalPos.Y, (int)finalPos.X];
        }

        private bool CheckDirection(Vector2 startPos, Vector2 endPos, ref Cell[,] refGrid)
        {
            int xDiff = (int)endPos.X - (int)startPos.X;
            int yDiff = (int)endPos.Y - (int)startPos.Y;

            if (!this.gameWorld.WorldGrid[(int)startPos.Y, (int)startPos.X + xDiff].IsWalkable)
            {
                refGrid[(int)startPos.Y + yDiff, (int)startPos.X].SetupLesserCell(startPos, endPos);
                return false;
            }

            if (!this.gameWorld.WorldGrid[(int)startPos.Y + yDiff, (int)startPos.X].IsWalkable)
            {
                refGrid[(int)startPos.Y, (int)startPos.X + xDiff].SetupLesserCell(startPos, endPos);
                return false;
            }

            return true;
        }
    }
}
