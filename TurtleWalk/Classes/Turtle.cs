using System.Windows;
using System.Windows.Controls;

namespace TurtleWalk.ClassTurtle
{
    sealed class Turtle
    {
        public Rect HitBox;

        public bool IsMoving;
        public bool IsDirectionForward;

        public double DistanceFromStart;
        public double SeaLevel;

        public Image Body;

        public Turtle(Rect HitBox, double distanceFromStart, double seaLevel)
        {
            this.HitBox = HitBox;

            DistanceFromStart = distanceFromStart;
            SeaLevel = seaLevel;

            IsMoving = true;
            IsDirectionForward = true;
        }

        public static Thickness Move(Turtle turtle, double x, double y)
        {
            return turtle.Body.Margin = new Thickness(x, y, 0, 0);
        }

        public static void HitBoxUpdate(Turtle turtle)
        {
            turtle.HitBox = new Rect(turtle.Body.Margin.Left, turtle.Body.Margin.Top, turtle.Body.Width, turtle.Body.Height);
            turtle.HitBox.Inflate(-(turtle.Body.Width / 3), -30);
        }

        public static Thickness GoToBeginning(Turtle turtle)
        {
            return turtle.Body.Margin = new Thickness(30, 830, 0, 0);
        }
    }
}
