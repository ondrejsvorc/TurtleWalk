using System;
using System.IO;
using System.Windows.Input;

namespace TurtleWalk.ClassConstants
{
    class Constants
    {
        private static readonly string APP_DATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string APP_LOCATION = Path.Combine(APP_DATA, "TurtleWalk");
        public static readonly string PROFILES_LOCATION = Path.Combine(APP_LOCATION, "profiles.txt");

        public const string PATH_DIRECTION_FORWARD = "pack://application:,,,/Resources/Images/Turtle/turtle_direction_forward.gif";
        public const string PATH_DIRECTION_BACKWARDS = "pack://application:,,,/Resources/Images/Turtle/turtle_direction_backwards.gif";

        public static readonly string PATH_DIRECTION_FORWARD_STOPPED = "pack://application:,,,/Resources/Images/Turtle/turtleStopped_direction_forward.gif";
        public static readonly string PATH_DIRECTION_BACKWARDS_STOPPED = "pack://application:,,,/Resources/Images/Turtle/turtleStopped_direction_backwards.gif";

        public static readonly string LAVA_DROP = "./Resources/Images/Deadly-Entities/lava_drop.png";
        public static readonly string LAVA = "./Resources/Images/Deadly-Entities/hot_lava.gif";

        public static readonly string PISTON = "./Resources/Images/Other/piston.gif";
        public static readonly string LEAF = "./Resources/Images/Points/leaf.gif";
        public static readonly string SIGN = "./Resources/Images/Other/finish.png";

        public static readonly string BUTTON = "./Resources/Images/Other/button.png";

        public static readonly Cursor CURSOR_HAND = new Cursor(new MemoryStream(Properties.Resources.cursorHand));
        public static readonly Cursor CURSOR_GRABBED = new Cursor(new MemoryStream(Properties.Resources.cursorGrabbed));

        public static readonly string AVAILABLE_LEVELS = "./Resources/Levels/Available/available_levels.txt";
        public static readonly string SCOREBOARD_DATA = "./Resources/scoreboard.txt";
    }
}
