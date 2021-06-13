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

        private static List<Rect> _staticGroundsHitBoxes = new List<Rect>();
        public static List<Ground> MovableGroundsList { get; private set; } = new List<Ground>();

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
            _staticGroundsHitBoxes.Add(groundHitBox);
        }

        public Ground(Rect groundHitBox, double inflationWidth, double inflationHeight, bool movable = true) : base(groundHitBox)
        {
            HitBox = groundHitBox;
            groundHitBox.Inflate(inflationWidth, inflationHeight);   // Pro větší přirozenost HitBoxu

            MovableGroundsList.Add(this);
        }

        public static bool CheckCollision(Turtle turtle)
        {
            bool result = false;

            foreach (Rect staticGroundHitBox in _staticGroundsHitBoxes)
            {
                if (turtle.CheckCollisionWith(staticGroundHitBox))
                {
                    result = true;
                    break;
                }
            }

            if (result)
            {
                return result;
            }
            else
            {
                foreach (Ground movableGroundHitBox in MovableGroundsList)
                {
                    if (turtle.CheckCollisionWith(movableGroundHitBox.HitBox))
                    {
                        result = true;
                        break;
                    }
                }

                return result;
            }
        }

        public static void NullValues()
        {
            _staticGroundsHitBoxes = new List<Rect>();
            MovableGroundsList = new List<Ground>();
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
