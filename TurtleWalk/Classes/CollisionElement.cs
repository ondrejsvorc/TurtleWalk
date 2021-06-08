using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TurtleWalk.ClassGround;
using TurtleWalk.ClassPiston;

namespace TurtleWalk.ClassCollisionElement
{
    class CollisionElement
    {
        public Rect HitBox;
        public Image Body;

        public CollisionElement(Rect hitBox)
        {
            HitBox = new Rect(hitBox.X, hitBox.Y, hitBox.Width, hitBox.Height);
        }

        public CollisionElement()
        {
        }

        public static Rect GetHitBox(Image img)
        {
            return new Rect(img.Margin.Left, img.Margin.Top, img.Width, img.Height);
        }
    }
}
