using System.Collections.Generic;
using System.Windows;
using TurtleWalk.ClassCollisionElement;
using TurtleWalk.ClassTurtle;

namespace TurtleWalk.ClassPiston
{
    sealed class Piston : CollisionElement
    {
        private static List<Rect> hitBoxes = new List<Rect>();

        public Piston(Rect pistonHitBox, double InflationWidth, double InflationHeight) : base(pistonHitBox)
        {
            pistonHitBox.Inflate(InflationWidth, InflationHeight);   // Pro větší přirozenost HitBoxu
            hitBoxes.Add(pistonHitBox);
        }

        public static bool CheckCollision(Turtle turtle)
        {
            bool result = false;

            foreach (Rect pistonHitBox in hitBoxes)
            {
                if (turtle.HitBox.IntersectsWith(pistonHitBox))
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

        public static void NullHitBoxes()
        {
            hitBoxes = new List<Rect>();
        }
    }
}
