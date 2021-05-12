using System.Windows;
using System.Windows.Controls;

namespace TurtleWalk.ClassCollisionElement
{
    abstract class CollisionElement                                         // třída by měla být abstraktní - nelze si představit, co přesně CollisionElement je
    {
        public Rect HitBox;
        public Image Body;

        public CollisionElement(Rect HitBox)
        {
            this.HitBox = new Rect(HitBox.X, HitBox.Y, HitBox.Width, HitBox.Height);
        }

        public static Rect SetHitBox(Image img)
        {
            return new Rect(img.Margin.Left, img.Margin.Top, img.Width, img.Height);
        }
    }
}
