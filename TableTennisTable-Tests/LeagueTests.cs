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
            var fullRow = new LeagueRow(1);
            const string existingPlayer = "existingPlayer";
            fullRow.Add(existingPlayer);
            var fullRows = new List<LeagueRow> {fullRow};
            var league = new League(fullRows);
            var oldRowCount = fullRows.Count;

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var newRowCount = league.GetRows().Count;
            Assert.AreEqual(oldRowCount + 1, newRowCount);
        }

        [TestMethod]
        public void LeagueWithAllRowsFull_AddPlayer_OnePlayerInBottomRow()
        {
            var fullRow = new LeagueRow(1);
            const string existingPlayer = "existingPlayer";
            fullRow.Add(existingPlayer);
            var fullRows = new List<LeagueRow> { fullRow };
            var league = new League(fullRows);

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var bottomRow = league.GetRows().Last();
            var bottomRowPlayers = bottomRow.GetPlayers();
            Assert.AreEqual(1, bottomRowPlayers.Count);
        }

        [TestMethod]
        public void LeagueWithAllRowsFull_AddPlayer_BottomRowContainsPlayerName()
        {
            var fullRow = new LeagueRow(1);
            const string existingPlayer = "existingPlayer";
            fullRow.Add(existingPlayer);
            var fullRows = new List<LeagueRow> { fullRow };
            var league = new League(fullRows);

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var bottomRow = league.GetRows().Last();
            var bottomRowPlayers = bottomRow.GetPlayers();
            CollectionAssert.Contains(bottomRowPlayers, newPlayer);
        }

        [TestMethod]
        public void LeagueWithIncompleteBottomRow_AddPlayer_RowCountDoesNotChange()
        {
            const string existingPlayer1 = "existingPlayer1";
            var fullRow = new LeagueRow(1);
            fullRow.Add(existingPlayer1);

            const string existingPlayer2 = "existingPlayer2";
            var incompleteRow = new LeagueRow(2);
            incompleteRow.Add(existingPlayer2);

            var rows = new List<LeagueRow> { fullRow, incompleteRow };
            var league = new League(rows);
            var oldRowCount = rows.Count;

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var newRowCount = league.GetRows().Count;
            Assert.AreEqual(oldRowCount, newRowCount);
        }

        [TestMethod]
        public void LeagueWithIncompleteBottomRow_AddPlayer_PlayerCountInBottomRowIncreasedByOne()
        {
            const string existingPlayer1 = "existingPlayer1";
            var fullRow = new LeagueRow(1);
            fullRow.Add(existingPlayer1);

            const string existingPlayer2 = "existingPlayer2";
            var incompleteRow = new LeagueRow(2);
            incompleteRow.Add(existingPlayer2);
            var oldPlayerCount = incompleteRow.GetPlayers().Count;

            var rows = new List<LeagueRow> { fullRow, incompleteRow };
            var league = new League(rows);

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);
            
            var updatedLastRow = league.GetRows().Last();
            var newPlayerCount = updatedLastRow.GetPlayers().Count;
            Assert.AreEqual(oldPlayerCount + 1, newPlayerCount);
        }

        [TestMethod]
        public void LeagueWithIncompleteBottomRow_AddPlayer_BottomRowContainsPlayerName()
        {
            const string existingPlayer1 = "existingPlayer1";
            var fullRow = new LeagueRow(1);
            fullRow.Add(existingPlayer1);

            const string existingPlayer2 = "existingPlayer2";
            var incompleteRow = new LeagueRow(2);
            incompleteRow.Add(existingPlayer2);

            var rows = new List<LeagueRow> { fullRow, incompleteRow };
            var league = new League(rows);

            const string newPlayer = "newPlayer";
            league.AddPlayer(newPlayer);

            var bottomRow = league.GetRows().Last();
            var bottomRowPlayers = bottomRow.GetPlayers();
            CollectionAssert.Contains(bottomRowPlayers, newPlayer);
        }

        [DataRow("player1")]
        [DataRow("player2")]
        [DataTestMethod]
        public void LeagueWithPlayers_AddPlayerWithNonUniqueName_ExceptionThrown(string nonUniquePlayerName)
        {
            const string existingPlayer1 = "player1";
            var row1 = new LeagueRow(1);
            row1.Add(existingPlayer1);

            const string existingPlayer2 = "player2";
            var row2 = new LeagueRow(2);
            row2.Add(existingPlayer2);

            var rows = new List<LeagueRow> { row1, row2 };
            var league = new League(rows);

            Assert.ThrowsException<ArgumentException>(() => league.AddPlayer(nonUniquePlayerName));
        }

        [TestMethod]
        public void NewLeague_GetRows_EmptyCollectionReturned()
        {
            var league = new League();

            var returnedRows = league.GetRows();

            CollectionAssert.AreEqual(new List<LeagueRow>(), returnedRows);
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

        [TestMethod]
        public void NewLeague_RecordWinner_ExceptionThrown()
        {
            var league = new League();
            const string winner = "winner";
            const string loser = "loser";

            Assert.ThrowsException<ArgumentException>(() => league.RecordWin(winner, loser));
        }

        [TestMethod]
        public void LeagueWithWinnerOnly_RecordWinner_ExceptionThrown()
        {
            const string winner = "winner";
            const string loser = "loser";
            var league = new League();
            league.AddPlayer(winner);

            Assert.ThrowsException<ArgumentException>(() => league.RecordWin(winner, loser));
        }

        [TestMethod]
        public void LeagueWithLoserOnly_RecordWinner_ExceptionThrown()
        {
            const string winner = "winner";
            const string loser = "loser";
            var league = new League();
            league.AddPlayer(loser);

            Assert.ThrowsException<ArgumentException>(() => league.RecordWin(winner, loser));
        }

        [TestMethod]
        public void LeagueWithWinnerInRowAboveLoser_RecordWinner_ExceptionThrown()
        {
            const string winner = "winner";
            var row1 = new LeagueRow(1);
            row1.Add(winner);

            const string loser = "loser";
            var row2 = new LeagueRow(2);
            row2.Add(loser);

            var league = new League(new List<LeagueRow>{row1, row2});

            Assert.ThrowsException<ArgumentException>(() => league.RecordWin(winner, loser));
        }

        [TestMethod]
        public void LeagueWithWinnerInSameRowAsLoser_RecordWinner_ExceptionThrown()
        {
            const string winner = "winner";
            const string loser = "loser";
            var row = new LeagueRow(2);
            row.Add(winner);
            row.Add(loser);

            var league = new League(new List<LeagueRow> { row });

            Assert.ThrowsException<ArgumentException>(() => league.RecordWin(winner, loser));
        }

        [TestMethod]
        public void LeagueWithLoserTwoRowsAboveWinner_RecordWinner_ExceptionThrown()
        {
            const string loser = "loser";
            var row1 = new LeagueRow(1);
            row1.Add(loser);

            var row2 = new LeagueRow(1);

            const string winner = "winner";
            var row3 = new LeagueRow(1);
            row3.Add(winner);

            var league = new League(new List<LeagueRow> { row1, row2, row3 });

            Assert.ThrowsException<ArgumentException>(() => league.RecordWin(winner, loser));
        }

        [DataRow(null)]
        [DataRow("")]
        [DataTestMethod]
        public void LeagueWithLoserAboveIncompleteRow_RecordWinnerWithWinnerNameNullOrEmpty_ExceptionThrown(string winner)
        {
            const string loser = "loser";
            var row1 = new LeagueRow(1);
            row1.Add(loser);

            const string otherPlayer = "otherPlayer";
            var row2 = new LeagueRow(2);
            row2.Add(otherPlayer);

            var league = new League(new List<LeagueRow>{row1, row2});

            Assert.ThrowsException<ArgumentException>(() => league.RecordWin(winner, loser));
        }

        [DataRow(null)]
        [DataRow("")]
        [DataTestMethod]
        public void LeagueWithWinner_RecordWinnerWithLoserNameNullOrEmpty_ExceptionThrown(string loser)
        {
            const string winner = "winner";
            var row = new LeagueRow(2);
            row.Add(winner);

            var league = new League(new List<LeagueRow> { row });

            Assert.ThrowsException<ArgumentException>(() => league.RecordWin(winner, loser));
        }

        [TestMethod]
        public void LeagueWithWinnerInRowBelowLoser_RecordWinner_WinnerMovedUpARow()
        {
            const string loser = "loser";
            var row1 = new LeagueRow(1);
            row1.Add(loser);

            const string winner = "winner";
            var row2 = new LeagueRow(1);
            row2.Add(winner);

            var league = new League(new List<LeagueRow> { row1, row2 });

            league.RecordWin(winner, loser);

            var firstRow = league.GetRows().First();
            var firstRowPlayers = firstRow.GetPlayers();
            CollectionAssert.Contains(firstRowPlayers, winner);
        }

        [TestMethod]
        public void LeagueWithWinnerInRowBelowLoser_RecordWinner_LoserMovedDownARow()
        {
            const string loser = "loser";
            var row1 = new LeagueRow(1);
            row1.Add(loser);

            const string winner = "winner";
            var row2 = new LeagueRow(1);
            row2.Add(winner);

            var league = new League(new List<LeagueRow> { row1, row2 });

            league.RecordWin(winner, loser);

            var secondRow = league.GetRows()[1];
            var secondRowPlayers = secondRow.GetPlayers();
            CollectionAssert.Contains(secondRowPlayers, loser);
        }
    }
}
