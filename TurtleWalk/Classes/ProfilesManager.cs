using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TurtleWalk.ClassProfile;

namespace TurtleWalk.ClassProfilesManager
{
    class ProfilesManager
    {
        private static List<Profile> _profiles = new List<Profile>();

        private StreamReader _reader;
        private StreamWriter _writer;

        public ProfilesManager()
        {

        }

        // multiple profiles with the same name can be addded - fix it
        public static void ProfileAdd(string name, UniformGrid profilesUniformGrid)
        {
            if (profilesUniformGrid.Children[0].GetType() == new TextBlock().GetType())
            {
                profilesUniformGrid.Children.Clear();
            }

            Button btnProfile = new Button()
            {
                Content = name
            };

            _profiles.Add(new Profile() { Name = name });
            profilesUniformGrid.Children.Add(btnProfile);
        }

        public static bool ProfileExists()
        {
            return false;
        }

        public static void ProfileDelete()
        {

        }
    }
}
