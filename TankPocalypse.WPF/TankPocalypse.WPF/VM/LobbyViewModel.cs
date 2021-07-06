// <copyright file="LobbyViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.VM
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// Lobby UserControls viewmodel.
    /// </summary>
    public class LobbyViewModel : ViewModelBase
    {
        private MainViewModel mainVM;
        private bool userIsReady;
        private string userReadyText;
        private byte selectedUnitCount;
        private List<byte> availableUnitCount;
        private BitmapImage background;
        private BitmapImage buttonDefault;
        private BitmapImage buttonHover;
        private Visibility networkUserConnected;

        /// <summary>
        /// Initializes a new instance of the <see cref="LobbyViewModel"/> class.
        /// </summary>
        /// <param name="main">Main viewmodel.</param>
        public LobbyViewModel(MainViewModel main)
        {
            this.mainVM = main;
            this.UserIsReady = false;

            this.Background = this.MainVm.MenuImages.Backgrounds["lobby"];
            this.ButtonDefault = this.MainVm.MenuImages.Buttons["default"];
            this.ButtonHover = this.MainVm.MenuImages.Buttons["hover"];

            this.AvailableUnitCount = new List<byte> { 2, 4, 8, 16, 24, 30, 36, 40 };
            this.MainVm.GlobalUnitCount = this.AvailableUnitCount[0];

            this.UserReadyText = "Not Ready";

            if (this.mainVM.AppIsServer)
            {
                this.mainVM.GlobatTeamId = 0;
                this.UserIsReady = true;
                this.UserReadyText = "Ready";
            }
            else
            {
                this.mainVM.GlobatTeamId = 1;
                this.MainVm.NetworkUserIsReady = true;
                this.MainVm.NetworkUserReadyText = "Ready";
            }

            this.ReadySetCommand = new RelayCommand(() =>
            {
                if (this.UserIsReady)
                {
                    this.UserIsReady = false;
                    this.UserReadyText = "Not Ready";
                }
                else
                {
                    this.UserIsReady = true;
                    this.UserReadyText = "Ready";
                }
            });

            this.StartGameCommand = new RelayCommand(() =>
            {
                if (this.UserIsReady && this.MainVm.NetworkUserIsReady)
                {
                    this.mainVM.TcpGameServer.StartGamePacket();
                    this.MainVm.GameViewCommand.Execute(null);
                }
            });

            this.SelectSaveToLoad = new RelayCommand(() =>
            {
                this.MainVm.LoadViewCommand.Execute(null);
            });
        }

        /// <summary>
        /// Gets the ready set command.
        /// </summary>
        public ICommand ReadySetCommand { get; private set; }

        /// <summary>
        /// Gets the start game command.
        /// </summary>
        public ICommand StartGameCommand { get; private set; }

        /// <summary>
        /// Gets the select save to load command.
        /// </summary>
        public ICommand SelectSaveToLoad { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether local user is ready or not.
        /// </summary>
        public bool UserIsReady
        {
            get => this.userIsReady;
            set => this.Set(ref this.userIsReady, value);
        }

        /// <summary>
        /// Gets or sets the user ready text.
        /// </summary>
        public string UserReadyText
        {
            get => this.userReadyText;
            set => this.Set(ref this.userReadyText, value);
        }

        /// <summary>
        /// Gets or sets the selected unit count.
        /// </summary>
        public byte SelectedUnitCount
        {
            get => this.selectedUnitCount;
            set => this.Set(ref this.selectedUnitCount, value);
        }

        /// <summary>
        /// Gets or sets the available unit count list.
        /// </summary>
        public List<byte> AvailableUnitCount
        {
            get => this.availableUnitCount;
            set => this.Set(ref this.availableUnitCount, value);
        }

        /// <summary>
        /// Gets or sets the main viewmodel.
        /// </summary>
        public MainViewModel MainVm
        {
            get => this.mainVM;
            set => this.mainVM = value;
        }

        /// <summary>
        /// Gets or sets the background image.
        /// </summary>
        public BitmapImage Background
        {
            get => this.background;
            set => this.Set(ref this.background, value);
        }

        /// <summary>
        /// Gets or sets the default button image.
        /// </summary>
        public BitmapImage ButtonDefault
        {
            get => this.buttonDefault;
            set => this.Set(ref this.buttonDefault, value);
        }

        /// <summary>
        /// Gets or sets the button hover image.
        /// </summary>
        public BitmapImage ButtonHover
        {
            get => this.buttonHover;
            set => this.Set(ref this.buttonHover, value);
        }

        /// <summary>
        /// Gets or sets the network user connected visibility property.
        /// </summary>
        public Visibility NetworkUserConnected
        {
            get => this.networkUserConnected;
            set => this.Set(ref this.networkUserConnected, value);
        }
    }
}
