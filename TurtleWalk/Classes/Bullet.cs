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

        public Rect HitBox;

        private Image _imgBullet;

        private DispatcherTimer timer;

        public Bullet(Enemy enemy, Grid grid)
        {
            _x = enemy.X;
            _y = enemy.Y;

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

            grid.Children.Add(_imgBullet);
        }

        private double _turtleX;
        private double _turtleY;

        // different approach, this sucks, i guess
        public void FlyToTurtle(double turtleX, double turtleY)
        {
            _turtleX = turtleX;
            _turtleY = turtleY;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(25);
            timer.Tick += ShootingAnimation;
            timer.Start();
        }

        private void ShootingAnimation(object sender, EventArgs e)
        {
            if (X != _turtleX && Y != _turtleY)
            {
                if (X < _turtleX)
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
                    Y += 8.5;
                }
            }
            else
            {
                Enemy.AreShooting = false;
            }
        }
    }
}
