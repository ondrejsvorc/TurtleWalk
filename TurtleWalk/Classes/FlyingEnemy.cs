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
using TurtleWalk.ClassLavaDrop;
using TurtleWalk.ClassTurtle;

namespace TurtleWalk.Classes
{
    // Bude vždy o kousek napřed před želvičkou, a bude na ni pálit malé kapky lávy (LavaDrop)
    // Pokud vyblokujeme plošinkou, odrazí se to pod stejným úhlem
    // Nahoře bude healthbar se životy FlyingEnemy
    // Level budeme moci dokončit až poté, co FlyingEnemy zahyne (do té doby: zátarasa)

    sealed class FlyingEnemy : Enemy
    {
        private double _distance;

        private Image _imgBullet;

        public FlyingEnemy(Rect hitBox, double distance) : base(hitBox)
        {
            _distance = distance;
        }

        public void StayAheadOfTurtle(Turtle turtle)
        {
            if (turtle.IsMoving)
            {
                Body.Margin = new Thickness(turtle.X + _distance, Y, 0, 0);
            }
        }

        // Bullet is now hidden under each enemy
        public override void Shoot(Turtle turtle, Grid grid)
        {
            if (grid.Children.Contains(_imgBullet))
            {
                grid.Children.Remove(_imgBullet);
            }

            _imgBullet = new Image()
            {
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Source = new BitmapImage(new Uri(Constants.LAVA_DROP, UriKind.Relative))
            };

            _bullet = new LavaDrop(CollisionElement.GetHitBox(_imgBullet), Y);
            _bullet.Body = _imgBullet;

            _bullet.Body.Margin = new Thickness(X + (2 * _bullet.Body.Width), Y + (2 * _bullet.Body.Height), 0, 0);

            grid.Children.Add(_bullet.Body);

            //_bullet.Body.Margin = ShootBullet(turtle.X, turtle.Y);
        }

        //private Thickness ShootBullet(double turtleX, double turtleY)
        //{
        //    double bulletX = _bullet.Body.Margin.Left;
        //    double bulletY = _bullet.Body.Margin.Top;
            
        //    if (bulletX != turtleX || bulletY != turtleY)
        //    {
        //        bulletX += 5;
        //        bulletY += 5;
        //    }

        //    return new Thickness(bulletX, bulletY, 0, 0);
        //}
    }
}
