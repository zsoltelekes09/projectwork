// <copyright file="TankIoc.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF
{
    using CommonServiceLocator;
    using GalaSoft.MvvmLight.Ioc;

    /// <summary>
    /// Custom IoC.
    /// </summary>
    public class TankIoc : SimpleIoc, IServiceLocator
    {
        /// <summary>
        /// Gets the Instance.
        /// </summary>
        public static TankIoc Instance { get; private set; } = new TankIoc();
    }
}