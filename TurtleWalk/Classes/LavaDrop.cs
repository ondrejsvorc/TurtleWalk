//using System.Collections.Generic;
//using System.Linq;
//using System.Windows;
//using System.Windows.Controls;

//namespace TurtleWalk
//{
//    public sealed class LavaDrop /*: CollisionElement*/
//    {
//        private static readonly Dictionary<Thickness, Rect> dictionary = new Dictionary<Thickness, Rect>();

//        //public LavaDrop(Thickness startImgMargin, Rect startLavaDropHitBox) : base(startLavaDropHitBox)
//        //{
//        //    dictionary.Add(startImgMargin, startLavaDropHitBox);
//        //}

//        public static void Fall(Image imgLavaDrop, LavaDrop lavaDrop, double marginTop)
//        {
//            marginTop += 25;

//            imgLavaDrop.Margin = new Thickness(imgLavaDrop.Margin.Left, marginTop, imgLavaDrop.Margin.Right, imgLavaDrop.Margin.Bottom);
//            lavaDrop.HitBox = new Rect(imgLavaDrop.Margin.Left, marginTop, imgLavaDrop.Width, imgLavaDrop.Height);
//        }

//        public static void ResetPosition(Image imgLavaDrop, LavaDrop lavaDrop)
//        {
//            int i = 0;

//            switch (imgLavaDrop.Name)
//            {
//                case "imgLavaDrop2":
//                    i++;
//                    break;

//                case "imgLavaDrop3":
//                    i += 2;
//                    break;

//                case "imgLavaDrop4":
//                    i += 3;
//                    break;
//            }

//            imgLavaDrop.Margin = dictionary.ElementAt(i).Key;
//            lavaDrop.HitBox = dictionary.ElementAt(i).Value;
//        }
//    }
//}
