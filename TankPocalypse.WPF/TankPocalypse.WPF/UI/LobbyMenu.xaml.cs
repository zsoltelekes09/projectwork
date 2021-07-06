// <copyright file="LobbyMenu.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using TankPocalypse.WPF.VM;

    /// <summary>
    /// Interaction logic for LobbyMenu.xaml.
    /// </summary>
    public partial class LobbyMenu : UserControl
    {
        private LobbyViewModel lobbyVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="LobbyMenu"/> class.
        /// </summary>
        public LobbyMenu()
        {
            this.InitializeComponent();
        }

        private void LobbyMenu_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.lobbyVM = (LobbyViewModel)this.DataContext;

            var startButton = this.FindName("StartButton") as Button;
            var readyButton = this.FindName("ReadyButton") as Button;
            var loadButton = this.FindName("LoadButton") as Button;
            var unitCombobox = this.FindName("UnitCountComboBox") as ComboBox;
            var unitCountLabel = this.FindName("UnitCountLabel") as Label;

            if (this.lobbyVM.MainVm.AppIsServer)
            {
                unitCombobox.SelectionChanged += this.lobbyVM.MainVm.TcpGameServer.UnitCountSend;
                readyButton.Visibility = Visibility.Collapsed;
                unitCountLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                readyButton.Click += this.lobbyVM.MainVm.TcpGameClient.SendReadyPacket;
                unitCombobox.Visibility = Visibility.Collapsed;
                startButton.Visibility = Visibility.Collapsed;
                loadButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
