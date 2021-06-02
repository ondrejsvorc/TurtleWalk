using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TurtleWalk.ClassLavaDrop;
using TurtleWalk.ClassTurtle;

namespace TurtleWalk.Enemies
{
    abstract class Enemy
    {
        public int Hp;
        public Image Body;
        public bool WasHit;
        public Rect HitBox;
        public static bool AreShooting = false;

        public double X => Body.Margin.Left;
        public double Y => Body.Margin.Top;

        public Enemy(Rect hitBox)
        {
            HitBox = new Rect(hitBox.X, hitBox.Y, hitBox.Width, hitBox.Height);
            Body = new Image();
        }

        public abstract void ShootBullet(Turtle turtle);
    }
}
