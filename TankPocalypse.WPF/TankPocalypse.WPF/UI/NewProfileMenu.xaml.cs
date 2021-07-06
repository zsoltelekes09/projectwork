// <copyright file="NewProfileMenu.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Interaction logic for NewProfileMenu.xaml.
    /// </summary>
    public partial class NewProfileMenu : UserControl
    {
        private NewProfileViewModel newVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewProfileMenu"/> class.
        /// </summary>
        public NewProfileMenu()
        {
            this.InitializeComponent();
        }

        private void NewProfileMenu_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.newVM = (NewProfileViewModel)this.DataContext;
        }
    }
}
