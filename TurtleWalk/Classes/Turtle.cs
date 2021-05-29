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
                Body.Margin = new Thickness(_x, _y, 0, 0);
            }
        }

        private double _x;

        public double X
        {
            get => _x;
            set
            {
                _x = value;
                Body.Margin = new Thickness(_x, _y, 0, 0);
            }
        }

        public Turtle(Rect hitBox, double x, double y)
        {
            HitBox = hitBox;

            _x = x;
            _y = y;

            IsMoving = true;
            IsDirectionForward = true;
        }

        public void HitBoxUpdate()
        {
            HitBox = new Rect(_x, _y, Body.Width, Body.Height);
            HitBox.Inflate(-(Body.Width / 3), -30);
        }
    }
}
