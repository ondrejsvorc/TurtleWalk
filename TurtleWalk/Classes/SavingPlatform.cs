﻿using System.Windows;
using System.Windows.Controls;
using TurtleWalk.ClassLavaDrop;

namespace TurtleWalk.ClassSavingPlatform
{
    sealed class SavingPlatform
    {
        public Rect HitBox { get; private set; }
        public Image Body { get; set; }

        public SavingPlatform()
        {
        }

        public static Thickness Move(SavingPlatform savingPlatform, double marginLeft)
        {
            return savingPlatform.Body.Margin = new Thickness(marginLeft, savingPlatform.Body.Margin.Top, 0, 0);
        }

        public static void HitBoxUpdate(SavingPlatform savingPlatform)
        {
            savingPlatform.HitBox = new Rect(savingPlatform.Body.Margin.Left, savingPlatform.Body.Margin.Top, savingPlatform.Body.Width, savingPlatform.Body.Height);
        }

        public static bool CheckCollisionBetween(SavingPlatform savingPlatform, LavaDrop lavaDrop)
        {
            if (savingPlatform.HitBox.IntersectsWith(lavaDrop.HitBox))
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
