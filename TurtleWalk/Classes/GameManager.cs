using System;
using System.IO;
using System.Windows.Controls.Primitives;

namespace TurtleWalk.ClassGameManager
{
    sealed class GameManager
    {
        private static StreamReader _reader;
        private static StreamWriter _writer;

        public GameManager()
        {
        }

        public static void GetAvailableLevels(UniformGrid uniformGridLevels)
        {
            // CHECKS FOR AVAILABLE LEVELS (EXCEPT FOR THE FIRST ONE, WHICH IS ALWAYS AVAILABLE)

            using (_reader = new StreamReader("./Resources/Levels/Available/available_levels.txt"))
            {
                if (!_reader.EndOfStream)        // If the file is empty, don't even bother checking available levels
                {
                    string[] availableLevels = _reader.ReadLine().Split(' ');

                    for (int i = 0; i < availableLevels.Length; i++)
                    {
                        if (availableLevels[i] == "1")
                        {
                            if (!uniformGridLevels.Children[i + 1].IsEnabled)
                            {
                                uniformGridLevels.Children[i + 1].IsEnabled = true;
                            }
                        }
                    }
                }
            }
        }

        public static void SetAvailableLevels(UniformGrid uniformGridLevels, string lvl)
        {
            // lvl = "01" -> substring(1, 1) -> lvl = "1"
            // index: 1 - 1 = 0
            
            int lvlIndex = Convert.ToInt16(lvl.Substring(1, 1)) - 1;

            if (!uniformGridLevels.Children[lvlIndex + 1].IsEnabled)
            {
                using (_writer = new StreamWriter("./Resources/Levels/Available/available_levels.txt", true))
                {
                    _writer.Write("1" + " ");
                    _writer.Flush();
                }
            }
        }

    }
}
