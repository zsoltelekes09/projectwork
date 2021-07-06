// <copyright file="MenuViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.VM
{
    using System.Windows.Media.Imaging;
    using GalaSoft.MvvmLight;

    /// <summary>
    /// Menu usercontrols viewmodel.
    /// </summary>
    public class MenuViewModel : ViewModelBase
    {
        private MainViewModel mainVM;
        private BitmapImage background;
        private BitmapImage buttonDefault;
        private BitmapImage buttonHover;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuViewModel"/> class.
        /// </summary>
        /// <param name="mainVM">Main viewmodel.</param>
        public MenuViewModel(MainViewModel mainVM)
        {
            this.mainVM = mainVM;
            this.Background = this.MainVm.MenuImages.Backgrounds["main"];
            this.ButtonDefault = this.MainVm.MenuImages.Buttons["default"];
            this.ButtonHover = this.MainVm.MenuImages.Buttons["hover"];
            this.MainVm.GlobalSaveFile = null;
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
