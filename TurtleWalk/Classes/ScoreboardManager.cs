using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using TurtleWalk.ClassConstants;
using TurtleWalk.ClassManager;
using TurtleWalk.ClassProfile;

namespace TurtleWalk.ClassScoreboardManager
{
    sealed class ScoreboardManager : Manager
    {
        private static List<Profile> _profiles = new List<Profile>();

        private static DataGrid _scoreboard;

        private static DataTable _dataTable;

        public ScoreboardManager(DataGrid scoreboard)
        {
            _scoreboard = scoreboard;
        }

        public void DataGet(List<Profile> profiles)
        {
            _profiles = profiles;
        }

        public void DataSet()
        {
            GenerateColumns();
            GenerateRows();
        }

        public void DataUpdate()
        {
            if (_dataTable != null)
            {
                _dataTable.Clear();
            }

            DataGet(_profiles);
            DataSet();
        }

        private void GenerateColumns()
        {
            _dataTable = new DataTable();

            _dataTable.Columns.Add("Hráč");

            for (int i = 1; i < 5; i++)
            {
                _dataTable.Columns.Add("Skóre level " + i);
            }
        }

        private void GenerateRows()
        {
            foreach (Profile player in _profiles)
            {
                _dataTable.Rows.Add(player.Name, player.ScoreList[0], player.ScoreList[1], player.ScoreList[2], player.ScoreList[3]);

                for (int columnIndex = 0; columnIndex < 3; columnIndex++)
                {
                    for (int rowIndex = 0; rowIndex < player.ScoreList.Count(); rowIndex++)
                    {
                        DataRow newRow = _dataTable.NewRow();
                        newRow.SetField(columnIndex, player.ScoreList[rowIndex]);
                    }
                }
            }

            _scoreboard.ItemsSource = _dataTable.DefaultView;
        }

        public void DataSave(List<Profile> profiles)
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
}
