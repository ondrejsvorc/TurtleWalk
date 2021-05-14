using System.Windows;
using System.Windows.Controls;

namespace TurtleWalk.ClassTurtle
{
    sealed class Turtle
    {
        public Rect HitBox;

        public bool IsMoving;
        public bool IsDirectionForward;

        public Image Body;

        private double _y;

        public double Y
        {
            get => _y;
            set
            {
                _y = value;

                if (_turtle != null)
                {
                    _turtle.Body.Margin = new Thickness(_x, _y, 0, 0);
                }
            }
        }

        private double _x;

        public double X
        {
            get => _x;
            set
            {
                _x = value;

                if (_turtle != null)
                {
                    _turtle.Body.Margin = new Thickness(_x, _y, 0, 0);
                }
            }
        }

        private Turtle _turtle;

        public Turtle(Rect HitBox, double x, double y)
        {
            this.HitBox = HitBox;

            _x = x;
            _y = y;

            IsMoving = true;
            IsDirectionForward = true;

            _turtle = this;
        }

        public void HitBoxUpdate()
        {
            _turtle.HitBox = new Rect(_x, _y, _turtle.Body.Width, _turtle.Body.Height);
            _turtle.HitBox.Inflate(-(_turtle.Body.Width / 3), -30);
        }
    }
}
