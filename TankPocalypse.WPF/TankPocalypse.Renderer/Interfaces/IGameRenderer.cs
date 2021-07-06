// <copyright file="IGameRenderer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Renderer.Interfaces
{
    using System.Windows.Media;

    /// <summary>
    /// GameRenderer interface.
    /// </summary>
    public interface IGameRenderer
    {
        /// <summary>
        /// Initialises the gamerenderer.
        /// </summary>
        /// <param name="winX">Windows actual width.</param>
        /// <param name="winY">Windows actual height.</param>
        public void SetupRender(int winX, int winY);

        /// <summary>
        /// Clear every cached dependencies.
        /// </summary>
        public void DeleteEverything();

        /// <summary>
        /// Creates a new drawing to render.
        /// </summary>
        /// <returns>New drawing.</returns>
        public Drawing CreateOnScreenDrawing();

        /// <summary>
        /// This method updates the zoomrate value of the camera.
        /// </summary>
        /// <param name="zoom">Cameras zoomrate.</param>
        public void ScreenZoomRateMethod(short zoom);
    }
}
