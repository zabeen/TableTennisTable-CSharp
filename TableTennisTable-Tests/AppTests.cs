using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using TableTennisTable_CSharp;

namespace TableTennisTable_Tests
{
    [TestClass]
    public class AppTests
    {
        [TestMethod]
        public void TestPrintsCurrentState()
        {
            var league = new League();
            var renderer = new Mock<ILeagueRenderer>();
            renderer.Setup(r => r.Render(league)).Returns("Rendered League");

            var app = new App(league, renderer.Object, null);

            Assert.AreEqual("Rendered League", app.SendCommand("print"));
        }

        [TestMethod]
        public void TestAddsPlayer()
        {
            var league = new League();
            var app = new App(league, null, null);

            app.SendCommand("add player Anna");

            var rows = league.GetRows();
            Assert.AreEqual(1, rows.Count);
            var firstRowPlayers = rows.First().GetPlayers();
            Assert.AreEqual(1, firstRowPlayers.Count);
            CollectionAssert.Contains(firstRowPlayers, "Anna");
        }

        [TestMethod]
        public void TestRecordsWins()
        {
            var league = new League();
            league.AddPlayer("Anna");
            league.AddPlayer("Bob");
            var app = new App(league, null, null);

            app.SendCommand("record win Bob Anna");

            var rows = league.GetRows();
            CollectionAssert.Contains(rows[0].GetPlayers(), "Bob");
            CollectionAssert.Contains(rows[1].GetPlayers(), "Anna");
        }

        [TestMethod]
        public void TestReportsInvalidWins()
        {
            var league = new League();
            league.AddPlayer("Anna");
            var app = new App(league, null, null);

            Assert.AreEqual("Player Zabine is not in the game", app.SendCommand("record win Anna Zabine"));
        }

        [TestMethod]
        public void TestShowsWinner()
        {
            var league = new League();
            league.AddPlayer("Anna");
            var app = new App(league, null, null);

            Assert.AreEqual("Anna", app.SendCommand("winner"));
        }

        [TestMethod]
        public void TestSavesCurrentState()
        {
            var league = new League();
            var fileService = new Mock<IFileService>();
            var app = new App(league, null, fileService.Object);

            app.SendCommand("save test-tournament");

            fileService.Verify(f => f.Save("test-tournament", league));
        }

        [TestMethod]
        public void TestLoadsFromFile()
        {
            var league = new League();
            var leagueToLoad = new League();
            leagueToLoad.AddPlayer("Anna");
            var fileService = new Mock<IFileService>();
            fileService.Setup(f => f.Load("test-tournament")).Returns(leagueToLoad);
            var app = new App(league, null, fileService.Object);

            app.SendCommand("load test-tournament");

            Assert.AreEqual("Anna", app.SendCommand("winner"));
        }

        [TestMethod]
        public void TestInvalidCommands()
        {
            var league = new League();
            var app = new App(league, null, null);

            Assert.AreEqual("Unknown command: invalid", app.SendCommand("invalid"));
        }
    }
}
