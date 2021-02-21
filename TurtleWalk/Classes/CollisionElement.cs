﻿using System.Windows;
using System.Windows.Controls;

namespace TurtleWalk
{
    public abstract class CollisionElement                                         // třída by měla být abstraktní - nelze si představit, co přesně CollisionElement je
    {
        public Rect HitBox;

        protected CollisionElement(Rect HitBox)
        {
            this.HitBox = new Rect(HitBox.X, HitBox.Y, HitBox.Width, HitBox.Height);
        }

        public static Thickness GetMargin(Image img)
        {
            return img.Margin;
        }

        public static Rect SetHitBox(Image img)
        {
            return new Rect(img.Margin.Left, img.Margin.Top, img.Width, img.Height);
        }
    }
}