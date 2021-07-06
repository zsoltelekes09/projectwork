// <copyright file="PacketType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.WPF.Logic.Networking
{
    /// <summary>
    /// PacketType.
    /// </summary>
    public enum PacketType : int
    {
        /// <summary>
        /// None.
        /// </summary>
        None,

        /// <summary>
        /// Username.
        /// </summary>
        Username = 1,

        /// <summary>
        /// UsernameMapData.
        /// </summary>
        UsernameMapData = 2,

        /// <summary>
        /// UserIsReady.
        /// </summary>
        UserIsReady = 3,

        /// <summary>
        /// ASD.
        /// </summary>
        UnitCount = 4,

        /// <summary>
        /// CanStartGame.
        /// </summary>
        CanStartGame = 5,

        /// <summary>
        /// Pause.
        /// </summary>
        Pause = 6,

        /// <summary>
        /// ClientPackage.
        /// </summary>
        ClientPackage = 7,

        /// <summary>
        /// ServerPackage.
        /// </summary>
        ServerPackage = 8,

        /// <summary>
        /// Ping.
        /// </summary>
        Ping = 9,

        /// <summary>
        /// Exit.
        /// </summary>
        Exit = 10,

        /// <summary>
        /// GameEnded.
        /// </summary>
        GameEnded = 11,

        /// <summary>
        /// SaveFile.
        /// </summary>
        SaveFile = 12,
    }
}
