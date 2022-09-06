using System;
using System.Collections.Generic;

namespace SampleProgram.Common
{
    public class Variables
    {
        public Variables()
        {
            Console.CursorVisible = false;

            if ( Difficulty == 1) { time = 18; }
            else if ( Difficulty == 2) { time = 10; }
            else { time = 6; }

            Highscore = 0;
            Lives = 3;

            enemyLines = new List<string>();
            failSave = 0;
        }

        public const string linePath = @"C:\Users\Public\Documents\Lines.json";
        public const string tutorialPath = @"C:\Users\Public\Documents\Tutorial.txt";
        public const string emptyString = "   ";
        public static string highscorePath = "";

        public static string PlayerName = "";
        public static string LastEnemyLine;

        public static string[] entities = new string[4] { "|X|", "\\O/", "%-%", "$$$" };

        public static List<Highscore> highScoreList = new List<Highscore>();

        public static LineClass playerLines = new LineClass() { Input = new List<string>() };

        public static bool loopController = true;

        public static int enemyLinesCount = 0;
        public static int playerLinesCount = 0;
        public static int Difficulty = 0;
        public static int repetitions = 0;
        public static int currentPosition;
        public static int newPosition;

        public List<string> enemyLines;
        public int time;
        public int Lives;
        public int Highscore;
        public int failSave;
    }
}
