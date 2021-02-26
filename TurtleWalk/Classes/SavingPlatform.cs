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
            //Body = new Image
            //{
            //    Width = Convert.ToDouble(MainWindow.Attributes[1]),
            //    Height = Convert.ToDouble(MainWindow.Attributes[2]),
            //    Margin = new Thickness(Convert.ToDouble(MainWindow.Attributes[3]), Convert.ToDouble(MainWindow.Attributes[4]), Convert.ToDouble(MainWindow.Attributes[5]), Convert.ToDouble(MainWindow.Attributes[6])),
            //    Source = new BitmapImage(new Uri($"./Resources/Levels/Level01/Platforms/ice_platform_2.png", UriKind.Relative)),
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    VerticalAlignment = VerticalAlignment.Top
            //};
        }

        public static Thickness Move(Image imgPlatform, double marginLeft, double marginRight)
        {
            return imgPlatform.Margin = new Thickness(marginLeft, imgPlatform.Margin.Top, marginRight, imgPlatform.Margin.Bottom);
        }

        public static void HitBoxUpdate(SavingPlatform savingPlatform, Image imgSavingPlatform)
        {
            savingPlatform.HitBox = new Rect(imgSavingPlatform.Margin.Left, imgSavingPlatform.Margin.Top, imgSavingPlatform.Width, imgSavingPlatform.Height);
            savingPlatform.HitBox.Inflate(-10, -20);
        }

        //public static bool CheckCollisionBetween(SavingPlatform savingPlatform, LavaDrop lavaDrop)
        //{
        //    if (savingPlatform.HitBox.IntersectsWith(lavaDrop.HitBox))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
