using System;

using SampleProgram.Additions;
using SampleProgram.Common;

using System.IO;
using System.Text.Json;
using System.Threading;

namespace SampleProgram
{
    class MainProgram
    {
        public static void SettingUpPlayerLines()
        {
            if (!File.Exists(Variables.linePath))
            {

                for (int i = 0; i < 9; i++)
                {
                    if (i == 4)
                    {
                        Variables.playerLines.Input.Add(Variables.entities[0]);
                    }
                    else
                    {
                        Variables.playerLines.Input.Add("   ");
                    }
                }
            }
            else
            {
                Variables.playerLines = JsonSerializer.Deserialize<LineClass>(File.ReadAllText(Variables.linePath));
            }
            if (Variables.playerLines.Input.Count <= 2) { Console.WriteLine("To small Game"); Console.ReadKey(); }

            for (int i = 0; i < Variables.playerLines.Input.Count; i++)
            {
                if (Variables.playerLines.Input[i] == Variables.entities[0])
                {
                    Variables.currentPosition = i;
                }
            }
            Variables.enemyLinesCount = Variables.playerLines.Input.Count;
        }

        static void Main()
        {
            Program.Start();

            Addition.NameSelection();
            Addition.DifficultySelection();

            SettingUpPlayerLines();

        Start:
            Program.SetGameUp();

            Program.BuildGame(true);

            while (!Variables.loopController)
            {
                TimeSpan timeSpan = new TimeSpan(0, 0, Variables.time);
                ConsoleKey cKey = ConsoleKey.Enter;

                Program.BuildGame(false);

                Console.SetCursorPosition(Variables.playerLines.Input.Count * 3 + 20, Variables.playerLines.Input.Count / 2);
                Console.Write("Your Points: {0}", Addition.HighscoreCalculator(true));

                Console.SetCursorPosition(Variables.playerLines.Input.Count * 3 + 20, Variables.playerLines.Input.Count / 2 + 1);
                Console.Write("Your Lives: {0}", Variables.Lives);

                Console.SetCursorPosition(Variables.playerLines.Input.Count * 3 + 10, Variables.playerLines.Input.Count / 2);
                while (timeSpan != new TimeSpan(0, 0, 0))
                {
                    Thread.Sleep(15);
                    if (Console.KeyAvailable)
                    {
                        cKey = Console.ReadKey().Key;
                        Console.Write("\b ");
                        break;
                    }
                    else
                    {
                        timeSpan = timeSpan.Subtract(new TimeSpan(0, 0, 1));
                    }
                }

                Addition.HighscoreCalculator(false);

                try
                {
                    Addition.MovementSelection(cKey, Variables.playerLines.Input);

                    Variables.playerLines.Input[Variables.currentPosition] = "   ";
                    Variables.playerLines.Input[Variables.newPosition] = Variables.entities[0];

                    Variables.currentPosition = Variables.newPosition;
                }
                catch (WrongInputException) { }

                Addition.HitCalculator(Variables.playerLines.Input);
            }
            Console.CursorVisible = true;

            Addition.RepeatProgram(Variables.playerLines.Input);

            if (!Variables.loopController)
            {
                Console.Clear();
                goto Start;
            }
        }
    }
}
