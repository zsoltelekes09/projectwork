// <copyright file="ProfileMenu.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Interaction logic for ProfileMenu.xaml.
    /// </summary>
    public partial class ProfileMenu : UserControl
    {
        private ProfileViewModel profileVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileMenu"/> class.
        /// </summary>
        public ProfileMenu()
        {
            this.InitializeComponent();
        }

        private void ProfileMenu_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.profileVM = (ProfileViewModel)this.DataContext;
        }
    }
}
