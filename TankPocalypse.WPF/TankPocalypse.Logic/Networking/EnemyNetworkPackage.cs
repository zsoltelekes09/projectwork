// <copyright file="EnemyNetworkPackage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Networking
{
    /// <summary>
    /// Enemy Team Network Package structure.
    /// </summary>
    public class EnemyNetworkPackage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyNetworkPackage"/> class.
        /// Sets HP and ID.
        /// </summary>
        /// <param name="iD">ID.</param>
        /// <param name="hP">Health.</param>
        public EnemyNetworkPackage(int iD, int hP)
        {
            this.ID = iD;
            this.HP = hP;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyNetworkPackage"/> class.
        /// Parameter Free constructor.
        /// </summary>
        public EnemyNetworkPackage()
        {
        }

        /// <summary>
        /// Gets or sets identifier.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets health.
        /// </summary>
        public int HP { get; set; }
    }
}
