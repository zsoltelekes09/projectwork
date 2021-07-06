// <copyright file="MainMenu.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Interaction logic for MainMenu.xaml.
    /// </summary>
    public partial class MainMenu : UserControl
    {
        private MenuViewModel menuVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu"/> class.
        /// </summary>
        public MainMenu()
        {
            this.InitializeComponent();
        }

        private void MainMenu_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.menuVM = (MenuViewModel)this.DataContext;
        }
    }
}
