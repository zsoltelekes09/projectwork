// <copyright file="LoadMenu.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Interaction logic for LoadMenu.xaml.
    /// </summary>
    public partial class LoadMenu : UserControl
    {
        private LoadViewModel loadVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadMenu"/> class.
        /// </summary>
        public LoadMenu()
        {
            this.InitializeComponent();
        }

        private void LoadMenu_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.loadVM = (LoadViewModel)this.DataContext;
        }
    }
}
