using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TurtleWalk.ClassConstants;
using TurtleWalk.ClassProfile;

namespace TurtleWalk.ClassProfilesManager
{
    class ProfilesManager
    {
        public List<Profile> Profiles { get; private set; } = new List<Profile>(); 

        private StreamReader _reader;
        private StreamWriter _writer;

        private UniformGrid _profilesUniformGrid;
        private Grid _profilesGrid;
        private Button _btnBack;
        private Grid _menuGrid;
        private Label _lbProfile;

        private Profile _currentProfile;

        public Profile CurrentProfile
        {
            get => _currentProfile;
        }

        public ProfilesManager(Grid profilesGrid, Grid gridMenu, UniformGrid profilesUniformGrid, Button btnBack, Label lbProfile)
        {
            _profilesGrid = profilesGrid;
            _menuGrid = gridMenu;
            _profilesUniformGrid = profilesUniformGrid;
            _btnBack = btnBack;
            _lbProfile = lbProfile;
        }

        public async Task ProfilesGet()
        {
            using (_reader = new StreamReader(Constants.PROFILES_LOCATION))
            {
                string[] profileAttributes;

                while (!_reader.EndOfStream)
                {
                    profileAttributes = _reader.ReadLine().Split(' ');

                    try
                    {
                        Profile profile = new Profile()
                        {
                            Name = profileAttributes[0],
                            LevelsAvailable = Convert.ToInt16(profileAttributes[1]),
                            ScoreList = new List<int>()
                            {
                                Convert.ToInt16(profileAttributes[2]),
                                Convert.ToInt16(profileAttributes[3]),
                                Convert.ToInt16(profileAttributes[4]),
                                Convert.ToInt16(profileAttributes[5])
                            }
                        };

                        Profiles.Add(profile);

                        if (_profilesUniformGrid.Children[0].GetType() == new TextBlock().GetType())
                        {
                            _profilesUniformGrid.Children.RemoveAt(0);
                        }

                        ProfileButtonVisualize(profile.Name);
                    }
                    catch { }
                }
             }
        }

        public void ProfileAdd(string newProfileName)
        {
            if (_profilesUniformGrid.Children[0].GetType() == new TextBlock().GetType())
            {
                _profilesUniformGrid.Children.Clear();
            }

            Profile profile = new Profile() { Name = newProfileName };

            Profiles.Add(profile);

            using (_writer = new StreamWriter(Constants.PROFILES_LOCATION, true))
            {
                string line = $"{profile.Name} {profile.LevelsAvailable} 0 0 0 0";

                _writer.WriteLine(line);
            }

            ProfileButtonVisualize(profile.Name);
        }

        private void ProfileChoose(object sender, RoutedEventArgs e)
        {
            _currentProfile = Profiles.First(profile => profile.Name == (string)((Button)sender).Content);

            _lbProfile.Content = _currentProfile.Name;

            _profilesGrid.Visibility = Visibility.Hidden;
            _menuGrid.Visibility = Visibility.Visible;
            _btnBack.Visibility = Visibility.Hidden;
        }

        public bool ProfileExists(string newProfileName)
        {
            bool result = false;

            foreach (Button profileButton in _profilesUniformGrid.Children.OfType<Button>())
            {
                if ((string)profileButton.Content == newProfileName)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public void ProfileButtonVisualize(string profileName)
        {
            Button btnProfile = new Button()
            {
                Content = profileName
            };

            btnProfile.Click += ProfileChoose;

            _profilesUniformGrid.Children.Add(btnProfile);
        }

        public void ProfileDelete()
        {

        }
    }
}
