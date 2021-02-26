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
        private SavingPlatform savingPlatform;

        private Uri source;

        private StreamReader reader;
        private StreamWriter writer;

        private DispatcherTimer timer;

        private string lvl;
        private int rowCount;

        private string levelInProgress;

        private Cursor cursorHand;
        private Cursor cursorGrabbed;

        private double marginLeft, marginRight;

        private const string PATH_DIRECTION_FORWARD = "./Resources/Images/Turtle/turtle_direction_forward.gif";
        private const string PATH_DIRECTION_BACKWARDS = "./Resources/Images/Turtle/turtle_direction_backwards.gif";

        private const string PATH_DIRECTION_FORWARD_STOPPED = "./Resources/Images/Turtle/turtleStopped_direction_forward.gif";
        private const string PATH_DIRECTION_BACKWARDS_STOPPED = "./Resources/Images/Turtle/turtleStopped_direction_backwards.gif";

        //private List<Ground> grounds;
        //private List<LavaDrop> lavaDrops;
        //private List<Leaf> leafs;
        //private List<Piston> pistons;

        public static Grid GridLvl;

        public MainWindow()
        {
            InitializeComponent();
            Setup();

            GridLvl = gridLvl;

            window.KeyDown += MovePlatform;
            window.KeyDown += LevelStop;
        }

        private void LevelStop(object sender, KeyEventArgs e)
        {
            if (levelInProgress != "none")
            {
                if (e.Key == Key.Escape)
                {
                    //turtle.IsMoving = false;
                    gridLvl.Visibility = Visibility.Hidden;
                    gridMenu.Visibility = Visibility.Visible;
                }
            }
        }

        // FIX THIS
        private void MovePlatform(object sender, KeyEventArgs e)
        {
            //switch (e.Key)
            //{
            //    case Key.Left:
            //    case Key.A:
            //        if (marginLeft > 0)
            //        {
            //            marginLeft -= 25;
            //            marginRight += 25;
            //        }
            //        break;

            //    case Key.Right:
            //    case Key.D:
            //        if (marginRight > 0)
            //        {
            //            marginRight -= 25;
            //            marginLeft += 25;
            //        }
            //        break;
            //}

            //SavingPlatform.Move(savingPlatform.Body, marginLeft, marginRight);
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

            //grounds = new List<Ground>();
            //lavaDrops = new List<LavaDrop>();
            //pistons = new List<Piston>();
            //leafs = new List<Leaf>();

            cursorHand = new Cursor(new MemoryStream(Properties.Resources.cursorHand));
            cursorGrabbed = new Cursor(new MemoryStream(Properties.Resources.cursorGrabbed));

            levelInProgress = "none";
        }

        private void Start()
        {
            // PROJEDE TEXTOVÝ SOUBOR A DLE TYPU PŘEDÁ INFO PŘÍSLUŠNÉ TŘÍDĚ, KTERÁ SE POSTARÁ O JEJÍ PŘIDÁNÍ NA GRID

            if (levelInProgress == "none")
            {
                levelInProgress = lvl;

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(25);
                timer.Tick += GameUpdate;
                timer.Start();

                LevelBuilder builder = new LevelBuilder($"./Resources/Levels/Level{lvl}/Start/start_lvl{lvl}.txt", lvl);
                builder.Start();

                gridLvl.Visibility = Visibility.Visible;
            }
            else
            {
                gridMenu.Visibility = Visibility.Hidden;
                gridLvl.Visibility = Visibility.Visible;
            }
            
        }

        private void GameUpdate(object sender, EventArgs e)
        {
            // NEUSTÁLE SE AKTUALIZUJÍCÍ A MODIFIKOVANÉ HITBOXY ŽELVIČKY A PLATFORMY PRO PŘIROZENOU DETEKCI KOLIZE V REÁLNÉM ČASE
            //Turtle.HitBoxUpdate(turtle, turtle.Body);
            //SavingPlatform.HitBoxUpdate(savingPlatform, savingPlatform.Body);

            // POČÍTÁNÍ TIKŮ PRO FUNKČNOST ALGORITMU NA AUTOMATICKÉ PADÁNÍ KAPEK
            //timeElapsed++;

            // ZELVIČKA JDE POPŘEDU A DOTÝKÁ SE
            // ZELVIČKA JDE POZPÁTKU A DOTÝKÁ SE

            //if (turtle.IsMoving && turtle.IsDirectionForward && Ground.CheckCollision(turtle))
            //{
            //    Turtle.Move(turtle.Body, turtle.DistanceFromStart += 5, turtle.SeaLevel);
            //}
            //else if (turtle.IsMoving && !turtle.IsDirectionForward && Ground.CheckCollision(turtle))
            //{
            //    Turtle.Move(turtle.Body, turtle.DistanceFromStart -= 5, turtle.SeaLevel);
            //}

            //// ZELVIČKA SE DOTÝKÁ LISTU
            //if (turtle.HitBox.IntersectsWith(leaf.HitBox))
            //{
            //    imgLeaf.Visibility = Visibility.Hidden;
            //    leaf.HitBox = Rect.Empty;

            //    lbScore.Content = $"Score: {scoreCount += 10}";
            //}

            // ŽElVIČKA STOUPLA NA PISTON
            //foreach (Piston piston in pistons)
            //{
            //    if (turtle.HitBox.IntersectsWith(piston.HitBox))
            //    {
            //        Turtle.Move(turtle.Body, turtle.DistanceFromStart += 4.5, turtle.SeaLevel -= 22.5);
            //    }
            //}

            // ZELVIČKA SE NIČEHO NEDOTÝKÁ
            //foreach (Piston piston in pistons)
            //{
            //    if (!(Ground.CheckCollision(turtle) || turtle.HitBox.IntersectsWith(piston.HitBox)))
            //    {
            //        Turtle.Move(turtle.Body, turtle.DistanceFromStart += 2, turtle.SeaLevel += 8);
            //    }
            //}

            //// ZELVIČKA SE NEHÝBE
            //if (!turtle.IsMoving)
            //{
            //    Turtle.DontMove(imgTurtle);
            //}

            //// VYMAZÁNÍ PŘEDEŠLÝCH ZAZNAMENANÝCH KOLIZÍ KAPEK A RESTART INDEXU PRO MOŽNOST ZNOVU ZAPISOVÁNÍ DO POLE
            //Array.Clear(collisionPlatform, 0, collisionPlatform.Length);
            //index = 0;

            //// DETEKCE KOLIZE MEZI PLATFORMOU A KAPKOU (INDIVIDUÁLNÍ PRO KAŽDOU KAPKU)

            //if (SavingPlatform.CheckCollisionBetween(platform, lavaDrop1))
            //{
            //    collisionPlatform[index++] = 1;
            //}

            //if (SavingPlatform.CheckCollisionBetween(platform, lavaDrop2))
            //{
            //    collisionPlatform[index++] = 2;
            //}

            //if (SavingPlatform.CheckCollisionBetween(platform, lavaDrop3))
            //{
            //    collisionPlatform[index++] = 3;
            //}

            //if (SavingPlatform.CheckCollisionBetween(platform, lavaDrop4))
            //{
            //    collisionPlatform[index++] = 4;
            //}

            //// ALGORITMUS PRO AUTOMATICKÉ PADÁNÍ KAPEK

            //// 1 tick = 25 ms
            //// 80 * 25 = 2000 ms = 2s

            //if (timeElapsed >= 0 && timeElapsed <= 80)
            //{
            //    // Jestliže se želvička dotýká země - začni padat a do té doby, co se kapka nedotýká platformy ani země, padej dál

            //    if (Ground.CheckCollisionBetween(turtle, ground2) && !collisionPlatform.Contains(1) && !lavaDrop1.HitBox.IntersectsWith(ground2.HitBox))
            //    {
            //        LavaDrop.Fall(imgLavaDrop1, lavaDrop1, imgLavaDrop1.Margin.Top);
            //    }

            //    if (!collisionPlatform.Contains(2))
            //    {
            //        LavaDrop.Fall(imgLavaDrop2, lavaDrop2, imgLavaDrop2.Margin.Top);
            //    }

            //    if (!collisionPlatform.Contains(3))
            //    {
            //        LavaDrop.Fall(imgLavaDrop3, lavaDrop3, imgLavaDrop3.Margin.Top);
            //    }

            //    if (!collisionPlatform.Contains(4))
            //    {
            //        LavaDrop.Fall(imgLavaDrop4, lavaDrop4, imgLavaDrop4.Margin.Top);
            //    }
            //}
            //else if (timeElapsed > 80)
            //{
            //    LavaDrop.ResetPosition(imgLavaDrop2, lavaDrop2);
            //    LavaDrop.ResetPosition(imgLavaDrop3, lavaDrop3);
            //    LavaDrop.ResetPosition(imgLavaDrop4, lavaDrop4);
            //    LavaDrop.ResetPosition(imgLavaDrop1, lavaDrop1);

            //    timeElapsed = 0;
            //}

            //// ŽELVIČKA SE DOTKLA KAPKY (pozn.: zoptimalizovat podmínku)
            //if (turtle.HitBox.IntersectsWith(lavaDrop2.HitBox) || turtle.HitBox.IntersectsWith(lavaDrop3.HitBox) || turtle.HitBox.IntersectsWith(lavaDrop4.HitBox) || turtle.HitBox.IntersectsWith(lavaDrop1.HitBox))
            //{
            //    GameRestart();
            //}

            ////ŽELVIČKA DOKONČILA LEVEL
            //if (turtle.HitBox.IntersectsWith(finishSign.HitBox))
            //{
            //
            //}
        }

        private void Finish()
        {
            levelInProgress = string.Empty;
        }

        private int CountRows()
        {
            StreamReader r = new StreamReader($"./Resources/Levels/Level{lvl}/Start/start_lvl{lvl}.txt");

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
                    lvl = ((Button)sender).Content.ToString();
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
    }
}
