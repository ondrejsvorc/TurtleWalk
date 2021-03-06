﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TurtleWalk.ClassConstants;
using TurtleWalk.ClassProfile;
using TurtleWalk.ClassManager;

namespace TurtleWalk.ClassLevelManager
{
    sealed class LevelsManager : Manager
    {
        private UniformGrid _uniformGridLevels;

        private bool _unlockNextLevel = false;

        public LevelsManager(UniformGrid uniformGridLevels)
        {
            _uniformGridLevels = uniformGridLevels;
        }

        public void ReadAvailableLevelsOfGuest()
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

        // NASTAVÍ OTEVŘENÉ LEVELY PŘI VÝBERU PROFILU
        public void SetAvailableLevelsByProfile(Profile profile)
        {
            ClearPreviousAvailableLevels();

            for (int i = 0; i < profile.LevelsAvailable; i++)
            {
                _uniformGridLevels.Children[i].IsEnabled = true;
            }
        }

        // AKTUALIZUJE LEVELY DLE VYBRANÉHO PROFILU
        public void UpdateAvailableLevelsOfProfile(string lvl, Profile profile)
        {
            int currentLvlIndex = Convert.ToInt16(lvl.Substring(1, 1)) - 1;

            if (IsThereNextLevel(currentLvlIndex))
            {
                if (!_uniformGridLevels.Children[currentLvlIndex + 1].IsEnabled)
                {
                    _uniformGridLevels.Children[currentLvlIndex + 1].IsEnabled = true;
                    profile.LevelsAvailable++;
                    _unlockNextLevel = true;
                }
                else
                {
                    if (_unlockNextLevel)
                    {
                        _unlockNextLevel = false;
                    }
                }
            }
        }

        // ULOŽÍ DO TEXTOVÉHO SOUBORU POČET LEVELŮ GUESTA
        public void SaveAvailableLevelsForGuest(string lvl)
        {
            // lvl = "01" -> substring(1, 1) -> lvl = "1"
            // index: 1 - 1 = 0
            
            int currentLvlIndex = Convert.ToInt16(lvl.Substring(1, 1)) - 1;

            if (IsThereNextLevel(currentLvlIndex))
            {
                if (!_uniformGridLevels.Children[currentLvlIndex + 1].IsEnabled)
                {
                    using (_writer = new StreamWriter(Constants.AVAILABLE_LEVELS, true))
                    {
                        _writer.Write("1" + " ");
                        _writer.Flush();
                    }
                }
            }
        }

        public bool IsThereNextLevel(int currentLvlIndex)
        {
            return _uniformGridLevels.Children.Count >= currentLvlIndex + 2;
        }

        // ULOŽÍ DO TEXTOVÉHO SOUBORU POČET LEVELŮ PRO PROFIL
        public void SaveAvailableLevelsForProfiles(List<Profile> profiles)
        {
            if (_unlockNextLevel)
            {
                using (_writer = new StreamWriter(Constants.PROFILES_LOCATION, false))
                {
                    foreach (Profile profile in profiles)
                    {
                        string line = profile.Name + " " + profile.LevelsAvailable;

                        foreach (int score in profile.ScoreList)
                        {
                            line += " " + score;
                        }

                        _writer.WriteLine(line);
                    }
                }
            }
        }

        public void ClearPreviousAvailableLevels()
        {
            _uniformGridLevels.Children.OfType<Button>().ToList().Where(btn => btn.IsEnabled == true).ToList().ForEach(btn => btn.IsEnabled = false);
        }
    }
}
