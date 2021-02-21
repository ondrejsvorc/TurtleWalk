using System.Windows;
using System.Windows.Controls;

namespace TurtleWalk
{
    public sealed class SavingPlatform
    {
        public Rect HitBox;

        public static Thickness Move(Image imgPlatform, double marginLeft, double marginRight)
        {
            return imgPlatform.Margin = new Thickness(marginLeft, imgPlatform.Margin.Top, marginRight, imgPlatform.Margin.Bottom);
        }

        public static void HitBoxUpdate(SavingPlatform savingPlatform, Image imgSavingPlatform)
        {
            savingPlatform.HitBox = new Rect(imgSavingPlatform.Margin.Left, imgSavingPlatform.Margin.Top, imgSavingPlatform.Width, imgSavingPlatform.Height);
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
