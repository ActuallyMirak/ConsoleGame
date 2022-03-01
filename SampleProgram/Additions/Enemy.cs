using System;
using SampleProgram.Common;
using System.Collections.Generic;
using System.Text;

namespace SampleProgram.Additions
{
    class Enemy
    {
        public static void EnemyLines(int linesCount)
        {
            if (Variables.failSave == 0)
            {
                for (int i = 0; i < linesCount; i++)
                {
                    string line = "#";

                    for (int a = 0; a < linesCount; a++)
                    {
                        line += "   ";
                    }
                    line += "#";

                    Variables.enemyLines.Add(line);
                }
                Variables.failSave++;

                foreach (string a in Variables.enemyLines)
                {
                    Console.WriteLine(a);
                }
            }
            else
            {
                Variables.enemyLines.RemoveAt(Variables.enemyLines.Count - 1);

                try
                {
                    Variables.LastEnemyLine = Variables.enemyLines[Variables.enemyLines.Count - 1];
                }
                catch { }

                string line = "#";
                for (int a = 0; a < linesCount; a++)
                {
                    line += Spawn();
                }
                Variables.enemyLines.Insert(0,line + "#");

                for (int i = 0; i < Variables.enemyLines.Count; i++)
                {
                    int correctLine = i + 1;
                    Console.SetCursorPosition(0, correctLine);
                    Console.WriteLine(Variables.enemyLines[i]);
                }
            }
        }
        public static string Spawn()
        {
            if (Variables.repetitions != 0)
            {
                Variables.repetitions--;
                return "   ";
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

            if (Variables.repetitions == 0)
            {
                number = random.Next(0, 20);

                if (number > 16)
                {
                    return Variables.entities[1];
                }
            }
            return "   ";
        }
    }
}
