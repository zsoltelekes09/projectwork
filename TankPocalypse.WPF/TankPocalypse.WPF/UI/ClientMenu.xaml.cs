// <copyright file="ClientMenu.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Interaction logic for ClientMenu.xaml.
    /// </summary>
    public partial class ClientMenu : UserControl
    {
        private ClientViewModel clientVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMenu"/> class.
        /// </summary>
        public ClientMenu()
        {
            this.InitializeComponent();
        }

        private void ClientMenu_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.clientVM = (ClientViewModel)this.DataContext;
        }
    }
}
