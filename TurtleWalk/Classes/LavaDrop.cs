using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TurtleWalk.ClassCollisionElement;

namespace TurtleWalk.ClassLavaDrop
{
    sealed class LavaDrop : CollisionElement
    {
        private static double _startY;

        public LavaDrop(Rect lavaDropHitBox, double y) : base(lavaDropHitBox)
        {
            _startY = y;
        }

        public void Fall()
        {
            double marginTop = Body.Margin.Top + 25;

            Body.Margin = new Thickness(Body.Margin.Left, marginTop, Body.Margin.Right, Body.Margin.Bottom);
            HitBox = new Rect(Body.Margin.Left, marginTop, Body.Width, Body.Height);
        }

        public static void ResetPositions(List<LavaDrop> lavaDrops)
        {
            foreach (LavaDrop lavaDrop in lavaDrops)
            {
                lavaDrop.Body.Margin = new Thickness(lavaDrop.Body.Margin.Left, _startY, 0, 0);
                lavaDrop.HitBox = new Rect(lavaDrop.Body.Margin.Left, _startY, lavaDrop.Body.Width, lavaDrop.Body.Height);
            }
        }
    }
}
