// <copyright file="GameRenderer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

[assembly: System.CLSCompliant(false)]

namespace TankPocalypse.Renderer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Numerics;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Model.Units;
    using TankPocalypse.Model.World;
    using TankPocalypse.Renderer.Interfaces;
    using Point = System.Drawing.Point;

    /// <summary>
    /// This is the Game Renderer class. This class handles all the rendering.
    /// </summary>
    public class GameRenderer : IGameRenderer
    {
        private IGameModel gameModel;
        private IGameWorld gameWorld;
        private IGameImageController gameImageController;
        private IAnimationController animationController;

        private Point worldDimensions;
        private Vector2 windowDimensions;
        private Point centerNodeIdx;

        private List<Node> nodesInWindowView = new List<Node>();
        private List<IVehicle> teamVehiclesUI = new List<IVehicle>();

        private SolidColorBrush blackBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private SolidColorBrush redBrush = new SolidColorBrush(Color.FromRgb(250, 0, 0));
        private SolidColorBrush blueBrush = new SolidColorBrush(Color.FromRgb(0, 102, 205));

        /// <summary>
        /// Initializes a new instance of the <see cref="GameRenderer"/> class.
        /// </summary>
        /// <param name="gameModel">GameModel entity.</param>
        /// <param name="gameWorld">GameWorld entity.</param>
        /// <param name="gameImage">GameImgae entity.</param>
        /// <param name="animControl">AnimControl entity.</param>
        public GameRenderer(IGameModel gameModel, IGameWorld gameWorld, IGameImageController gameImage, IAnimationController animControl)
        {
            this.gameModel = gameModel;
            this.gameWorld = gameWorld;
            this.gameImageController = gameImage;
            this.animationController = animControl;
        }

        /// <summary>
        /// Gets or sets the zoomrate of the "camera".
        /// </summary>
        public int ZoomRate { get; set; } = 3;

        /// <summary>
        /// Initialises the gamerenderer.
        /// </summary>
        /// <param name="winX">Windows actual width.</param>
        /// <param name="winY">Windows actual height.</param>
        public void SetupRender(int winX, int winY)
        {
            this.windowDimensions = new Vector2(winX, winY);
            this.worldDimensions = this.gameWorld.GetWorldDimensions();
            this.centerNodeIdx = new Point(int.MaxValue, int.MaxValue);

            foreach (IVehicle vehicle in this.gameModel.Vehicles)
            {
                if (vehicle.TeamId == this.gameModel.TeamID)
                {
                    this.teamVehiclesUI.Add(vehicle);
                }
            }
        }

        /// <summary>
        /// Clear every cached dependencies.
        /// </summary>
        public void DeleteEverything()
        {
            this.gameModel = null;
            this.gameWorld = null;
            this.gameImageController.DeleteEverything();
            this.gameImageController = null;
        }

        /// <summary>
        /// This method updates the zoomrate value of the camera.
        /// </summary>
        /// <param name="zoom">Cameras zoomrate.</param>
        public void ScreenZoomRateMethod(short zoom)
        {
            this.ZoomRate = zoom;
        }

        /// <summary>
        /// Converts world coordinates to ismetric view coordinates.
        /// </summary>
        /// <param name="toConvertVector">Vector to convert.</param>
        /// <returns>Isometric view vector.</returns>
        public Vector2 WorldToScreenSpace(Vector2 toConvertVector)
        {
            var zoomOffset = 25 * this.ZoomRate;
            var startPos = new Vector2(0, 0);
            startPos.X += zoomOffset;
            startPos += this.gameModel.ScreenOffset;

            var toConvertVectorCorrected = toConvertVector * this.ZoomRate;
            var rotatedVector = toConvertVectorCorrected.RotateByAngle(45f);
            var finalVector = ScalarProjectionOnVector(rotatedVector, toConvertVectorCorrected);
            finalVector.Y /= 2;

            return finalVector + startPos;
        }

        /// <summary>
        /// Creates a new drawing to render.
        /// </summary>
        /// <returns>New drawing.</returns>
        public Drawing CreateOnScreenDrawing()
        {
            DrawingGroup dg = new DrawingGroup();
            Vector2 screenOffset = this.gameModel.ScreenOffset;
            List<IReorderable> reorderableObj = new List<IReorderable>();

            this.CalculateNodesInWindow();

            this.CreateNodes(ref dg, screenOffset, ref reorderableObj);

            this.SetVehiclesVisibleOnScreen(ref reorderableObj);

            var orderedEnumerable = reorderableObj.OrderBy(x => x.WorldPosition.X + x.WorldPosition.Y);
            this.gameModel.OnScreenObjects = reorderableObj;

            this.CreateObjects(ref dg, screenOffset, ref orderedEnumerable);

            this.animationController.UpdateAnimations(ref dg);

            this.DrawHUD(ref dg);

            this.DrawBasePercents(ref dg);

            this.DrawPauseAndGameOverScreen(ref dg);

            return dg;
        }

        private static GeometryDrawing DrawText(string textToDraw, float x, float y)
        {
            Brush textBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            Typeface typeface = new Typeface("Arial");

            FormattedText formatted_text = new FormattedText(textToDraw, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, typeface, 24, textBrush, pixelsPerDip: 5)
            {
                TextAlignment = TextAlignment.Center,
            };

            Geometry textGeometry = formatted_text.BuildGeometry(new System.Windows.Point(x, y));
            GeometryDrawing textGeo = new GeometryDrawing(textBrush, null, textGeometry);
            return textGeo;
        }

        private static Vector2 ScalarProjectionOnVector(Vector2 vecA, Vector2 vecB)
        {
            var vecANormalized = Vector2.Normalize(vecA);
            return Vector2.Multiply(Vector2.Dot(vecB, vecANormalized), vecANormalized);
        }

        private void CreateNodes(ref DrawingGroup dg, Vector2 screenOffset, ref List<IReorderable> reorderableObj)
        {
            var layer_0_GrassGroupHidden = new GeometryGroup();
            var layer_1_GrassGroupHidden = new GeometryGroup();

            var layer_0_WaterGroupHidden = new GeometryGroup();
            var layer_1_WaterGroupHidden = new GeometryGroup();

            var layer_0_GrassGroup = new GeometryGroup();
            var layer_1_GrassGroup = new GeometryGroup();

            var layer_0_WaterGroup = new GeometryGroup();
            var layer_1_WaterGroup = new GeometryGroup();

            var layer_0_team0baseGroup = new GeometryGroup();
            var layer_1_team0baseGroup = new GeometryGroup();

            var layer_0_team1baseGroup = new GeometryGroup();
            var layer_1_team1baseGroup = new GeometryGroup();

            foreach (Node node in this.nodesInWindowView)
            {
                var screenPosition = this.ToScreenSpace(node);
                node.ScreenPosition = screenPosition;
                Geometry box = new RectangleGeometry(new Rect(screenPosition.X + screenOffset.X, screenPosition.Y + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate));

                if ((node.GridIdx.X + node.GridIdx.Y) % 2 == 0)
                {
                    switch (node.NodeType)
                    {
                        case 0: // grass;
                            if (node.IsRevealed)
                            {
                                layer_0_GrassGroup.Children.Add(box);
                            }
                            else
                            {
                                layer_0_GrassGroupHidden.Children.Add(box);
                            }

                            break;
                        case 1: // water;
                            if (node.IsRevealed)
                            {
                                layer_0_WaterGroup.Children.Add(box);
                            }
                            else
                            {
                                layer_0_WaterGroupHidden.Children.Add(box);
                            }

                            break;
                        case 8: // team0base;
                            if (node.IsRevealed)
                            {
                                layer_0_team0baseGroup.Children.Add(box);
                            }
                            else
                            {
                                layer_0_GrassGroupHidden.Children.Add(box);
                            }

                            break;
                        case 9: // team1base;
                            if (node.IsRevealed)
                            {
                                layer_0_team1baseGroup.Children.Add(box);
                            }
                            else
                            {
                                layer_0_GrassGroupHidden.Children.Add(box);
                            }

                            break;
                        default: // forest;
                            if (node.IsRevealed)
                            {
                                layer_0_GrassGroup.Children.Add(box);
                            }
                            else
                            {
                                layer_0_GrassGroupHidden.Children.Add(box);
                            }

                            reorderableObj.Add(node);
                            break;
                    }
                }
                else
                {
                    switch (node.NodeType)
                    {
                        case 0: // grass;
                            if (node.IsRevealed)
                            {
                                layer_1_GrassGroup.Children.Add(box);
                            }
                            else
                            {
                                layer_1_GrassGroupHidden.Children.Add(box);
                            }

                            break;
                        case 1: // water;
                            if (node.IsRevealed)
                            {
                                layer_1_WaterGroup.Children.Add(box);
                            }
                            else
                            {
                                layer_1_WaterGroupHidden.Children.Add(box);
                            }

                            break;
                        case 8: // team0base;
                            if (node.IsRevealed)
                            {
                                layer_1_team0baseGroup.Children.Add(box);
                            }
                            else
                            {
                                layer_1_GrassGroupHidden.Children.Add(box);
                            }

                            break;
                        case 9: // team1base;
                            if (node.IsRevealed)
                            {
                                layer_1_team1baseGroup.Children.Add(box);
                            }
                            else
                            {
                                layer_1_GrassGroupHidden.Children.Add(box);
                            }

                            break;
                        default: // forest;
                            if (node.IsRevealed)
                            {
                                layer_1_GrassGroup.Children.Add(box);
                            }
                            else
                            {
                                layer_1_GrassGroupHidden.Children.Add(box);
                            }

                            reorderableObj.Add(node);
                            break;
                    }
                }
            }

            // :::::::::: GRASS AND WATER REVEALED NODES ::::::::::::: //
            ImageBrush imgB = new ImageBrush(this.gameImageController.NodeTextures[1])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0 + screenOffset.X, 0 + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_0_GrassRevealed = new GeometryDrawing(imgB, null, layer_0_GrassGroup);

            imgB = new ImageBrush(this.gameImageController.NodeTextures[1])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect((25 * this.ZoomRate) + screenOffset.X, (12.5f * this.ZoomRate) + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_1_GrassRevealed = new GeometryDrawing(imgB, null, layer_1_GrassGroup);

            ImageBrush waterB = new ImageBrush(this.gameImageController.NodeTextures[3])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0 + screenOffset.X, 0 + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_0_WaterRevealed = new GeometryDrawing(waterB, null, layer_0_WaterGroup);

            waterB = new ImageBrush(this.gameImageController.NodeTextures[3])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect((25 * this.ZoomRate) + screenOffset.X, (12.5f * this.ZoomRate) + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_1_WaterRevealed = new GeometryDrawing(waterB, null, layer_1_WaterGroup);

            // :::::::::: TEAM NODES ::::::::::::: //
            ImageBrush team0Brush = new ImageBrush(this.gameImageController.NodeTextures[4])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0 + screenOffset.X, 0 + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_0_team0base = new GeometryDrawing(team0Brush, null, layer_0_team0baseGroup);

            team0Brush = new ImageBrush(this.gameImageController.NodeTextures[4])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect((25 * this.ZoomRate) + screenOffset.X, (12.5f * this.ZoomRate) + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_1_team0base = new GeometryDrawing(team0Brush, null, layer_1_team0baseGroup);

            ImageBrush team1Brush = new ImageBrush(this.gameImageController.NodeTextures[5])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0 + screenOffset.X, 0 + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_0_team1base = new GeometryDrawing(team1Brush, null, layer_0_team1baseGroup);

            team1Brush = new ImageBrush(this.gameImageController.NodeTextures[5])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect((25 * this.ZoomRate) + screenOffset.X, (12.5f * this.ZoomRate) + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_1_team1base = new GeometryDrawing(team1Brush, null, layer_1_team1baseGroup);

            // :::::::::: HIDDEN NODES ::::::::::::: //
            imgB = new ImageBrush(this.gameImageController.NodeTextures[0])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0 + screenOffset.X, 0 + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_0_GrassHidden = new GeometryDrawing(imgB, null, layer_0_GrassGroupHidden);

            imgB = new ImageBrush(this.gameImageController.NodeTextures[0])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect((25 * this.ZoomRate) + screenOffset.X, (12.5f * this.ZoomRate) + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_1_GrassHidden = new GeometryDrawing(imgB, null, layer_1_GrassGroupHidden);

            waterB = new ImageBrush(this.gameImageController.NodeTextures[2])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0 + screenOffset.X, 0 + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_0_WaterHidden = new GeometryDrawing(waterB, null, layer_0_WaterGroupHidden);

            waterB = new ImageBrush(this.gameImageController.NodeTextures[2])
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect((25 * this.ZoomRate) + screenOffset.X, (12.5f * this.ZoomRate) + screenOffset.Y, 50 * this.ZoomRate, 25 * this.ZoomRate),
                ViewportUnits = BrushMappingMode.Absolute,
            };
            var layer_1_WaterHidden = new GeometryDrawing(waterB, null, layer_1_WaterGroupHidden);

            // :::::::::: ADDING LAYERS TO DRAWINGGROUP ::::::::::::: //
            dg.Children.Add(layer_0_GrassRevealed);
            dg.Children.Add(layer_1_GrassRevealed);
            dg.Children.Add(layer_0_WaterRevealed);
            dg.Children.Add(layer_1_WaterRevealed);
            dg.Children.Add(layer_0_GrassHidden);
            dg.Children.Add(layer_1_GrassHidden);
            dg.Children.Add(layer_0_WaterHidden);
            dg.Children.Add(layer_1_WaterHidden);
            dg.Children.Add(layer_0_team0base);
            dg.Children.Add(layer_1_team0base);
            dg.Children.Add(layer_0_team1base);
            dg.Children.Add(layer_1_team1base);
        }

        private void SetVehiclesVisibleOnScreen(ref List<IReorderable> reorderableObj)
        {
            foreach (IVehicle v in this.gameModel.Vehicles)
            {
                var nodeIdx = v.GetNodeIdxVehicleIsOn();
                if (this.gameWorld.WorldGrid[nodeIdx.Y, nodeIdx.X].IsVisible)
                {
                    reorderableObj.Add(v as IReorderable);
                }
                else
                {
                    reorderableObj.Remove(v as IReorderable);
                }
            }
        }

        private void CreateObjects(ref DrawingGroup dg, Vector2 screenOffset, ref IOrderedEnumerable<IReorderable> orderedObjects)
        {
            var vehiclesOnScreen = new List<IReorderable>();
            var clickableOnScreen = new List<IClickableObject>();

            foreach (IReorderable item in orderedObjects)
            {
                switch (item.GetObjectType)
                {
                    case 0: // item is a node;
                        Node nodeItem = (Node)item;

                        BitmapImage forestTexture;
                        if (nodeItem.IsRevealed)
                        {
                            forestTexture = this.gameImageController.ObjectTextures[1];
                        }
                        else
                        {
                            forestTexture = this.gameImageController.ObjectTextures[0];
                        }

                        Geometry forestBox =
                            new RectangleGeometry(new Rect(nodeItem.ScreenPosition.X + screenOffset.X, nodeItem.ScreenPosition.Y - (12 * this.ZoomRate) + screenOffset.Y, 50 * this.ZoomRate, 32 * this.ZoomRate));
                        GeometryDrawing forestDrawing =
                            new GeometryDrawing(new ImageBrush(forestTexture), null, forestBox);

                        nodeItem.ObjectScreenStartPosition = new Vector2(
                            nodeItem.ScreenPosition.X + screenOffset.X,
                            nodeItem.ScreenPosition.Y - (12 * this.ZoomRate) + screenOffset.Y);
                        nodeItem.ObjectScreenEndPosition = new Vector2(nodeItem.ObjectScreenStartPosition.X + (50 * this.ZoomRate), nodeItem.ObjectScreenStartPosition.Y + (32 * this.ZoomRate));

                        dg.Children.Add(forestDrawing);

                        break;
                    case 1: // item is a vehicle;

                        IVehicle vehicle = (IVehicle)item;

                        var nodeIdx = vehicle.GetNodeIdxVehicleIsOn();
                        if (!this.gameWorld.WorldGrid[nodeIdx.Y, nodeIdx.X].IsRevealed)
                        {
                            continue;
                        }

                        Vector2 screenPos = this.WorldToScreenSpace(vehicle.Position);
                        int bodyIdx = vehicle.BodyIdx;
                        int turretIdx = vehicle.TurretIdx;

                        ImageBrush bodyBrush;
                        ImageBrush turretBrush;

                        if (vehicle.TeamId == 0)
                        {
                            bodyBrush = new ImageBrush(this.gameImageController.TankBodyTextures[bodyIdx]);
                            turretBrush = new ImageBrush(this.gameImageController.TankTurretTextures[turretIdx]);
                        }
                        else
                        {
                            bodyBrush = new ImageBrush(this.gameImageController.Team1BodyTextures[bodyIdx]);
                            turretBrush = new ImageBrush(this.gameImageController.Team1TurretTextures[turretIdx]);
                        }

                        vehicle.ScreenPosition = screenPos;
                        Vector2 screenStartPos = new Vector2(screenPos.X - (12.5f * this.ZoomRate), screenPos.Y - ((6.5f + 4f) * this.ZoomRate));
                        Vector2 sceenEndPos = new Vector2(screenStartPos.X + (25 * this.ZoomRate), screenStartPos.Y + ((13 + 4f) * this.ZoomRate));

                        vehicle.ObjectScreenStartPosition = screenStartPos;
                        vehicle.ObjectScreenEndPosition = sceenEndPos;

                        if (vehicle.IsSelected)
                        {
                            Geometry selectionRingBox = new RectangleGeometry(new Rect(screenPos.X - (12.5f * this.ZoomRate), screenPos.Y - (6.5f * this.ZoomRate), 25 * this.ZoomRate, 12.5f * this.ZoomRate));
                            GeometryDrawing selectionRingDrawing = new GeometryDrawing(new ImageBrush(this.gameImageController.OtherTextures[0]), null, selectionRingBox);
                            dg.Children.Add(selectionRingDrawing);
                        }

                        Geometry bodyBox = new RectangleGeometry(new Rect(screenPos.X - (12.5f * this.ZoomRate), screenPos.Y - (6.5f * this.ZoomRate), 25 * this.ZoomRate, 13 * this.ZoomRate));
                        GeometryDrawing bodyDrawing = new GeometryDrawing(bodyBrush, null, bodyBox);

                        dg.Children.Add(bodyDrawing);

                        if (vehicle.Behavior != VehicleBehavior.Dead)
                        {
                            Geometry turretBox = new RectangleGeometry(new Rect(screenPos.X - (13.5f * this.ZoomRate), screenPos.Y - ((6.5f + 4f) * this.ZoomRate), 27 * this.ZoomRate, 13 * this.ZoomRate));
                            GeometryDrawing turretDrawing = new GeometryDrawing(turretBrush, null, turretBox);

                            dg.Children.Add(turretDrawing);
                        }

                        if (vehicle.IsSelected || vehicle.IsMouseOver)
                        {
                            Geometry healthBarBox = new RectangleGeometry(new Rect(screenPos.X - (9f * this.ZoomRate), screenPos.Y - (9.5f * this.ZoomRate), 18 * this.ZoomRate, 2 * this.ZoomRate));
                            GeometryDrawing healthBarDrawing = new GeometryDrawing(new ImageBrush(this.gameImageController.HealthBarTextures[vehicle.Health]), null, healthBarBox);
                            dg.Children.Add(healthBarDrawing);
                        }

                        vehiclesOnScreen.Add(item as Vehicle);
                        clickableOnScreen.Add(item as Vehicle);

                        break;
                }
            }

            this.gameModel.OnScreenVehicles = vehiclesOnScreen;
            this.gameModel.OnScreenClickableObjects = clickableOnScreen;
        }

        private void DrawBasePercents(ref DrawingGroup dg)
        {
            if (this.gameModel.Team0BasePercent > 0)
            {
                var percentStartLoc = new Point((int)(this.windowDimensions.X / 2) - 300, 80);
                Geometry geoOutside = new RectangleGeometry(new Rect(percentStartLoc.X, percentStartLoc.Y, 600, 8));
                GeometryDrawing geoOutsideDrawing = new GeometryDrawing(this.blackBrush, null, geoOutside);

                Geometry geoInside = new RectangleGeometry(new Rect(percentStartLoc.X + 2, percentStartLoc.Y + 2, this.gameModel.Team0BasePercent * 5.96, 4));
                GeometryDrawing geoInsideDrawing = new GeometryDrawing(this.blueBrush, null, geoInside);

                dg.Children.Add(geoOutsideDrawing);
                dg.Children.Add(geoInsideDrawing);
            }

            if (this.gameModel.Team1BasePercent > 0)
            {
                var percentStartLoc = new Point((int)(this.windowDimensions.X / 2) - 300, 95);
                Geometry geoOutside = new RectangleGeometry(new Rect(percentStartLoc.X, percentStartLoc.Y, 600, 8));
                GeometryDrawing geoOutsideDrawing = new GeometryDrawing(this.blackBrush, null, geoOutside);

                Geometry geoInside = new RectangleGeometry(new Rect(percentStartLoc.X + 2, percentStartLoc.Y + 2, this.gameModel.Team1BasePercent * 5.96, 4));
                GeometryDrawing geoInsideDrawing = new GeometryDrawing(this.redBrush, null, geoInside);

                dg.Children.Add(geoOutsideDrawing);
                dg.Children.Add(geoInsideDrawing);
            }
        }

        private void DrawHUD(ref DrawingGroup dg)
        {
            Geometry overlayGeo = new RectangleGeometry(new Rect(0, 0, this.windowDimensions.X, this.windowDimensions.Y));
            GeometryDrawing overlayDrawing = new GeometryDrawing(new ImageBrush(this.gameImageController.UITextures[3]), null, overlayGeo);
            dg.Children.Add(overlayDrawing);

            GeometryDrawing teamRation = DrawText(this.gameModel.VehiclesTeam0.Count + " : " + this.gameModel.VehiclesTeam1.Count, this.windowDimensions.X / 2, 10);
            dg.Children.Add(teamRation);

            GeometryDrawing sessionTime = DrawText(this.gameModel.SessionTime.ToString(), this.windowDimensions.X / 2, 35);
            dg.Children.Add(sessionTime);

            this.teamVehiclesUI = this.teamVehiclesUI.OrderByDescending(x => x.Health != 0 ? 1 : 0).ToList();

            Vector2 startPos = new Vector2(387, 602);
            int i = 0;
            int j = 0;
            foreach (IVehicle vehicle in this.teamVehiclesUI)
            {
                Vector2 screenPos = new Vector2(startPos.X + ((50 + 5) * i), startPos.Y + ((50 + 5) * j));
                Geometry vehicleUiGeo = new RectangleGeometry(new Rect(screenPos.X, screenPos.Y, 50, 50));
                GeometryDrawing vehicleUiDrawing = new GeometryDrawing(new ImageBrush(this.gameImageController.UITextures[vehicle.Health != 0 ? (vehicle.TeamId == 0 ? 0 : 1) : 2]), null, vehicleUiGeo);

                dg.Children.Add(vehicleUiDrawing);

                if (vehicle.Behavior != VehicleBehavior.Dead && vehicle.IsSelected)
                {
                    GeometryDrawing selectUIDrawing = new GeometryDrawing(new ImageBrush(this.gameImageController.UITextures[4]), null, vehicleUiGeo);
                    dg.Children.Add(selectUIDrawing);
                }

                if (vehicle.Behavior != VehicleBehavior.Dead)
                {
                    Geometry healthBGgeo = new RectangleGeometry(new Rect(screenPos.X + 10, screenPos.Y + 40, 30, 6));
                    GeometryDrawing heathBGdrawing = new GeometryDrawing(new SolidColorBrush(Color.FromRgb(0, 0, 0)), null, healthBGgeo);
                    dg.Children.Add(heathBGdrawing);

                    Geometry healthVALgeo = new RectangleGeometry(new Rect(screenPos.X + 11, screenPos.Y + 41, vehicle.Health / 100f * 28, 4));
                    GeometryDrawing heathVALdrawing = new GeometryDrawing(new SolidColorBrush(vehicle.Health > 70 ? Color.FromRgb(0, 255, 0) : (vehicle.Health > 30 ? Color.FromRgb(243, 140, 0) : Color.FromRgb(255, 0, 0))), null, healthVALgeo);
                    dg.Children.Add(heathVALdrawing);
                }

                i++;
                if (i >= 9)
                {
                    i = 0;
                    j++;
                }
            }

            if (this.gameModel.Multiselect)
            {
                Geometry tesztGeo = new RectangleGeometry(new Rect(this.gameModel.SelectStart, this.gameModel.SelectEnd));
                GeometryDrawing tesztDraw = new GeometryDrawing(new SolidColorBrush(Color.FromArgb(100, 0, 255, 0)), null, tesztGeo);
                dg.Children.Add(tesztDraw);
            }
        }

        private void DrawPauseAndGameOverScreen(ref DrawingGroup dg)
        {
            if (this.gameModel.GameOver)
            {
                Geometry pauseGeometry =
                    new RectangleGeometry(new Rect(0, 0, this.windowDimensions.X, this.windowDimensions.Y));

                GeometryDrawing pauseBg = new GeometryDrawing(new ImageBrush(this.gameImageController.PauseBackgound), null, pauseGeometry);
                dg.Children.Add(pauseBg);

                GeometryDrawing gameOverText = DrawText("Game Over", this.windowDimensions.X / 2, 100);

                string winnerString;

                switch (this.gameModel.OutcomeCase)
                {
                    case 0:
                        winnerString = "Blue team won";
                        break;
                    case 1:
                        winnerString = "Red team won";
                        break;
                    default:
                        winnerString = "DRAW";
                        break;
                }

                GeometryDrawing winnerText = DrawText(winnerString, this.windowDimensions.X / 2, 125);

                dg.Children.Add(gameOverText);
                dg.Children.Add(winnerText);
            }
            else if (this.gameModel.Paused)
            {
                Geometry pauseGeometry =
                    new RectangleGeometry(new Rect(0, 0, this.windowDimensions.X, this.windowDimensions.Y));

                GeometryDrawing pauseBg = new GeometryDrawing(new ImageBrush(this.gameImageController.PauseBackgound), null, pauseGeometry);
                dg.Children.Add(pauseBg);

                var pauseText = DrawText("Paused", this.windowDimensions.X / 2, 100);

                dg.Children.Add(pauseText);
            }
        }

        private Vector2 ToScreenSpace(Node node)
        {
            float x = (node.GridIdx.X - node.GridIdx.Y) * 25f * this.ZoomRate;
            float y = (node.GridIdx.X + node.GridIdx.Y) * 12.5f * this.ZoomRate;
            return new Vector2(x, y);
        }

        private Point GetCenterNodeIdxOnScreen()
        {
            double x = (this.windowDimensions.X / 2) / (50 * this.ZoomRate);
            double y = (this.windowDimensions.Y / 2) / (25 * this.ZoomRate);

            double offX = this.gameModel.ScreenOffset.X / (50 * this.ZoomRate);
            double offY = this.gameModel.ScreenOffset.Y / (25 * this.ZoomRate);

            double a = (x - offX) + (y - offY);
            double b = (x - offX) - (y - offY);

            Point returnPoint = new Point((int)Math.Floor(a), (int)Math.Floor(b * -1));

            return returnPoint;
        }

        private bool IsInRange(float idxX, float idxY)
        {
            return idxX >= 0 && idxX < this.worldDimensions.X && idxY >= 0 && idxY < this.worldDimensions.Y;
        }

        private void CalculateNodesInWindow()
        {
            Point centerIdx = this.GetCenterNodeIdxOnScreen();

            if (centerIdx == this.centerNodeIdx)
            {
                return;
            }

            this.centerNodeIdx = centerIdx;

            int maxWidthCount = (int)(this.windowDimensions.X / (50 * this.ZoomRate)) + 4;
            int maxHeightCount = (int)(this.windowDimensions.Y / (25 * this.ZoomRate)) + 4;
            int wStart = centerIdx.X + centerIdx.Y - maxWidthCount;
            int wEnd = centerIdx.X + centerIdx.Y + maxWidthCount;
            int hStart = centerIdx.X - centerIdx.Y - maxHeightCount;
            int hEnd = centerIdx.X - centerIdx.Y + maxHeightCount;

            float idxX = 0;
            float idxY = 0;

            List<Node> nodesInViewList = new List<Node>();

            for (int i = wStart; i < wEnd; i++)
            {
                for (int j = hStart; j < hEnd; j++)
                {
                    idxX = (i + j) / 2f;
                    if (idxX % 1 != 0)
                    {
                        continue;
                    }

                    idxY = (i - j) / 2f;

                    if (this.IsInRange(idxX, idxY))
                    {
                        nodesInViewList.Add(this.gameWorld.WorldGrid[(int)idxY, (int)idxX]);
                    }
                }
            }

            var invisibleNodes = this.nodesInWindowView.Except(nodesInViewList).ToList();
            foreach (Node node in invisibleNodes)
            {
                node.IsVisible = false;
            }

            var newVisibleNodes = nodesInViewList.Except(this.nodesInWindowView).ToList();
            foreach (Node node in newVisibleNodes)
            {
                node.IsVisible = true;
            }

            this.nodesInWindowView = nodesInViewList;
        }
    }
}
