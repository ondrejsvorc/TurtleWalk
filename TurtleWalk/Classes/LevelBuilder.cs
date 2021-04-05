using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections;
using WpfAnimatedGif;

namespace TurtleWalk
{
    sealed class LevelBuilder
    {
        private StreamReader _reader { get; set; }
        private Grid _gridLvl { get; set; }
        private string _path { get; }
        private string _lvl { get; }

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

            int rowCount = CountRows();

            _reader = new StreamReader(_path);

            for (int i = 0; i < rowCount; i++)
            {
                string[] attributes = _reader.ReadLine().Split(' ');

                switch (attributes[0])
                {
                    case "Turtle":
                        source = new Uri("./Resources/Images/Turtle/turtle_direction_forward.gif", UriKind.Relative);
                        break;

                    case "Piston":
                        source = new Uri("./Resources/Images/Other/piston.gif", UriKind.Relative);
                        break;

                    case "Leaf":
                        source = new Uri("./Resources/Images/Points/leaf.gif", UriKind.Relative);
                        break;

                    case "Background":
                        source = new Uri($"./Resources/Levels/Level{_lvl}/Background/background_lvl{_lvl}.jpg", UriKind.Relative);
                        attributes[1] = SystemParameters.PrimaryScreenWidth.ToString();
                        attributes[2] = SystemParameters.PrimaryScreenHeight.ToString();
                        break;

                        // zatím jen pro lvl01
                    case "SavingPlatform":
                        source = new Uri($"./Resources/Levels/Level{_lvl}/Platforms/ice_platform_2.png", UriKind.Relative);
                        break;

                        // zatím jen pro lvl01
                    case "Ground":
                        if (attributes[1] == "1030")
                        {
                            source = new Uri($"./Resources/Levels/Level{_lvl}/Platforms/ice_platform_2.png", UriKind.Relative);
                        }
                        else
                        {
                            source = new Uri($"./Resources/Levels/Level{_lvl}/Platforms/ice_platform_1.png", UriKind.Relative);
                        }
                        break;

                    case "LavaDrop":
                        source = new Uri($"./Resources/Images/Deadly-Entities/lava_drop.png", UriKind.Relative);
                        break;

                    case "Sign":
                        source = new Uri($"./Resources/Images/Other/finish.png", UriKind.Relative);
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

        private int CountRows()
        {
            _reader = new StreamReader(_path);

            int i = 0;

            while (_reader.ReadLine() != null)
            {
                i++;
            }

            _reader.Close();

            return i;
        }
    }
}
