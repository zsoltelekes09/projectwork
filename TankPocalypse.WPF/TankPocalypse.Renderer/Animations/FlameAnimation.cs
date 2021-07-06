// <copyright file="FlameAnimation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Renderer.Animations
{
    using System.Numerics;
    using System.Windows;
    using System.Windows.Media;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Renderer.Interfaces;

    /// <summary>
    /// Burning animation for vehicle.
    /// </summary>
    public class FlameAnimation : ICustomAnimation
    {
        private int tick;
        private IVehicle vehicle;
        private byte animIdx;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlameAnimation"/> class.
        /// </summary>
        /// <param name="vehicle">Reference vehicle.</param>
        public FlameAnimation(IVehicle vehicle)
        {
            this.vehicle = vehicle;
            this.tick = 0;
            this.IsFinished = false;
            this.animIdx = 0;
        }

        /// <summary>
        /// Gets a value indicating whether animation is finished or not.
        /// </summary>
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Increases animation tick.
        /// </summary>
        public void IncreaseTick()
        {
            this.tick++;

            if (this.tick < 361 && this.tick % 5 == 0)
            {
                if (this.animIdx < 6)
                {
                    this.animIdx++;
                }
                else
                {
                    this.animIdx = 0;
                }
            }

            if (this.tick >= 361)
            {
                this.IsFinished = true;
            }
        }

        /// <summary>
        /// Creates a drawing from the animations actual state.
        /// </summary>
        /// <param name="dg">The animation adds its image to this drawing group.</param>
        /// <param name="imageController">The animation gets its images from the ImageController.</param>
        public void DrawAnimation(ref DrawingGroup dg, ref IGameImageController imageController)
        {
            if (this.vehicle.IsRevealed)
            {
                var screenStartPos = new Vector2(this.vehicle.ScreenPosition.X - 25, this.vehicle.ScreenPosition.Y - 19.5f - 23);
                Geometry flameGeo = new RectangleGeometry(new Rect(screenStartPos.X, screenStartPos.Y, 70, 39));
                GeometryDrawing flameDrawing = new GeometryDrawing(new ImageBrush(imageController?.FlameTextures[this.animIdx]), null, flameGeo);
                dg?.Children.Add(flameDrawing);
            }
        }
    }
}
