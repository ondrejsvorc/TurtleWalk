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
using TurtleWalk.ClassProfile;

namespace TurtleWalk.ClassScoreboard
{
    class Scoreboard
    {
        private static List<Profile> _players = new List<Profile>();

        private static DataGrid _scoreboard;

        private static DataTable _dataTable;

        public static void DataGet()
        {
            using (StreamReader streamReader = new StreamReader(Constants.SCOREBOARD_DATA))
            {
                while (!streamReader.EndOfStream)
                {
                    string[] playerAttributes = streamReader.ReadLine().Split(' ');

                    Profile player = new Profile();
                    player.Name = playerAttributes[0];

                    for (int i = 1; i < playerAttributes.Length; i++)
                    {
                        player.ScoreList.Add(Convert.ToInt32(playerAttributes[i]));
                    }

                    _players.Add(player);
                }
            }
        }

        public static void DataSet(DataGrid scoreboard)
        {
            _scoreboard = scoreboard;

            GenerateColumns();
            GenerateRows();
        }

        public static void DataUpdate()
        {
            _dataTable.Clear();

            DataGet();
            DataSet(_scoreboard);
        }

        private static void GenerateColumns()
        {
            _dataTable = new DataTable();

            _dataTable.Columns.Add("Hráč");

            for (int i = 1; i < 4; i++)
            {
                _dataTable.Columns.Add("Skóre level " + i);
            }
        }

        private static void GenerateRows()
        {
            int playersCount = _players.Count();

            foreach (Profile player in _players)
            {
                _dataTable.Rows.Add(player.Name, player.ScoreList[0], player.ScoreList[1], player.ScoreList[2]);

                for (int columnIndex = 1; columnIndex < 3; columnIndex++)
                {
                    for (int rowIndex = 0; rowIndex < playersCount; rowIndex++)
                    {
                        DataRow newRow = _dataTable.NewRow();
                        newRow.SetField(columnIndex, player.ScoreList[rowIndex]);
                    }
                }
            }

            _scoreboard.ItemsSource = _dataTable.DefaultView;
        }
    }
}
