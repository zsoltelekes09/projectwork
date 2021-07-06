// <copyright file="SaveFileHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using TankPocalypse.Logic.Interfaces;

    /// <summary>
    /// SaveFile Helper class.
    /// </summary>
    public class SaveFileHelper : IUISavedGame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileHelper"/> class.
        /// Set all property.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="sname">sname.</param>
        /// <param name="savexdoc">xdoc.</param>
        public SaveFileHelper(byte id, string sname, XDocument savexdoc)
        {
            this.SaveId = id;
            this.SaveName = sname;
            this.SaveFileXDoc = savexdoc;
        }

        /// <inheritdoc/>
        public byte SaveId { get; }

        /// <inheritdoc/>
        public string SaveName { get; }

        /// <inheritdoc/>
        public XDocument SaveFileXDoc { get; }
    }
}
