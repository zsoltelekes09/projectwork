// <copyright file="MenuImages.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using TankPocalypse.WPF.Interfaces;

    /// <summary>
    /// Setup Menu Images.
    /// </summary>
    public class MenuImages : IMenuImages
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuImages"/> class.
        /// </summary>
        public MenuImages()
        {
            this.SetupBackgrounds();
            this.SetupButtons();

            ControlTemplate newCont = new ControlTemplate();
        }

        /// <inheritdoc/>
        public Dictionary<string, BitmapImage> Backgrounds { get; private set; }

        /// <inheritdoc/>
        public Dictionary<string, BitmapImage> Buttons { get; private set; }

        private void SetupBackgrounds()
        {
            this.Backgrounds = new Dictionary<string, BitmapImage>();

            BitmapImage bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Background.main_menu_bg.png");
            bitmapImg.EndInit();
            this.Backgrounds.Add("main", bitmapImg);

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Background.new_profile_bg_final.png");
            bitmapImg.EndInit();
            this.Backgrounds.Add("new", bitmapImg);

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Background.client_menu_bg_rdy.png");
            bitmapImg.EndInit();
            this.Backgrounds.Add("client", bitmapImg);

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Background.server_menu_bg_rdy.png");
            bitmapImg.EndInit();
            this.Backgrounds.Add("server", bitmapImg);

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Background.load_menu_bg_rdy.png");
            bitmapImg.EndInit();
            this.Backgrounds.Add("load", bitmapImg);

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Background.highscore_menu_bg_rdy.png");
            bitmapImg.EndInit();
            this.Backgrounds.Add("highscore", bitmapImg);

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Background.profile_menu_bg_rdy.png");
            bitmapImg.EndInit();
            this.Backgrounds.Add("profile", bitmapImg);

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Button.tmpButton_hover_def.png");
            bitmapImg.EndInit();
            this.Backgrounds.Add("blueteam", bitmapImg);

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Button.red_team_textbg.png");
            bitmapImg.EndInit();
            this.Backgrounds.Add("redteam", bitmapImg);

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Background.lobby_menu_bg_rdy.png");
            bitmapImg.EndInit();
            this.Backgrounds.Add("lobby", bitmapImg);

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Background.game_view_bg_2.png");
            bitmapImg.EndInit();
            this.Backgrounds.Add("game", bitmapImg);
        }

        private void SetupButtons()
        {
            this.Buttons = new Dictionary<string, BitmapImage>();

            BitmapImage bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Button.button_static_final.png");
            bitmapImg.EndInit();
            this.Buttons.Add("default", bitmapImg);

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Button.tmpButton_hover_def.png");
            bitmapImg.EndInit();
            this.Buttons.Add("hover", bitmapImg);
        }
    }
}
