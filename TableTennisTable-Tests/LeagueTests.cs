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
        [TestMethod]
        public void TestAddPlayer()
        {
            // Given
            League league = new League();

            // When
            league.AddPlayer("Bob");

            // Then
            var rows = league.GetRows();
            Assert.AreEqual(1, rows.Count);
            var firstRowPlayers = rows.First().GetPlayers();
            Assert.AreEqual(1, firstRowPlayers.Count);
            CollectionAssert.Contains(firstRowPlayers, "Bob");
        }

        [TestMethod]
        public void TestAddsPlayerToSecondRow()
        {
            // Given
            League league = new League();

            // When
            league.AddPlayer("Anna");
            league.AddPlayer("Bob");

            // Then
            var rows = league.GetRows();
            Assert.AreEqual(2, rows.Count);
            CollectionAssert.Contains(rows[0].GetPlayers(), "Anna");
            CollectionAssert.Contains(rows[1].GetPlayers(), "Bob");
        }

        [TestMethod]
        public void TestBuildsPyramid()
        {
            // Given
            League league = new League();

            // When
            league.AddPlayer("Anna");
            league.AddPlayer("Bob");
            league.AddPlayer("Camille");
            league.AddPlayer("David");
            league.AddPlayer("Emily");
            league.AddPlayer("Fuad");

            // Then
            var rows = league.GetRows();
            Assert.AreEqual(3, rows.Count);
            Assert.AreEqual(1, rows[0].GetPlayers().Count);
            Assert.AreEqual(2, rows[1].GetPlayers().Count);
            Assert.AreEqual(3, rows[2].GetPlayers().Count);
        }

        [TestMethod]
        public void TestRejectsDuplicates()
        {
            // Given
            League league = new League();

            // When
            league.AddPlayer("Anna");

            // Then
            Assert.ThrowsException<ArgumentException>(() => league.AddPlayer("Anna"));
        }

        [TestMethod]
        public void TestRejectsNamesWithSpaces()
        {
            // Given
            League league = new League();

            // When/Then
            Assert.ThrowsException<ArgumentException>(() => league.AddPlayer("Anna Belle"));
        }

        [TestMethod]
        public void TestSwapsWinningAndLosingPlayers()
        {
            // Given
            League league = new League();
            league.AddPlayer("Anna");
            league.AddPlayer("Bob");
            league.AddPlayer("Camille");

            // When
            league.RecordWin("Camille", "Anna");

            // Then
            var rows = league.GetRows();
            Assert.AreEqual(2, rows.Count);
            CollectionAssert.AreEqual(rows[0].GetPlayers(), new List<string>() { "Camille" });
            CollectionAssert.AreEqual(rows[1].GetPlayers(), new List<string>() { "Bob", "Anna" });
        }

        [TestMethod]
        public void TestRejectsWinFromHigherLevelPlayer()
        {
            // Given
            League league = new League();
            league.AddPlayer("Anna");
            league.AddPlayer("Bob");
            league.AddPlayer("Camille");

            // When/Then
            Assert.ThrowsException<ArgumentException>(() => league.RecordWin("Anna", "Bob"));
        }

        [TestMethod]
        public void TestRejectsWinFromSameLevelPlayer()
        {
            // Given
            League league = new League();
            league.AddPlayer("Anna");
            league.AddPlayer("Bob");
            league.AddPlayer("Camille");

            // When/Then
            Assert.ThrowsException<ArgumentException>(() => league.RecordWin("Camille", "Bob"));
        }

        [TestMethod]
        public void TestRejectsWinFromNonexistentPlayer()
        {
            // Given
            League league = new League();
            league.AddPlayer("Anna");
            league.AddPlayer("Bob");

            // When/Then
            Assert.ThrowsException<ArgumentException>(() => league.RecordWin("Zabine", "Bob"));
        }

        [TestMethod]
        public void TestRejectsLossFromNonexistentPlayer()
        {
            // Given
            League league = new League();
            league.AddPlayer("Anna");
            league.AddPlayer("Bob");

            // When/Then
            Assert.ThrowsException<ArgumentException>(() => league.RecordWin("Anna", "Zabine"));
        }

        [TestMethod]
        public void TestGetWinnerReturnsNullWithNoPlayers()
        {
            // Given
            League league = new League();

            // When/Then
            Assert.IsNull(league.GetWinner());
        }

        [TestMethod]
        public void TestGetWinnerReturnsFirstPlayer()
        {
            // Given
            League league = new League();
            league.AddPlayer("Anna");
            league.AddPlayer("Bob");
            league.AddPlayer("Camille");

            // When/Then
            Assert.AreEqual("Anna", league.GetWinner());
        }

        [TestMethod]
        public void TestGetWinnerReturnsNewWinner()
        {
            // Given
            League league = new League();
            league.AddPlayer("Anna");
            league.AddPlayer("Bob");
            league.AddPlayer("Camille");

            // When
            league.RecordWin("Camille", "Anna");

            // Then
            Assert.AreEqual("Camille", league.GetWinner());
        }
    }
}
