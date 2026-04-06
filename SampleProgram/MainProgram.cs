using SampleProgram.Additions;
using SampleProgram.Common;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace SampleProgram;

class MainProgram
{
    static void Main()
    {
        Program.Start();

        var playerName = Addition.NameSelection();
        var highscorePath = Addition.SetupHighscore();

        var gameState = new GameState(playerName, highscorePath);

        Addition.HallOfFame(gameState);

        Addition.DifficultySelection(gameState);

        do
        {
            SettingUpPlayerLines(gameState);

            Program.BuildGame(gameState, shouldPrintWall: true);

            var x = gameState.PlayerLines.Input.Count * 3 + 20;
            var y = gameState.PlayerLines.Input.Count / 2;

            while (gameState.IsGameLoopActive)
            {
                var cKey = ConsoleKey.Enter;

                Program.BuildGame(gameState, shouldPrintWall: false);

                Addition.SetPositionAndWrite(x, y, $"Your Points: {Addition.HighscoreCalculator(gameState, getHighscore: true)}");

                Addition.SetPositionAndWrite(x, y + 1, $"Your Lives: {gameState.Lives}");

                Console.SetCursorPosition(gameState.PlayerLines.Input.Count * 3 + 10, gameState.PlayerLines.Input.Count / 2);
                for (int i = gameState.Time; i > 0; i--)
                {
                    Thread.Sleep(15);
                    if (Console.KeyAvailable)
                    {
                        cKey = Console.ReadKey().Key;
                        Console.Write("\b ");
                        break;
                    }
                }

                gameState.Highscore = Addition.HighscoreCalculator(gameState, getHighscore: false);

                try
                {
                    Addition.MovementSelection(gameState, cKey);

                    gameState.PlayerLines.Input[gameState.CurrentPosition] = "   ";
                    gameState.PlayerLines.Input[gameState.NewPosition] = GameState.Entities[0];

                    gameState.CurrentPosition = gameState.NewPosition;
                }
                catch (WrongInputException) { }

                gameState.Lives = Addition.HitCalculator(gameState);
            }
            Console.CursorVisible = true;

            Addition.RepeatProgram(ref gameState);

            Console.Clear();
        }
        while (gameState.IsGameLoopActive);
    }

    private static void SettingUpPlayerLines(GameState gameState)
    {
        if (!File.Exists(GameState.LinePath))
        {

            for (int i = 0; i < 9; i++)
            {
                if (i == 4)
                {
                    gameState.PlayerLines.Input.Add(GameState.Entities[0]);
                }
                else
                {
                    gameState.PlayerLines.Input.Add("   ");
                }
            }
        }
        else
        {
            gameState.PlayerLines = JsonSerializer.Deserialize<LineClass>(File.ReadAllText(GameState.LinePath));
        }

        if (gameState.PlayerLines.Input.Count <= 2)
        {
            Console.WriteLine("To small Game");
            Console.ReadKey();
        }

        for (int i = 0; i < gameState.PlayerLines.Input.Count; i++)
        {
            if (gameState.PlayerLines.Input[i] == GameState.Entities[0])
            {
                gameState.CurrentPosition = i;
            }
        }

        gameState.EnemyLinesCount = gameState.PlayerLines.Input.Count;
    }
}
