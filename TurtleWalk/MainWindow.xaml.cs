using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using WpfAnimatedGif;
using System.Collections.Generic;

namespace TurtleWalk
{
    public partial class MainWindow : Window
    {
        private Turtle turtle;

        private StreamReader reader;
        private StreamWriter writer;

        private BitmapImage bitmapImgTurtle;

        private string lvlNum;
        private string[] rows;
        private int rowCount;

        private string levelInProgress;

        private Cursor cursorHand;
        private Cursor cursorGrabbed;

        private const string PATH_DIRECTION_FORWARD = "./Resources/Images/Turtle/turtle_direction_forward.gif";
        private const string PATH_DIRECTION_BACKWARDS = "./Resources/Images/Turtle/turtle_direction_backwards.gif";

        private const string PATH_DIRECTION_FORWARD_STOPPED = "./Resources/Images/Turtle/turtleStopped_direction_forward.gif";
        private const string PATH_DIRECTION_BACKWARDS_STOPPED = "./Resources/Images/Turtle/turtleStopped_direction_backwards.gif";

        public MainWindow()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            // CHECKS FOR AVAILABLE LEVELS (EXCEPT FOR THE FIRST ONE, WHICH IS ALWAYS AVAILABLE)

            reader = new StreamReader("./Resources/Levels/Available/available_levels.txt");

            string[] availableLevels = reader.ReadLine().Split(' ');

            for (int i = 0; i < availableLevels.Length - 1; i++)
            {
                if (availableLevels[i + 1] == "1")
                {
                    gridLevels.Children[i + 1].IsEnabled = true; 
                }
            }

            reader.Close();

            cursorHand = new Cursor(new MemoryStream(Properties.Resources.cursorHand));
            cursorGrabbed = new Cursor(new MemoryStream(Properties.Resources.cursorGrabbed));

            levelInProgress = "none";
        }

        private void Start()
        {
            // PROJEDE TEXTOVÝ SOUBOR A DLE TYPU PŘEDÁ INFO PŘÍSLUŠNÉ TŘÍDĚ, KTERÁ SE POSTARÁ O JEJÍ PŘIDÁNÍ NA GRID

            levelInProgress = lvlNum;

            rowCount = CountRows();

            for (int i = 0; i < rowCount; i++)
            {
                reader = new StreamReader($"./Resources/Levels/Level{lvlNum}/Start/start_lvl{lvlNum}.txt");
                rows = reader.ReadLine().Split(' ');

                switch (rows[0])
                {
                    case "Turtle":
                        Turtle turtle = new Turtle(rows, gridLvl);
                        break;

                    case "Background":

                        break;

                    case "Ground":
                        break;

                    case "SavingPlatform":
                        
                        break;

                    case "Piston":
                        
                        break;

                    case "Leaf":
                        
                        break;
                }
            }

            reader.Close();

            gridLvl.Visibility = Visibility.Visible;
        }

        private void Finish()
        {
            levelInProgress = string.Empty;
        }

        private int CountRows()
        {
            StreamReader r = new StreamReader($"./Resources/Levels/Level{lvlNum}/Start/start_lvl{lvlNum}.txt");

            int i = 0;

            while (r.ReadLine() != null)
            {
                i++;
            }

            return i;
        }

        //public void AdditionalImageConfig(Image img)
        //{
        //    img.HorizontalAlignment = HorizontalAlignment.Left;
        //    img.VerticalAlignment = VerticalAlignment.Top;
        //    img.Stretch = Stretch.UniformToFill;
        //}

        private void TurtleChangeDirection(object sender, MouseButtonEventArgs e)
        {

        }

        private void TurtleStopByMouse(object sender, MouseButtonEventArgs e)
        {

        }

        private void CursorEnters(object sender, MouseEventArgs e)
        {
            Button btnSender = (Button)sender;

            btnSender.Cursor = cursorGrabbed;
            btnSender.Background = new SolidColorBrush(Colors.White);
            btnSender.Foreground = new SolidColorBrush(Color.FromRgb(69, 189, 120));

            for (int i = 0; i < btnSender.Content.ToString().Length; i++)
            {
                if (char.IsDigit(btnSender.Content.ToString()[i]))
                {
                    lbHeading.Content = "Level " + btnSender.Content.ToString();
                    break;
                }
                else
                {
                    lbHeading.Content = btnSender.Content.ToString();
                }
            }
        }

        private void CursorLeaves(object sender, MouseEventArgs e)
        {
            ((Button)sender).Cursor = cursorHand;
            ((Button)sender).Background = new SolidColorBrush(Color.FromRgb(69, 189, 120));
            ((Button)sender).Foreground = new SolidColorBrush(Colors.White);

            lbHeading.Content = "TurtleWalk";
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            gridLevels.Visibility = Visibility.Hidden;
            gridButtons.Visibility = Visibility.Visible;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LanguageChange(object sender, MouseButtonEventArgs e)
        {
            switch (((Image)sender).Name)
            {
                case "Czech":
                    btnPlay.Content = "Hrát";
                    btnSettings.Content = "Nastavení";
                    btnControls.Content = "Ovládaní";
                    btnExit.Content = "Odejít";
                    btnBack.Content = "Zpět";
                    break;

                case "English":
                    foreach (Button btnMenu in gridButtons.Children)
                    {
                       btnMenu.Content = btnMenu.Name.Remove(0, 3);
                    }
                    btnBack.Content = btnBack.Name.Remove(0, 3);
                    break;
            }
        }

        private void FlagColorChange(object sender, MouseEventArgs e)
        {
            ((Image)sender).Opacity = 0.5;
            ((Image)sender).Cursor = cursorGrabbed;
        }

        private void FlagColorBackToNormal(object sender, MouseEventArgs e)
        {
            ((Image)sender).Opacity = 1;
            ((Image)sender).Cursor = cursorHand;
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            gridButtons.Visibility = Visibility.Hidden;
            gridLevels.Visibility = Visibility.Visible;
        }

        private void ResumeLevel()
        {

        }

        private void StartLevel(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Content)
            {
                case "01":
                case "02":
                case "03":
                case "04":
                case "05":
                case "06":
                case "07":
                    gridMenu.Visibility = Visibility.Hidden;
                    lvlNum = ((Button)sender).Content.ToString();
                    Start();
                    //if (!levelInProgress)
                    //{
                    //    //Start();
                    //}
                    //else
                    //{
                    //    //ResumeLevel();
                    //}
                    break;
            }
        }

        private void LevelStop(object sender, KeyEventArgs e)
        {
            if (levelInProgress != "none")
            {
                if (e.Key == Key.Escape)
                {
                    turtle.IsMoving = false;
                    gridLvl.Visibility = Visibility.Hidden;
                    gridMenu.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
