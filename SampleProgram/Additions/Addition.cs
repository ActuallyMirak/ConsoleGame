using SampleProgram.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace SampleProgram.Additions;

public class Addition
{
    public static string SetupHighscore()
    {
        const string path = @"C:\Users";

        var directory = new DirectoryInfo(path).GetDirectories().Where(x => !x.Name.Contains("User") && !x.Name.Contains("Default") && !x.Name.Contains("Public")).ToList().First();

        var highscorePath = $"{directory.FullName}\\source\\repos\\Sample\\Highscores.json";

        if (!File.Exists(highscorePath))
        {
            highscorePath = "C:\\Users\\Public\\Documents\\Highscores.json";

            if (!File.Exists(highscorePath))
            {
                File.WriteAllText(highscorePath, "[]");
            }
        }

        return highscorePath;
    }

    public static void HallOfFame(GameState gameState)
    {
        var highscores = JsonSerializer.Deserialize<List<Highscore>>(File.ReadAllText(gameState.HighscorePath));

        gameState.HighScoreList = highscores;

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
        SetPositionAndWrite(30, 11, "          Das bist du:\t\t\t\t\t\t" + GameState.Entities[0]);
        SetPositionAndWrite(30, 12, "          Das sind Gegner:\t\t\t\t\t" + GameState.Entities[1]);
        SetPositionAndWrite(30, 13, "          Dieses Item verhindert die nächsten 8 Spawns:\t\t" + GameState.Entities[2]);
        SetPositionAndWrite(30, 14, "(selten)  Dieses Item gibt dir 5000 Punkte:\t\t\t" + GameState.Entities[3]);

        Console.ReadKey();
        Console.Clear();

        using var _ = File.Create(GameState.TutorialPath);
    }

    public static string NameSelection()
    {
        Console.Clear();
        Console.CursorVisible = true;

        SetPositionAndWrite(50, 10, "Please enter your name: ");
        var playerName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(playerName))
        {
            Console.SetCursorPosition(74 + playerName.Length, 10);
            for (int i = 0; i < playerName.Length; i++)
            {
                Console.Write("\b");
            }
            for (int i = 0; i < playerName.Length; i++)
            {
                Console.Write(" ");
            }

            SetPositionAndWrite(50, 8, "Your name has to contain a symbol\n");

            return NameSelection();
        }
        else if (playerName.Length >= 35)
        {
            return NameSelection();
        }
        else if (playerName.Length > 5)
        {
            Console.SetCursorPosition(74 + playerName.Length, 10);
            for (int i = 0; i < playerName.Length; i++)
            {
                Console.Write("\b");
            }
            for (int i = 0; i < playerName.Length; i++)
            {
                Console.Write(" ");
            }

            SetPositionAndWrite(50, 7, "Your name can only be 5 letters long");
            
            return NameSelection();
        }

        Console.Clear();

        return playerName;
    }

    public static void DifficultySelection(GameState gameState)
    {
        SetPositionAndWrite(50, 10, "Please select a difficulty/multiplier");
        SetPositionAndWrite(50, 11, "(1)easy = points * 0");
        SetPositionAndWrite(50, 12, "(2)normal = points * 1");
        SetPositionAndWrite(50, 13, "(3)hard = points * 2");
        SetPositionAndWrite(50, 14, "Your choice: ");

        while (gameState.Difficulty == 0)
        {
            Console.SetCursorPosition(63, 14);
            var cKey = Console.ReadKey().Key;

            switch (cKey)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    gameState.Difficulty = 1;
                    break;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    gameState.Difficulty = 2;
                    break;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    gameState.Difficulty = 3;
                    break;

                default:
                    Console.Write("\b ");

                    SetPositionAndWrite(35, 14, "Invalid Input");
                    break;
            }
        }
        Console.Clear();
    }

    public static void MovementSelection(GameState gameState, ConsoleKey cKey)
    {
        var lines = gameState.PlayerLines.Input;

        switch (cKey)
        {
            case ConsoleKey.A:
            case ConsoleKey.LeftArrow:
                int newPosition = gameState.CurrentPosition - 1;
                if (newPosition >= 0 && newPosition < lines.Count)
                {
                    gameState.NewPosition = newPosition;
                }
                break;

            case ConsoleKey.D:
            case ConsoleKey.RightArrow:
                newPosition = gameState.CurrentPosition + 1;
                if (newPosition >= 0 && newPosition < lines.Count)
                {
                    gameState.NewPosition = newPosition;
                }
                break;

            default:
                throw new WrongInputException();
        }
    }

    public static void PlayerLines(List<string> lines)
    {
        Console.Write("#");

        foreach (string a in lines)
        {
            Console.Write(a);
        }

        Console.Write("#\n");
    }

    public static void Wall(int count)
    {
        Console.Write(new string('#', count * 3));

        Console.Write("##\n");
    }

    public static int HitCalculator(GameState gameState)
    {
        var i = 0;
        var enemyPosition = new List<string>();

        if (gameState.LastEnemyLine.Contains(GameState.Entities[0][0]) ||
            gameState.LastEnemyLine.Contains(GameState.Entities[1][0]) ||
            gameState.LastEnemyLine.Contains(GameState.Entities[2][0]) ||
            gameState.LastEnemyLine.Contains(GameState.Entities[3][0]))
        {
            try
            {
                foreach (char a in gameState.LastEnemyLine)
                {
                    if (i >= gameState.LastEnemyLine.Length - 1) 
                    { 
                        break;
                    }
                    else if (a != '#')
                    {
                        enemyPosition.Add(gameState.LastEnemyLine.Substring(i, 3));
                        i += 3;
                    }
                    else 
                    {
                        i++;
                    }
                }

                var lines = gameState.PlayerLines.Input;

                for (i = 0; i < lines.Count; i++)
                {
                    if (lines[i] == GameState.Entities[0] && enemyPosition[i] == GameState.Entities[1])
                    {
                        gameState.Lives--;
                        LivesBlink(gameState.Lives, lines.Count);

                        break;
                    }
                    else if (lines[i] == GameState.Entities[0] && enemyPosition[i] == GameState.Entities[2])
                    {
                        gameState.Repetitions += 8;

                        break;
                    }
                    else if (lines[i] == GameState.Entities[0] && enemyPosition[i] == GameState.Entities[3])
                    {
                        gameState.Highscore += 5000;

                        break;
                    }
                }
            }
            catch (Exception) { }

            gameState.IsGameLoopActive = gameState.Lives != 0;
        }

        return gameState.Lives;
    }

    public static void LivesBlink(int lives, int LinesCount)
    {
        const string livesTextTemplate = "Your Lives: {0}";

        for (int i = 0; i < 10; i++)
        {
            SetPositionAndWrite(LinesCount * 3 + 33, LinesCount / 2 + 1, new string('\b', livesTextTemplate.Length) + new string(' ', livesTextTemplate.Length));

            Thread.Sleep(50);

            SetPositionAndWrite(LinesCount * 3 + 20, LinesCount / 2 + 1, string.Format(livesTextTemplate, lives));

            Thread.Sleep(50);
        }

        // Ensures no movement happens while lives are blinking
        while (Console.KeyAvailable)
        {
            Console.ReadKey();
            Console.Write("\b ");
        }
    }

    public static int HighscoreCalculator(GameState gameState,bool getHighscore)
    {
        if (!getHighscore)
        {
            gameState.Highscore += 23 * (gameState.Difficulty - 1);
        }

        return gameState.Highscore;
    }

    public static void RepeatProgram(ref GameState gameState)
    {
        bool newHighscore = false;
        if (gameState.Difficulty != 1)
        {
            newHighscore = SaveHighscore(gameState);
        }

        Console.Clear();

        SetPositionAndWrite(50, 8, "--------------------------");
        SetPositionAndWrite(50, 10, "      Game over");
        SetPositionAndWrite(50, 11, string.Format("   Your Highscore | {0}", HighscoreCalculator(gameState, getHighscore: true)));

        if (newHighscore == true)
        {
            SetPositionAndWrite(50, 12, "    ! NEW HIGHSCORE !");
        }

        SetPositionAndWrite(50, 13, "--------------------------");

        SetPositionAndWrite(50, 14, "Try again?");

        SetPositionAndWrite(50, 15, "Yes (Y) or No (N): ");

        Console.SetCursorPosition(70, 15);
        string input = Console.ReadLine();

        while (true)
        {
            if (input.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                gameState = new GameState(gameState);
                break;
            }
            else if (input.Equals("N", StringComparison.InvariantCultureIgnoreCase))
            {
                gameState.IsGameLoopActive = false;
                break;
            }
            else
            {
                Console.Clear();

                Console.WriteLine("Please enter one of the given options...\n========================================\n");
                Console.WriteLine("Yes (Y) or No (N)");

                input = Console.ReadLine();
            }
        }
    }

    public static bool SaveHighscore(GameState gameState)
    {
        var courseName = gameState.Difficulty == 2 ? "normal" : "hard";

        gameState.HighScoreList.Add(new Highscore() 
        {
            Score = HighscoreCalculator(gameState, getHighscore: true), 
            Name = gameState.PlayerName, MapSize = $"{gameState.PlayerLines.Input.Count}x{gameState.PlayerLines.Input.Count}",
            Course = courseName 
        });

        gameState.HighScoreList = gameState.HighScoreList.OrderByDescending(x => x.Score).Distinct().ToList();

        // "normal" shouldn't be worth as much as "hard"
        try
        {
            for (int i = 0; i < 9; i++)
            {
                if (gameState.HighScoreList[i].Course == "normal")
                {
                    if (gameState.HighScoreList[i].Score > (gameState.HighScoreList[gameState.HighScoreList.Count - 1].Score * 2))
                    {
                        gameState.HighScoreList.RemoveAt(i);
                    }
                }
            }
        }
        catch (ArgumentOutOfRangeException) { }

        if (gameState.HighScoreList.Count >= 10)
        {
            gameState.HighScoreList.RemoveAt(gameState.HighScoreList.Count - 1);
        }

        var serializedHighscore = JsonSerializer.Serialize(gameState.HighScoreList);
        File.WriteAllText(gameState.HighscorePath, serializedHighscore);

        return gameState.Highscore == gameState.HighScoreList[0].Score;
    }

    public static void SetPositionAndWrite(int x, int y, string output)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(output);
    }
}