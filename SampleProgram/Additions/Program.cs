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

            Variables.playerLines = new LineClass();
            Variables.playerLines.Input = new List<string>();

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

        public static void SetGameUp()
        {
            Console.CursorVisible = false;

            if (Variables.Difficulty == 1) { Variables.time = 18; }
            else if (Variables.Difficulty == 2) { Variables.time = 10; }
            else { Variables.time = 6; }

            Variables.Highscore = 0;
            Variables.Lives = 3;

            Variables.enemyLines.Clear();
            Variables.failSave = 0;
        }

        public static void BuildGame(bool wall)
        {
            if (wall == true)
            {
                Addition.Wall(Variables.enemyLinesCount);

                Enemy.EnemyLines(Variables.enemyLinesCount);
                Addition.PlayerLines(Variables.playerLines.Input);

                Addition.Wall(Variables.enemyLinesCount);
            }
            else
            {
                Enemy.EnemyLines(Variables.enemyLinesCount);
                Addition.PlayerLines(Variables.playerLines.Input);
            }
        }
    }
}
