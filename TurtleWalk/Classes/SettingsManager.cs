using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleWalk
{
    // MOMENTÁLNĚ NEFUKNČNÍ, LZE ZPROVOZNIT... JINÝ PŘÍSTUP? (TENTO JIŽ OZKOUŠEN NA JINÝCH PROJEKTECH)

    public class SettingsManager
    {

        public SettingsManager()
        {
        }

        public void SettingsSave()
        {
            //Properties.Settings.Default.stateLvl2 = startMenu.btnLvl2.IsEnabled;
        }

        public void SettingsLoad()
        {
            //startMenu.btnLvl2.IsEnabled = Properties.Settings.Default.stateLvl2;
        }
    }
}
