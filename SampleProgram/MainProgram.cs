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
            if (!File.Exists(Constants.linePath))
            {

                for (int i = 0; i < 9; i++)
                {
                    if (i == 4)
                    {
                        Constants.playerLines.Input.Add(Constants.entities[0]);
                    }
                    else
                    {
                        Constants.playerLines.Input.Add("   ");
                    }
                }
            }
            else
            {
                Constants.playerLines = JsonSerializer.Deserialize<LineClass>(File.ReadAllText(Constants.linePath));
            }
            if (Constants.playerLines.Input.Count <= 2) { Console.WriteLine("To small Game"); Console.ReadKey(); }

            for (int i = 0; i < Constants.playerLines.Input.Count; i++)
            {
                if (Constants.playerLines.Input[i] == Constants.entities[0])
                {
                    Constants.currentPosition = i;
                }
            }
            Constants.enemyLinesCount = Constants.playerLines.Input.Count;
        }

        static void Main()
        {
            Program.Start();

            Addition.NameSelection();
            Addition.DifficultySelection();

            SettingUpPlayerLines();

            do
            {
                Constants variables = new Constants();

                Program.BuildGame(variables.enemyLines, true);

                int x = Constants.playerLines.Input.Count * 3 + 20;
                int y = Constants.playerLines.Input.Count / 2;

                while (Constants.loopController)
                {
                    ConsoleKey cKey = ConsoleKey.Enter;

                    Program.BuildGame(variables.enemyLines, false);

                    Addition.SetPositionAndWrite(x, y, $"Your Points: {Addition.HighscoreCalculator(variables.Highscore, true)}");

                    Addition.SetPositionAndWrite(x, y + 1, $"Your Lives: {variables.Lives}");

                    Console.SetCursorPosition(Constants.playerLines.Input.Count * 3 + 10, Constants.playerLines.Input.Count / 2);
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
                        Addition.MovementSelection(cKey, Constants.playerLines.Input);

                        Constants.playerLines.Input[Constants.currentPosition] = "   ";
                        Constants.playerLines.Input[Constants.newPosition] = Constants.entities[0];

                        Constants.currentPosition = Constants.newPosition;
                    }
                    catch (WrongInputException) { }

                    variables.Lives = Addition.HitCalculator(Constants.playerLines.Input, variables.Lives, variables.Highscore);
                }
                Console.CursorVisible = true;

                Addition.RepeatProgram(Constants.playerLines.Input, variables.Highscore);

                Console.Clear();
            }
            while (Constants.loopController);
        }
    }
}
