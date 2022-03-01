using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SampleProgram.Common
{
    public static class Variables
    {
        public const string linePath = @"C:\Users\Public\Documents\Lines.json";
        public const string tutorialPath = @"C:\Users\Public\Documents\Tutorial.txt";
        public const string highscorePath = @"C:\Users\Public\Documents\Highscores.json";

        public static string PlayerName = "";
        public static string LastEnemyLine;

        public static string[] entities = new string[4] { "|X|", "\\O/", "%-%", "$$$" };

        public static List<string> enemyLines = new List<string>();
        public static List<Highscore> highScoreList = new List<Highscore>();

        public static LineClass playerLines;

        public static bool loopController = false;

        public static int enemyLinesCount = 0;
        public static int playerLinesCount = 0;
        public static int time;
        public static int Lives = 3;
        public static int Difficulty = 0;
        public static int Highscore = 0;
        public static int failSave = 0;
        public static int repetitions = 0;
        public static int currentPosition;
        public static int newPosition;
    }
}
