using System.Windows;
using System.Windows.Controls;
using TurtleWalk.ClassCollisionElement;

namespace TurtleWalk.ClassTurtle
{
    class Turtle
    {
        public bool IsMoving;
        public bool IsDirectionForward;

        public Image Body;

        private Rect _hitBox;

        private double _y;

        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                Body.Margin = new Thickness(_x, _y, 0, 0);
                HitBoxUpdate();
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
                HitBoxUpdate();
            }
        }

        public Turtle(Rect hitBox, double x, double y)
        {
            _hitBox = hitBox;

            _x = x;
            _y = y;

            IsMoving = true;
            IsDirectionForward = true;
        }

        private void HitBoxUpdate()
        {
            _hitBox = new Rect(_x, _y, Body.Width, Body.Height);
            _hitBox.Inflate(-(Body.Width / 3), -30);
        }

        public bool CheckCollisionWith(CollisionElement collisionElement)
        {
            bool result = false;

            if (_hitBox.IntersectsWith(collisionElement.HitBox))
            {
                result = true;
            }

            return result;
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
