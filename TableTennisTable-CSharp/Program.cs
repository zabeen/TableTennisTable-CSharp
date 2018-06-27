using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennisTable_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the table tennis table");

            var league = new League();
            var game = new App(league);

            bool isGameActive = true;

            while (isGameActive)
            {
                Console.Write("> ");
                string command = Console.ReadLine();

                if (command == "quit")
                {
                    isGameActive = false;
                }
                else
                {
                    Console.WriteLine(game.SendCommand(command));
                }


            }
        }
    }
}
