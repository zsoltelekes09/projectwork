// <copyright file="ServerMenu.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Interaction logic for ServerMenu.xaml.
    /// </summary>
    public partial class ServerMenu : UserControl
    {
        private ServerViewModel serverVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerMenu"/> class.
        /// </summary>
        public ServerMenu()
        {
            this.InitializeComponent();
        }

        private void ServerMenu_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.serverVM = (ServerViewModel)this.DataContext;

            // this.InitializeComponent();
        }
    }
}
