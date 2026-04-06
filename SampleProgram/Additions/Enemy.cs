using SampleProgram.Common;

using System;

namespace SampleProgram.Additions;

class Enemy
{
    public static void EnemyLines(GameState gameState, bool initial)
    {
        var linesCount = gameState.EnemyLinesCount;

        if (initial)
        {
            for (int i = 0; i < linesCount; i++)
            {
                gameState.EnemyLines.Add($"#{new string(' ', linesCount * 3)}#");
            }

            foreach (string line in gameState.EnemyLines)
            {
                Console.WriteLine(line);
            }
        }
        else
        {
            gameState.EnemyLines.RemoveAt(gameState.EnemyLines.Count - 1);

            try
            {
                gameState.LastEnemyLine = gameState.EnemyLines[gameState.EnemyLines.Count - 1];
            }
            catch { }

            var line = "#";
            for (int a = 0; a < linesCount; a++)
            {
                line += Spawn(gameState);
            }

            gameState.EnemyLines.Insert(0, line + "#");

            for (int i = 0; i < gameState.EnemyLines.Count; i++)
            {
                int correctLine = i + 1;
                Console.SetCursorPosition(0, correctLine);
                Console.WriteLine(gameState.EnemyLines[i]);
            }
        }
    }

    public static string Spawn(GameState gameState)
    {
        if (gameState.Repetitions > 0)
        {
            gameState.Repetitions--;
            return GameState.EmptyString;
        }

        var number = Random.Shared.Next(0, 200);
        if (number == 1)
        {
            return GameState.Entities[2];
        }

        number = Random.Shared.Next(0, 10000);
        if (number == 1)
        {
            return GameState.Entities[3];
        }

        number = Random.Shared.Next(0, 20);

        if (number > 16)
        {
            return GameState.Entities[1];
        }
        else
        {
            return GameState.EmptyString;
        }
    }
}
