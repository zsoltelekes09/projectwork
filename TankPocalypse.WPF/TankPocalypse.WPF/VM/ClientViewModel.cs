// <copyright file="ClientViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.VM
{
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using TankPocalypse.WPF.Logic.Networking;

    /// <summary>
    /// Client UserControls view model.
    /// </summary>
    public class ClientViewModel : ViewModelBase
    {
        private MainViewModel mainVM;
        private string ipAddress;
        private BitmapImage background;
        private BitmapImage buttonDefault;
        private BitmapImage buttonHover;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientViewModel"/> class.
        /// </summary>
        /// <param name="mainVM">Main viewmodel.</param>
        public ClientViewModel(MainViewModel mainVM)
        {
            this.mainVM = mainVM;
            this.IpAddress = "127.0.0.1";
            this.Background = this.MainVm.MenuImages.Backgrounds["client"];
            this.ButtonDefault = this.MainVm.MenuImages.Buttons["default"];
            this.ButtonHover = this.MainVm.MenuImages.Buttons["hover"];

            this.mainVM.AppIsServer = false;

            this.JoinLobbyCommand = new RelayCommand(() =>
            {
                this.mainVM.TcpGameClient = new TcpGameClient(this.mainVM);
                this.mainVM.TcpGameClient.Connect(this.ipAddress);

                Task.Run(() =>
                {
                    this.mainVM.StartClient(this.IpAddress);
                });

                if (this.mainVM.TcpGameClient.Running)
                {
                    this.mainVM.LobbyMenuCommand.Execute(null);
                }
            });
        }

        /// <summary>
        /// Gets or sets the IpAddress of the connection.
        /// </summary>
        public string IpAddress
        {
            get => this.ipAddress;
            set => this.Set(ref this.ipAddress, value);
        }

        /// <summary>
        /// Gets the join lobby command.
        /// </summary>
        public ICommand JoinLobbyCommand { get; private set; }

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
