using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableTennisTable_CSharp;

namespace TableTennisTable_Tests
{
    [TestClass]
    public class LeagueTests
    {
        [DataRow("name-with-hyphens")]
        [DataRow("name with spaces")]
        [DataRow("name.With,Punctuation!")]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        [DataTestMethod]
        public void NewLeague_AddPlayerWithInvalidPlayerName_ExceptionThrown(string playerName)
        {
            var league = new League();

            Assert.ThrowsException<ArgumentException>(() => league.AddPlayer(playerName));
        }

        [TestMethod]
        public void NewLeague_AddPlayer_RowCountOne()
        {
            const string playerName = "testPlayer";
            var league = new League();

            league.AddPlayer(playerName);

            var rows = league.GetRows();
            Assert.AreEqual(1, rows.Count);
        }

        [TestMethod]
        public void NewLeague_AddPlayer_OnePlayerInFirstRow()
        {
            const string playerName = "testPlayer";
            var league = new League();

            league.AddPlayer(playerName);

            var rows = league.GetRows();
            var firstRowPlayers = rows.First().GetPlayers();
            Assert.AreEqual(1, firstRowPlayers.Count);
        }

        [TestMethod]
        public void NewLeague_AddPlayer_FirstRowContainsPlayerName()
        {
            const string playerName = "testPlayer";
            var league = new League();

            league.AddPlayer(playerName);

            var rows = league.GetRows();
            var firstRowPlayers = rows.First().GetPlayers();
            CollectionAssert.Contains(firstRowPlayers, playerName);
        }

        [TestMethod]
        public void LeagueWithAllRowsFull_AddPlayer_NewRowAdded()
        {
            var league = new League();
            const string existingPlayer = "existingPlayer";
            league.AddPlayer(existingPlayer);
            var existingRowCount = league.GetRows().Count;

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var newRowCount = league.GetRows().Count;
            Assert.AreEqual(existingRowCount + 1, newRowCount);
        }

        [TestMethod]
        public void LeagueWithAllRowsFull_AddPlayer_OnePlayerInLastRow()
        {
            var league = new League();
            const string existingPlayer = "existingPlayer";
            league.AddPlayer(existingPlayer);

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var rows = league.GetRows();
            var lastRowPlayers = rows.Last().GetPlayers();
            Assert.AreEqual(1, lastRowPlayers.Count);
        }

        [TestMethod]
        public void LeagueWithAllRowsFull_AddPlayer_LastRowContainsPlayerName()
        {
            var league = new League();
            const string existingPlayer = "existingPlayer";
            league.AddPlayer(existingPlayer);

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var rows = league.GetRows();
            var lastRowPlayers = rows.Last().GetPlayers();
            CollectionAssert.Contains(lastRowPlayers, newPlayer);
        }

        [DataRow("player1")]
        [DataRow("player2")]
        [DataRow("player3")]
        [DataRow("player4")]
        [DataTestMethod]
        public void LeagueWithPlayers_AddPlayerWithNonUniqueName_ExceptionThrown(string nonUniquePlayerName)
        {
            var league = new League();
            var existingPlayers = new[] { "player1", "player2", "player3", "player4" };
            foreach (var existingPlayer in existingPlayers)
            {
                league.AddPlayer(existingPlayer);
            }

            Assert.ThrowsException<ArgumentException>(() => league.AddPlayer(nonUniquePlayerName));
        }
    }
}
