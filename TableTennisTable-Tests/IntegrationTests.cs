using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TableTennisTable_CSharp;

namespace TableTennisTable_Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void TestPrintsEmptyGame()
        {
            var app = CreateApp();

            Assert.AreEqual("No players yet", app.SendCommand("print"));
        }

        [TestMethod]
        public void TestAddPlayer()
        {
            var app = CreateApp();
            app.SendCommand("add player Anna");

            Assert.AreEqual(
@"          -------------------
          |      Anna       |
          -------------------",
                app.SendCommand("print"));
        }

        [TestMethod]
        public void TestPyramidShape()
        {
            var app = CreateApp();
            app.SendCommand("add player Anna");
            app.SendCommand("add player Bob");
            app.SendCommand("add player Camille");
            app.SendCommand("add player David");

            Assert.AreEqual(
@"                              -------------------
                              |      Anna       |
                              -------------------
                    ------------------- -------------------
                    |       Bob       | |     Camille     |
                    ------------------- -------------------
          ------------------- ------------------- -------------------
          |      David      | |                 | |                 |
          ------------------- ------------------- -------------------",
                app.SendCommand("print"));
        }

        [TestMethod]
        public void TestCurrentWinner()
        {
            var app = CreateApp();
            app.SendCommand("add player Anna");
            app.SendCommand("add player Bob");

            Assert.AreEqual("Anna", app.SendCommand("winner"));
        }

        // Could include some record win tests here along similar lines

        [TestMethod]
        public void TestSaveEmptyToDisk()
        {
            var path = Path.GetTempFileName();
            var app = CreateApp();

            app.SendCommand($"save {path}");

            var fileContents = File.ReadAllText(path);
            Assert.AreEqual("", fileContents);
        }

        [TestMethod]
        public void TestSaveInProgressToDisk()
        {
            var path = Path.GetTempFileName();
            var app = CreateApp();
            app.SendCommand("add player Anna");
            app.SendCommand("add player Bob");
            app.SendCommand("add player Camille");
            app.SendCommand("add player David");

            app.SendCommand($"save {path}");

            var fileContents = File.ReadAllText(path);
            Assert.AreEqual(
@"Anna
Bob,Camille
David
",
                fileContents);
        }

        [TestMethod]
        public void TestSaveToInvalidPath()
        {
            var app = CreateApp();

            var response = app.SendCommand($"save invalid/path");
            Assert.AreEqual("Could not save league invalid/path", response);
        }

        [TestMethod]
        public void TestLoadEmptyFromDisk()
        {
            var path = Path.GetTempFileName();
            var app = CreateApp();
            app.SendCommand("add player Anna");

            app.SendCommand($"load {path}");

            Assert.AreEqual("No players yet", app.SendCommand("print"));
        }

        [TestMethod]
        public void TestLoadInProgressFromDisk()
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path,
@"Anna
Bob,Camille
David
");
            var app = CreateApp();

            app.SendCommand($"load {path}");

            Assert.AreEqual(
@"                              -------------------
                              |      Anna       |
                              -------------------
                    ------------------- -------------------
                    |       Bob       | |     Camille     |
                    ------------------- -------------------
          ------------------- ------------------- -------------------
          |      David      | |                 | |                 |
          ------------------- ------------------- -------------------",
                app.SendCommand("print"));
        }

        [TestMethod]
        public void TestLoadFromInvalidPath()
        {
            var app = CreateApp();

            var response = app.SendCommand($"load invalid/path");
            Assert.AreEqual("Could not load league invalid/path", response);
        }

        [TestMethod]
        public void TestLoadInvalidFileFromDisk()
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path, "invalid,state");
            var app = CreateApp();

            var response = app.SendCommand($"load {path}");
            Assert.AreEqual($"League {path} was not valid", response);
        }

        private App CreateApp()
        {
            return new App(new League(), new LeagueRenderer(), new FileService());
        }
    }
}
