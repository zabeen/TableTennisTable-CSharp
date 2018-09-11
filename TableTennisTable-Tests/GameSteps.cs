using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TableTennisTable_CSharp;
using TechTalk.SpecFlow;

namespace TableTennisTable_Tests
{
    [Binding]
    public class GameSteps
    {
        // Current app instance
        private App app;

        // Last response
        private String response;

        [BeforeScenario]
        public void CreateApp()
        {
            var league = new League();
            var leagueRenderer = new LeagueRenderer();
            var fileService = new FileService();
            app = new App(league, leagueRenderer, fileService);
        }

        [Given("the league has no players")]
        public void GivenTheLeagueHasNoPlayers()
        {
            // Nothing to do - the default league starts with no players
        }

        [Given("the league has players")]
        public void GivenTheLeagueHasPlayers(Table players)
        {
            players.Rows.ToList().ForEach(row => app.SendCommand($"add player {row["Player"]}"));
        }
        
        [When("I print the league")]
        public void WhenIPrintTheLeague()
        {
            response = app.SendCommand("print");
        }

        [When("I check the winner")]
        public void WhenICheckTheWinner()
        {
            response = app.SendCommand("winner");
        }

        [When("\"(.*)\" wins a game against \"(.*)\"")]
        public void WhenWinIsRecorded(string winner, string loser)
        {
            response = app.SendCommand($"record win {winner} {loser}");
        }

        [When("I save the game to \"(.*)\"")]
        public void WhenISave(string file)
        {
            response = app.SendCommand($"save {file}");
        }

        [When("I load the game from \"(.*)\"")]
        public void WhenILoad(string file)
        {
            response = app.SendCommand($"load {file}");
        }

        [When("I reset the game")]
        public void WhenIReset()
        {
            CreateApp();
        }

        [Then("I should see \"(.*)\"")]
        public void ThenIShouldSee(string expected)
        {
            Assert.AreEqual(expected, response);
        }

        [Then("I should see \"(.*)\" in row (\\d+)")]
        public void ThenIShouldSeeInRow(string name, int row)
        {
            var rowIndex = 3 * row - 2;
            var responseRow = response.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)[rowIndex];

            StringAssert.Contains(responseRow, name);
        }
    }
}
