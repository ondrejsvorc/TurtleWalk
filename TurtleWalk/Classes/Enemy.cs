using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TurtleWalk.ClassBulet;
using TurtleWalk.ClassLavaDrop;
using TurtleWalk.ClassTurtle;

namespace TurtleWalk.Enemies
{
    abstract class Enemy
    {
        public Rect HitBox { get; private set; }
        public int Hp;
        public bool IsShooting;
        public Image Body;

        protected Bullet _bullet;

        public double X => Body.Margin.Left;
        public double Y => Body.Margin.Top;

        public Enemy(Rect hitBox)
        {
            HitBox = new Rect(hitBox.X, hitBox.Y, hitBox.Width, hitBox.Height);
            IsShooting = false;
            Body = new Image();
        }

        public abstract void ShootBullet(Turtle turtle);
    }
}
