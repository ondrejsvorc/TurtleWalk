using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TurtleWalk
{
    public sealed class SavingPlatform
    {
        public Rect HitBox;
        public Image Body;

        public SavingPlatform()
        {
        }

        //ERORR KDYŽ KLIKNU NA ŠIPKU V MENU
        public static Thickness Move(SavingPlatform savingPlatform, double marginLeft)
        {
            return savingPlatform.Body.Margin = new Thickness(marginLeft, savingPlatform.Body.Margin.Top, 0, 0);
        }

        public static void HitBoxUpdate(SavingPlatform savingPlatform)
        {
            savingPlatform.HitBox = new Rect(savingPlatform.Body.Margin.Left, savingPlatform.Body.Margin.Top, savingPlatform.Body.Width, savingPlatform.Body.Height);
            savingPlatform.HitBox.Inflate(-10, -20);
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
