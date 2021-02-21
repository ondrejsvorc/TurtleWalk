using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.Generic;
using System.IO;
using WpfAnimatedGif;

namespace TurtleWalk
{
    public partial class MainWindow : Window
    {
        private Turtle turtle;

        private StreamReader reader;
        private StreamWriter writer;

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

        private List<Ground> grounds;

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

            grounds = new List<Ground>();

            cursorHand = new Cursor(new MemoryStream(Properties.Resources.cursorHand));
            cursorGrabbed = new Cursor(new MemoryStream(Properties.Resources.cursorGrabbed));

            levelInProgress = "none";
        }

        private void Start()
        {
            // PROJEDE TEXTOVÝ SOUBOR A DLE TYPU PŘEDÁ INFO PŘÍSLUŠNÉ TŘÍDĚ, KTERÁ SE POSTARÁ O JEJÍ PŘIDÁNÍ NA GRID

            levelInProgress = lvlNum;

            rowCount = CountRows();

            reader = new StreamReader($"./Resources/Levels/Level{lvlNum}/Start/start_lvl{lvlNum}.txt");

            for (int i = 0; i < rowCount; i++)
            {
                rows = reader.ReadLine().Split(' ');

                switch (rows[0])
                {
                    case "Turtle":
                        turtle = new Turtle(rows, gridLvl);
                        break;

                    case "Background":
                        Image imgBackground = new Image
                        {
                            Width = gridMain.Width,
                            Height = gridMain.Height,
                            Source = new BitmapImage(new Uri($"./Resources/Levels/Level{lvlNum}/Background/background_lvl{lvlNum}.jpg", UriKind.Relative))
                        };

                        Panel.SetZIndex(imgBackground, 0);
                        gridLvl.Children.Add(imgBackground);
                        break;

                    case "Ground":
                        Image imgGround = new Image
                        {
                            Width = Convert.ToDouble(rows[1]),
                            Height = Convert.ToDouble(rows[2]),
                            Margin = new Thickness(Convert.ToDouble(rows[3]), Convert.ToDouble(rows[4]), Convert.ToDouble(rows[5]), Convert.ToDouble(rows[6])),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };

                        if (imgGround.Width < 1000)
                        {
                            imgGround.Source = new BitmapImage(new Uri($"./Resources/Levels/Level{lvlNum}/Platforms/ice_platform_1.png", UriKind.Relative));
                        }
                        else
                        {
                            imgGround.Source = new BitmapImage(new Uri($"./Resources/Levels/Level{lvlNum}/Platforms/ice_platform_2.png", UriKind.Relative));
                        }

                        Panel.SetZIndex(imgGround, 1);
                        gridLvl.Children.Add(imgGround);

                        //Ground ground = new Ground(CollisionElement.SetHitBox(imgGround), 0, 0);
                        //grounds.Add(ground);
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
