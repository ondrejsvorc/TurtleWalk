using System.Collections.Generic;
using System.Windows;
using TurtleWalk.ClassCollisionElement;
using TurtleWalk.ClassTurtle;

namespace TurtleWalk.ClassGround
{
    sealed class Ground : CollisionElement
    {
        // READONLY - HODNOTY MUSÍME NASTAVIT BUĎ PŘI INICIALIZACI, NEBO V KONSTRUKTORU
        // CONST - HODNOTY MUSÍME NASTAVIT PŘI INICIALIZACI (NÁZVY PROMĚNNÝCH TOHOTO TYPU PÍŠEME VELKÝMI PÍSMENY, ODDĚLUJEME PODTRŽÍTKEM)
        private static List<Rect> _hitBoxes = new List<Rect>();

        public static List<Ground> MovableGrounds { get; private set; } = new List<Ground>();

        private double _y;
        public double Y
        {
            get => _y;
            set 
            {
                _y = value;

                if (Body != null)
                {
                    Body.Margin = new Thickness(_x, _y, 0, 0);
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

                if (Body != null)
                {
                    Body.Margin = new Thickness(_x, _y, 0, 0);
                }
            }
        }

        public Ground(Rect groundHitBox, double inflationWidth, double inflationHeight) : base(groundHitBox)
        {
            groundHitBox.Inflate(inflationWidth, inflationHeight);   // Pro větší přirozenost HitBoxu
            _hitBoxes.Add(groundHitBox);
        }

        private bool _movable;

        public Ground(Rect groundHitBox, double inflationWidth, double inflationHeight, bool movable) : base(groundHitBox)
        {
            _movable = movable;

            groundHitBox.Inflate(inflationWidth, inflationHeight);   // Pro větší přirozenost HitBoxu

            MovableGrounds.Add(this);
            _hitBoxes.Add(groundHitBox);
        }

        public static bool CheckCollision(Turtle turtle)
        {
            bool result = true;

            foreach (Rect groundHitBox in _hitBoxes)
            {
                if (turtle.HitBox.IntersectsWith(groundHitBox))
                {
                    result = true;
                    break;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public static void NullValues()
        {
            _hitBoxes = new List<Rect>();


        }

        //public static bool CheckCollisionBetween(Turtle turtle, Ground ground)
        //{
        //    if (turtle.HitBox.IntersectsWith(ground.HitBox))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
