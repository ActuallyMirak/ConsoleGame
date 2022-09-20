using System;
using System.Collections.Generic;

namespace SampleProgram.Common
{
    public class Constants
    {
        private int _time;

        public Constants()
        {
            Console.CursorVisible = false;

            Highscore = 0;
            Lives = 3;

            failSave = 0;
            enemyLines = new List<string>();
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

        public int time
        {
            get
            {
                if (Difficulty == 1) { return 18; }
                else if (Difficulty == 2) { return 10; }
                else { return 6; }
            }
        }
        public List<string> enemyLines;
        public int Lives;
        public int Highscore;
        public int failSave;
    }
}
