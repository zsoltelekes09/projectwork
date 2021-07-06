// <copyright file="IFlowField.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model.Interfaces
{
    using System.Numerics;
    using TankPocalypse.Model.FlowField;

    /// <summary>
    /// Interface for flowfield pathinder class.
    /// </summary>
    public interface IFlowField
    {
        /// <summary>
        /// Creates the flowfield and returns it to the caller.
        /// </summary>
        /// <param name="destinationPos">Target destination.</param>
        /// <returns>Flowfield.</returns>
        public Cell[,] RequestRoute(Vector2 destinationPos);

        /// <summary>
        /// Initialises the flowfield.
        /// </summary>
        public void SetupFlowField();
    }
}
