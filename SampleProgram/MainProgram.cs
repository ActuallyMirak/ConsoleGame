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

            do
            {
                Variables variables = new Variables();

                Program.BuildGame(variables.enemyLines, true);

                int x = Variables.playerLines.Input.Count * 3 + 20;
                int y = Variables.playerLines.Input.Count / 2;

                while (Variables.loopController)
                {
                    ConsoleKey cKey = ConsoleKey.Enter;

                    Program.BuildGame(variables.enemyLines, false);

                    Addition.SetPositionAndWrite(x, y, $"Your Points: {Addition.HighscoreCalculator(variables.Highscore, true)}");

                    Addition.SetPositionAndWrite(x, y + 1, $"Your Lives: {variables.Lives}");

                    Console.SetCursorPosition(Variables.playerLines.Input.Count * 3 + 10, Variables.playerLines.Input.Count / 2);
                    for (int i = variables.time; i > 0; i--)
                    {
                        Thread.Sleep(15);
                        if (Console.KeyAvailable)
                        {
                            cKey = Console.ReadKey().Key;
                            Console.Write("\b ");
                            break;
                        }
                    }

                    variables.Highscore = Addition.HighscoreCalculator(variables.Highscore, false);

                    try
                    {
                        Addition.MovementSelection(cKey, Variables.playerLines.Input);

                        Variables.playerLines.Input[Variables.currentPosition] = "   ";
                        Variables.playerLines.Input[Variables.newPosition] = Variables.entities[0];

                        Variables.currentPosition = Variables.newPosition;
                    }
                    catch (WrongInputException) { }

                    variables.Lives = Addition.HitCalculator(Variables.playerLines.Input, variables.Lives, variables.Highscore);
                }
                Console.CursorVisible = true;

                Addition.RepeatProgram(Variables.playerLines.Input, variables.Highscore);

                Console.Clear();
            }
            while (Variables.loopController);
        }
    }
}
