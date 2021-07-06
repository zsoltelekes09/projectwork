// <copyright file="Tank.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.Networking
{
    /// <summary>
    /// Tank basic structure.
    /// </summary>
    public class Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tank"/> class.
        /// Setup properties via contructor.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="pos">pos.</param>
        /// <param name="turret">turret.</param>
        /// <param name="body">body.</param>
        /// /// <param name="shootid">shootid.</param>
        public Tank(int id, VectorExtend pos, int turret, int body, byte shootid)
        {
            this.Id = id;
            this.Position = pos;
            this.TurretIdx = turret;
            this.BodyIdx = body;
            this.ShootIdx = shootid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tank"/> class.
        /// </summary>
        public Tank()
        {
        }

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Position.
        /// </summary>
        public VectorExtend Position { get; set; }

        /// <summary>
        /// Gets or sets turretIdx.
        /// </summary>
        public int TurretIdx { get; set; }

        /// <summary>
        /// Gets or sets bodyIdx.
        /// </summary>
        public int BodyIdx { get; set; }

        /// <summary>
        /// Gets or sets shootIdx.
        /// </summary>
        public byte ShootIdx { get; set; }
    }
}
