// <copyright file="LoadViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.VM
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.WPF.Logic;

    /// <summary>
    /// Loadgame usercontrols viewmodel.
    /// </summary>
    public class LoadViewModel : ViewModelBase
    {
        private MainViewModel mainVM;
        private GameControl gameControl;
        private List<IUISavedGame> allSaves;
        private IUISavedGame selectedSave;
        private BitmapImage background;
        private BitmapImage buttonDefault;
        private BitmapImage buttonHover;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadViewModel"/> class.
        /// </summary>
        /// <param name="mainVM">Main viewmodel.</param>
        public LoadViewModel(MainViewModel mainVM)
        {
            this.mainVM = mainVM;

            this.Background = this.MainVm.MenuImages.Backgrounds["load"];
            this.ButtonDefault = this.MainVm.MenuImages.Buttons["default"];
            this.ButtonHover = this.MainVm.MenuImages.Buttons["hover"];

            this.gameControl = new GameControl(true);

            this.AllSaves = this.gameControl.GetAllSaves();
            this.SelectedSave = null;

            this.SelectCommand = new RelayCommand(() =>
            {
                if (this.SelectedSave != null)
                {
                    this.mainVM.GlobalSaveFile = this.SelectedSave;
                    this.mainVM.TcpGameServer.SaveFileSend();
                    this.mainVM.LobbyMenuCommand.Execute(null);
                }
            });

            this.BackToLobbyCommand = new RelayCommand(() =>
            {
                this.mainVM.GlobalSaveFile = null;
                this.mainVM.TcpGameServer.SaveFileSend();
                this.mainVM.LobbyMenuCommand.Execute(null);
            });
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
        /// Gets or sets all sotred saves.
        /// </summary>
        public List<IUISavedGame> AllSaves
        {
            get => this.allSaves;
            set => this.Set(ref this.allSaves, value);
        }

        /// <summary>
        /// Gets or sets the selected save.
        /// </summary>
        public IUISavedGame SelectedSave
        {
            get => this.selectedSave;
            set => this.Set(ref this.selectedSave, value);
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        public ICommand SelectCommand { get; private set; }

        /// <summary>
        /// Gets the back command.
        /// </summary>
        public ICommand BackToLobbyCommand { get; private set; }

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
    }
}
