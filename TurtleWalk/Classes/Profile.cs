using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurtleWalk.ClassConstants;

namespace TurtleWalk.ClassProfile
{
    class Profile
    {
        public string Name { get; set; }
        public List<int> ScoreList { get; set; } = new List<int>();

        public int LevelsAvailable = 1;

        public Profile()
        {

        }
    }
}
