// <copyright file="UnitTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

[assembly: System.CLSCompliant(false)]

namespace TankPocalypse.LogicTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Numerics;
    using System.Xml.Linq;
    using Moq;
    using NUnit.Framework;
    using TankPocalypse.Logic;
    using TankPocalypse.Logic.Interfaces;
    using TankPocalypse.Logic.UIClasses;
    using TankPocalypse.Model.Interfaces;
    using TankPocalypse.Repository.Classes;
    using TankPocalypse.Repository.Interfaces;

    /// <summary>
    /// Unit test class. Contains every unit test cases.
    /// </summary>
    [TestFixture]
    public class UnitTests
    {
        /// <summary>
        /// Tests the savegame method inside GameLogic.cs.
        /// </summary>
        [Test]
        public void SaveGameTest()
        {
            Mock<IGameRepository> mockedRepo = new Mock<IGameRepository>();
            GameLogic gm = new GameLogic(null, mockedRepo.Object, null, null, null, null);
            gm.SaveGame();
            mockedRepo.Verify(r => r.SaveGame(), Times.Once);
        }

        /// <summary>
        /// Tests the GetAllProfiles method inside GameLogic.cs.
        /// </summary>
        [Test]
        public void GetAllProfilesTest()
        {
            Mock<IGameRepository> mockedRepo = new Mock<IGameRepository>();
            GameLogic gameLogic = new GameLogic(null, mockedRepo.Object, null, null, null, null);
            List<IProfile> repoProfileList = new List<IProfile>
            {
                new RepoProfile("tesztElek", 0),
                new RepoProfile("tesztAnna", 1),
                new RepoProfile("tesztBéle", 2),
            };

            mockedRepo.Setup(r => r.GetAllProfiles()).Returns(repoProfileList);
            ObservableCollection<IUIProfile> expectedProfiles = new ObservableCollection<IUIProfile>
            {
                new UIProfile("tesztElek", 0),
                new UIProfile("tesztAnna", 1),
                new UIProfile("tesztBéle", 2),
            };

            var result = gameLogic.GetAllProfiles();
            Assert.That(result, Is.EquivalentTo(expectedProfiles));
            mockedRepo.Verify(r => r.GetAllProfiles(), Times.Once);
        }

        /// <summary>
        /// Tests the GetAllMaps method inside GameLogic.cs.
        /// </summary>
        [Test]
        public void GetAllMapsTest()
        {
            Mock<IGameRepository> mockedRepo = new Mock<IGameRepository>();
            GameLogic gameLogic = new GameLogic(null, mockedRepo.Object, null, null, null, null);
            List<IMap> repoMaps = new List<IMap>
            {
                new Map("tesztMap0", new List<string> { "2", "2", "00", "00" }),
                new Map("tesztMap1", new List<string> { "3", "3", "001", "002", "000" }),
            };

            mockedRepo.Setup(r => r.GetAllMaps()).Returns(repoMaps);
            List<IUIMap> expectedMaps = new List<IUIMap>
            {
                new UIMap("tesztMap0", new List<string> { "2", "2", "00", "00" }),
                new UIMap("tesztMap1", new List<string> { "3", "3", "001", "002", "000" }),
            };

            var result = gameLogic.GetAllMaps();
            Assert.That(result, Is.EquivalentTo(expectedMaps));
            mockedRepo.Verify(r => r.GetAllMaps(), Times.Once);
        }

        /// <summary>
        /// Tests the GetAllSavedGame method inside GameLogic.cs.
        /// </summary>
        [Test]
        public void GetAllSavedGameTest()
        {
            Mock<IGameRepository> mockedRepo = new Mock<IGameRepository>();
            GameLogic gameLogic = new GameLogic(null, mockedRepo.Object, null, null, null, null);
            List<ISavedGame> repoSaveGame = new List<ISavedGame>
            {
                new SavedGame(0, "TESZT", new XDocument()),
                new SavedGame(1, "TESZT2", new XDocument()),
                new SavedGame(2, "tesztteszt2", new XDocument()),
            };

            mockedRepo.Setup(r => r.GetSavedGames()).Returns(repoSaveGame);
            List<UISavedGame> expectedSavedGames = new List<UISavedGame>
            {
                new UISavedGame(0, "TESZT", new XDocument()),
                new UISavedGame(1, "TESZT2", new XDocument()),
                new UISavedGame(2, "tesztteszt2", new XDocument()),
            };

            var result = gameLogic.GetAllSavedGames();
            Assert.That(result, Is.EquivalentTo(expectedSavedGames));
            mockedRepo.Verify(r => r.GetSavedGames(), Times.Once);
        }

        /// <summary>
        /// Tests the ApplyWorldCenterCameraPosition method inside GameInputController.cs.
        /// </summary>
        [Test]
        public void CameraCenterTester()
        {
            Mock<IGameWorld> mockedGameWorld = new Mock<IGameWorld>(MockBehavior.Loose);
            Mock<IGameModel> mockedGameModel = new Mock<IGameModel>();

            mockedGameWorld.Setup(w => w.GridWidth).Returns(8);
            mockedGameWorld.Setup(w => w.GridHeight).Returns(8);

            mockedGameModel.SetupProperty(m => m.ScreenOffset);

            IGameInputController gameInputController = new GameInputController(mockedGameModel.Object, mockedGameWorld.Object, null);

            var expectedVector = new Vector2(557, 55.5f);

            gameInputController.ApplyWorldCenterCameraPosition(1264, 711);

            mockedGameWorld.Verify(w => w.GridHeight, Times.Once);
            mockedGameWorld.Verify(w => w.GridWidth, Times.Once);

            Assert.That(mockedGameModel.Object.ScreenOffset, Is.EqualTo(expectedVector));
        }
    }
}
