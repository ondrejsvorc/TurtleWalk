using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TurtleWalk.ClassCollisionElement;
using TurtleWalk.ClassConstants;
using TurtleWalk.ClassSavingPlatform;
using TurtleWalk.ClassTurtle;
using TurtleWalk.Enemies;

namespace TurtleWalk.ClassBulet
{
    class Bullet
    {
        private double _x;

        public double X
        {
            get => _x;
            set
            {
                _x = value;
                _imgBullet.Margin = new Thickness(_x + (2 * _imgBullet.Width), _y + (2* _imgBullet.Height), 0, 0);
                HitBox = new Rect(_x, _y, _imgBullet.Width, _imgBullet.Height);
            }
        }

        private double _y;

        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                _imgBullet.Margin = new Thickness(_x + (2 * _imgBullet.Width), _y + (2 * _imgBullet.Height), 0, 0);
                HitBox = new Rect(_x, _y, _imgBullet.Width, _imgBullet.Height);
            }
        }

        private Image _imgBullet;

        private DispatcherTimer timer;

        public bool HasFinished;

        public Rect HitBox;

        public static List<Bullet> BulletsList = new List<Bullet>();

        private double _turtleX;
        private double _turtleY;

        private Enemy _enemy;

        public Bullet(Enemy enemy, Grid grid)
        {
            _x = enemy.X;
            _y = enemy.Y;

            HasFinished = false;

            _imgBullet = new Image()
            {
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Source = new BitmapImage(new Uri(Constants.LAVA_DROP, UriKind.Relative)),
                Margin = new Thickness(_x, _y, 0, 0)
            };

            HitBox = new Rect(_x, _y, _imgBullet.Width, _imgBullet.Height);
            BulletsList.Add(this);

            grid.Children.Add(_imgBullet);
        }

        public void Stop()
        {
            _enemy.IsShooting = false;
            HasFinished = true;
            HitBox = Rect.Empty;
        }

        public static bool CheckCollisionWith(Turtle turtle)
        {
            bool result = false;

            foreach (Bullet bullet in BulletsList)
            {
                if (turtle.CheckCollisionWith(bullet.HitBox))
                {
                    bullet.Stop();
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static void CheckCollision(SavingPlatform savingPlatform)
        {
            foreach (Bullet bullet in BulletsList)
            {
                if (savingPlatform.CheckCollisionWith(bullet.HitBox))
                {
                    bullet.Stop();
                    break;
                }
            }
        }

        public void FlyToTurtle(double turtleX, double turtleY, Enemy enemy)
        {
            _turtleX = turtleX;
            _turtleY = turtleY;

            _enemy = enemy;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += ShootingAnimation;
            timer.Start();
        }

        private void ShootingAnimation(object sender, EventArgs e)
        {
            if (_x != _turtleX && _y != _turtleY)
            {
                if (_x < _turtleX)
                {
                    X += 5;
                }
                else
                {
                    X -= 5;
                }

                if (_y < _turtleY)
                {
                    Y += 8.5;
                }
                else
                {
                    Y -= 8.5;
                }

                HitBox = new Rect(_x, _y, _imgBullet.Width, _imgBullet.Height);
            }
            else
            {
                Stop();
            }
        }
    }
}
