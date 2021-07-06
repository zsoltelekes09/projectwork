// <copyright file="App.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF
{
    using System.Windows;
    using CommonServiceLocator;
    using TankPocalypse.Logic;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.Model;
    using TankPocalypse.Model.FlowField;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Model.Physics;
    using TankPocalypse.Model.World;
    using TankPocalypse.Renderer;
    using TankPocalypse.Renderer.Interfaces;
    using TankPocalypse.Repository;
    using TankPocalypse.Repository.Interfaces;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            ServiceLocator.SetLocatorProvider(() => TankIoc.Instance);
            RegisterInstances();
        }

        /// <summary>
        /// Registers IoC instances.
        /// </summary>
        public static void RegisterInstances()
        {
            TankIoc.Instance.Register<IGameLogic, GameLogic>();
            TankIoc.Instance.Register<IGameRenderer, GameRenderer>();
            TankIoc.Instance.Register<IGameModel, GameModel>();
            TankIoc.Instance.Register<IGameRepository, GameRepository>();
            TankIoc.Instance.Register<IGameWorld, GameWorld>();
            TankIoc.Instance.Register<IGameInputController, GameInputController>();
            TankIoc.Instance.Register<IGameImageController, GameImageController>();
            TankIoc.Instance.Register<IFlowField, FlowFieldController>();
            TankIoc.Instance.Register<IGamePhysics, GamePhysics>();
            TankIoc.Instance.Register<IAnimationController, AnimationController>();
        }
    }
}
