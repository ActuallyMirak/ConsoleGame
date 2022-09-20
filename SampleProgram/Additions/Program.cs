using SampleProgram.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SampleProgram.Additions
{
    public class Program
    {
        public static void Start()
        {
            Console.Title = "Arcade";
            Console.CursorVisible = false;

            Addition.CheckForUser();

            Constants.highScoreList = JsonSerializer.Deserialize<List<Highscore>>(File.ReadAllText(Constants.highscorePath));
            if (!File.Exists(Constants.tutorialPath))
            {
                Addition.Tutorial();
            }

            Addition.HallOfFame();
        }

        public static void BuildGame(List<string> enemyLines, bool wall)
        {
            if (wall == true)
            {
                Addition.Wall(Constants.enemyLinesCount);

                Enemy.EnemyLines(enemyLines, Constants.enemyLinesCount, true);
                Addition.PlayerLines(Constants.playerLines.Input);

                Addition.Wall(Constants.enemyLinesCount);
            }
            else
            {
                Enemy.EnemyLines(enemyLines, Constants.enemyLinesCount, false);
                Addition.PlayerLines(Constants.playerLines.Input);
            }
        }
    }
}
