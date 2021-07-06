// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.mainVM = this.FindResource("MainVM") as MainViewModel;
            this.Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Cursors.none.cur"));
        }
    }
}
