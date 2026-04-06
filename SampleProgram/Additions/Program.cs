using SampleProgram.Common;

using System;
using System.IO;

namespace SampleProgram.Additions;

public class Program
{
    public static void Start()
    {
        Console.Title = "Arcade";
        Console.CursorVisible = false;

        if (!File.Exists(GameState.TutorialPath))
        {
            Addition.Tutorial();
        }
    }

    public static void BuildGame(GameState gameState, bool shouldPrintWall)
    {
        if (shouldPrintWall == true)
        {
            Addition.Wall(gameState.EnemyLinesCount);

            Enemy.EnemyLines(gameState, shouldPrintWall);
            Addition.PlayerLines(gameState.PlayerLines.Input);

            Addition.Wall(gameState.EnemyLinesCount);
        }
        else
        {
            Enemy.EnemyLines(gameState, shouldPrintWall);
            Addition.PlayerLines(gameState.PlayerLines.Input);
        }
    }
}
