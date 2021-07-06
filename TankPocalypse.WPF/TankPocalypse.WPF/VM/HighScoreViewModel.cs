// <copyright file="HighScoreViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.VM
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media.Imaging;
    using GalaSoft.MvvmLight;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.WPF.Logic;

    /// <summary>
    /// Highscore UserControls viewmodel.
    /// </summary>
    public class HighScoreViewModel : ViewModelBase
    {
        private MainViewModel mainVM;
        private List<IUIProfile> profiles;
        private BitmapImage background;
        private BitmapImage buttonDefault;
        private BitmapImage buttonHover;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighScoreViewModel"/> class.
        /// </summary>
        /// <param name="mainVM">Main viewmodel.</param>
        public HighScoreViewModel(MainViewModel mainVM)
        {
            this.mainVM = mainVM;

            this.Background = this.MainVm.MenuImages.Backgrounds["highscore"];
            this.ButtonDefault = this.MainVm.MenuImages.Buttons["default"];
            this.ButtonHover = this.MainVm.MenuImages.Buttons["hover"];

            this.Profiles = new GameControl(true).GetProfiles().OrderByDescending(x => x.Scores).ToList();
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
        /// Gets or sets the stored profiles.
        /// </summary>
        public List<IUIProfile> Profiles
        {
            get => this.profiles;
            set => this.Set(ref this.profiles, value);
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