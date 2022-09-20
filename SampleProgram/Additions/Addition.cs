using SampleProgram.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text.Json;

namespace SampleProgram.Additions
{
    public class Addition
    {
        public static void CheckForUser()
        {
            string path = @"C:\Users";

            var directory = new DirectoryInfo(path).GetDirectories().Where(x => !x.Name.Contains("User") && !x.Name.Contains("Default") && !x.Name.Contains("Public")).ToList().First();

            Constants.highscorePath = $"{directory.FullName}\\source\\repos\\Sample\\Highscores.json";

            if (!File.Exists(Constants.highscorePath))
            {
                Constants.highscorePath = "C:\\Users\\Public\\Documents\\highscores.json";

                if (!File.Exists(Constants.highscorePath))
                {
                    File.WriteAllText(Constants.highscorePath, "[]");
                }
            }
        }
        public static void HallOfFame()
        {
            List<Highscore> highscores = Constants.highScoreList;

            SetPositionAndWrite(50, 10, "/ Hall of Fame \\");

            SetPositionAndWrite(40, 13, "  Score    \tName  \tMapsize    Difficulty");
            SetPositionAndWrite(40, 14, "=============================================");

            for (int i = 0; i < highscores.Count; i++)
            {
                SetPositionAndWrite(35, i + 15, string.Format("{0,13}\t{1,5}\t  {2}        {3}", highscores[i].Score, highscores[i].Name, highscores[i].MapSize, highscores[i].Course));
            }
            Console.ReadKey();
            Console.Clear();
        }
        public static void Tutorial()
        {
            SetPositionAndWrite(40, 8, "Tutorial");
            SetPositionAndWrite(39, 9, "----------");
            SetPositionAndWrite(30, 11, "          Das bist du:\t\t\t\t\t\t" + Constants.entities[0]);
            SetPositionAndWrite(30, 12, "          Das sind Gegner:\t\t\t\t\t" + Constants.entities[1]);
            SetPositionAndWrite(30, 13, "          Dieses Item verhindert die nächsten 8 Spawns:\t\t" + Constants.entities[2]);
            SetPositionAndWrite(30, 14, "(selten)  Dieses Item gibt dir 5000 Punkte:\t\t\t" + Constants.entities[3]);
            Console.ReadKey();
            Console.Clear();

            FileStream cache = File.Create(Constants.tutorialPath);

            cache.Close();
            cache.Dispose();
        }
        public static void NameSelection()
        {
            Console.Clear();
            Console.CursorVisible = true;

            SetPositionAndWrite(50, 10, "Please enter your name: ");
            Constants.PlayerName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(Constants.PlayerName))
            {
                Console.SetCursorPosition(74 + Constants.PlayerName.Length, 10);
                for (int i = 0; i < Constants.PlayerName.Length; i++)
                {
                    Console.Write("\b");
                }
                for (int i = 0; i < Constants.PlayerName.Length; i++)
                {
                    Console.Write(" ");
                }

                SetPositionAndWrite(50, 8, "Your name has to contain a symbol\n");
                NameSelection();
            }
            else if (Constants.PlayerName.Length >= 35)
            {
                NameSelection();
            }
            else if (Constants.PlayerName.Length > 5)
            {
                Console.SetCursorPosition(74 + Constants.PlayerName.Length, 10);
                for (int i = 0; i < Constants.PlayerName.Length; i++)
                {
                    Console.Write("\b");
                }
                for (int i = 0; i < Constants.PlayerName.Length; i++)
                {
                    Console.Write(" ");
                }

                SetPositionAndWrite(50, 7, "Your name can only be 5 letters long");
                NameSelection();
            }
            Console.Clear();
        }
        public static void DifficultySelection()
        {
            SetPositionAndWrite(50, 10, "Please select a difficulty/multipliere");
            SetPositionAndWrite(50, 11, "(1)easy = points * 0");
            SetPositionAndWrite(50, 12, "(2)normal = points * 1");
            SetPositionAndWrite(50, 13, "(3)hard = points * 2");
            SetPositionAndWrite(50, 14, "Your choice: ");

            while (Constants.Difficulty == 0)
            {
                Console.SetCursorPosition(63, 14);
                ConsoleKey cKey = Console.ReadKey().Key;

                switch (cKey)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:

                        Constants.Difficulty = 1;
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:

                        Constants.Difficulty = 2;
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:

                        Constants.Difficulty = 3;
                        break;

                    default:
                        Console.Write("\b ");

                        SetPositionAndWrite(35, 14, "Invalid Input");
                        break;
                }
            }
            Console.Clear();
        }
        public static void MovementSelection(ConsoleKey cKey, List<string> Lines)
        {
            switch (cKey)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    int newPosition = Constants.currentPosition - 1;
                    if (newPosition >= 0 && newPosition < Lines.Count)
                    {
                        Constants.newPosition = newPosition;
                    }
                    break;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    newPosition = Constants.currentPosition + 1;
                    if (newPosition >= 0 && newPosition < Lines.Count)
                    {
                        Constants.newPosition = newPosition;
                    }
                    break;

                default:
                    throw new WrongInputException();
            }
        }
        public static void PlayerLines(List<string> Lines)
        {
            Console.Write("#");
            foreach (string a in Lines)
            {
                Console.Write(a);
            }
            Console.Write("#\n");
        }
        public static void Wall(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Console.Write("###");
            }
            Console.Write("##\n");
        }
        public static int HitCalculator(List<string> Lines, int lives, int highscore)
        {
            List<string> enemyPosition = new List<string>();
            int i = 0;

            if (Constants.LastEnemyLine.Contains(Constants.entities[0][0]) ||
                Constants.LastEnemyLine.Contains(Constants.entities[1][0]) ||
                Constants.LastEnemyLine.Contains(Constants.entities[2][0]) ||
                Constants.LastEnemyLine.Contains(Constants.entities[3][0]))
            {
                try
                {
                    foreach (char a in Constants.LastEnemyLine)
                    {
                        if (i >= Constants.LastEnemyLine.Length - 1) { break; }
                        if (a != '#')
                        {
                            enemyPosition.Add(Constants.LastEnemyLine.Substring(i, 3));
                            i += 3;
                        }
                        else { i++; }
                    }

                    for (i = 0; i < Lines.Count; i++)
                    {
                        if (Lines[i] == Constants.entities[0] && enemyPosition[i] == Constants.entities[1])
                        {
                            lives--;
                            Lives_Blink(lives, Lines.Count);

                            break;
                        }
                        else if (Lines[i] == Constants.entities[0] && enemyPosition[i] == Constants.entities[2])
                        {
                            Constants.repetitions += 8;

                            break;
                        }
                        else if (Lines[i] == Constants.entities[0] && enemyPosition[i] == Constants.entities[3])
                        {
                            highscore += 5000;

                            break;
                        }
                    }
                }
                catch (Exception) { }

                if (lives == 0) { Constants.loopController = false; }
            }

            return lives;
        }
        public static void Lives_Blink(int lives, int LinesCount)
        {
            for (int i = 0; i < 10; i++)
            {
                SetPositionAndWrite(LinesCount * 3 + 33, LinesCount / 2 + 1, "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b                     ");

                Thread.Sleep(50);

                SetPositionAndWrite(LinesCount * 3 + 20, LinesCount / 2 + 1, string.Format("Your Lives: {0}", lives));

                Thread.Sleep(50);
            }

            // Stellt sicher das man sich nicht bewegen kann.
            while (Console.KeyAvailable)
            {
                Console.ReadKey();
                Console.Write("\b ");
            }
        }
        public static int HighscoreCalculator(int highscore, bool getHighscore)
        {
            if (!getHighscore)
            {
                highscore += 23 * (Constants.Difficulty - 1);
            }

            return highscore;
        }
        public static void RepeatProgram(List<string> Lines, int highscore)
        {
            bool newHighscore = false;
            if (Constants.Difficulty != 1)
            {
                newHighscore = SaveHighscore(highscore, Lines.Count);
            }

            Console.Clear();

            SetPositionAndWrite(50, 8, "--------------------------");
            SetPositionAndWrite(50, 10, "      Game over");
            SetPositionAndWrite(50, 11, string.Format("   Your Highscore | {0}", HighscoreCalculator(highscore, true)));

            if (newHighscore == true)
            {
                SetPositionAndWrite(50, 12, "    ! NEW HIGHSCORE !");
            }

            SetPositionAndWrite(50, 13, "--------------------------");

            SetPositionAndWrite(50, 14, "Try again?");

            SetPositionAndWrite(50, 15, "Yes (Y) or No (N): ");

            Console.SetCursorPosition(70, 15);
            string input = Console.ReadLine();

        Validation:
            if (input.ToUpper() == "Y")
            {
                Constants.loopController = true;
            }
            else if (input.ToUpper() == "N")
            {
                Constants.loopController = false;
            }
            else
            {
                Console.Clear();

                Console.WriteLine("Please enter one of the given options...\n========================================\n");
                Console.WriteLine("Yes (Y) or No (N)");

                input = Console.ReadLine();
                goto Validation;
            }
        }
        public static bool SaveHighscore(int highscore, int LinesCount)
        {
            string CourseName;

            if (Constants.Difficulty == 2) { CourseName = "normal"; }
            else { CourseName = "hard"; }

            Constants.highScoreList.Add(new Highscore() { Score = HighscoreCalculator(highscore, true), Name = Constants.PlayerName, MapSize = $"{LinesCount}x{LinesCount}", Course = CourseName });
            Constants.highScoreList = Constants.highScoreList.OrderByDescending(x => x.Score).Distinct().ToList();

            // "normal" shouldn't be worth as much as "hard"
            try
            {
                for (int i = 0; i < 9; i++)
                {
                    if (Constants.highScoreList[i].Course == "normal")
                    {
                        if (Constants.highScoreList[i].Score > (Constants.highScoreList[Constants.highScoreList.Count - 1].Score * 2))
                        {
                            Constants.highScoreList.RemoveAt(i);
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException) { }

            if (Constants.highScoreList.Count >= 10)
            {
                Constants.highScoreList.RemoveAt(Constants.highScoreList.Count - 1);
            }

            string serializedHighscore = JsonSerializer.Serialize(Constants.highScoreList);
            File.WriteAllText(Constants.highscorePath, serializedHighscore);

            if (highscore == Constants.highScoreList[0].Score) { return true; }
            return false;
        }

        public static void SetPositionAndWrite(int x, int y, string output)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(output);
        }
    }
}