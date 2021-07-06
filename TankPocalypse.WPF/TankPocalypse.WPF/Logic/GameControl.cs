// <copyright file="GameControl.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using TankPocalypse.Logic;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.Renderer.Interfaces;
    using TankPocalypse.WPF.Interfaces;

    /// <summary>
    /// This is the game control class.
    /// </summary>
    public class GameControl : FrameworkElement, IGameControl
    {
        private IGameRenderer gameRenderer;
        private IGameLogic gameLogic;
        private IGameInputController gameInput;
        private Window mainWindow;
        private bool isGameStarted;
        private bool isServer;
        private Dictionary<string, Cursor> cursorCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControl"/> class.
        /// </summary>
        /// <param name="minimal">Indicates if constructor was called for repository actions.</param>
        public GameControl(bool minimal)
        {
            System.Diagnostics.Debug.WriteLine("Minimal gamectrol created with parameter :: " + minimal);
            this.gameLogic = new GameLogic();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControl"/> class.
        /// </summary>
        public GameControl()
        {
            this.gameLogic = TankIoc.Instance.GetInstanceWithoutCaching<IGameLogic>();
            this.gameRenderer = TankIoc.Instance.GetInstanceWithoutCaching<IGameRenderer>();
            this.gameInput = TankIoc.Instance.GetInstanceWithoutCaching<IGameInputController>();
            this.gameLogic.SubscribeInvalidateMethod(this.UpdateScreen);
            this.gameInput.SubscribeToRequestMousePosEvent(this.GiveCurrentMousePosToLogic);
            this.gameInput.SubscribeToZoomRateChangedEvent(this.gameRenderer.ScreenZoomRateMethod);
            this.gameLogic.SubscribeToGameEndedEvent(this.GameEnded);
            this.SetupCursors();

            this.Loaded += this.GameControl_Loaded;
        }

        /// <inheritdoc/>
        public event Action PauseEvent;

        /// <inheritdoc/>
        public event Action GameHasEndedNet;

        /// <inheritdoc/>
        public bool IsGameEnded { get; set; }

        /// <inheritdoc/>
        public bool ConnectionLost
        {
            get { return this.gameLogic.ConnectionLost; }
        }

        /// <inheritdoc/>
        public void StartNewGame(IUIMap mapToLoad, byte teamId, byte unitCount, IUIProfile globProfile, bool isServer, string ip)
        {
            if (isServer)
            {
                this.isServer = true;
                this.gameLogic.SetAppAsServer();
            }

            this.SetTeamID(teamId);
            this.gameLogic.SetGlobalProfile(globProfile);
            this.gameLogic.StartNewGame(mapToLoad, unitCount);
            this.gameInput.ApplyWorldCenterCameraPosition((int)this.ActualWidth, (int)this.ActualHeight);
            this.gameRenderer.SetupRender((int)this.ActualWidth, (int)this.ActualHeight);
            if (isServer)
            {
                this.gameLogic.StartUdpServer();
            }
            else
            {
                this.gameLogic.StartUdpClient(ip);
            }

            this.InvalidateVisual();
        }

        /// <inheritdoc/>
        public void LoadGame(IUISavedGame saveFile, byte teamId, IUIProfile globProfile, bool isServer, string ip)
        {
            if (isServer)
            {
                this.isServer = true;
                this.gameLogic.SetAppAsServer();
            }

            this.SetTeamID(teamId);
            this.gameLogic.SetGlobalProfile(globProfile);
            this.gameLogic.LoadGame(saveFile);
            this.gameInput.ApplyWorldCenterCameraPosition((int)this.ActualWidth, (int)this.ActualHeight);
            this.gameRenderer.SetupRender((int)this.ActualWidth, (int)this.ActualHeight);
            if (isServer)
            {
                this.gameLogic.StartUdpServer();
            }
            else
            {
                this.gameLogic.StartUdpClient(ip);
            }

            this.InvalidateVisual();
        }

        /// <inheritdoc/>
        public void SetTeamID(byte teamID)
        {
            this.gameLogic.SetTeamID(teamID);
        }

        /// <inheritdoc/>
        public void SetEnd()
        {
            this.gameLogic.GameOver = true;
        }

        /// <inheritdoc/>
        public void DeleteEverything()
        {
            if (this.isServer)
            {
                this.gameLogic.StopUdpServer();
            }
            else
            {
                this.gameLogic.StopUdpClient();
            }

            this.gameLogic.StopGameTimer();
            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.KeyDown -= this.gameInput.KeyDown;
                win.KeyDown -= this.KeyDownGameControl;
                win.KeyUp -= this.gameInput.KeyUp;

                // win.MouseWheel -= this.gameInput.MouseWheelEvent;
                win.MouseLeftButtonDown -= this.gameInput.MouseLeftButtonDown;
                win.MouseRightButtonDown -= this.gameInput.MouseRightButtonDown;
                win.MouseLeftButtonUp -= this.gameInput.MouseLeftButtonUp;
                win.MouseRightButtonUp -= this.gameInput.MouseRightButtonUp;
                win.MouseDown -= this.gameInput.MouseButtonDown;
                win.MouseUp -= this.gameInput.MouseButtonUp;
                win.SizeChanged -= this.gameInput.UpdateWindowDimensions;
            }

            this.gameLogic.DeleteEverything();
            this.gameLogic = null;
            this.gameRenderer.DeleteEverything();
            this.gameRenderer = null;
            this.gameInput = null;
            this.PauseEvent = null;
            GC.Collect();
        }

        /// <inheritdoc/>
        public void PauseGame()
        {
            this.gameLogic.PauseGame(); // Logic felé menő hívás;
            this.Dispatcher.Invoke(() =>
            {
                this.mainWindow.Cursor = this.cursorCollection["none"];
            });

            if (this.isServer)
            {
                if (!this.isGameStarted)
                {
                    this.gameLogic.StartSessionTimer();
                    this.isGameStarted = true;
                }
                else
                {
                    this.gameLogic.PauseSessionTimer();
                }
            }

            this.PauseEvent?.Invoke();
        }

        /// <inheritdoc/>
        public void SaveGame()
        {
            this.gameLogic.SaveGame();
        }

        /// <inheritdoc/>
        public ObservableCollection<IUIProfile> GetProfiles()
        {
            return this.gameLogic.GetAllProfiles();
        }

        /// <inheritdoc/>
        public void DeleteProfile(IUIProfile selectedProfile)
        {
            this.gameLogic.DeleteProfile(selectedProfile.UserId);
        }

        /// <inheritdoc/>
        public void AddNewProfile(IUIProfile newProfile)
        {
            this.gameLogic.AddNewProfile(newProfile);
        }

        /// <inheritdoc/>
        public List<IUIMap> GetAllMaps()
        {
            return this.gameLogic.GetAllMaps();
        }

        /// <inheritdoc/>
        public List<IUISavedGame> GetAllSaves()
        {
            return this.gameLogic.GetAllSavedGames();
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            this.IsHitTestVisible = false;
            drawingContext.DrawDrawing(this.gameRenderer.CreateOnScreenDrawing());
            var mousePos = Mouse.GetPosition(null);
            this.gameInput.SetMousePos(mousePos.X, mousePos.Y);
        }

        private void ChangeCursorMethod(string index)
        {
            this.Dispatcher.InvokeAsync(() =>
            {
                switch (index)
                {
                    case "none":
                        this.mainWindow.Cursor = this.cursorCollection["none"];
                        break;
                    case "select":
                        this.mainWindow.Cursor = this.cursorCollection["select"];
                        break;
                    case "enemy":
                        this.mainWindow.Cursor = this.cursorCollection["enemy"];
                        break;
                    case "attack":
                        this.mainWindow.Cursor = this.cursorCollection["attack"];
                        break;
                    case "move":
                        this.mainWindow.Cursor = this.cursorCollection["move"];
                        break;
                    default:
                        this.mainWindow.Cursor = this.cursorCollection["none"];
                        break;
                }
            });
        }

        private void SetupCursors()
        {
            this.cursorCollection = new Dictionary<string, Cursor>();

            var tempResource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Cursors.none.cur");
            this.cursorCollection.Add("none", new Cursor(tempResource));

            tempResource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Cursors.enemy.cur");
            this.cursorCollection.Add("enemy", new Cursor(tempResource));

            tempResource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Cursors.move.cur");
            this.cursorCollection.Add("move", new Cursor(tempResource));

            tempResource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Cursors.attack.cur");
            this.cursorCollection.Add("attack", new Cursor(tempResource));

            tempResource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TankPocalypse.WPF.Images.Cursors.select.cur");
            this.cursorCollection.Add("select", new Cursor(tempResource));
        }

        private void KeyDownGameControl(object sender, KeyEventArgs e)
        {
            if (this.isServer && !this.IsGameEnded && (e.Key == Key.Pause || e.Key == Key.Escape))
            {
                this.PauseGame();
            }
        }

        private void GameEnded()
        {
            this.IsGameEnded = true;

            this.Dispatcher.Invoke(() =>
            {
                this.mainWindow.Cursor = this.cursorCollection["none"];
                if (this.gameLogic.ConnectionLost)
                {
                    this.PauseEvent?.Invoke();
                }
                else
                {
                    this.GameHasEndedNet?.Invoke();
                    this.PauseEvent?.Invoke();
                }
            });
        }

        private void GiveCurrentMousePosToLogic()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    var mousePos = Mouse.GetPosition(null);
                    this.gameInput?.SetMousePos(mousePos.X, mousePos.Y);
                });
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                // well then
            }
        }

        private void GameControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window win = Window.GetWindow(this);
            this.mainWindow = win;
            if (win != null)
            {
                win.KeyDown += this.gameInput.KeyDown;
                win.KeyDown += this.KeyDownGameControl;
                win.KeyUp += this.gameInput.KeyUp;

                // win.MouseWheel += this.gameInput.MouseWheelEvent;
                win.MouseLeftButtonDown += this.gameInput.MouseLeftButtonDown;
                win.MouseRightButtonDown += this.gameInput.MouseRightButtonDown;
                win.MouseLeftButtonUp += this.gameInput.MouseLeftButtonUp;
                win.MouseRightButtonUp += this.gameInput.MouseRightButtonUp;
                win.MouseDown += this.gameInput.MouseButtonDown;
                win.MouseUp += this.gameInput.MouseButtonUp;
                win.SizeChanged += this.gameInput.UpdateWindowDimensions;
                win.Focus();
            }

            this.gameInput.SubscribeToCursorChangedEvent(this.ChangeCursorMethod);
        }

        private void UpdateScreen()
        {
            this.Dispatcher.InvokeAsync(this.InvalidateVisual);
        }
    }
}
