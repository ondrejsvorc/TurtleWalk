using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TurtleWalk
{
    public class CollisionElement                                         // třída by měla být abstraktní - nelze si představit, co přesně CollisionElement je
    {
        public Rect HitBox;
        public Image Body;

        public CollisionElement(Uri source)
        {
            //Body = new Image
            //{
            //    Width = Convert.ToDouble(MainWindow.Attributes[1]),
            //    Height = Convert.ToDouble(MainWindow.Attributes[2]),
            //    Margin = new Thickness(Convert.ToDouble(MainWindow.Attributes[3]), Convert.ToDouble(MainWindow.Attributes[4]), Convert.ToDouble(MainWindow.Attributes[5]), Convert.ToDouble(MainWindow.Attributes[6])),
            //    Source = new BitmapImage(source),
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    VerticalAlignment = VerticalAlignment.Top
            //};

            this.HitBox = new Rect(Body.Margin.Left, Body.Margin.Top, Body.Width, Body.Height);

            Panel.SetZIndex(Body, 1);                   // pak vyřešit v souboru (ne vždy to bude 1)
            MainWindow.GridLvl.Children.Add(Body);
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
