using System;
using System.Collections.Generic;
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
            var oldRowCount = league.GetRows().Count;

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var newRowCount = league.GetRows().Count;
            Assert.AreEqual(oldRowCount + 1, newRowCount);
        }

        [TestMethod]
        public void LeagueWithAllRowsFull_AddPlayer_OnePlayerInBottomRow()
        {
            var league = new League();
            const string existingPlayer = "existingPlayer";
            league.AddPlayer(existingPlayer);

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var rows = league.GetRows();
            var bottomRowPlayers = rows.Last().GetPlayers();
            Assert.AreEqual(1, bottomRowPlayers.Count);
        }

        [TestMethod]
        public void LeagueWithAllRowsFull_AddPlayer_BottomRowContainsPlayerName()
        {
            var league = new League();
            const string existingPlayer = "existingPlayer";
            league.AddPlayer(existingPlayer);

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var bottomRow = league.GetRows().Last();
            var bottomRowPlayers = bottomRow.GetPlayers();
            CollectionAssert.Contains(bottomRowPlayers, newPlayer);
        }

        [TestMethod]
        public void LeagueWithIncompleteBottomRow_AddPlayer_RowCountDoesNotChange()
        {
            var league = new League();
            const string existingPlayer1 = "existingPlayer1";
            const string existingPlayer2 = "existingPlayer2";
            league.AddPlayer(existingPlayer1);
            league.AddPlayer(existingPlayer2);
            var oldRowCount = league.GetRows().Count;

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var newRowCount = league.GetRows().Count;
            Assert.AreEqual(oldRowCount, newRowCount);
        }

        [TestMethod]
        public void LeagueWithIncompleteBottomRow_AddPlayer_PlayerCountInBottomRowIncreasedByOne()
        {
            var league = new League();
            const string existingPlayer1 = "existingPlayer1";
            const string existingPlayer2 = "existingPlayer2";
            league.AddPlayer(existingPlayer1);
            league.AddPlayer(existingPlayer2);
            var bottomRow = league.GetRows().Last();
            var oldPlayerCount = bottomRow.GetPlayers().Count;

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var updatedLastRow = league.GetRows().Last();
            var newPlayerCount = updatedLastRow.GetPlayers().Count;
            Assert.AreEqual(oldPlayerCount + 1, newPlayerCount);
        }

        [TestMethod]
        public void LeagueWithIncompleteBottomRow_AddPlayer_BottomRowContainsPlayerName()
        {
            var league = new League();
            const string existingPlayer1 = "existingPlayer1";
            const string existingPlayer2 = "existingPlayer2";
            league.AddPlayer(existingPlayer1);
            league.AddPlayer(existingPlayer2);

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var bottomRow = league.GetRows().Last();
            var bottomRowPlayers = bottomRow.GetPlayers();
            CollectionAssert.Contains(bottomRowPlayers, newPlayer);
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

        [TestMethod]
        public void NewLeagueFromProvidedRows_GetRows_ProvidedRowsReturned()
        {
            var row1 = new LeagueRow(1);
            var row2 = new LeagueRow(2);
            var providedRows = new List<LeagueRow> {row1, row2};
            var league = new League(providedRows);

            var returnedRows = league.GetRows();

            CollectionAssert.AreEqual(providedRows, returnedRows);
        }

        [TestMethod]
        public void NewLeagueFromEmptyCollection_GetRows_EmptyCollectionReturned()
        {
            var providedRows = new List<LeagueRow>();
            var league = new League(providedRows);

            var returnedRows = league.GetRows();

            CollectionAssert.AreEqual(providedRows, returnedRows);
        }

        [TestMethod]
        public void NewLeagueFromNull_GetRows_EmptyCollectionReturned()
        {
            var league = new League(null);

            var returnedRows = league.GetRows();

            CollectionAssert.AreEqual(new List<LeagueRow>(), returnedRows);
        }
    }
}
