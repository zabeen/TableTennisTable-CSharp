using System;

namespace TableTennisTable_CSharp
{
    public class App
    {
        private League _league;

        public App(League initialLeague)
        {
            _league = initialLeague;
        }

        public string SendCommand(string command)
        {
            try
            {
                if (command.StartsWith("add player"))
                {
                    string player = command.Substring(11);
                    _league.AddPlayer(player);
                    return $"Added player {player}";
                }

                if (command.StartsWith("record win"))
                {
                    string playersString = command.Substring(11);
                    var players = playersString.Split(' ');
                    string winner = players[0];
                    string loser = players[1];
                    _league.RecordWin(winner, loser);
                    return $"Recorded {winner} win against {loser}";
                }

                if (command == "print")
                {
                    return LeagueRenderer.Render(_league);
                }

                if (command == "winner")
                {
                    return _league.GetWinner();
                }

                return $"Unknown command: {command}";
            }
            catch (ArgumentException e)
            {
                return e.Message;
            }
        }
    }
}
