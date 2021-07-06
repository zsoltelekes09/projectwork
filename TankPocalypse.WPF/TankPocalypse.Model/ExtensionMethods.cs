// <copyright file="ExtensionMethods.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Model
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

        /// <summary>
        /// Returns the angle between two vector in degrees.
        /// </summary>
        /// <param name="vecA">Vector one.</param>
        /// <param name="vecB">Vector two.</param>
        /// <returns>Angle value in degrees.</returns>
        public static double VectorAngle(this Vector2 vecA, Vector2 vecB)
        {
            double len1 = Math.Sqrt((vecA.X * vecA.X) + (vecA.Y * vecA.Y));
            double len2 = Math.Sqrt((vecB.X * vecB.X) + (vecB.Y * vecB.Y));

            double dotProduct = (vecA.X * vecB.X) + (vecA.Y * vecB.Y);
            double cos = dotProduct / len1 / len2;

            double crossProduct = (vecA.X * vecB.Y) - (vecA.Y * vecB.X);
            double sin = crossProduct / len1 / len2;

            cos = Math.Round(cos, 5);

            double angle = Math.Acos(cos);
            if (sin < 0)
            {
                angle = -angle;
            }

            var returnVal = angle * 180 / Math.PI;

            return returnVal;
        }

        /// <summary>
        /// Clamps the double input angle value to theta if it exceeds theta input.
        /// </summary>
        /// <param name="angle">Input value to be clamped.</param>
        /// <param name="theta">Maximum value.</param>
        /// <returns>Clamped value.</returns>
        public static int ClampAngleToNearestTheta(this double angle, int theta)
        {
            var tmp = angle % theta;
            var diff = (float)theta / 2;

            if (angle < 0)
            {
                if (tmp >= -diff)
                {
                    return (int)(angle - tmp);
                }

                return (int)(angle - (theta + tmp));
            }
            else
            {
                if (tmp >= diff)
                {
                    return (int)(angle + (theta - tmp));
                }

                return (int)(angle - tmp);
            }
        }

        /// <summary>
        /// Clamps the integer input value to theta if it exceeds theta input.
        /// </summary>
        /// <param name="input">Input value to be clamped.</param>
        /// <param name="theta">Maximum value.</param>
        /// <returns>Clamped value.</returns>
        public static int ClampIntegerToNearestTheta(this int input, int theta)
        {
            var tmp = input % theta;
            var diff = (float)theta / 2;

            if (input < 0)
            {
                if (tmp >= -diff)
                {
                    return (int)(input - tmp);
                }

                return (int)(input - (theta + tmp));
            }
            else
            {
                if (tmp >= diff)
                {
                    return (int)(input + (theta - tmp));
                }

                return (int)(input - tmp);
            }
        }

        /// <summary>
        /// Clamps the input double value if it exceeds the maximum value.
        /// </summary>
        /// <param name="angle">Input value to clamp if exceeds max value.</param>
        /// <param name="maxSpeed">Maximum value.</param>
        /// <returns>Clamped value.</returns>
        public static double ClampAngleToMaxValue(this double angle, int maxSpeed)
        {
            if (angle < 0)
            {
                if (angle < -maxSpeed)
                {
                    return -maxSpeed;
                }

                return angle;
            }
            else
            {
                if (angle > maxSpeed)
                {
                    return maxSpeed;
                }

                return angle;
            }
        }
    }
}
