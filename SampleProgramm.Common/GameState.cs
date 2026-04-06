using System;
using System.Collections.Generic;

namespace SampleProgram.Common;

public class GameState
{
    public const string LinePath = @"C:\Users\Public\Documents\Lines.json";
    public const string TutorialPath = @"C:\Users\Public\Documents\Tutorial.txt";
    public const string EmptyString = "   ";

    public static string[] Entities = ["|X|", "\\O/", "%-%", "$$$"];

    public GameState(string playerName, string highscorePath)
    {
        Console.CursorVisible = false;

        PlayerName = playerName;
        HighscorePath = highscorePath;
    }

    public GameState(GameState gameState)
    {
        Console.CursorVisible = false;

        HighScoreList = gameState.HighScoreList;
        PlayerName = gameState.PlayerName;
        HighscorePath = gameState.HighscorePath;
        Difficulty = gameState.Difficulty;
    }

    // Set by constructor
    public string PlayerName;
    public string HighscorePath;

    // Set on runtime
    public string LastEnemyLine;
    public int CurrentPosition;
    public int NewPosition;

    // Initialized and updated on runtime
    public List<Highscore> HighScoreList = [];
    public List<string> EnemyLines = [];
    public LineClass PlayerLines = new() { Input = [] };
    public int EnemyLinesCount = 0;
    public int PlayerLinesCount = 0;
    public int Difficulty = 0;
    public int Repetitions = 0;
    public int Lives = 3;
    public int Highscore = 0;
    public int failSave = 0;
    public bool IsGameLoopActive = true;

    public int Time
    {
        get
        {
            if (Difficulty == 1) 
            {
                return 18; 
            }
            else if (Difficulty == 2) 
            { 
                return 10;
            }
            else 
            {
                return 6;
            }
        }
    }
}
