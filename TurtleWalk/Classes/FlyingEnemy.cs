using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TurtleWalk.ClassBulet;
using TurtleWalk.ClassCollisionElement;
using TurtleWalk.ClassConstants;
using TurtleWalk.ClassLavaDrop;
using TurtleWalk.ClassTurtle;

namespace TurtleWalk.Enemies
{
    // Bude vždy o kousek napřed před želvičkou, a bude na ni pálit malé kapky lávy (LavaDrop)
    // Pokud vyblokujeme plošinkou, odrazí se to pod stejným úhlem
    // Nahoře bude healthbar se životy FlyingEnemy
    // Level budeme moci dokončit až poté, co FlyingEnemy zahyne (do té doby: zátarasa)

    sealed class FlyingEnemy : Enemy
    {
        private double _distance;

        public FlyingEnemy(Rect hitBox, double distance, Grid grid) : base(hitBox)
        {
            _distance = distance;
            _bullet = new Bullet(this, grid);
        }

        public void StayAheadOfTurtle(Turtle turtle)
        {
            if (turtle.IsMoving)
            {
                Body.Margin = new Thickness(turtle.X + _distance, Y, 0, 0);
            }

            if (!IsShooting)
            {
                _bullet.X = X;
                _bullet.Y = Y;
            }
        }

        public override void ShootBullet(Turtle turtle)
        {
            if (!_bullet.HasFinished)
            {
                _bullet.FlyToTurtle(turtle.X, turtle.Y, this);
            }
        }
    }
}
