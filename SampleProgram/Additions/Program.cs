using Newtonsoft.Json;
using SampleProgram.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace SampleProgram.Additions
{
    public class Program
    {
        public static void Start()
        {
            Console.Title = "Arcade";
            Console.CursorVisible = false;

            Variables.playerLines = new LineClass();
            Variables.playerLines.Input = new List<string>();

            Addition.CheckForUser();

            if (File.Exists(Variables.highscorePath))
            {
                Variables.highScoreList = JsonConvert.DeserializeObject<List<Highscore>>(File.ReadAllText(Variables.highscorePath));
            }
            if (!File.Exists(Variables.tutorialPath))
            {
                Addition.Tutorial();
            }
            Addition.HallOfFame();
        }

        public static void Selections()
        {
            Addition.NameSelection();
            Addition.DifficultySelection();
        }

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
                Variables.playerLines = JsonConvert.DeserializeObject<LineClass>(File.ReadAllText(Variables.linePath));
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

        public static void SetGameUp()
        {
            Console.CursorVisible = false;

            if (Variables.Difficulty == 1) { Variables.time = 18; }
            else if (Variables.Difficulty == 2) { Variables.time = 10; }
            else { Variables.time = 6; }

            Variables.Highscore = 0;
            Variables.Lives = 3;

            Variables.enemyLines.Clear();
            Variables.failSave = 0;
        }

        public static void BuildGame(bool wall)
        {
            if (wall == true)
            {
                Addition.Wall(Variables.enemyLinesCount);

                Enemy.EnemyLines(Variables.enemyLinesCount);
                Addition.PlayerLines(Variables.playerLines.Input);

                Addition.Wall(Variables.enemyLinesCount);
            }
            else
            {
                Enemy.EnemyLines(Variables.enemyLinesCount);
                Addition.PlayerLines(Variables.playerLines.Input);
            }
        }


        public static void Initialize()
        {
            Start();

            Selections();

            SettingUpPlayerLines();
        }
        public static void Run()
        {
            Start:
            SetGameUp();

            BuildGame(true);

            while (!Variables.loopController)
            {
                TimeSpan timeSpan = new TimeSpan(0, 0, Variables.time);
                ConsoleKey cKey = ConsoleKey.Enter;

                BuildGame(false);

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
