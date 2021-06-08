using System.Windows;
using System.Windows.Controls;
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
            }
        }

        public SavingPlatform(double x)
        {
            _x = x;
        }

        public void HitBoxUpdate()
        {
            _hitBox = new Rect(_x, Body.Margin.Top, Body.Width, Body.Height);
        }

        public bool CheckCollisionWith(LavaDrop lavaDrop)
        {
            if (_hitBox.IntersectsWith(lavaDrop.HitBox))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
