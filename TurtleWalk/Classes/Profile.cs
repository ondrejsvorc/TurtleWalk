using System.Collections.Generic;

namespace TurtleWalk.ClassProfile
{
    class Profile
    {
        public string Name { get; set; }
        public List<int> ScoreList { get; set; } = new List<int>();

        public int LevelsAvailable = 1;
    }
}
