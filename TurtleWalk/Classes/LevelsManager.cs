using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TurtleWalk.ClassConstants;

namespace TurtleWalk.ClassLevelManager
{
    sealed class LevelsManager
    {
        private StreamReader _reader;
        private StreamWriter _writer;

        private UniformGrid _uniformGridLevels;

        public LevelsManager(UniformGrid uniformGridLevels)
        {
            _uniformGridLevels = uniformGridLevels;
        }

        public async Task GetAvailableLevels()
        {
            // CHECKS FOR AVAILABLE LEVELS (EXCEPT FOR THE FIRST ONE, WHICH IS ALWAYS AVAILABLE)

            using (_reader = new StreamReader(Constants.AVAILABLE_LEVELS))
            {
                if (!_reader.EndOfStream)        // If the file is empty, don't even bother checking available levels
                {
                    string[] availableLevels = _reader.ReadLine().Split(' ');

                    for (int i = 0; i < availableLevels.Length; i++)
                    {
                        if (availableLevels[i] == "1")
                        {
                            if (!_uniformGridLevels.Children[i + 1].IsEnabled)
                            {
                                _uniformGridLevels.Children[i + 1].IsEnabled = true;
                            }
                        }
                    }
                }
            }
        }

        public void SetAvailableLevels(string lvl)
        {
            // lvl = "01" -> substring(1, 1) -> lvl = "1"
            // index: 1 - 1 = 0
            
            int lvlIndex = Convert.ToInt16(lvl.Substring(1, 1)) - 1;

            if (!_uniformGridLevels.Children[lvlIndex + 1].IsEnabled)
            {
                using (_writer = new StreamWriter(Constants.AVAILABLE_LEVELS, true))
                {
                    _writer.Write("1" + " ");
                    _writer.Flush();
                }
            }
        }

    }
}
