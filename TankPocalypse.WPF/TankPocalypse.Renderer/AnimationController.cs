// <copyright file="AnimationController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Renderer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Renderer.Animations;
    using TankPocalypse.Renderer.Interfaces;

    /// <summary>
    /// Animation controller handles.
    /// </summary>
    public class AnimationController : IAnimationController
    {
        private IGameImageController imageController;
        private IGameModel gameModel;
        private List<ICustomAnimation> animations;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationController"/> class.
        /// </summary>
        /// <param name="imageController">GameImageController entity.</param>
        /// <param name="gameModel">GameModel entitry.</param>
        public AnimationController(IGameImageController imageController, IGameModel gameModel)
        {
            this.imageController = imageController;
            this.gameModel = gameModel;
            this.gameModel.UnitCreated += this.SubscribeToCreateAnimEvent;
            this.Setup();
        }

        /// <summary>
        /// Updates the animations when method is called.
        /// </summary>
        /// <param name="dg">Drawing handler for animations to be drawn with.</param>
        public void UpdateAnimations(ref DrawingGroup dg)
        {
            foreach (ICustomAnimation animation in this.animations)
            {
                animation.IncreaseTick();
                animation.DrawAnimation(ref dg, ref this.imageController);
            }

            var finishedAnim = this.animations.Where(x => x.IsFinished).ToList();
            if (finishedAnim.Count != 0)
            {
                finishedAnim.ForEach(x => this.animations.Remove(x));
            }
        }

        /// <summary>
        /// Creates a new animation on vheicle, depending on the input identifier value.
        /// </summary>
        /// <param name="vehicle">Source vehicle.</param>
        /// <param name="identifier">Type of animation.</param>
        public void AddAnimationFromVehicle(IVehicle vehicle, string identifier)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                switch (identifier)
                {
                    case "shoot":
                        this.animations.Add(new ShootAnimation(vehicle));
                        break;
                    case "explosion":
                        this.animations.Add(new ExplosionAnimation(vehicle));
                        break;
                    case "flame":
                        this.animations.Add(new FlameAnimation(vehicle));
                        break;
                    default:
                        break;
                }
            });
        }

        private void Setup()
        {
            this.animations = new List<ICustomAnimation>();
        }

        private void SubscribeToCreateAnimEvent(IVehicle vehicle)
        {
            vehicle.CreateAnimation += this.AddAnimationFromVehicle;
        }
    }
}
