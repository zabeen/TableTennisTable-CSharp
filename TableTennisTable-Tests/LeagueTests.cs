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
    }
}
