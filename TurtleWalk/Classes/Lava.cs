using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TurtleWalk.ClassCollisionElement;
using TurtleWalk.ClassTurtle;

namespace TurtleWalk.ClassLava
{
    sealed class Lava : CollisionElement
    {
        private static List<Rect> _hitBoxes = new List<Rect>();

        public Lava(Rect lavaHitBox, double InflationWidth, double InflationHeight) : base(lavaHitBox)
        {
            lavaHitBox.Inflate(InflationWidth, InflationHeight);   // Pro větší přirozenost HitBoxu
            _hitBoxes.Add(lavaHitBox);
        }

        public static bool CheckCollision(Turtle turtle)
        {
            bool result = false;

            foreach (Rect lavaHitBox in _hitBoxes)
            {
                if (turtle.HitBox.IntersectsWith(lavaHitBox))
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
            _hitBoxes = new List<Rect>();
        }
    }
}
