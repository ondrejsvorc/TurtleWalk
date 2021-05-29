using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TurtleWalk.ClassBtn;
using TurtleWalk.ClassCollisionElement;
using TurtleWalk.ClassTurtle;

namespace TurtleWalk.ClassGround
{
    class Ground : CollisionElement
    {
        // READONLY - HODNOTY MUSÍME NASTAVIT BUĎ PŘI INICIALIZACI, NEBO V KONSTRUKTORU
        // CONST - HODNOTY MUSÍME NASTAVIT PŘI INICIALIZACI (NÁZVY PROMĚNNÝCH TOHOTO TYPU PÍŠEME VELKÝMI PÍSMENY, ODDĚLUJEME PODTRŽÍTKEM)

        private static List<Rect> _staticGroundsHitBoxes = new List<Rect>();
        public static List<Ground> MovableGroundsList { get; private set; } = new List<Ground>();

        private bool _movable;

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

        public Ground(Rect groundHitBox, double inflationWidth, double inflationHeight, bool movable) : base(groundHitBox)
        {
            HitBox = groundHitBox;
            groundHitBox.Inflate(inflationWidth, inflationHeight);   // Pro větší přirozenost HitBoxu

            _movable = movable;

            MovableGroundsList.Add(this);
        }

        public static bool CheckCollision(Turtle turtle)
        {
            bool result1 = false;
            bool result2 = false;

            foreach (Rect staticGround in _staticGroundsHitBoxes)
            {
                if (turtle.HitBox.IntersectsWith(staticGround))
                {
                    result1 = true;
                    break;
                }
                else
                {
                    result1 = false;
                }
            }

            foreach (Ground movableGround in MovableGroundsList)
            {
                if (turtle.HitBox.IntersectsWith(movableGround.HitBox))
                {
                    result2 = true;
                    break;
                }
                else
                {
                    result2 = false;
                }
            }

            return result1 || result2;
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
