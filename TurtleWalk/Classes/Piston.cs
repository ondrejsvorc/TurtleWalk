using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TurtleWalk
{
    public sealed class Piston : CollisionElement
    {
        private static readonly List<Rect> hitBoxes = new List<Rect>();

        public Piston(Rect pistonHitBox) : base(pistonHitBox)
        {
            hitBoxes.Add(pistonHitBox);
        }

        public static bool CheckCollision(Turtle turtle)
        {
            bool result = true;

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
    }
}
