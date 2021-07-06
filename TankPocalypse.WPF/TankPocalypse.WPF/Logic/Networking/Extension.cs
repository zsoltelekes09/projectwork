// <copyright file="Extension.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    using System;

    /// <summary>
    /// Extension method.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Extension for Subarrays.
        /// </summary>
        /// <typeparam name="T">T.</typeparam>
        /// <param name="array">Array.</param>
        /// <param name="offset">offset.</param>
        /// <param name="length">length.</param>
        /// <returns> T type subarray.</returns>
        public static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            T[] result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }
}
