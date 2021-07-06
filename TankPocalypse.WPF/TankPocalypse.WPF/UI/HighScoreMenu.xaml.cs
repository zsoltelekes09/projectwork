// <copyright file="HighScoreMenu.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Interaction logic for HighScoreMenu.xaml.
    /// </summary>
    public partial class HighScoreMenu : UserControl
    {
        private HighScoreViewModel highVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighScoreMenu"/> class.
        /// </summary>
        public HighScoreMenu()
        {
            this.InitializeComponent();
        }

        private void HighScoreMenu_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.highVM = (HighScoreViewModel)this.DataContext;
        }
    }
}
