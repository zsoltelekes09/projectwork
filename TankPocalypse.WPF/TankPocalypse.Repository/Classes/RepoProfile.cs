// <copyright file="RepoProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TankPocalypse.Repository.Classes
{
    using System.IO;
    using System.Xml.Linq;
    using TankPocalypse.Repository.Interfaces;

    /// <summary>
    /// This is the profile class. It handels the stored profiles data.
    /// </summary>
    public class RepoProfile : IProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepoProfile"/> class.
        /// </summary>
        /// <param name="userName">Profile name.</param>
        /// <param name="userId">Profile id.</param>
        public RepoProfile(string userName, int userId)
        {
            this.UserName = userName;
            this.UserId = userId;
            this.SaveToXml();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepoProfile"/> class.
        /// </summary>
        public RepoProfile()
        {
        }

        /// <summary>
        /// Gets the profile name.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the UserId.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Gets the profiel scores.
        /// </summary>
        public int Scores => (this.UnitsKilled * 100) + (this.GamesWon * 500);

        /// <summary>
        /// Gets the killed unit count.
        /// </summary>
        public int UnitsKilled { get; private set; }

        /// <summary>
        /// Gets the lost unit count.
        /// </summary>
        public int UnitsLost { get; private set; }

        /// <summary>
        /// Gets the win count.
        /// </summary>
        public int GamesWon { get; private set; }

        /// <summary>
        /// Gets the lose count.
        /// </summary>
        public int GamesLost { get; private set; }

        /// <summary>
        /// Static method, which creates a Profile entity from the given xml input.
        /// </summary>
        /// <param name="profX">Profile xml data.</param>
        /// <returns>New IProfile entity.</returns>
        public static IProfile CreateFromXml(XDocument profX)
        {
            RepoProfile prof = new RepoProfile();
            prof.UserId = int.Parse(profX?.Root.Element("Id").Value);
            prof.UserName = profX.Root.Element("UserName").Value;
            prof.GamesLost = int.Parse(profX.Root.Element("GamesLost").Value);
            prof.GamesWon = int.Parse(profX.Root.Element("GamesWon").Value);
            prof.UnitsKilled = int.Parse(profX.Root.Element("UnitsKilled").Value);
            prof.UnitsLost = int.Parse(profX.Root.Element("UnitsLost").Value);
            return prof;
        }

        /// <summary>
        /// Updates the profiles statistics after matches.
        /// </summary>
        /// <param name="unitsKilled">Kill count.</param>
        /// <param name="unitsLost">Lose count.</param>
        /// <param name="won">Did user won or not.</param>
        public void Update(int unitsKilled, int unitsLost, bool won)
        {
            this.UnitsKilled += unitsKilled;
            this.UnitsLost += unitsLost;
            if (won)
            {
                this.GamesWon++;
            }
            else
            {
                this.GamesLost++;
            }

            this.SaveToXml();
        }

        private void SaveToXml()
        {
            XDocument profX = new XDocument();
            profX.Add(new XElement("Profile"));
            profX.Root.Add(new XElement("UserName", this.UserName));
            profX.Root.Add(new XElement("Id", this.UserId));
            profX.Root.Add(new XElement("GamesWon", this.GamesWon));
            profX.Root.Add(new XElement("GamesLost", this.GamesLost));
            profX.Root.Add(new XElement("UnitsKilled", this.UnitsKilled));
            profX.Root.Add(new XElement("UnitsLost", this.UnitsLost));

            if (!Directory.Exists("profiles"))
            {
                Directory.CreateDirectory("profiles");
            }

            string path = "profiles/" + this.UserId + "_" + this.UserName;

            profX.Save(path);
        }
    }
}
