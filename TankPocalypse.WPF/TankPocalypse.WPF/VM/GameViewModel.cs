// <copyright file="GameViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.VM
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using TankPocalypse.WPF.Interfaces;

    /// <summary>
    /// GameUserControls viewmodel.
    /// </summary>
    public class GameViewModel : ViewModelBase
    {
        private MainViewModel mainVM;
        private IGameControl gameControl;
        private BitmapImage background;
        private Visibility menuVisibility;
        private BitmapImage buttonDefault;
        private BitmapImage buttonHover;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameViewModel"/> class.
        /// </summary>
        /// <param name="mainVM">Main viewmodel.</param>
        public GameViewModel(MainViewModel mainVM)
        {
            this.mainVM = mainVM;

            this.Background = this.MainVm.MenuImages.Backgrounds["game"];
            this.ButtonDefault = this.MainVm.MenuImages.Buttons["default"];
            this.ButtonHover = this.MainVm.MenuImages.Buttons["hover"];

            this.ContinueCommand = new RelayCommand(() =>
            {
                if (!this.gameControl.IsGameEnded)
                {
                    this.gameControl.PauseGame();
                }
            });

            this.SaveGameCommand = new RelayCommand(() =>
            {
                if (!this.gameControl.IsGameEnded)
                {
                    this.gameControl.SaveGame();
                }
            });
        }

        /// <summary>
        /// Event for removing UserControl.
        /// </summary>
        public event Action RemoveUC;

        /// <summary>
        /// Gets or sets the menu visibility property.
        /// </summary>
        public Visibility MenuVisibility
        {
            get => this.menuVisibility;
            set => this.Set(ref this.menuVisibility, value);
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
        /// Gets or sets the GameControl.
        /// </summary>
        public IGameControl GameControl
        {
            get => this.gameControl;
            set => this.gameControl = value;
        }

        /// <summary>
        /// Gets a value indicating whether game is paused or not.
        /// </summary>
        public bool IsGamePaused { get; private set; } = true;

        /// <summary>
        /// Gets the continue game command.
        /// </summary>
        public ICommand ContinueCommand { get; private set; }

        /// <summary>
        /// Gets the savegame command.
        /// </summary>
        public ICommand SaveGameCommand { get; private set; }

        /// <summary>
        /// Gets or sets the background property.
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
        /// Sets the game control.
        /// </summary>
        /// <param name="gc">GameControl entity.</param>
        public void SetGameControl(IGameControl gc)
        {
            this.gameControl = gc;

            this.MenuVisibility = Visibility.Visible;

            if (this.mainVM.AppIsServer)
            {
                this.gameControl.GameHasEndedNet += this.mainVM.TcpGameServer.SendGameOverPacket;
            }

            this.gameControl.PauseEvent += this.PauseHandler;
            if (this.mainVM.AppIsServer)
            {
                this.mainVM.TcpGameServer.Gamecontrol = gc;
                this.gameControl.PauseEvent += this.mainVM.TcpGameServer.StartPausePacket;
            }
            else
            {
                this.mainVM.TcpGameClient.Gamecontrol = gc;
            }

            if (this.MainVm.GlobalSaveFile != null)
            {
                this.gameControl.LoadGame(this.MainVm.GlobalSaveFile, this.MainVm.GlobatTeamId, this.MainVm.GlobalProfile, this.MainVm.AppIsServer, this.mainVM.IpAddress);
            }
            else
            {
                this.gameControl.StartNewGame(this.MainVm.GlobalMap, this.MainVm.GlobatTeamId, this.MainVm.GlobalUnitCount, this.MainVm.GlobalProfile, this.MainVm.AppIsServer, this.mainVM.IpAddress);
            }
        }

        /// <summary>
        /// Cleares every dependency by removing them.
        /// </summary>
        public void DeleteEverything()
        {
            this.mainVM = null;
            this.gameControl.DeleteEverything();
            this.gameControl = null;
            this.RemoveUC?.Invoke();
            this.RemoveUC = null;
        }

        /// <summary>
        /// Refresh screen.
        /// </summary>
        public void InvalidateVisual()
        {
            this.gameControl.InvalidateVisual();
        }

        private void PauseHandler()
        {
            if (this.gameControl.ConnectionLost)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.MainVm.LobbyMenuCommand.Execute(null);
                });
            }
            else
            {
                if (this.IsGamePaused)
                {
                    this.IsGamePaused = false;
                    this.MenuVisibility = Visibility.Collapsed;
                }
                else
                {
                    this.IsGamePaused = true;
                    this.MenuVisibility = Visibility.Visible;
                }
            }
        }
    }
}
