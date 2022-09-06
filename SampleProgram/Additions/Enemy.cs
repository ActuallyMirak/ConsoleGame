using SampleProgram.Common;

using System;
using System.Collections.Generic;

namespace SampleProgram.Additions
{
    class Enemy
    {
        public static void EnemyLines(List<string> enemyLines, int linesCount, bool initial)
        {
            if (initial)
            {
                for (int i = 0; i < linesCount; i++)
                {
                    string line = "#";

                    for (int a = 0; a < linesCount; a++)
                    {
                        line += "   ";
                    }
                    line += "#";

                    enemyLines.Add(line);
                }

                foreach (string a in enemyLines)
                {
                    Console.WriteLine(a);
                }
            }
            else
            {
                enemyLines.RemoveAt(enemyLines.Count - 1);

                try
                {
                    Variables.LastEnemyLine = enemyLines[enemyLines.Count - 1];
                }
                catch { }

                string line = "#";
                for (int a = 0; a < linesCount; a++)
                {
                    line += Spawn();
                }
                enemyLines.Insert(0, line + "#");

                for (int i = 0; i < enemyLines.Count; i++)
                {
                    int correctLine = i + 1;
                    Console.SetCursorPosition(0, correctLine);
                    Console.WriteLine(enemyLines[i]);
                }
            }
        }
        public static string Spawn()
        {
            if (Variables.repetitions > 0)
            {
                Variables.repetitions--;
                return Variables.emptyString;
            }

            Random random = new Random();
            int number;

            number = random.Next(0, 200);
            if (number == 1)
            {
                return Variables.entities[2];
            }

            number = random.Next(0, 10000);
            if (number == 1)
            {
                return Variables.entities[3];
            }

            number = random.Next(0, 20);

            if (number > 16)
            {
                return Variables.entities[1];
            }
            else
            {
                return Variables.emptyString;
            }
        }
    }
}
