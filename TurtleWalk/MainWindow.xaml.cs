﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TurtleWalk.ClassBtn;
using TurtleWalk.ClassBulet;
using TurtleWalk.ClassCollisionElement;
using TurtleWalk.ClassConstants;
using TurtleWalk.ClassGround;
using TurtleWalk.ClassLava;
using TurtleWalk.ClassLavaDrop;
using TurtleWalk.ClassLeaf;
using TurtleWalk.ClassLevelBuilder;
using TurtleWalk.ClassLevelManager;
using TurtleWalk.ClassPiston;
using TurtleWalk.ClassProfilesManager;
using TurtleWalk.ClassSavingPlatform;
using TurtleWalk.ClassScoreboardManager;
using TurtleWalk.ClassSign;
using TurtleWalk.ClassTurtle;
using TurtleWalk.Enemies;
using WpfAnimatedGif;

namespace TurtleWalk
{
    public partial class MainWindow : Window
    {
        private Turtle turtle;
        private SavingPlatform savingPlatform;
        private Sign finishSign;

        private DispatcherTimer timer;

        private string lvl;

        private int scoreCount;

        private int ticks;

        private string levelInProgress;

        private int clickCountDirection, clickCountMovement;

        private int[] collisionPlatform;
        private int timeElapsed;

        private ProfilesManager profilesManager;
        private LevelsManager levelsManager;
        private ScoreboardManager scoreboardManager;

        private List<Enemy> enemies;
        private List<LavaDrop> lavaDrops;
        private List<CollisionElement> collisionElements;

        public MainWindow()
        {
            InitializeComponent();
            Setup();

            window.KeyDown += MovePlatform;
            window.KeyDown += LevelStop;
        }

        private async Task Setup()
        {
            if (!Directory.Exists(Constants.APP_LOCATION))
            {
                Directory.CreateDirectory(Constants.APP_LOCATION);
            }

            if (!File.Exists(Constants.PROFILES_LOCATION))
            {
                File.Create(Constants.PROFILES_LOCATION);
            }

            lavaDrops = new List<LavaDrop>();
            enemies = new List<Enemy>();
            collisionElements = new List<CollisionElement>();

            levelsManager = new LevelsManager(uniformGridLevels);
            profilesManager = new ProfilesManager(gridProfiles, gridMenu, uniformGridProfiles, uniformGridLevels, btnBack, lbProfile);
            scoreboardManager = new ScoreboardManager(dataGridScoreboard);

            await profilesManager.ProfilesGet();
            levelsManager.ReadAvailableLevelsOfGuest();

            scoreboardManager.DataGet(profilesManager.Profiles);
            scoreboardManager.DataSet();

            clickCountDirection = 0;
            clickCountMovement = 0;

            collisionPlatform = new int[4];

            timeElapsed = 0;
            ticks = 0;

            levelInProgress = "none";
        }

        private void LevelStart()
        {
            levelInProgress = lvl;

            string currentLevelPath = $"./Resources/Levels/Level{lvl}/Start/start_lvl{lvl}.txt";

            LevelBuilder builder = new LevelBuilder(currentLevelPath, lvl, gridLvl);
            builder.BuildLevel();

            using (StreamReader reader = new StreamReader(currentLevelPath))
            {
                double entityWidth;
                double entityHeight;

                double entityMarginLeft;
                double entityMarginTop;

                string[] rowProperties;

                foreach (Image image in builder.Images)
                {
                    rowProperties = reader.ReadLine().Split(' ');

                    entityWidth = Convert.ToDouble(rowProperties[1]);
                    entityHeight = Convert.ToDouble(rowProperties[2]);

                    entityMarginLeft = Convert.ToDouble(rowProperties[3]);
                    entityMarginTop = Convert.ToDouble(rowProperties[4]);

                    CollisionElement collisionElement = new CollisionElement();

                    if (image.Width == entityWidth && image.Height == entityHeight && image.Margin.Left == entityMarginLeft && image.Margin.Top == entityMarginTop)
                    {
                        switch (rowProperties[0])
                        {
                            case "Turtle":
                                turtle = new Turtle(CollisionElement.GetHitBox(image), entityMarginLeft, entityMarginTop);
                                turtle.Body = image;
                                break;

                            case "SavingPlatform":
                                savingPlatform = new SavingPlatform(entityMarginLeft);
                                savingPlatform.Body = image;
                                break;

                            case "Sign":
                                finishSign = new Sign(CollisionElement.GetHitBox(image));
                                break;

                            case "Ground":
                                Ground ground;
                                try
                                {
                                    ground = new Ground(CollisionElement.GetHitBox(image), Convert.ToDouble(rowProperties[5]), Convert.ToDouble(rowProperties[6]), Convert.ToBoolean(rowProperties[7]));
                                    ground.Body = image;
                                    ground.X = Convert.ToDouble(rowProperties[3]);
                                    ground.Y = Convert.ToDouble(rowProperties[4]);
                                }
                                catch
                                {
                                    ground = new Ground(CollisionElement.GetHitBox(image), Convert.ToDouble(rowProperties[5]), Convert.ToDouble(rowProperties[6]));
                                }
                                break;

                            case "Leaf":
                                collisionElement = new Leaf(CollisionElement.GetHitBox(image));
                                break;

                            case "Piston":
                                Piston piston = new Piston(CollisionElement.GetHitBox(image), Convert.ToDouble(rowProperties[5]), Convert.ToDouble(rowProperties[6]));
                                break;

                            case "LavaDrop":
                                LavaDrop lavaDrop = new LavaDrop(CollisionElement.GetHitBox(image), image.Margin.Top);
                                lavaDrop.Body = image;
                                lavaDrops.Add(lavaDrop);
                                break;

                            case "Lava":
                                collisionElement = new Lava(CollisionElement.GetHitBox(image), Convert.ToDouble(rowProperties[5]), Convert.ToDouble(rowProperties[6]));
                                break;

                            case "Button":
                                image.Tag = Convert.ToInt32(rowProperties[7]);
                                Btn btn = new Btn(image);
                                break;

                            case "FlyingEnemy":
                                Enemy enemy = new FlyingEnemy(CollisionElement.GetHitBox(image), Convert.ToDouble(rowProperties[5]), gridLvl);
                                enemy.Body = image;
                                enemies.Add(enemy);
                                break;
                        }

                        collisionElement.Body = image;
                        collisionElements.Add(collisionElement);
                    }
                }
            }

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            timer.Tick += GameUpdate;
            timer.Start();

            gridLvl.Visibility = Visibility.Visible;
        }

        // TATO METODA SE VOLÁ I PŘI SMRTI ŽELVY, TÍM PÁDEM SE VYKRESLUJE CELEJ LEVEL ZNOVA ZBYTEČNĚ - OŠETŘIT TO TAK, ABY SE DALA ŽELVA A PLOŠINKA NA ZAČÁTEK
        // + ABY SE VYKRESLILY ZNOVA LISTY, TY TOTIŽ MŮŽEME V PRŮBĚHU HRY SEŽRAT A PAK BY TAM CHYBĚLI
        // ALE ABY SE TAM VYKRESLOVALI ZNOVA GROUND APOD. TO JE ZBYTEČNÝ
        private void LevelResetValues()
        {
            if (timer != null)
            {
                timer.Stop();
            }

            Ground.NullValues();
            Piston.NullHitBoxes();
            Lava.NullHitBoxes();

            Bullet.BulletsList.Clear();

            enemies = new List<Enemy>();
            lavaDrops = new List<LavaDrop>();
            collisionElements = new List<CollisionElement>();

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

        private void LevelFinish()
        {
            if (profilesManager.CurrentProfile != null)
            {
                int lvlScoreIndex = Convert.ToInt32(lvl.Substring(1, 1)) - 1;

                if (scoreCount > profilesManager.CurrentProfile.ScoreList[lvlScoreIndex])
                {
                    profilesManager.CurrentProfile.ScoreList[lvlScoreIndex] = scoreCount;
                }

                levelsManager.UpdateAvailableLevelsOfProfile(lvl, profilesManager.CurrentProfile);
                levelsManager.SaveAvailableLevelsForProfiles(profilesManager.Profiles);

                scoreboardManager.DataUpdate();
                scoreboardManager.DataSave(profilesManager.Profiles);
            }
            else
            {
                levelsManager.SaveAvailableLevelsForGuest(lvl);
                levelsManager.ReadAvailableLevelsOfGuest();
            }

            LevelResetValues();

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
                foreach (Leaf leaf in collisionElements.OfType<Leaf>())
                {
                    if (turtle.CheckCollisionWith(leaf))
                    {
                        leaf.HitBox = Rect.Empty;

                        gridLvl.Children.Remove(leaf.Body);
                        lbScore.Content = $"Score: {scoreCount += 10}";
                        break;
                    }
                }

                // ŽELVIČKA DOKONČILA LEVEL
                if (turtle.CheckCollisionWith(finishSign))
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
            if (!Ground.CheckCollision(turtle) && !Piston.CheckCollision(turtle))
            {
                turtle.X += 2;
                turtle.Y += 8;

                if (Lava.CheckCollision(turtle))
                {
                    LevelRestart();
                }
            }

            if (!IsListEmpty(enemies.OfType<FlyingEnemy>()))
            {
                ticks++;

                foreach (FlyingEnemy enemy in enemies.OfType<FlyingEnemy>())
                {
                    if (ticks % 30 == 0)
                    {
                        enemy.IsShooting = true;
                        enemy.ShootBullet(turtle);

                        ticks = 0;
                    }

                    enemy.StayAheadOfTurtle(turtle);
                }
            }

            if (!IsListEmpty(Bullet.BulletsList))
            {
                Bullet.CheckCollision(savingPlatform);

                if (Bullet.CheckCollisionWith(turtle))
                {
                    LevelRestart();
                }
            }

            if (!IsListEmpty(lavaDrops))
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
                // 2000 / 30 = 66 ticks
                // 66 * 30 = 2000 ms = 2s

                if (timeElapsed >= 0 && timeElapsed <= 66)
                {
                    if (!collisionPlatform.Contains(1))
                    {
                        lavaDrops[0].Fall();
                    }

                    if (!collisionPlatform.Contains(2))
                    {
                        lavaDrops[1].Fall();
                    }

                    if (!collisionPlatform.Contains(3))
                    {
                        lavaDrops[2].Fall();
                    }
                }
                else if (timeElapsed > 66)
                {
                    LavaDrop.ResetPositions(lavaDrops);
                    timeElapsed = 0;
                }

                foreach (LavaDrop lavaDrop in lavaDrops)
                {
                    if (turtle.CheckCollisionWith(lavaDrop))
                    {
                        LevelRestart();
                    }
                }
            }
        }

        private bool IsListEmpty<T>(IEnumerable<T> list)
        {
            return list.Count() == 0;
        }

        private void LevelStop(object sender, KeyEventArgs e)
        {
            if (levelInProgress != "none")
            {
                if (e.Key == Key.Escape && gridLvl.Visibility == Visibility.Visible)
                {
                    timer.Stop();

                    gridLvl.Visibility = Visibility.Hidden;
                    gridMenu.Visibility = Visibility.Visible;
                }
            }
        }

        private void MovePlatform(object sender, KeyEventArgs e)
        {
            if (savingPlatform != null)
            {
                bool userNotInMenu = gridLvl.Visibility == Visibility.Visible;

                if (userNotInMenu)
                {
                    double step = 0;

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
                            if (savingPlatform.X < 675)
                            {
                                step = 25;
                            }
                            break;
                    }

                    savingPlatform.X += step;
                }
            }
        }

        private void TurtleChangeDirection(object sender, MouseButtonEventArgs e)
        {
            // CLICKCOUNTDIRECTION NABÝVÁ POUZE HODNOT -1; 0, 1, PŘIČEMŽ ROZHODUJE NA ZÁKLADĚ SUDÉ 0 A LICHÉ 1

            if (++clickCountDirection % 2 > 0)   // lichý klik (první, třetí, pátý, ...)
            {
                imgDirection.Cursor = Constants.CURSOR_GRABBED;

                lbDirection.Content = "Forward";
                turtle.IsDirectionForward = false;
                clickCountDirection = -1;       // resetování hodnoty, takže při dalším kroku to je sudé (tedy 0) - nezahlcujeme paměť
            }
            else                                // sudý klik (druhý, čtvrtý, šestý, ...)
            {
                imgDirection.Cursor = Constants.CURSOR_HAND;

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
                imgTurtleState.Cursor = Constants.CURSOR_GRABBED;

                lbState.Content = "Walk";
                turtle.IsMoving = false;
                clickCountMovement = -1;       // resetování hodnoty, takže při dalším kroku to je sudé (tedy 0) - nezahlcujeme paměť
            }
            else                               // sudý klik (druhý, čtvrtý, šestý, ...)
            {
                imgTurtleState.Cursor = Constants.CURSOR_HAND;

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

            btnSender.Cursor = Constants.CURSOR_GRABBED;
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
            ((Button)sender).Cursor = Constants.CURSOR_HAND;
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
            switch ((sender as Image).Name)
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
            ((Image)sender).Cursor = Constants.CURSOR_GRABBED;
        }

        private void FlagColorBackToNormal(object sender, MouseEventArgs e)
        {
            ((Image)sender).Opacity = 1;
            ((Image)sender).Cursor = Constants.CURSOR_HAND;
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            if (profilesManager.CurrentProfile != null)
            {
                levelsManager.SetAvailableLevelsByProfile(profilesManager.CurrentProfile);
            }

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

        private async void NewProfile(object sender, RoutedEventArgs e)
        {
            if (!profilesManager.ProfileExists(txtBoxNewProfile.Text))
            {
                profilesManager.ProfileAdd(txtBoxNewProfile.Text);
            }
            else
            {
                txtBlockMessage.Text = "Tento profil byl již vytvořen";
                await Task.Delay(2000);
                txtBlockMessage.Text = string.Empty;
            }
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

        private void DisableSpacesInProfileName(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
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
