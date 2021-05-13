using System.Windows;
using System.Windows.Controls;
using TurtleWalk.ClassLavaDrop;

namespace TurtleWalk.ClassSavingPlatform
{
    sealed class SavingPlatform
    {
        public Rect HitBox { get; private set; }
        public Image Body;

        private SavingPlatform _savingPlatform;

        private double _x;

        public double X
        {
            get => _x;
            set
            {
                _x = value;

                if (_savingPlatform != null)
                {
                    _savingPlatform.Body.Margin = new Thickness(_x, _savingPlatform.Body.Margin.Top, 0, 0);
                }
            }
        }

        public SavingPlatform(double x)
        {
            _x = x;
            _savingPlatform = this;
        }

        public void HitBoxUpdate()
        {
            _savingPlatform.HitBox = new Rect(_x, _savingPlatform.Body.Margin.Top, _savingPlatform.Body.Width, _savingPlatform.Body.Height);
        }

        public bool CheckCollisionWith(LavaDrop lavaDrop)
        {
            if (_savingPlatform.HitBox.IntersectsWith(lavaDrop.HitBox))
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
