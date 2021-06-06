using System.Windows;
using System.Windows.Controls;
using TurtleWalk.ClassLavaDrop;

namespace TurtleWalk.ClassSavingPlatform
{
    sealed class SavingPlatform
    {
        public Rect HitBox { get; private set; }
        public Image Body;

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
            HitBox = new Rect(_x, Body.Margin.Top, Body.Width, Body.Height);
        }

        public bool CheckCollisionWith(LavaDrop lavaDrop)
        {
            if (HitBox.IntersectsWith(lavaDrop.HitBox))
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
