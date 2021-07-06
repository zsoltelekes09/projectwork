// <copyright file="NewProfileViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.VM
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.Logic.UIClasses;
    using TankPocalypse.WPF.Interfaces;
    using TankPocalypse.WPF.Logic;

    /// <summary>
    /// NewProfile usercontrols viewmodel.
    /// </summary>
    public class NewProfileViewModel : ViewModelBase
    {
        private Visibility backVisibility;
        private IUIProfile newProfile;
        private IGameControl gameControl;
        private string textboxContent;
        private BitmapImage background;
        private BitmapImage buttonDefault;
        private BitmapImage buttonHover;
        private MainViewModel mainVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewProfileViewModel"/> class.
        /// </summary>
        /// <param name="mainVM">Main viewmodel.</param>
        public NewProfileViewModel(MainViewModel mainVM)
        {
            this.mainVM = mainVM;
            this.gameControl = new GameControl(true);
            this.Background = this.MainVm.MenuImages.Backgrounds["new"];
            this.ButtonDefault = this.MainVm.MenuImages.Buttons["default"];
            this.ButtonHover = this.MainVm.MenuImages.Buttons["hover"];

            if (this.mainVM.NewProfileNeeded)
            {
                this.BackVisibility = Visibility.Collapsed;
            }
            else
            {
                this.BackVisibility = Visibility.Visible;
            }

            this.AddCommand = new RelayCommand(() =>
            {
                if (!string.IsNullOrEmpty(this.TextboxContent))
                {
                    if (this.gameControl.GetProfiles().All(x => x.UserName != this.TextboxContent))
                    {
                        IUIProfile profile = new UIProfile(this.TextboxContent, this.gameControl.GetProfiles().Count);
                        this.gameControl.AddNewProfile(profile);
                        if (this.mainVM.NewProfileNeeded)
                        {
                            this.mainVM.NewProfileNeeded = false;
                            this.mainVM.GlobalProfile = profile;
                            this.mainVM.MainMenuCommand.Execute(null);
                        }
                        else
                        {
                            this.mainVM.ProfileMenuCommand.Execute(null);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Gets the add new porofile command.
        /// </summary>
        public ICommand AddCommand { get; private set; }

        /// <summary>
        /// Gets or sets the back button visibility.
        /// </summary>
        public Visibility BackVisibility
        {
            get => this.backVisibility;
            set => this.Set(ref this.backVisibility, value);
        }

        /// <summary>
        /// Gets or sets the new profile property.
        /// </summary>
        public IUIProfile NewProfile
        {
            get => this.newProfile;
            set => this.Set(ref this.newProfile, value);
        }

        /// <summary>
        /// Gets or sets the iput field content.
        /// </summary>
        public string TextboxContent
        {
            get => this.textboxContent;
            set => this.Set(ref this.textboxContent, value);
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
