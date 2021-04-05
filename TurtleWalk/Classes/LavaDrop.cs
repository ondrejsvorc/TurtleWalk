using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System;

namespace TurtleWalk
{
    sealed class LavaDrop : CollisionElement
    {
        private const double _marginTop = -70;

        public LavaDrop(Rect lavaDropHitBox) : base(lavaDropHitBox)
        {
        }

        public static void Fall(LavaDrop lavaDrop, double marginTop)
        {
            marginTop += 25;

            lavaDrop.Body.Margin = new Thickness(lavaDrop.Body.Margin.Left, marginTop, lavaDrop.Body.Margin.Right, lavaDrop.Body.Margin.Bottom);
            lavaDrop.HitBox = new Rect(lavaDrop.Body.Margin.Left, marginTop, lavaDrop.Body.Width, lavaDrop.Body.Height);
        }

        // THERE MIGHT BE PROBLEMS
        public static void ResetPositions(List<LavaDrop> lavaDrops)
        {
            foreach (LavaDrop lavaDrop in lavaDrops)
            {
                lavaDrop.Body.Margin = new Thickness(lavaDrop.Body.Margin.Left, _marginTop, 0, 0);
                //lavaDrop.Hitbox ?
            }
        }
    }
}
