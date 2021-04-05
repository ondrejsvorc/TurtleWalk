using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TurtleWalk
{
    sealed class Ground : CollisionElement
    {
        // READONLY - HODNOTY MUSÍME NASTAVIT BUĎ PŘI INICIALIZACI, NEBO V KONSTRUKTORU
        // CONST - HODNOTY MUSÍME NASTAVIT PŘI INICIALIZACI (NÁZVY PROMĚNNÝCH TOHOTO TYPU PÍŠEME VELKÝMI PÍSMENY, ODDĚLUJEME PODTRŽÍTKEM)
        private static readonly List<Rect> hitBoxes = new List<Rect>();

        public Ground(Rect groundHitBox, double InflationWidth, double InflationHeight) : base(groundHitBox)
        {
            groundHitBox.Inflate(InflationWidth, InflationHeight);   // Pro větší přirozenost HitBoxu
            hitBoxes.Add(groundHitBox);
        }

        public static bool CheckCollision(Turtle turtle)
        {
            bool result = true;

            foreach (Rect groundHitBox in hitBoxes)
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

        public static bool CheckCollisionBetween(Turtle turtle, Ground ground)
        {
            if (turtle.HitBox.IntersectsWith(ground.HitBox))
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
