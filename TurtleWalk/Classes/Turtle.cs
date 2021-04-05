using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace TurtleWalk
{
    sealed class Turtle
    {
        public Rect HitBox;

        public bool IsMoving;
        public bool IsDirectionForward;

        public double DistanceFromStart;
        public double SeaLevel;

        public bool WasMoving;

        public Image Body;

        public Turtle(Rect HitBox)
        {
            // POUZE PRO lvl01
            // SeaLevel nastavovat pro každý level zvlášť
            SeaLevel = 830;
            IsMoving = true;
            IsDirectionForward = true;
            DistanceFromStart = 30;

            this.HitBox = HitBox;
        }

        public static Thickness Move(Turtle turtle, double x, double y)
        {
            return turtle.Body.Margin = new Thickness(x, y, 0, 0);
        }

        public static Thickness DontMove(Turtle turtle)
        {
            return turtle.Body.Margin;
        }

        public static void HitBoxUpdate(Turtle turtle)
        {
            turtle.HitBox = new Rect(turtle.Body.Margin.Left, turtle.Body.Margin.Top, turtle.Body.Width, turtle.Body.Height);
            turtle.HitBox.Inflate(-turtle.Body.Width / 3, -15);
        }

        public static Thickness GoToBeginning(Turtle turtle)
        {
            return turtle.Body.Margin = new Thickness(30, 830, 0, 0);
        }
    }
}
