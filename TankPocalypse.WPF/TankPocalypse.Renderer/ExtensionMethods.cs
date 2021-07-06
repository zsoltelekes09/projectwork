// <copyright file="ExtensionMethods.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Renderer
{
    using System;
    using System.Numerics;

    /// <summary>
    /// This is an extension class, which adds extension methods to the specified type.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Rotates the given vector by the given degree input. Degree value is float.
        /// </summary>
        /// <param name="v">Vector to rotate.</param>
        /// <param name="degrees">Rotation unit in degrees.</param>
        /// <returns>The input vectors rotated version.</returns>
        public static Vector2 RotateByAngle(this Vector2 v, float degrees)
        {
            double radians = degrees * (Math.PI / 180);
            var x2 = (v.X * Math.Cos(radians)) - (v.Y * Math.Sin(radians));
            var y2 = (v.X * Math.Sin(radians)) + (v.Y * Math.Cos(radians));
            return new Vector2(Convert.ToSingle(x2), Convert.ToSingle(y2));
        }

        /// <summary>
        /// Rotates the given vector by the given degree input. Degree value is double.
        /// </summary>
        /// <param name="v">Vector to rotate.</param>
        /// <param name="degrees">Rotation unit in degrees.</param>
        /// <returns>The input vectors rotated version.</returns>
        public static Vector2 RotateByAngle(this Vector2 v, double degrees)
        {
            double radians = degrees * (Math.PI / 180);
            var x2 = (v.X * Math.Cos(radians)) - (v.Y * Math.Sin(radians));
            var y2 = (v.X * Math.Sin(radians)) + (v.Y * Math.Cos(radians));
            return new Vector2(Convert.ToSingle(x2), Convert.ToSingle(y2));
        }
    }
}
