// <copyright file="ProfileViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.VM
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.WPF.Interfaces;
    using TankPocalypse.WPF.Logic;

    /// <summary>
    /// Profile usercontrols viewmodel.
    /// </summary>
    public class ProfileViewModel : ViewModelBase
    {
        private MainViewModel mainVM;
        private IGameControl gameControl;
        private IUIProfile selectedProfile;
        private ObservableCollection<IUIProfile> profiles;
        private BitmapImage background;
        private BitmapImage buttonDefault;
        private BitmapImage buttonHover;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileViewModel"/> class.
        /// </summary>
        /// <param name="mainVM">Main viewmodel.</param>
        public ProfileViewModel(MainViewModel mainVM)
        {
            this.mainVM = mainVM;

            this.Background = this.MainVm.MenuImages.Backgrounds["profile"];
            this.ButtonDefault = this.MainVm.MenuImages.Buttons["default"];
            this.ButtonHover = this.MainVm.MenuImages.Buttons["hover"];

            this.gameControl = new GameControl(true);
            this.profiles = this.gameControl.GetProfiles();
            this.SelectedProfile = this.profiles.FirstOrDefault();

            this.DeleteCommand = new RelayCommand(() =>
            {
                if (this.SelectedProfile != null)
                {
                    this.gameControl.DeleteProfile(this.SelectedProfile);
                    this.profiles.Remove(this.SelectedProfile);
                }
            });

            this.SelectCommand = new RelayCommand(() =>
            {
                this.MainVm.GlobalProfile = this.SelectedProfile;
                this.MainVm.MainMenuCommand.Execute(null);
            });

            this.AddCommand = new RelayCommand(() =>
            {
                this.MainVm.NewProfileCommand.Execute(null);
            });
        }

        /// <summary>
        /// Gets the add profile command.
        /// </summary>
        public ICommand AddCommand { get; private set; }

        /// <summary>
        /// Gets the dele profile command.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        /// <summary>
        /// Gets the select profile command.
        /// </summary>
        public ICommand SelectCommand { get; private set; }

        /// <summary>
        /// Gets or sets the available profiles.
        /// </summary>
        public ObservableCollection<IUIProfile> Profiles
        {
            get => this.profiles;
            set => this.Set(ref this.profiles, value);
        }

        /// <summary>
        /// Gets or sets the selected profile.
        /// </summary>
        public IUIProfile SelectedProfile
        {
            get => this.selectedProfile;
            set => this.Set(ref this.selectedProfile, value);
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
    }
}
