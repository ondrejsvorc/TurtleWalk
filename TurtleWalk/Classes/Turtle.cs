using System.Windows;
using System.Windows.Controls;

namespace TurtleWalk
{
    public class Turtle
    {
        public Rect HitBox;

        public bool IsMoving;
        public bool IsDirectionForward;

        public double DistanceFromStart;
        public double SeaLevel;

        public Turtle()
        {
            SeaLevel = 830;
            IsMoving = true;
            IsDirectionForward = true;
            DistanceFromStart = 0;
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
