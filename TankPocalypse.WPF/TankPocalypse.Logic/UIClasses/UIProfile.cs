// <copyright file="UIProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Logic.UIClasses
{
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.Repository.Classes;
    using TankPocalypse.Repository.Interfaces;

    /// <summary>
    /// UIProfile class. Represents the Profile class for the ui layer.
    /// </summary>
    public class UIProfile : IUIProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIProfile"/> class.
        /// </summary>
        /// <param name="userName">Name of the profile.</param>
        /// <param name="userId">Profile Id.</param>
        /// <param name="scores">Profile score.</param>
        /// <param name="uKilled">Units killed count.</param>
        /// <param name="uLost">Units lost count.</param>
        /// <param name="gW">Games won count.</param>
        /// <param name="gL">Games lost count.</param>
        public UIProfile(string userName, int userId, int scores, int uKilled, int uLost, int gW, int gL)
        {
            this.UserName = userName;
            this.UserId = userId;
            this.Scores = scores;
            this.UnitsKilled = uKilled;
            this.UnitsLost = uLost;
            this.GamesWon = gW;
            this.GamesLost = gL;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIProfile"/> class.
        /// </summary>
        /// <param name="name">Profile name.</param>
        /// <param name="id">Profile id.</param>
        public UIProfile(string name, int id)
        {
            this.UserName = name;
            this.UserId = id;
        }

        /// <inheritdoc/>
        public int UnitsKilled { get; private set; }

        /// <inheritdoc/>
        public int UnitsLost { get; private set; }

        /// <inheritdoc/>
        public int GamesWon { get; private set; }

        /// <inheritdoc/>
        public int GamesLost { get; private set; }

        /// <inheritdoc/>
        public string UserName { get; private set; }

        /// <inheritdoc/>
        public int UserId { get; private set; }

        /// <inheritdoc/>
        public int Scores { get; private set; }

        /// <summary>
        /// Converts Profile object to UIProfile object.
        /// </summary>
        /// <param name="repoProf">Profile input object.</param>
        /// <returns>New UIProfile object.</returns>
        public static IUIProfile ConvertProfileToUIProfile(IProfile repoProf)
        {
            return new UIProfile(repoProf?.UserName, repoProf.UserId, repoProf.Scores, repoProf.UnitsKilled, repoProf.UnitsLost, repoProf.GamesWon, repoProf.GamesLost);
        }

        /// <summary>
        /// Converts UIProfile object to Profile object.
        /// </summary>
        /// <param name="logicProf">UIProfile input object.</param>
        /// <returns>New Profile object.</returns>
        public static IProfile ConvertUIProfileToProfile(IUIProfile logicProf)
        {
            return new RepoProfile(logicProf?.UserName, logicProf.UserId);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.UserName;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is UIProfile uip)
            {
                if (uip.GamesLost == this.GamesLost && uip.GamesWon == this.GamesWon && uip.Scores == this.Scores && uip.UnitsKilled == this.UnitsKilled && uip.UserId == this.UserId && uip.UserName == this.UserName)
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
