using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TurtleWalk.ClassGround;

namespace TurtleWalk.ClassCollisionElement
{
    abstract class CollisionElement                                         // třída by měla být abstraktní - nelze si představit, co přesně CollisionElement je
    {
        public Rect HitBox;
        public Image Body;

        public CollisionElement(Rect hitBox)
        {
            HitBox = new Rect(hitBox.X, hitBox.Y, hitBox.Width, hitBox.Height);
        }

        public static Rect GetHitBox(Image img)
        {
            return new Rect(img.Margin.Left, img.Margin.Top, img.Width, img.Height);
        }
    }
}
