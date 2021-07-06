// <copyright file="GameInputController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.Model;
    using TankPocalypse.Model.FlowField;
    using TankPocalypse.Model.Interfaces;

    /// <summary>
    /// Game input controller class. Handles all input events necessary for gameplay.
    /// </summary>
    public class GameInputController : IGameInputController
    {
        private IGameModel gameModel;
        private IGameWorld gameWorld;
        private IFlowField flowField;
        private Point mousePos;
        private Point requestedMousePos;
        private Point mousePositionOnLeftClick;
        private Point mousePositionOnRightClick;
        private Point insideWindowMousePos;
        private bool middleMouseDown;
        private bool leftMouseDown;
        private bool rightMouseDown;
        private bool leftMouseReleased;
        private bool rightMouseReleased;
        private bool screenMovedByMouse;
        private short zoomRate = 3;
        private string cursorIndex;
        private Vector2 windowDimensions = new Vector2(800, 450);
        private Vector2 offsetVector;
        private List<IClickableObject> selectedObject = new List<IClickableObject>();
        private Dictionary<Key, bool> heldDownKeys;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInputController"/> class.
        /// </summary>
        /// <param name="gameModel">GameModel entity.</param>
        /// <param name="gameWorld">GameWorld entity.</param>
        /// <param name="flowField">FlowField entity.</param>
        public GameInputController(IGameModel gameModel, IGameWorld gameWorld, IFlowField flowField)
        {
            this.gameModel = gameModel;
            this.gameWorld = gameWorld;
            this.flowField = flowField;
            this.offsetVector = Vector2.Zero;
            this.insideWindowMousePos = new Point(50, 50);

            this.SetupKeys();
        }

        /// <summary>
        /// Screen offset changed event.
        /// </summary>
        public event Action<float, float> ScreenOffsetChanged;

        /// <summary>
        /// Screen zoomrate changed event.
        /// </summary>
        public event Action<short> ScreenZoomRateChanged;

        /// <summary>
        /// Nem mouse position requester event.
        /// </summary>
        public event Action RequestNewMousePosition;

        /// <summary>
        /// Change cursor event.
        /// </summary>
        public event Action<string> ChangeCursor;

        /// <summary>
        /// Handels input methods inside logic...
        /// </summary>
        public void RefreshKeys()
        {
            this.CheckUnitsIfMouseIsOver();
            this.HeldDownKeysHandler();
            this.LeftMouseDownHandler();
            this.LeftMouseReleasedHandler();
            this.RightMouseReleasedHandler();
        }

        /// <summary>
        /// Subscribes for the cursor changed event.
        /// </summary>
        /// <param name="method">Input method to subsribe.</param>
        public void SubscribeToCursorChangedEvent(Action<string> method)
        {
            this.ChangeCursor += method;
        }

        /// <summary>
        /// Mouse position request method subscriber.
        /// </summary>
        /// <param name="method">Request method.</param>
        public void SubscribeToRequestMousePosEvent(Action method)
        {
            this.RequestNewMousePosition += method;
        }

        /// <summary>
        /// Canvas moved event subscriber method.
        /// </summary>
        /// <param name="method">Canvas new dimentsion.</param>
        public void SubscribeToCanvasMovedEvent(Action<float, float> method)
        {
            this.ScreenOffsetChanged += method;
        }

        /// <summary>
        /// ZoomRate changed event subscriber method.
        /// </summary>
        /// <param name="method">Input method.</param>
        public void SubscribeToZoomRateChangedEvent(Action<short> method)
        {
            this.ScreenZoomRateChanged += method;
        }

        /// <summary>
        /// Key down event method.
        /// </summary>
        /// <param name="sender">Event sender object.</param>
        /// <param name="e">Event data.</param>
        public void KeyDown(object sender, KeyEventArgs e)
        {
            this.heldDownKeys[e.Key] = true;
        }

        /// <summary>
        /// Key up event method.
        /// </summary>
        /// <param name="sender">Event sender object.</param>
        /// <param name="e">Event data.</param>
        public void KeyUp(object sender, KeyEventArgs e)
        {
            this.heldDownKeys[e.Key] = false;
        }

        /// <summary>
        /// Mouse left button down.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.leftMouseReleased = false;
            this.leftMouseDown = true;
            this.mousePositionOnLeftClick = e.GetPosition(null);
        }

        /// <summary>
        /// Mouse wheel rolled event method.
        /// </summary>
        /// <param name="sender">Event sender object.</param>
        /// <param name="e">Event data.</param>
        public void MouseWheelEvent(object sender, MouseWheelEventArgs e)
        {
            // Disabled due to unintended behavior.
            if (e.Delta > 0)
            {
                if (this.zoomRate + 1 < 7)
                {
                    this.zoomRate += 1;

                    this.ScreenZoomRateChanged?.Invoke(this.zoomRate);
                }
            }
            else
            {
                if (this.zoomRate - 1 > 2)
                {
                    this.zoomRate -= 1;

                    this.ScreenZoomRateChanged?.Invoke(this.zoomRate);
                }
            }
        }

        /// <summary>
        /// Mouse button down method.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                this.middleMouseDown = true;

                this.mousePos = Mouse.GetPosition(null);

                new Task(async () =>
                  {
                      while (this.middleMouseDown)
                      {
                          this.RequestNewMousePosition?.Invoke();
                          await Task.Delay(15).ConfigureAwait(true);

                          this.gameModel.ScreenOffset += new Vector2(Convert.ToSingle(this.requestedMousePos.X - this.mousePos.X), Convert.ToSingle(this.requestedMousePos.Y - this.mousePos.Y));

                          this.ScreenOffsetChanged?.Invoke(this.offsetVector.X, this.offsetVector.Y);

                          this.mousePos = this.requestedMousePos;
                      }
                  }).Start();
            }
        }

        /// <summary>
        /// Mouse button up method.
        /// </summary>
        /// <param name="sender">Event sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                this.middleMouseDown = false;
            }
        }

        /// <summary>
        /// Mouse right button down.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.rightMouseReleased = false;
            this.rightMouseDown = true;
            System.Diagnostics.Debug.WriteLine("Mouse Right Button DOWN");
            this.mousePositionOnRightClick = e.GetPosition(null);
        }

        /// <summary>
        /// Mouse left button up.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.leftMouseDown = false;
            this.leftMouseReleased = true;
        }

        /// <summary>
        /// Mouse right button up.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.rightMouseDown = false;
            this.rightMouseReleased = true;
        }

        /// <summary>
        /// Sets mouse position.
        /// </summary>
        /// <param name="mouseX">Mouse x coordinate on window.</param>
        /// <param name="mouseY">Mouse y coordinate on window.</param>
        public void SetMousePos(double mouseX, double mouseY)
        {
            this.requestedMousePos = new Point(mouseX, mouseY);
        }

        /// <summary>
        /// Sets the view offset to the center of the map.
        /// </summary>
        /// <param name="winX">Window x size.</param>
        /// <param name="winY">Window y size.</param>
        public void ApplyWorldCenterCameraPosition(int winX, int winY)
        {
            this.windowDimensions = new Vector2(winX, winY);
            var centerLoc = new Vector2(this.gameWorld.GridWidth / 2 * 50, this.gameWorld.GridHeight / 2 * 50);
            var zoomOffset = 25 * this.zoomRate;
            var startPos = new Vector2(0, 0);
            startPos.X += zoomOffset;

            var toConvertVectorCorrected = centerLoc * this.zoomRate;
            var rotatedVector = toConvertVectorCorrected.RotateByAngle(45f);

            var finalVector = ScalarProjectionOnVector(rotatedVector, toConvertVectorCorrected);
            finalVector.Y /= 2;
            finalVector += startPos;

            var centerOfWindow = new Vector2(this.windowDimensions.X / 2, this.windowDimensions.Y / 2);
            var finPos = finalVector - centerOfWindow;

            this.offsetVector = finPos * -1;
            this.gameModel.ScreenOffset = finPos * -1;
        }

        /// <summary>
        /// Subscriber event to windows sizechanged event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void UpdateWindowDimensions(object sender, SizeChangedEventArgs e)
        {
            this.windowDimensions.X = Convert.ToSingle(e.NewSize.Width);
            this.windowDimensions.Y = Convert.ToSingle(e.NewSize.Height);
            System.Diagnostics.Debug.WriteLine("Input screen size:: " + this.windowDimensions.X + " x " + this.windowDimensions.Y);
        }

        private static bool IsItemInsideSelection(IClickableObject item, Point sTopLeft, Point sBottomRight)
        {
            var centerLoc = (item.ObjectScreenStartPosition + item.ObjectScreenEndPosition) / 2;
            if (centerLoc.X >= sTopLeft.X && centerLoc.X <= sBottomRight.X && centerLoc.Y >= sTopLeft.Y && centerLoc.Y <= sBottomRight.Y)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculates a scalar projection of vectorB on vectorA.
        /// </summary>
        /// <param name="vecA">First vector.</param>
        /// <param name="vecB">Second vector.</param>
        /// <returns>Scalar projection vector.</returns>
        private static Vector2 ScalarProjectionOnVector(Vector2 vecA, Vector2 vecB)
        {
            var vecANormalized = Vector2.Normalize(vecA);
            return Vector2.Multiply(Vector2.Dot(vecB, vecANormalized), vecANormalized);
        }

        private void SetupKeys()
        {
            this.heldDownKeys = new Dictionary<Key, bool>
            {
                { Key.Space, false },
                { Key.F, false },
                { Key.Up, false },
                { Key.Down, false },
                { Key.Left, false },
                { Key.Right, false },
                { Key.Enter, false },
                { Key.LeftShift, false },
                { Key.LeftCtrl, false },
            };
        }

        private bool IsLocationCorrect(Vector2 location)
        {
            if (location.Y > 0 && location.Y < this.gameWorld.GridHeight * 50 && location.X > 0 && location.X < this.gameWorld.GridWidth * 50 && this.gameWorld.GetNodeByWorldCoordinates(location).IsWalkable)
            {
                return true;
            }

            return false;
        }

        private void DeselectAllSelectedObject()
        {
            foreach (IClickableObject item in this.selectedObject)
            {
                item.SetDeselected();
            }

            this.selectedObject.Clear();
        }

        // Unused feature.
        private void CameraWindowSideMovement()
        {
            if (this.requestedMousePos.X != 0 && this.requestedMousePos.Y != 0)
            {
                this.insideWindowMousePos = this.requestedMousePos;
            }

            // Move Left
            if (this.insideWindowMousePos.X < 10)
            {
                this.offsetVector.X += 5;
                this.screenMovedByMouse = true;
            }

            // Move Right
            if (this.insideWindowMousePos.X > this.windowDimensions.X - 10 - 17)
            {
                this.offsetVector.X -= 5;
                this.screenMovedByMouse = true;
            }

            // Move Up
            if (this.insideWindowMousePos.Y < 10)
            {
                this.offsetVector.Y += 5;
                this.screenMovedByMouse = true;
            }

            // Move Down
            if (this.insideWindowMousePos.Y > this.windowDimensions.Y - 10 - 41)
            {
                this.offsetVector.Y -= 5;
                this.screenMovedByMouse = true;
            }

            if (this.screenMovedByMouse)
            {
                this.ScreenOffsetChanged?.Invoke(this.offsetVector.X, this.offsetVector.Y);
                this.screenMovedByMouse = false;
            }
        }

        private void CheckUnitsIfMouseIsOver()
        {
            string tempIndex = this.selectedObject.Count != 0 ? "move" : "none";

            int selectedObjectCount = this.selectedObject.Count;
            if (this.gameModel.OnScreenClickableObjects != null)
            {
                foreach (IClickableObject item in this.gameModel.OnScreenClickableObjects)
                {
                    if (item is IVehicle vehicle)
                    {
                        if (vehicle.IsRevealed &&
                            this.requestedMousePos.X >= item.ObjectScreenStartPosition.X &&
                            this.requestedMousePos.X <= item.ObjectScreenEndPosition.X &&
                            this.requestedMousePos.Y >= item.ObjectScreenStartPosition.Y &&
                            this.requestedMousePos.Y <= item.ObjectScreenEndPosition.Y)
                        {
                            vehicle.IsMouseOver = true;
                            if (vehicle.TeamId == this.gameModel.TeamID)
                            {
                                tempIndex = "select";
                            }
                            else
                            {
                                if (selectedObjectCount != 0)
                                {
                                    tempIndex = "attack";
                                }
                                else
                                {
                                    tempIndex = "enemy";
                                }
                            }
                        }
                        else
                        {
                            vehicle.IsMouseOver = false;
                        }
                    }
                }
            }

            if (tempIndex != this.cursorIndex)
            {
                this.cursorIndex = tempIndex;
                this.ChangeCursor?.Invoke(this.cursorIndex);
            }
        }

        private Vector2 MousePositionToWorldSpace(Vector2 toReverseVector)
        {
            var zoomOffset = 25 * this.zoomRate;
            var startPos = new Vector2(0, 0);
            startPos.X += zoomOffset;
            startPos += this.gameModel.ScreenOffset;

            var revVector0 = toReverseVector - startPos;
            revVector0.Y *= 2;

            var revVector1Length = revVector0.Length();
            var finalLength = Convert.ToSingle(Math.Sqrt((revVector1Length * revVector1Length) + (revVector1Length * revVector1Length)));

            var finalVector = revVector0.RotateByAngle(-45f);

            finalVector = Vector2.Normalize(finalVector) * finalLength;
            return finalVector / this.zoomRate;
        }

        private void HeldDownKeysHandler()
        {
            if ((this.heldDownKeys[Key.Up] || this.heldDownKeys[Key.Down] || this.heldDownKeys[Key.Left] || this.heldDownKeys[Key.Right]) && !this.rightMouseDown)
            {
                Vector2 tmpOffset = this.gameModel.ScreenOffset;
                if (this.heldDownKeys[Key.Up])
                {
                    tmpOffset.Y += 7;
                }

                if (this.heldDownKeys[Key.Down])
                {
                    tmpOffset.Y -= 7;
                }

                if (this.heldDownKeys[Key.Left])
                {
                    tmpOffset.X += 7;
                }

                if (this.heldDownKeys[Key.Right])
                {
                    tmpOffset.X -= 7;
                }

                this.gameModel.ScreenOffset = tmpOffset;
            }
        }

        private void LeftMouseDownHandler()
        {
            if (this.leftMouseDown)
            {
                this.RequestNewMousePosition?.Invoke();
                var vect1 = new Vector2((int)this.requestedMousePos.X, (int)this.requestedMousePos.Y);
                var vect2 = new Vector2((int)this.mousePositionOnLeftClick.X, (int)this.mousePositionOnLeftClick.Y);
                var dist = Vector2.Distance(vect1, vect2);
                if (dist > 5)
                {
                    this.leftMouseDown = false;
                    this.gameModel.Multiselect = true;
                    var t = new Task(async () =>
                    {
                        while (!this.leftMouseReleased)
                        {
                            this.RequestNewMousePosition?.Invoke();
                            await Task.Delay(15).ConfigureAwait(true);
                            this.CalculateSelectionRegion();
                        }
                    });
                    t.Start();
                }
            }
        }

        private void CalculateSelectionRegion()
        {
            Point selectStart;
            Point selectEnd;
            if (this.mousePositionOnLeftClick.X < this.requestedMousePos.X)
            {
                selectStart.X = this.mousePositionOnLeftClick.X;
                selectEnd.X = this.requestedMousePos.X;
            }
            else
            {
                selectEnd.X = this.mousePositionOnLeftClick.X;
                selectStart.X = this.requestedMousePos.X;
            }

            if (this.mousePositionOnLeftClick.Y < this.requestedMousePos.Y)
            {
                selectStart.Y = this.mousePositionOnLeftClick.Y;
                selectEnd.Y = this.requestedMousePos.Y;
            }
            else
            {
                selectEnd.Y = this.mousePositionOnLeftClick.Y;
                selectStart.Y = this.requestedMousePos.Y;
            }

            this.gameModel.SelectStart = selectStart;
            this.gameModel.SelectEnd = selectEnd;
        }

        private void LeftMouseReleasedHandler()
        {
            if (this.leftMouseReleased && !this.gameModel.Multiselect)
            {
                this.leftMouseReleased = false;
                List<IClickableObject> clickedOnItem = new List<IClickableObject>();

                if (!this.heldDownKeys[Key.LeftShift])
                {
                    this.DeselectAllSelectedObject();
                }

                foreach (IClickableObject item in this.gameModel.OnScreenClickableObjects)
                {
                    if (item.TeamId == this.gameModel.TeamID &&
                        this.mousePositionOnLeftClick.X >= item.ObjectScreenStartPosition.X &&
                        this.mousePositionOnLeftClick.X <= item.ObjectScreenEndPosition.X &&
                        this.mousePositionOnLeftClick.Y >= item.ObjectScreenStartPosition.Y &&
                        this.mousePositionOnLeftClick.Y <= item.ObjectScreenEndPosition.Y)
                    {
                        clickedOnItem.Add(item);
                    }
                }

                var count = clickedOnItem.Count;

                if (count > 0)
                {
                    IClickableObject item;

                    if (count == 1)
                    {
                        item = clickedOnItem.FirstOrDefault();
                        if (!this.selectedObject.Contains(item))
                        {
                            item.SetSelected();
                            this.selectedObject.Add(item);
                        }
                    }
                    else
                    {
                        item = clickedOnItem.OrderByDescending(z => z.Position.X + z.Position.Y).First();
                    }

                    if (!this.selectedObject.Contains(item))
                    {
                        item.SetSelected();
                        this.selectedObject.Add(item);
                    }
                }
                else
                {
                    this.DeselectAllSelectedObject();
                }
            }
            else if (this.leftMouseReleased && this.gameModel.Multiselect)
            {
                this.leftMouseReleased = false;

                this.gameModel.Multiselect = false;

                if (!this.heldDownKeys[Key.LeftShift])
                {
                    this.DeselectAllSelectedObject();
                }

                foreach (IClickableObject item in this.gameModel.OnScreenClickableObjects)
                {
                    if (item is IVehicle vehicle && vehicle.TeamId == this.gameModel.TeamID && vehicle.Behavior != VehicleBehavior.Dead && IsItemInsideSelection(item, this.gameModel.SelectStart, this.gameModel.SelectEnd) && !this.selectedObject.Contains(item))
                    {
                        item.SetSelected();
                        this.selectedObject.Add(item);
                    }
                }

                this.gameModel.SelectStart = new Point(0, 0);
                this.gameModel.SelectEnd = new Point(0, 0);
            }
        }

        private void RightMouseReleasedHandler()
        {
            if (this.rightMouseReleased)
            {
                this.rightMouseReleased = false;

                if (this.selectedObject.Count > 0)
                {
                    Vector2 endMouseVector = new Vector2((float)this.mousePositionOnRightClick.X, (float)this.mousePositionOnRightClick.Y);

                    List<IClickableObject> clickedOnItem = new List<IClickableObject>();

                    foreach (IClickableObject item in this.gameModel.OnScreenClickableObjects)
                    {
                        if (item.TeamId != this.gameModel.TeamID &&
                            this.mousePositionOnRightClick.X >= item.ObjectScreenStartPosition.X &&
                            this.mousePositionOnRightClick.X <= item.ObjectScreenEndPosition.X &&
                            this.mousePositionOnRightClick.Y >= item.ObjectScreenStartPosition.Y &&
                            this.mousePositionOnRightClick.Y <= item.ObjectScreenEndPosition.Y)
                        {
                            clickedOnItem.Add(item);
                        }
                    }

                    var count = clickedOnItem.Count;
                    IVehicle targetVehicle = null;

                    if (count > 0)
                    {
                        if (count == 1 && clickedOnItem.First() is IVehicle target)
                        {
                            targetVehicle = target;
                        }
                        else if (count > 1 && clickedOnItem.OrderByDescending(z => z.Position.X + z.Position.Y).First() is IVehicle enemy)
                        {
                            targetVehicle = enemy;
                        }
                    }

                    if (targetVehicle != null)
                    {
                        // Then attack target vehicle with every selected unit!
                        foreach (IClickableObject clickableObject in this.selectedObject)
                        {
                            if (clickableObject is IVehicle vehicle && vehicle.Behavior != VehicleBehavior.Dead)
                            {
                                // Attack target...
                                vehicle.TargetVehicle = targetVehicle;
                                vehicle.Behavior = VehicleBehavior.Attacking;
                            }
                        }
                    }
                    else
                    {
                        // Then move to the clicked location...
                        var worldPos = this.MousePositionToWorldSpace(endMouseVector);

                        if (this.IsLocationCorrect(worldPos))
                        {
                            Cell[,] requestedFlowField = this.flowField.RequestRoute(worldPos);
                            var selectedCount = this.selectedObject.Count;

                            foreach (IClickableObject selected in this.selectedObject)
                            {
                                if (selected is IVehicle vehicle && vehicle.Behavior != VehicleBehavior.Dead)
                                {
                                    vehicle.SetPositionToMove(worldPos, requestedFlowField, selectedCount > 1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
