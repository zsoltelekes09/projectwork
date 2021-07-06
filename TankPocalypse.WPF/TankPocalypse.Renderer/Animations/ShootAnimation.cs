// <copyright file="ShootAnimation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Renderer.Animations
{
    using System;
    using System.Numerics;
    using System.Windows;
    using System.Windows.Media;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Renderer.Interfaces;

    /// <summary>
    /// Shooting animation for vehicle.
    /// </summary>
    public class ShootAnimation : ICustomAnimation
    {
        private int tick;
        private IVehicle vehicle;
        private byte shootIdx;
        private string[] shootingStages;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShootAnimation"/> class.
        /// </summary>
        /// <param name="vehicle">Reference vehicle.</param>
        public ShootAnimation(IVehicle vehicle)
        {
            this.vehicle = vehicle;
            this.IsFinished = false;
            this.tick = 0;
            this.shootIdx = 0;
            this.shootingStages = new[] { "2100", "3210", "4320", "5322", "4323", "3202", "2101" };
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

            if (this.tick < 20 && this.tick % 3 == 0)
            {
                this.shootIdx++;
            }

            if (this.tick >= 21)
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
                string shootingStage = this.shootingStages[this.shootIdx];

                for (int i = 0; i < shootingStage.Length; i++)
                {
                    byte idx = (byte)char.GetNumericValue(shootingStage[i]);
                    if (idx != 0)
                    {
                        var turretIdx = this.vehicle.TurretIdx;
                        if (turretIdx is not(>= 0 and <= 355))
                        {
                            turretIdx = 0;
                        }

                        turretIdx -= 45;

                        float x = Convert.ToSingle((i + 6) * 3 * 2.1 * Math.Cos(turretIdx * Math.PI / 180));
                        float y = Convert.ToSingle((i + 6) * 3 * 2.1 * Math.Sin(turretIdx * Math.PI / 180) * -1 / 2);
                        var centerPos = new Vector2(this.vehicle.ScreenPosition.X - 10 + x, this.vehicle.ScreenPosition.Y - 10 - 13 + y);
                        Geometry shootGeo = new RectangleGeometry(new Rect(centerPos.X, centerPos.Y, 20, 20));
                        GeometryDrawing shootDrawing = new GeometryDrawing(new ImageBrush(imageController?.Shooting[idx]), null, shootGeo);

                        dg?.Children.Add(shootDrawing);
                    }
                }
            }
        }
    }
}
