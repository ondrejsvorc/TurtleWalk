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

        private Image imgTurtle;

        StreamReader streamReader;
        StreamWriter streamWriter;

        private BitmapImage bitmapImgTurtle;

        private string lvlNum;
        private string[] rows;
        int rowsCount;

        private const string PATH_DIRECTION_FORWARD = "./Resources/Images/Turtle/turtle_direction_forward.gif";
        private const string PATH_DIRECTION_BACKWARDS = "./Resources/Images/Turtle/turtle_direction_backwards.gif";

        private const string PATH_DIRECTION_FORWARD_STOPPED = "./Resources/Images/Turtle/turtleStopped_direction_forward.gif";
        private const string PATH_DIRECTION_BACKWARDS_STOPPED = "./Resources/Images/Turtle/turtleStopped_direction_backwards.gif";

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {

            int i = 0;
            lvlNum = "01";

            streamReader = new StreamReader($"./Resources/Levels/Level{lvlNum}/Start/start_lvl{lvlNum}.txt");
            rows = streamReader.ReadLine().Split(' ');
            streamReader.Close();

            rowsCount = CountRows();

            // 1 300 250 30 830 0 0

            imgTurtle = new Image
            {
                Width = Convert.ToDouble(rows[i + 1]),
                Height = Convert.ToDouble(rows[i + 2]),
                Margin = new Thickness(Convert.ToDouble(rows[i + 3]), Convert.ToDouble(rows[i + 4]), Convert.ToDouble(rows[i + 5]), Convert.ToDouble(rows[i + 6]))
            };

            gridLvl.Children.Add(imgTurtle);

            bitmapImgTurtle = new BitmapImage();
            bitmapImgTurtle.BeginInit();
            bitmapImgTurtle.UriSource = new Uri(PATH_DIRECTION_FORWARD, UriKind.Relative);
            bitmapImgTurtle.EndInit();
            ImageBehavior.SetAnimatedSource(imgTurtle, bitmapImgTurtle);

            AdditionalImageConfig(imgTurtle);
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

        private void AdditionalImageConfig(Image img)
        {
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Top;
            img.Stretch = Stretch.UniformToFill;
        }

        private void TurtleChangeDirection(object sender, MouseButtonEventArgs e)
        {

        }

        private void TurtleStopByMouse(object sender, MouseButtonEventArgs e)
        {

        }

        private void CursorEnters(object sender, MouseEventArgs e)
        {

        }

        private void CursorLeaves(object sender, MouseEventArgs e)
        {

        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {

        }

        private void Exit(object sender, RoutedEventArgs e)
        {

        }

        private void LanguageChange(object sender, MouseButtonEventArgs e)
        {

        }

        private void FlagColorChange(object sender, MouseEventArgs e)
        {

        }

        private void FlagColorBackToNormal(object sender, MouseEventArgs e)
        {

        }

        private void Play(object sender, RoutedEventArgs e)
        {

        }

        private void StartLevel(object sender, RoutedEventArgs e)
        {

        }
    }
}
