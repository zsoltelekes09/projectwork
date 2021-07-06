// <copyright file="GameViewUserControl.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using TankPocalypse.WPF.Logic;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Interaction logic for GameViewUserControl.xaml.
    /// </summary>
    public partial class GameViewUserControl : UserControl
    {
        private GameViewModel gameVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameViewUserControl"/> class.
        /// </summary>
        public GameViewUserControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Clears every dependency connection.
        /// </summary>
        public void RemoveEverything()
        {
            this.Resources.Clear();
        }

        private void GameViewUserControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.gameVM = (GameViewModel)this.DataContext;
            this.gameVM.SetGameControl(this.FindResource("GameControl") as GameControl);
            this.gameVM.RemoveUC += this.RemoveEverything;
            this.gameVM.InvalidateVisual();
        }
    }
}
