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

        private Image imgTurtle;
        private BitmapImage bitmapImgTurtle;

        public Turtle(string[] attributes, Grid gridLvl)
        {
            SeaLevel = 830;
            IsMoving = true;
            IsDirectionForward = true;
            DistanceFromStart = 0;

            imgTurtle = new Image
            {
                Width = Convert.ToDouble(attributes[1]),
                Height = Convert.ToDouble(attributes[2]),
                Margin = new Thickness(Convert.ToDouble(attributes[3]), Convert.ToDouble(attributes[4]), Convert.ToDouble(attributes[5]), Convert.ToDouble(attributes[6])),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            bitmapImgTurtle = new BitmapImage();
            bitmapImgTurtle.BeginInit();
            bitmapImgTurtle.UriSource = new Uri("./Resources/Images/Turtle/turtle_direction_forward.gif", UriKind.Relative);
            bitmapImgTurtle.EndInit();

            ImageBehavior.SetAnimatedSource(imgTurtle, bitmapImgTurtle);

            gridLvl.Children.Add(imgTurtle);
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
