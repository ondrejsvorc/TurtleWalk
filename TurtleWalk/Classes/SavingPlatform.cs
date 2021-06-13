using System.Windows;
using System.Windows.Controls;
using TurtleWalk.ClassCollisionElement;
using TurtleWalk.ClassLavaDrop;

namespace TurtleWalk.ClassSavingPlatform
{
    sealed class SavingPlatform
    {
        public Image Body;
        private Rect _hitBox;

        private double _x;

        public double X
        {
            get => _x;
            set
            {
                _x = value;
                Body.Margin = new Thickness(_x, Body.Margin.Top, 0, 0);
                _hitBox = new Rect(_x, Body.Margin.Top, Body.Width, Body.Height);
            }
        }

        public SavingPlatform(double x)
        {
            _x = x;
        }

        public bool CheckCollisionWith(CollisionElement collisionElement)
        {
            if (_hitBox.IntersectsWith(collisionElement.HitBox))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckCollisionWith(Rect hitBox)
        {
            bool result = false;

            if (_hitBox.IntersectsWith(hitBox))
            {
                result = true;
            }

            return result;
        }
    }
}
