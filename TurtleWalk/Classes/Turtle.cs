using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace TurtleWalk
{
    public class Turtle
    {
        public Rect HitBox;

        public bool IsMoving;
        public bool IsDirectionForward;

        public double DistanceFromStart;
        public double SeaLevel;

        public Image Body;
        private BitmapImage bitmapBody;

        public Turtle()
        {
            //SeaLevel = Convert.ToDouble(MainWindow.Attributes[4]);
            IsMoving = true;
            IsDirectionForward = true;
            DistanceFromStart = 0;

            //Body = new Image
            //{
            //    Width = Convert.ToDouble(MainWindow.Attributes[1]),
            //    Height = Convert.ToDouble(MainWindow.Attributes[2]),
            //    Margin = new Thickness(Convert.ToDouble(MainWindow.Attributes[3]), Convert.ToDouble(MainWindow.Attributes[4]), 0, 0),
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    VerticalAlignment = VerticalAlignment.Top
            //};

            //bitmapBody = new BitmapImage();
            //bitmapBody.BeginInit();
            //bitmapBody.UriSource = new Uri("./Resources/Images/Turtle/turtle_direction_forward.gif", UriKind.Relative);
            //bitmapBody.EndInit();

            //ImageBehavior.SetAnimatedSource(Body, bitmapBody);
        }

        public static Thickness Move(Image imgTurtle, double x, double y)
        {
            return imgTurtle.Margin = new Thickness(x, y, 0, 0);
        }

        public static Thickness DontMove(Image imgTurtle)
        {
            return imgTurtle.Margin;
        }

        public static void HitBoxUpdate(Turtle turtle, Image turtleImg)
        {
            turtle.HitBox = new Rect(turtleImg.Margin.Left, turtleImg.Margin.Top, turtleImg.Width, turtleImg.Height);
            turtle.HitBox.Inflate(-turtleImg.Width / 3, -15);
        }
    }
}
