// <copyright file="IGameInputController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Interfaces
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// GameInputController interface.
    /// </summary>
    public interface IGameInputController
    {
        /// <summary>
        /// Sets the view offset to the center of the map.
        /// </summary>
        /// <param name="winX">Window x size.</param>
        /// <param name="winY">Window y size.</param>
        public void ApplyWorldCenterCameraPosition(int winX, int winY);

        /// <summary>
        /// Key down event method.
        /// </summary>
        /// <param name="sender">Event sender object.</param>
        /// <param name="e">Event data.</param>
        public void KeyDown(object sender, KeyEventArgs e);

        /// <summary>
        /// Key up event method.
        /// </summary>
        /// <param name="sender">Event sender object.</param>
        /// <param name="e">Event data.</param>
        public void KeyUp(object sender, KeyEventArgs e);

        /// <summary>
        /// Mouse wheel rolled event method.
        /// </summary>
        /// <param name="sender">Event sender object.</param>
        /// <param name="e">Event data.</param>
        public void MouseWheelEvent(object sender, MouseWheelEventArgs e);

        /// <summary>
        /// Mouse button down method.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseButtonDown(object sender, MouseButtonEventArgs e);

        /// <summary>
        /// Mouse button up method.
        /// </summary>
        /// <param name="sender">Event sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseButtonUp(object sender, MouseButtonEventArgs e);

        /// <summary>
        /// Mouse left button down.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseLeftButtonDown(object sender, MouseButtonEventArgs e);

        /// <summary>
        /// Mouse right button down.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseRightButtonDown(object sender, MouseButtonEventArgs e);

        /// <summary>
        /// Mouse right button up.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseLeftButtonUp(object sender, MouseButtonEventArgs e);

        /// <summary>
        /// Mouse right button up.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void MouseRightButtonUp(object sender, MouseButtonEventArgs e);

        /// <summary>
        /// Sets mouse position.
        /// </summary>
        /// <param name="mouseX">Mouse x coordinate on window.</param>
        /// <param name="mouseY">Mouse y coordinate on window.</param>
        public void SetMousePos(double mouseX, double mouseY);

        /// <summary>
        /// Mouse position request method subscriber.
        /// </summary>
        /// <param name="method">Request method.</param>
        public void SubscribeToRequestMousePosEvent(Action method);

        /// <summary>
        /// Canvas moved event subscriber method.
        /// </summary>
        /// <param name="method">Canvas new dimentsion.</param>
        public void SubscribeToCanvasMovedEvent(Action<float, float> method);

        /// <summary>
        /// ZoomRate changed event subscriber method.
        /// </summary>
        /// <param name="method">Input method.</param>
        public void SubscribeToZoomRateChangedEvent(Action<short> method);

        /// <summary>
        /// Handels input methods inside logic...
        /// </summary>
        public void RefreshKeys();

        /// <summary>
        /// Subscriber event to windows sizechanged event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Sent data.</param>
        public void UpdateWindowDimensions(object sender, SizeChangedEventArgs e);

        /// <summary>
        /// Subscribes for the cursor changed event.
        /// </summary>
        /// <param name="method">Input method to subsribe.</param>
        public void SubscribeToCursorChangedEvent(Action<string> method);
    }
}
