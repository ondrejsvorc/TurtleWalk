using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;

namespace TurtleWalk
{
    public class GameManager
    {
        private static StreamReader _reader;
        private static StreamWriter _writer;

        public GameManager()
        {
        }

        public static void GetAvailableLevels(Grid gridLevels)
        {
            // CHECKS FOR AVAILABLE LEVELS (EXCEPT FOR THE FIRST ONE, WHICH IS ALWAYS AVAILABLE)

            using (_reader = new StreamReader("./Resources/Levels/Available/available_levels.txt"))
            {
                if (!_reader.EndOfStream)        // if file isn't empty, don't even bother checking available levels
                {
                    string[] availableLevels = _reader.ReadLine().Split(' ');

                    for (int i = 0; i < availableLevels.Length; i++)
                    {
                        if (availableLevels[i] == "1")
                        {
                            gridLevels.Children[i + 1].IsEnabled = true;
                        }
                    }
                }
            }
        }

        // WORKS FINE BUT IT NEEDS TO BE SET SO WHEN LVL1 IS FINISHED, IT ONLY UNLOCKS LV2
        // MAYBE: 1 1 - LVL1 UNLOCKED LVL2
        public static void SetAvailableLevels()
        {
            using (_writer = new StreamWriter("./Resources/Levels/Available/available_levels.txt", true))
            {
                _writer.Write("1" + " ");
                _writer.Flush();
            }
        }

    }
}
