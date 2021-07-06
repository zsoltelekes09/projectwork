// <copyright file="GameRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

[assembly: System.CLSCompliant(false)]

namespace TankPocalypse.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Xml.Linq;
    using GalaSoft.MvvmLight.Ioc;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Repository.Classes;
    using TankPocalypse.Repository.Interfaces;

    /// <summary>
    /// This is the games repository. It handels all the sotred data management.
    /// </summary>
    public class GameRepository : IGameRepository
    {
        private IGameModel gameModel;
        private IGameWorld gameWorld;
        private List<IProfile> profiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameRepository"/> class.
        /// </summary>
        /// <param name="gameModel">GameModel entity.</param>
        /// <param name="gameWorld">GameWorld entity.</param>
        [PreferredConstructor]
        public GameRepository(IGameModel gameModel, IGameWorld gameWorld)
        {
            this.gameModel = gameModel;
            this.gameWorld = gameWorld;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameRepository"/> class.
        /// </summary>
        public GameRepository()
        {
        }

        /// <summary>
        /// Loads the given saved game.
        /// </summary>
        /// <param name="savedGame">SavedGame entity.</param>
        public void LoadSaved(ISavedGame savedGame)
        {
            XDocument save = savedGame?.SaveXDoc;

            this.gameModel.SessionTime = int.Parse(save.Root.Element("SessionTime")?.Value);
            this.gameModel.Team0BasePercent = byte.Parse(save.Root.Element("Team0Base")?.Value);
            this.gameModel.Team1BasePercent = byte.Parse(save.Root.Element("Team1Base")?.Value);

            var mapX = save.Root.Element("WorldMap");
            List<string> mapToLoad = new List<string>();
            mapToLoad.Add(mapX.Element("Height")?.Value);
            mapToLoad.Add(mapX.Element("Width")?.Value);
            foreach (XElement rowX in mapX.Descendants("Row"))
            {
                mapToLoad.Add(rowX.Value);
            }

            this.gameWorld.SetupGrid(mapToLoad);

            var cultInfo = CultureInfo.InvariantCulture.NumberFormat;
            this.gameModel.CreateVehicles(byte.Parse(save.Root.Element("Vehicles").Attribute("Count").Value));

            int i = 0;
            foreach (XElement vehicle in save.Root.Element("Vehicles").Descendants("Vehicle"))
            {
                XElement posX = vehicle.Element("Position");
                Vector2 pos = new Vector2(float.Parse(posX.Element("X").Value, cultInfo), float.Parse(posX.Element("Y").Value, cultInfo));
                byte id = byte.Parse(vehicle.Element("Id").Value);
                byte hp = byte.Parse(vehicle.Element("Hp").Value);
                short bodyIdx = short.Parse(vehicle.Element("BodyIdx").Value);
                short turrIdx = short.Parse(vehicle.Element("TurretIdx").Value);
                XElement veloX = vehicle.Element("Velocity");
                Vector2 veloNorm = new Vector2(float.Parse(veloX.Element("X").Value, cultInfo), float.Parse(veloX.Element("Y").Value, cultInfo));
                XElement turrX = vehicle.Element("TurretNormal");
                Vector2 turrNorm = new Vector2(float.Parse(turrX.Element("X").Value, cultInfo), float.Parse(turrX.Element("Y").Value, cultInfo));

                this.gameModel.Vehicles[i].LoadFromSave(pos, hp, bodyIdx, turrIdx, veloNorm, turrNorm);
                i++;
            }
        }

        /// <summary>
        /// Saves the current state of the game.
        /// </summary>
        public void SaveGame()
        {
            XDocument save = new XDocument();
            save.Add(new XElement("SaveGameXML"));

            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }

            // save basic info
            DateTime currentDate = DateTime.Now;
            string desc = string.Empty;
            desc += this.gameModel.VehiclesTeam0.Count + "vs" + this.gameModel.VehiclesTeam1.Count + "#";
            desc += currentDate.Year + "_" + currentDate.Month + "_" + currentDate.Day + "#" + currentDate.Hour + "_" +
                    currentDate.Minute;

            save.Root.Add(new XElement("Description", desc));
            save.Root.Add(new XElement("SessionTime", this.gameModel.SessionTime));
            save.Root.Add(new XElement("Team0Base", this.gameModel.Team0BasePercent));
            save.Root.Add(new XElement("Team1Base", this.gameModel.Team1BasePercent));

            // save map
            XElement map = new XElement("WorldMap");
            map.Add(new XElement("Height", this.gameWorld.GridHeight));
            map.Add(new XElement("Width", this.gameWorld.GridWidth));
            for (int j = 0; j < this.gameWorld.GridHeight; j++)
            {
                string rowString = string.Empty;
                for (int i = 0; i < this.gameWorld.GridWidth; i++)
                {
                    rowString += this.gameWorld.WorldGrid[j, i].NodeType;
                }

                map.Add(new XElement("Row", rowString));
            }

            save.Root.Add(map);

            // save vehicles
            XElement vehicles = new XElement("Vehicles");
            vehicles.Add(new XAttribute("Count", this.gameModel.Vehicles.Count));

            foreach (IVehicle vehicle in this.gameModel.Vehicles)
            {
                XElement tank = new XElement("Vehicle");
                tank.Add(new XElement("Id", vehicle.UniqueId));
                tank.Add(new XElement("Hp", vehicle.Health));
                XElement location = new XElement("Position");
                location.Add(new XElement("X", vehicle.Position.X));
                location.Add(new XElement("Y", vehicle.Position.Y));
                tank.Add(new XElement(location));
                tank.Add(new XElement("BodyIdx", vehicle.BodyIdx));
                tank.Add(new XElement("TurretIdx", vehicle.TurretIdx));
                XElement velocity = new XElement("Velocity");
                velocity.Add(new XElement("X", vehicle.Velocity.X));
                velocity.Add(new XElement("Y", vehicle.Velocity.Y));
                tank.Add(new XElement(velocity));
                XElement turretNorm = new XElement("TurretNormal");
                turretNorm.Add(new XElement("X", vehicle.TurretDirectionNormal.X));
                turretNorm.Add(new XElement("Y", vehicle.TurretDirectionNormal.Y));
                tank.Add(new XElement(turretNorm));
                vehicles.Add(tank);
            }

            save.Root.Add(vehicles);

            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }

            string savePath = "saves/savegame_" + Directory.GetFiles("saves").Length + ".xml";
            save.Save(savePath);

            System.Diagnostics.Debug.WriteLine("GameRepository SaveGame Method RAN!");
        }

        /// <summary>
        /// Returns every saved game files in a list.
        /// </summary>
        /// <returns>List of ISavedGame entities.</returns>
        public List<ISavedGame> GetSavedGames()
        {
            List<ISavedGame> saves = new List<ISavedGame>();

            if (Directory.Exists("saves"))
            {
                var filesName = Directory.GetFiles("saves");
                for (int i = 0; i < filesName.Length; i++)
                {
                    var save = XDocument.Load(filesName[i]);
                    SavedGame savedGame = SavedGame.CreateSavedGameFromXML(save);
                    saves.Add(savedGame);
                }
            }
            else
            {
                Directory.CreateDirectory("saves");
            }

            return saves;
        }

        /// <summary>
        /// Returns every stored profile in a list.
        /// </summary>
        /// <returns>IProfile list.</returns>
        public List<IProfile> GetAllProfiles()
        {
            if (this.profiles == null)
            {
                if (!Directory.Exists("profiles"))
                {
                    Directory.CreateDirectory("profiles");
                }

                var files = Directory.GetFiles("profiles");
                this.profiles = new List<IProfile>();
                foreach (string file in files)
                {
                    XDocument profX = XDocument.Load(file);
                    this.profiles.Add(RepoProfile.CreateFromXml(profX));
                }
            }

            return this.profiles;
        }

        /// <summary>
        /// Returns every stored map in a list.
        /// </summary>
        /// <returns>IMap list.</returns>
        public List<IMap> GetAllMaps()
        {
            List<IMap> maps = new List<IMap>();

            if (!Directory.Exists("maps"))
            {
                Directory.CreateDirectory("maps");
            }

            var mapFiles = Directory.GetFiles("maps");

            if (mapFiles.Length == 0)
            {
                return maps;
            }

            foreach (string path in mapFiles)
            {
                XDocument xMap = XDocument.Load(path);
                IMap map = Map.CreateMapFromXml(xMap);
                maps.Add(map);
            }

            return maps;
        }

        /// <summary>
        /// Updates the profile statistics.
        /// </summary>
        /// <param name="id">Profile id.</param>
        /// <param name="userName">Profile name.</param>
        /// <param name="didWin">Bool, which represents the outcome of the game.</param>
        /// <param name="killedUnits">Killed unit count.</param>
        /// <param name="lostUnits">Lost unit count.</param>
        public void UpdateProfile(int id, string userName, bool didWin, int killedUnits, int lostUnits)
        {
            string filePath = "profiles/" + id + "_" + userName;
            XDocument profile = XDocument.Load(filePath);
            if (didWin)
            {
                int wonCount = int.Parse(profile.Root.Element("GamesWon").Value);
                wonCount++;
                profile.Root.Element("GamesWon").Value = wonCount.ToString();
            }
            else
            {
                int looseCount = int.Parse(profile.Root.Element("GamesLost").Value);
                looseCount++;
                profile.Root.Element("GamesLost").Value = looseCount.ToString();
            }

            int killCount = int.Parse(profile.Root.Element("UnitsKilled").Value);
            killCount += killedUnits;
            profile.Root.Element("UnitsKilled").Value = killCount.ToString();

            int lostCount = int.Parse(profile.Root.Element("UnitsLost").Value);
            lostCount += lostUnits;
            profile.Root.Element("UnitsLost").Value = lostCount.ToString();

            profile.Save(filePath);
        }

        /// <summary>
        /// Deletes the profile by id.
        /// </summary>
        /// <param name="id">Profile id.</param>
        public void DeleteProfile(int id)
        {
            IProfile toDelete = this.profiles.FirstOrDefault(x => x.UserId == id);
            if (toDelete != null)
            {
                this.profiles.Remove(toDelete);
                string toDelString = "profiles/" + toDelete.UserId + "_" + toDelete.UserName;
                File.Delete(toDelString);
            }
        }

        /// <summary>
        /// Adds a new profile to the repository.
        /// </summary>
        /// <param name="profil">New profile.</param>
        public void AddProfile(IProfile profil)
        {
            this.profiles.Add(profil);
        }
    }
}
