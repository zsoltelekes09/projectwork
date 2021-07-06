// <copyright file="UISavedGame.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.UIClasses
{
    using System.Xml.Linq;
    using TankPocalypse.Logic.Interfaces;

    /// <summary>
    /// UISavedGame class. Represents the SavedGame class for ui layer.
    /// </summary>
    public class UISavedGame : IUISavedGame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UISavedGame"/> class.
        /// </summary>
        /// <param name="id">Save Id.</param>
        /// <param name="saveName">Save name.</param>
        /// <param name="saveFileXDoc">Save data.</param>
        public UISavedGame(byte id, string saveName, XDocument saveFileXDoc)
        {
            this.SaveId = id;
            this.SaveName = saveName;
            this.SaveFileXDoc = saveFileXDoc;
        }

        /// <inheritdoc/>
        public byte SaveId { get; private set; }

        /// <inheritdoc/>
        public string SaveName { get; private set; }

        /// <inheritdoc/>
        public XDocument SaveFileXDoc { get; private set; }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is UISavedGame uisave)
            {
                if (this.SaveId == uisave.SaveId && this.SaveName == uisave.SaveName && XNode.DeepEquals(this.SaveFileXDoc, uisave.SaveFileXDoc))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
