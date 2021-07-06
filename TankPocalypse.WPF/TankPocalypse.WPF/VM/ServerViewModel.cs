// <copyright file="ServerViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.VM
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.WPF.Interfaces;
    using TankPocalypse.WPF.Logic;

    /// <summary>
    /// Server usercontrols viewmodel.
    /// </summary>
    public class ServerViewModel : ViewModelBase
    {
        private MainViewModel mainVM;
        private IGameControl gameControl;
        private IUIMap selectedMap;
        private List<IUIMap> maps;
        private BitmapImage background;
        private BitmapImage buttonDefault;
        private BitmapImage buttonHover;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerViewModel"/> class.
        /// </summary>
        /// <param name="mainVM">Main viewmodel.</param>
        public ServerViewModel(MainViewModel mainVM)
        {
            this.mainVM = mainVM;
            this.mainVM.AppIsServer = true;

            this.Background = this.MainVm.MenuImages.Backgrounds["server"];
            this.ButtonDefault = this.MainVm.MenuImages.Buttons["default"];
            this.ButtonHover = this.MainVm.MenuImages.Buttons["hover"];

            this.gameControl = new GameControl(true);
            this.Maps = this.gameControl.GetAllMaps();
            this.SelectedMap = this.Maps.FirstOrDefault();

            this.StartServerCommand = new RelayCommand(() =>
            {
                this.mainVM.GlobalMap = this.SelectedMap;
                Task.Run(() =>
                {
                    this.mainVM.StartServer();
                });

                this.mainVM.LobbyMenuCommand.Execute(null);
            });
        }

        /// <summary>
        /// Gets the start server command.
        /// </summary>
        public ICommand StartServerCommand { get; private set; }

        /// <summary>
        /// Gets or sets the selected map.
        /// </summary>
        public IUIMap SelectedMap
        {
            get => this.selectedMap;
            set => this.Set(ref this.selectedMap, value);
        }

        /// <summary>
        /// Gets or sets the available maps.
        /// </summary>
        public List<IUIMap> Maps
        {
            get => this.maps;
            set => this.Set(ref this.maps, value);
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
        /// Gets or sets the main viewmodel.
        /// </summary>
        public MainViewModel MainVm
        {
            get => this.mainVM;
            set => this.mainVM = value;
        }
    }
}
