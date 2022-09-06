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

            if (File.Exists(Variables.highscorePath))
            {
                Variables.highScoreList = JsonSerializer.Deserialize<List<Highscore>>(File.ReadAllText(Variables.highscorePath));
            }
            if (!File.Exists(Variables.tutorialPath))
            {
                Addition.Tutorial();
            }
            Addition.HallOfFame();
        }

        public static void BuildGame(List<string> enemyLines, bool wall)
        {
            if (wall == true)
            {
                Addition.Wall(Variables.enemyLinesCount);

                Enemy.EnemyLines(enemyLines, Variables.enemyLinesCount, true);
                Addition.PlayerLines(Variables.playerLines.Input);

                Addition.Wall(Variables.enemyLinesCount);
            }
            else
            {
                Enemy.EnemyLines(enemyLines, Variables.enemyLinesCount, false);
                Addition.PlayerLines(Variables.playerLines.Input);
            }
        }
    }
}
