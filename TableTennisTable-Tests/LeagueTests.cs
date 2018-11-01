using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableTennisTable_CSharp;

namespace TableTennisTable_Tests
{
    [TestClass]
    public class LeagueTests
    {
        [TestMethod]
        public void EmptyLeague_AddPlayer_RowCountOne()
        {
            var league = new League();

            league.AddPlayer("Bob");

            var rows = league.GetRows();
            Assert.AreEqual(1, rows.Count);
        }

        [TestMethod]
        public void EmptyLeague_AddPlayer_OnePlayerInFirstRow()
        {
            var league = new League();

            league.AddPlayer("Bob");

            var rows = league.GetRows();
            var firstRowPlayers = rows.First().GetPlayers();
            Assert.AreEqual(1, firstRowPlayers.Count);
        }

        [TestMethod]
        public void EmptyLeague_AddPlayer_FirstRowContainsPlayerName()
        {
            var league = new League();

            league.AddPlayer("Bob");

            var rows = league.GetRows();
            var firstRowPlayers = rows.First().GetPlayers();
            CollectionAssert.Contains(firstRowPlayers, "Bob");
        }
    }
}
