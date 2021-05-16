﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Collections.Generic;
using System.IO;
using WpfAnimatedGif;
using TurtleWalk.ClassCollisionElement;
using TurtleWalk.ClassConstants;
using TurtleWalk.ClassGround;
using TurtleWalk.ClassLavaDrop;
using TurtleWalk.ClassLeaf;
using TurtleWalk.ClassLevelBuilder;
using TurtleWalk.ClassPiston;
using TurtleWalk.ClassSavingPlatform;
using TurtleWalk.ClassSign;
using TurtleWalk.ClassTurtle;
using TurtleWalk.ClassLevelManager;
using TurtleWalk.ClassScoreboard;
using TurtleWalk.ClassProfilesManager;

namespace TurtleWalk
{
    // TO-DO

    // - Při dokončení levelu odemknout vždy jen další level (dokončení levelu 1 odekmkne level 2) 
    //      : HOTOVO

    // - Přepínání na další levely
    //      : HOTOVO

    // - Design 2. levelu
    //      :

    // - Scoreboard
    //      :

    // - Design 3. levelu
    //      :

    public partial class MainWindow : Window
    {
        private Turtle turtle;
        private SavingPlatform savingPlatform;
        private Sign finishSign;

        private DispatcherTimer timer;

        private string lvl;

        private int scoreCount;

        private string levelInProgress;

        private Cursor cursorHand;
        private Cursor cursorGrabbed;

        private int clickCountDirection, clickCountMovement;

        private int[] collisionPlatform;
        private int timeElapsed;

        private List<LavaDrop> lavaDrops;
        private List<Leaf> leafs;

        public List<string> players = new List<string>() { "Test", "Test2" };

        public MainWindow()
        {
            InitializeComponent();
            Setup();

            window.KeyDown += MovePlatform;
            window.KeyDown += LevelStop;
        }

        private void Setup()
        {
            lavaDrops = new List<LavaDrop>();
            leafs = new List<Leaf>();

            LevelsManager.GetAvailableLevels(uniformGridLevels);

            Scoreboard.DataGet();
            Scoreboard.DataSet(dataGridScoreboard);

            cursorHand = new Cursor(new MemoryStream(Properties.Resources.cursorHand));
            cursorGrabbed = new Cursor(new MemoryStream(Properties.Resources.cursorGrabbed));

            clickCountDirection = 0;
            clickCountMovement = 0;

            collisionPlatform = new int[4];

            timeElapsed = 0;

            levelInProgress = "none";
        }
        private void LevelStart()
        {
            //if (levelInProgress == "none")
            //{
                levelInProgress = lvl;

                string currentLevelPath = $"./Resources/Levels/Level{lvl}/Start/start_lvl{lvl}.txt";

                LevelBuilder builder = new LevelBuilder(currentLevelPath, lvl, gridLvl);
                builder.BuildLevel();

                using (StreamReader reader = new StreamReader(currentLevelPath))
                {
                    foreach (Image image in builder.Images)
                    {
                        string[] rowProperties = reader.ReadLine().Split(' ');

                        if (image.Width.ToString() == rowProperties[1] && image.Height.ToString() == rowProperties[2] && image.Margin.Left.ToString() == rowProperties[3] && image.Margin.Top.ToString() == rowProperties[4])
                        {
                            switch (rowProperties[0])
                            {
                                case "Turtle":
                                    turtle = new Turtle(CollisionElement.SetHitBox(image), image.Margin.Left, image.Margin.Top);
                                    turtle.Body = image;
                                    break;

                                case "SavingPlatform":
                                    savingPlatform = new SavingPlatform(image.Margin.Left);
                                    savingPlatform.Body = image;
                                    break;

                                case "Sign":
                                    finishSign = new Sign(CollisionElement.SetHitBox(image));
                                    break;

                                case "Ground":
                                    Ground ground = new Ground(CollisionElement.SetHitBox(image), Convert.ToDouble(rowProperties[5]), Convert.ToDouble(rowProperties[6]));
                                    break;

                                case "Leaf":
                                    Leaf leaf = new Leaf(CollisionElement.SetHitBox(image));
                                    leaf.Body = image;
                                    leafs.Add(leaf);
                                    break;

                                case "Piston":
                                    Piston piston = new Piston(CollisionElement.SetHitBox(image), Convert.ToDouble(rowProperties[5]), Convert.ToDouble(rowProperties[6]));
                                    break;

                                case "LavaDrop":
                                    LavaDrop lavaDrop = new LavaDrop(CollisionElement.SetHitBox(image), image.Margin.Top);
                                    lavaDrop.Body = image;
                                    lavaDrops.Add(lavaDrop);
                                    break;
                            }
                        }
                    }
                }

                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
                timer.Tick += GameUpdate;
                timer.Start();

                gridLvl.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    LevelResume();
            //}
        }

        // TATO METODA SE VOLÁ I PŘI SMRTI ŽELVY, TÍM PÁDEM SE VYKRESLUJE CELEJ LEVEL ZNOVA ZBYTEČNĚ - OŠETŘIT TO TAK, ABY SE DALA ŽELVA A PLOŠINKA NA ZAČÁTEK
        // + ABY SE VYKRESLILY ZNOVA LISTY, TY TOTIŽ MŮŽEME V PRŮBĚHU HRY SEŽRAT A PAK BY TAM CHYBĚLI
        // ALE ABY SE TAM VYKRESLOVALI ZNOVA GROUND APOD. TO JE ZBYTEČNÝ
        private void LevelResetValues()
        {
            timer.Stop();

            lavaDrops = new List<LavaDrop>();
            leafs = new List<Leaf>();

            Ground.NullHitBoxes();
            Piston.NullHitBoxes();

            levelInProgress = "none";
            timeElapsed = 0;

            clickCountDirection = 0;
            clickCountMovement = 0;

            lbScore.Content = $"Score: {scoreCount = 0}";

            int lastIndex = gridLvl.Children.Count - 1;

            while (gridLvl.Children.Count != 7)
            {
                gridLvl.Children.RemoveAt(lastIndex--);
            }
        }

        private void LevelRestart()
        {
            LevelResetValues();
            LevelStart();
        }

        private void LevelResume()
        {
            timer.Start();

            gridMenu.Visibility = Visibility.Hidden;
            gridLvl.Visibility = Visibility.Visible;
        }

        // Indexy 0 - 6 jsou vždy na gridu --> overlay 
        // Z gridu odebíráme všechny potomky do té doby, než tam nezůstane pouze overlay, jehož odstranění je nežádoucí

        private void LevelFinish()
        {
            LevelResetValues();

            LevelsManager.SetAvailableLevels(uniformGridLevels, lvl);
            LevelsManager.GetAvailableLevels(uniformGridLevels);

            gridLvl.Visibility = Visibility.Hidden;
            gridMenu.Visibility = Visibility.Visible;
        }

        private void LevelSwitch()
        {
            LevelResetValues();
            LevelStart();
        }

        private void GameUpdate(object sender, EventArgs e)
        {
            // NEUSTÁLE SE AKTUALIZUJÍCÍ A MODIFIKOVANÉ HITBOXY ŽELVIČKY A PLATFORMY PRO PŘIROZENOU DETEKCI KOLIZE V REÁLNÉM ČASE
            turtle.HitBoxUpdate();

            if (savingPlatform != null)
            {
                savingPlatform.HitBoxUpdate();
            }

            // POČÍTÁNÍ TIKŮ PRO FUNKČNOST ALGORITMU NA AUTOMATICKÉ PADÁNÍ KAPEK
            timeElapsed++;

            // ŽELVIČKA JDE POPŘEDU A DOTÝKÁ SE
            // ŽELVIČKA JDE POZPÁTKU A DOTÝKÁ SE

            if (turtle.IsMoving && turtle.IsDirectionForward && Ground.CheckCollision(turtle))
            {
                turtle.X += 5;
            }
            else if (turtle.IsMoving && !turtle.IsDirectionForward && Ground.CheckCollision(turtle))
            {
                if (turtle.X > -(turtle.Body.Width / 4))
                {
                    turtle.X -= 5;
                }
            }

            if (turtle.IsMoving)
            {
                // ŽELVIČKA SE DOTÝKÁ LISTU
                foreach (Leaf leaf in leafs)
                {
                    if (turtle.HitBox.IntersectsWith(leaf.HitBox))
                    {
                        leaf.HitBox = Rect.Empty;
                        gridLvl.Children.Remove(leaf.Body);

                        lbScore.Content = $"Score: {scoreCount += 10}";
                    }
                }

                // ŽELVIČKA DOKONČILA LEVEL
                if (turtle.HitBox.IntersectsWith(finishSign.HitBox))
                {
                    LevelFinish();
                }
            }

            // ŽElVIČKA STOUPLA NA PISTON
            if (Piston.CheckCollision(turtle))
            {
                turtle.X += 4.5;
                turtle.Y -= 22.5;
            }

            // ZELVIČKA SE NIČEHO NEDOTÝKÁ
            if (!(Ground.CheckCollision(turtle) || Piston.CheckCollision(turtle)))
            {
                turtle.X += 2;
                turtle.Y += 8;
            }

            if (lavaDrops.Count() != 0)
            {
                // VYMAZÁNÍ PŘEDEŠLÝCH ZAZNAMENANÝCH KOLIZÍ KAPEK A RESTART INDEXU PRO MOŽNOST ZNOVU ZAPISOVÁNÍ DO POLE
                collisionPlatform = new int[4];

                // DETEKCE KOLIZE MEZI PLATFORMOU A KAPKOU (INDIVIDUÁLNÍ PRO KAŽDOU KAPKU)
                if (savingPlatform.CheckCollisionWith(lavaDrops[0]))
                {
                    collisionPlatform[0] = 1;
                }

                if (savingPlatform.CheckCollisionWith(lavaDrops[1]))
                {
                    collisionPlatform[1] = 2;
                }

                if (savingPlatform.CheckCollisionWith(lavaDrops[2]))
                {
                    collisionPlatform[2] = 3;
                }

                // ALGORITMUS PRO AUTOMATICKÉ PADÁNÍ KAPEK

                // 1 tick = 30 ms
                // 66 * 30 = 2000 ms = 2s

                if (timeElapsed >= 0 && timeElapsed <= 66)
                {
                    if (!collisionPlatform.Contains(1))
                    {
                        LavaDrop.Fall(lavaDrops[0], lavaDrops[0].Body.Margin.Top);
                    }

                    if (!collisionPlatform.Contains(2))
                    {
                        LavaDrop.Fall(lavaDrops[1], lavaDrops[1].Body.Margin.Top);
                    }

                    if (!collisionPlatform.Contains(3))
                    {
                        LavaDrop.Fall(lavaDrops[2], lavaDrops[2].Body.Margin.Top);
                    }
                }
                else if (timeElapsed > 66)
                {
                    LavaDrop.ResetPositions(lavaDrops);

                    timeElapsed = 0;
                }

                foreach (LavaDrop lavaDrop in lavaDrops)
                {
                    if (turtle.HitBox.IntersectsWith(lavaDrop.HitBox))
                    {
                        LevelRestart();
                    }
                }
            }
        }

        private void LevelStop(object sender, KeyEventArgs e)
        {
            if (levelInProgress != "none")
            {
                if (e.Key == Key.Escape)
                {
                    timer.Stop();

                    gridLvl.Visibility = Visibility.Hidden;
                    gridMenu.Visibility = Visibility.Visible;
                }
            }
        }

        private void MovePlatform(object sender, KeyEventArgs e)
        {
            if (levelInProgress != "none" && (gridMenu.Visibility != Visibility.Visible || uniformGridLevels.Visibility != Visibility.Visible))
            {
                int step = 0;

                switch (e.Key)
                {
                    case Key.Left:
                    case Key.A:
                        if (savingPlatform.X > 25)
                        {
                            step = -25;
                        }
                        break;

                    case Key.Right:
                    case Key.D:
                        if (savingPlatform.X < 650)
                        {
                            step = 25;
                        }
                        break;
                }

                savingPlatform.X += step;
            }
        }

        private void TurtleChangeDirection(object sender, MouseButtonEventArgs e)
        {
            // CLICKCOUNTDIRECTION NABÝVÁ POUZE HODNOT -1; 0, 1, PŘIČEMŽ ROZHODUJE NA ZÁKLADĚ SUDÉ 0 A LICHÉ 1

            if (++clickCountDirection % 2 > 0)   // lichý klik (první, třetí, pátý, ...)
            {
                imgDirection.Cursor = cursorGrabbed;

                lbDirection.Content = "Forward";
                turtle.IsDirectionForward = false;
                clickCountDirection = -1;       // resetování hodnoty, takže při dalším kroku to je sudé (tedy 0) - nezahlcujeme paměť
            }
            else                                // sudý klik (druhý, čtvrtý, šestý, ...)
            {
                imgDirection.Cursor = cursorHand;

                lbDirection.Content = "Backwards";
                turtle.IsDirectionForward = true;
            }

            UpdateImages();
        }

        private void TurtleStopByMouse(object sender, MouseButtonEventArgs e)
        {
            // CLICKCOUNTMOVEMENT NABÝVÁ POUZE HODNOT -1; 0, 1, PŘIČEMŽ ROZHODUJE NA ZÁKLADĚ SUDÉ 0 A LICHÉ 1

            if (++clickCountMovement % 2 > 0)   // lichý klik (první, třetí, pátý, ...)
            {
                imgTurtleState.Cursor = cursorGrabbed;

                lbState.Content = "Walk";
                turtle.IsMoving = false;
                clickCountMovement = -1;       // resetování hodnoty, takže při dalším kroku to je sudé (tedy 0) - nezahlcujeme paměť
            }
            else                               // sudý klik (druhý, čtvrtý, šestý, ...)
            {
                imgTurtleState.Cursor = cursorHand;

                lbState.Content = "Stop";
                turtle.IsMoving = true;
            }

            UpdateImages();
        }

        private void UpdateImages()
        {
            // ZELVIČKA SE HÝBE A SMĚR JE NASTAVEN DOPŘEDU 
            // ZELVIČKA SE HÝBE A SMĚR JE NASTAVEN POZPÁTKU

            // ZELVIČKA SE NEHÝBE A SMĚR JE NASTAVEN DOPŘEDU
            // ZELVIČKA SE NEHÝBE A SMĚR JE NASTAVEN POZPÁTKU

            Uri gifSourceTurtle = null;
            Uri gifSourceDirection = null;

            if (turtle.IsMoving && turtle.IsDirectionForward)
            {
                gifSourceTurtle = new Uri(Constants.PATH_DIRECTION_FORWARD);
                gifSourceDirection = new Uri(Constants.PATH_DIRECTION_BACKWARDS);
            }
            else if (turtle.IsMoving && !turtle.IsDirectionForward)
            {
                gifSourceTurtle = new Uri(Constants.PATH_DIRECTION_BACKWARDS);
                gifSourceDirection = new Uri(Constants.PATH_DIRECTION_FORWARD);
            }
            else if (!turtle.IsMoving && turtle.IsDirectionForward)
            {
                gifSourceTurtle = new Uri(Constants.PATH_DIRECTION_FORWARD_STOPPED);
            }
            else if (!turtle.IsMoving && !turtle.IsDirectionForward)
            {
                gifSourceTurtle = new Uri(Constants.PATH_DIRECTION_BACKWARDS_STOPPED);
            }

            BitmapImage bitmapBody = new BitmapImage();
            bitmapBody.BeginInit();
            bitmapBody.UriSource = gifSourceTurtle;
            bitmapBody.EndInit();

            ImageBehavior.SetAnimatedSource(turtle.Body, bitmapBody);

            if (gifSourceDirection != null)
            {
                bitmapBody = new BitmapImage();
                bitmapBody.BeginInit();
                bitmapBody.UriSource = gifSourceDirection;
                bitmapBody.EndInit();

                ImageBehavior.SetAnimatedSource(imgDirection, bitmapBody);
            }
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
            if (uniformGridLevels.Visibility == Visibility.Visible)
            {
                uniformGridLevels.Visibility = Visibility.Hidden;
            }
            else if (gridProfiles.Visibility == Visibility.Visible)
            {
                gridProfiles.Visibility = Visibility.Hidden;
            }
            else
            {
                dataGridScoreboard.Visibility = Visibility.Hidden;
            }

            btnBack.Visibility = Visibility.Hidden;
            gridMenu.Visibility = Visibility.Visible;
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
                    btnProfiles.Content = "Profily";
                    btnScoreboard.Content = "Žebříček";
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
            uniformGridLevels.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Visible;
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            LevelRestart();
        }

        private void ShowScoreboard(object sender, RoutedEventArgs e)
        {
            dataGridScoreboard.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Visible;

            gridMenu.Visibility = Visibility.Hidden;
        }

        private void NewProfile(object sender, RoutedEventArgs e)
        {
            ProfilesManager.ProfileAdd(txtBoxNewProfile.Text, uniformGridProfiles);
        }

        private void ProfileNameCheck(object sender, TextChangedEventArgs e)
        {
            TextBox txtBoxSender = sender as TextBox;

            if (!string.IsNullOrWhiteSpace(txtBoxSender.Text))
            {
               btnCreate.IsEnabled = true;
            }
            else
            {
                if (btnCreate.IsEnabled)
                {
                    btnCreate.IsEnabled = false;
                }
            }
        }

        private void ShowProfiles(object sender, RoutedEventArgs e)
        {
            gridProfiles.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Visible;

            gridMenu.Visibility = Visibility.Hidden;
        }

        private void StartLevel(object sender, RoutedEventArgs e)
        {
            string lvlClickedOn = (string)((Button)sender).Content;
            lvl = lvlClickedOn;

            gridMenu.Visibility = Visibility.Hidden;

            if (levelInProgress == "none")
            {
                LevelStart();
            }
            else if (levelInProgress == lvlClickedOn)
            {
                LevelResume();
            }
            else
            {
                LevelSwitch();
            }
        }
    }
}
