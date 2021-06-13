using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TurtleWalk.ClassConstants;
using TurtleWalk.ClassLevelManager;
using TurtleWalk.ClassManager;
using TurtleWalk.ClassProfile;

namespace TurtleWalk.ClassProfilesManager
{
    class ProfilesManager : Manager
    {
        public List<Profile> Profiles { get; private set; } = new List<Profile>(); 

        private UniformGrid _uniformGridProfiles;
        private UniformGrid _uniformGridLevels;

        private Grid _profilesGrid;
        private Button _btnBack;
        private Grid _menuGrid;
        private Label _lbProfile;

        private Profile _currentProfile;

        private LevelsManager _levelManager;

        public Profile CurrentProfile
        {
            get => _currentProfile;
        }

        public ProfilesManager(Grid profilesGrid, Grid gridMenu, UniformGrid uniformGridProfiles, UniformGrid uniformGridLevels, Button btnBack, Label lbProfile)
        {
            _profilesGrid = profilesGrid;
            _uniformGridLevels = uniformGridLevels;

            _menuGrid = gridMenu;
            _uniformGridProfiles = uniformGridProfiles;
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
                            LevelsAvailable = Convert.ToInt32(profileAttributes[1]),
                            ScoreList = new List<int>()
                            {
                                Convert.ToInt32(profileAttributes[2]),
                                Convert.ToInt32(profileAttributes[3]),
                                Convert.ToInt32(profileAttributes[4]),
                                Convert.ToInt32(profileAttributes[5])
                            }
                        };

                        Profiles.Add(profile);

                        if (_uniformGridProfiles.Children[0].GetType() == new TextBlock().GetType())
                        {
                            _uniformGridProfiles.Children.RemoveAt(0);
                        }

                        ProfileButtonVisualize(profile.Name);
                    }
                    catch { }
                }
             }
        }

        public void ProfileAdd(string newProfileName)
        {
            if (_uniformGridProfiles.Children.Count > 0)
            {
                if (_uniformGridProfiles.Children[0].GetType() == new TextBlock().GetType())
                {
                    _uniformGridProfiles.Children.Clear();
                }
            }

            Profile profile = new Profile() 
            { 
                Name = newProfileName, 
                ScoreList = new List<int>() { 0, 0, 0, 0 } 
            };

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

            foreach (Button profileButton in _uniformGridProfiles.Children.OfType<Button>())
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
            btnProfile.MouseRightButtonUp += ProfileWishToDelete;

            _uniformGridProfiles.Children.Add(btnProfile);
        }

        private void ProfileWishToDelete(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Opravdu si tento profil přejete odstranit?", "Vymazání profilu", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                ProfileDelete(result, sender as Button);
            }
        }

        public void ProfileDelete(MessageBoxResult result, Button profileBtn)
        {
            _uniformGridProfiles.Children.Remove(profileBtn);

            Profiles.RemoveAll(profile => profile.Name == (string)profileBtn.Content);

            using (_writer = new StreamWriter(Constants.PROFILES_LOCATION, false))
            {
                foreach (Profile profile in Profiles)
                {
                    string line = profile.Name + " " + profile.LevelsAvailable;

                    foreach (int score in profile.ScoreList)
                    {
                        line += " " + score;
                    }

                    _writer.WriteLine(line);
                }
            }

            _currentProfile = null;

            _levelManager = new LevelsManager(_uniformGridLevels);
            _levelManager.ReadAvailableLevelsOfGuest();
            _lbProfile.Content = "Guest";
        }
    }
}

