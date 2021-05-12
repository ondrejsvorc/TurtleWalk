using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;
using TurtleWalk.ClassConstants;
using TurtleWalk;

namespace TurtleWalk.ClassLevelBuilder
{
    sealed class LevelBuilder
    {
        private StreamReader _reader;
        private Grid _gridLvl;

        private readonly string _path;
        private readonly string _lvl;

        public List<Image> Images { get; private set; }

        public LevelBuilder(string path, string lvl, Grid gridLvl)
        {
            _path = path;
            _lvl = lvl;
            _gridLvl = gridLvl;
        }

        // Proměnné zaniknou společně s funkcí - nepotřebujeme o nich znát nadále žádné informace
        public void BuildLevel()
        {
            Image Element;
            BitmapImage bitmapBody;

            Uri source = null;

            Images = new List<Image>();

            _reader = new StreamReader(_path);

            while (!_reader.EndOfStream)
            {
                string[] attributes = _reader.ReadLine().Split(' ');

                switch (attributes[0])
                {
                    case "Turtle":
                        source = new Uri(Constants.PATH_DIRECTION_FORWARD);
                        break;

                    case "Piston":
                        source = new Uri(Constants.PISTON, UriKind.Relative);
                        break;

                    case "Leaf":
                        source = new Uri(Constants.LEAF, UriKind.Relative);
                        break;

                    case "Background":
                        source = new Uri($"./Resources/Levels/Level{_lvl}/Background/background_lvl{_lvl}.jpg", UriKind.Relative);
                        attributes[1] = Convert.ToString(1300);
                        attributes[2] = Convert.ToString(750);
                        break;

                        // zatím jen pro lvl01
                    case "SavingPlatform":
                        source = new Uri($"./Resources/Levels/Level{_lvl}/Platforms/ice_platform_2.png", UriKind.Relative);
                        break;

                        // zatím jen pro lvl01
                    case "Ground":
                        if (attributes[1] == "670")
                        {
                            source = new Uri($"./Resources/Levels/Level{_lvl}/Platforms/ice_platform_2.png", UriKind.Relative);
                        }
                        else
                        {
                            source = new Uri($"./Resources/Levels/Level{_lvl}/Platforms/ice_platform_1.png", UriKind.Relative);
                        }
                        break;

                    case "LavaDrop":
                        source = new Uri(Constants.LAVA_DROP, UriKind.Relative);
                        break;

                    case "Sign":
                        source = new Uri(Constants.SIGN, UriKind.Relative);
                        break;
                }

                Element = new Image
                {
                    Width = Convert.ToDouble(attributes[1]),
                    Height = Convert.ToDouble(attributes[2]),
                    Margin = new Thickness(Convert.ToDouble(attributes[3]), Convert.ToDouble(attributes[4]), 0, 0),
                    Source = new BitmapImage(source),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };

                if (attributes[0] == "Turtle" || attributes[0] == "Piston" || attributes[0] == "Leaf")
                {
                    bitmapBody = new BitmapImage();
                    bitmapBody.BeginInit();
                    bitmapBody.UriSource = source;
                    bitmapBody.EndInit();

                    ImageBehavior.SetAnimatedSource(Element, bitmapBody);
                }

                Images.Add(Element);

                if (attributes[0] != "Background" && attributes[0] != "Piston" && attributes[0] != "Sign" && attributes[0] != "LavaDrop")
                {
                    Panel.SetZIndex(Element, 1);
                }
                else
                {
                    Panel.SetZIndex(Element, 0);
                }

                _gridLvl.Children.Add(Element);
            }

            _reader.Close();
        }
    }
}
