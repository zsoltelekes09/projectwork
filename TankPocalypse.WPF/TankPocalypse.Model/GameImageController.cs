// <copyright file="GameImageController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Media.Imaging;
    using TankPocalypse.Model.Interfaces;

    /// <summary>
    /// The game image controller class stores access to all the embended resorce images.
    /// </summary>
    public class GameImageController : IGameImageController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameImageController"/> class.
        /// </summary>
        public GameImageController()
        {
            System.Diagnostics.Debug.WriteLine("GameImageController created;");
            this.SetupResources();
            this.SetupVehicleTextures();
            this.SetupHealthBarTextures();
            this.SetupOtherTextures();
            this.SetupShootingTextures();
            this.SetupExplosionTextures();
            this.SetupFlameTextures();
        }

        /// <summary>
        /// Gets the node textures dicitonary.
        /// </summary>
        public Dictionary<int, BitmapImage> NodeTextures { get; private set; }

        /// <summary>
        /// Gets the object textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> ObjectTextures { get; private set; }

        /// <summary>
        /// Gets the debug textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> DebugTextures { get; private set; }

        /// <summary>
        /// Gets the tank team0 body textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> TankBodyTextures { get; private set; }

        /// <summary>
        /// Gets the tank team0 turret textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> TankTurretTextures { get; private set; }

        /// <summary>
        /// Gets the tank team1 body textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> Team1BodyTextures { get; private set; }

        /// <summary>
        /// Gets the tank team1 turret textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> Team1TurretTextures { get; private set; }

        /// <summary>
        /// Gets the shooting animation textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> Shooting { get; private set; }

        /// <summary>
        /// Gets the healthbar textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> HealthBarTextures { get; private set; }

        /// <summary>
        /// Gets the other textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> OtherTextures { get; private set; }

        /// <summary>
        /// Gets the UI textures dictionary.
        /// </summary>
        public Dictionary<int, BitmapImage> UITextures { get; private set; }

        /// <inheritdoc/>
        public Dictionary<int, BitmapImage> ExplosionTextures { get; private set; }

        /// <inheritdoc/>
        public Dictionary<int, BitmapImage> FlameTextures { get; private set; }

        /// <summary>
        /// Gets the pause screen backgorund.
        /// </summary>
        public BitmapImage PauseBackgound { get; private set; }

        /// <summary>
        /// Clears every texture reference.
        /// </summary>
        public void DeleteEverything()
        {
            this.NodeTextures.Clear();
            this.NodeTextures = null;
            this.ObjectTextures.Clear();
            this.ObjectTextures = null;
            this.DebugTextures.Clear();
            this.DebugTextures = null;
            this.TankBodyTextures.Clear();
            this.TankBodyTextures = null;
            this.TankTurretTextures.Clear();
            this.TankTurretTextures = null;
            this.Team1BodyTextures.Clear();
            this.Team1BodyTextures = null;
            this.Team1TurretTextures.Clear();
            this.Team1TurretTextures = null;
            this.Shooting.Clear();
            this.Shooting = null;
            this.HealthBarTextures.Clear();
            this.HealthBarTextures = null;
            this.OtherTextures.Clear();
            this.OtherTextures = null;
            this.UITextures.Clear();
            this.UITextures = null;
            this.PauseBackgound = null;
        }

        private void SetupResources()
        {
            // ******************************************************************************** //
            // **********  G R O U N D _ T E X T U R E S ************************************** //
            // ******************************************************************************** //
            this.NodeTextures = new Dictionary<int, BitmapImage>();

            // ******** '0' idx === GrassHidden ******** //
            BitmapImage tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.Hidden.tileGrassBnw.png");
            tmpBmp.EndInit();
            this.NodeTextures.Add(0, tmpBmp);

            // ******** '1' idx === GrassRevealed ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.Revealed.tileGrass.png");
            tmpBmp.EndInit();
            this.NodeTextures.Add(1, tmpBmp);

            // ******** '2' idx === WaterHidden ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.Hidden.tileWaterBnw.png");
            tmpBmp.EndInit();
            this.NodeTextures.Add(2, tmpBmp);

            // ******** '3' idx === WaterRevealed ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.Revealed.tileWater.png");
            tmpBmp.EndInit();
            this.NodeTextures.Add(3, tmpBmp);

            // ******** '4' idx === Team0BaseRevealed ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.Revealed.base0grass.png");
            tmpBmp.EndInit();
            this.NodeTextures.Add(4, tmpBmp);

            // ******** '5' idx === Team1BaseRevealed ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.Revealed.base1grass.png");
            tmpBmp.EndInit();
            this.NodeTextures.Add(5, tmpBmp);

            // ******************************************************************************** //
            // **********  O B J E C T _ T E X T U R E S ************************************** //
            // ******************************************************************************** //
            this.ObjectTextures = new Dictionary<int, BitmapImage>();

            // ******** '0' idx === Forest_hidden ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.Hidden.forestBnw.png");
            tmpBmp.EndInit();
            this.ObjectTextures.Add(0, tmpBmp);

            // ******** '1' idx === Forest_revealed ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.Revealed.forest.png");
            tmpBmp.EndInit();
            this.ObjectTextures.Add(1, tmpBmp);

            // ******** '2' idx === Forest_revealed ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.transparentForest.png");
            tmpBmp.EndInit();
            this.ObjectTextures.Add(2, tmpBmp);

            // ******** '3' idx === Forest_revealed ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.purple.png");
            tmpBmp.EndInit();
            this.ObjectTextures.Add(3, tmpBmp);

            // ***************************************************************************** //
            // ********** D E B U G _ T E X T U R E S ************************************** //
            // ***************************************************************************** //
            this.DebugTextures = new Dictionary<int, BitmapImage>();

            // ******** '0' idx === Arrow_down ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.arrow.png");
            tmpBmp.EndInit();
            this.DebugTextures.Add(0, tmpBmp);

            // ******** '1' idx === Purple ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.purple.png");
            tmpBmp.EndInit();
            this.DebugTextures.Add(1, tmpBmp);

            // ******** '2' idx === Transparent ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.StaticWorld.transparentForest.png");
            tmpBmp.EndInit();
            this.DebugTextures.Add(2, tmpBmp);

            // ***************************************************************************** //
            // ************ M E N U _ T E X T U R E S ************************************** //
            // ***************************************************************************** //
            this.UITextures = new Dictionary<int, BitmapImage>();

            // ******** '0' idx === BlueTankUI ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.UI.blueUItank.png");
            tmpBmp.EndInit();
            this.UITextures.Add(0, tmpBmp);

            // ******** '1' idx === RedTankUI ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.UI.redUItank.png");
            tmpBmp.EndInit();
            this.UITextures.Add(1, tmpBmp);

            // ******** '2' idx === DeadTankUI ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.UI.deadUItank.png");
            tmpBmp.EndInit();
            this.UITextures.Add(2, tmpBmp);

            // ******** '3' idx === UI_Overlay ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.UI.UIoverlay.png");
            tmpBmp.EndInit();
            this.UITextures.Add(3, tmpBmp);

            // ******** '4' idx === UI_Tank_Selection ******** //
            tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.UI.UItankSelect.png");
            tmpBmp.EndInit();
            this.UITextures.Add(4, tmpBmp);

            this.PauseBackgound = new BitmapImage();
            this.PauseBackgound.BeginInit();
            this.PauseBackgound.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.Model.Images.ingame_pause_bg_rdy.png");
            this.PauseBackgound.EndInit();
        }

        private void SetupVehicleTextures()
        {
            this.TankBodyTextures = new Dictionary<int, BitmapImage>();
            this.TankTurretTextures = new Dictionary<int, BitmapImage>();

            this.Team1BodyTextures = new Dictionary<int, BitmapImage>();
            this.Team1TurretTextures = new Dictionary<int, BitmapImage>();

            for (int i = 0; i < 360; i += 5)
            {
                BitmapImage tmpBmp = new BitmapImage();
                tmpBmp.BeginInit();
                string path = "TankPocalypse.Model.Images.Units.Team0.Tank.Body." + i + ".png";
                tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                tmpBmp.EndInit();
                this.TankBodyTextures.Add(i, tmpBmp);

                tmpBmp = new BitmapImage();
                tmpBmp.BeginInit();
                path = "TankPocalypse.Model.Images.Units.Team0.Tank.Turret." + i + ".png";
                tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                tmpBmp.EndInit();
                this.TankTurretTextures.Add(i, tmpBmp);

                tmpBmp = new BitmapImage();
                tmpBmp.BeginInit();
                path = "TankPocalypse.Model.Images.Units.Team1.Tank.Body." + i + ".png";
                tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                tmpBmp.EndInit();
                this.Team1BodyTextures.Add(i, tmpBmp);

                tmpBmp = new BitmapImage();
                tmpBmp.BeginInit();
                path = "TankPocalypse.Model.Images.Units.Team1.Tank.Turret." + i + ".png";
                tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                tmpBmp.EndInit();
                this.Team1TurretTextures.Add(i, tmpBmp);
            }
        }

        private void SetupHealthBarTextures()
        {
            this.HealthBarTextures = new Dictionary<int, BitmapImage>();

            for (int i = 0; i <= 100; i = i + 5)
            {
                BitmapImage tmpBmp = new BitmapImage();
                tmpBmp.BeginInit();
                string path = "TankPocalypse.Model.Images.Units.Healthbar." + i + ".png";
                tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                tmpBmp.EndInit();
                this.HealthBarTextures.Add(i, tmpBmp);
            }
        }

        private void SetupOtherTextures()
        {
            this.OtherTextures = new Dictionary<int, BitmapImage>();

            BitmapImage tmpBmp = new BitmapImage();
            tmpBmp.BeginInit();
            string path = "TankPocalypse.Model.Images." + "selection_ring" + ".png";
            tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            tmpBmp.EndInit();
            this.OtherTextures.Add(0, tmpBmp);
        }

        private void SetupShootingTextures()
        {
            this.Shooting = new Dictionary<int, BitmapImage>();

            for (int i = 0; i < 5; i++)
            {
                BitmapImage tmpBmp = new BitmapImage();
                tmpBmp.BeginInit();
                string path = "TankPocalypse.Model.Images.Units.Shooting." + i + ".png";
                tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                tmpBmp.EndInit();
                this.Shooting.Add(i + 1, tmpBmp);
            }
        }

        private void SetupExplosionTextures()
        {
            this.ExplosionTextures = new Dictionary<int, BitmapImage>();

            for (int i = 0; i < 8; i++)
            {
                BitmapImage tmpBmp = new BitmapImage();
                tmpBmp.BeginInit();
                string path = "TankPocalypse.Model.Images.Units.Explosion." + i + ".png";
                tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                tmpBmp.EndInit();
                this.ExplosionTextures.Add(i, tmpBmp);
            }
        }

        private void SetupFlameTextures()
        {
            this.FlameTextures = new Dictionary<int, BitmapImage>();

            for (int i = 0; i < 7; i++)
            {
                BitmapImage tmpBmp = new BitmapImage();
                tmpBmp.BeginInit();
                string path = "TankPocalypse.Model.Images.Units.Flame." + i + ".png";
                tmpBmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                tmpBmp.EndInit();
                this.FlameTextures.Add(i, tmpBmp);
            }
        }
    }
}
