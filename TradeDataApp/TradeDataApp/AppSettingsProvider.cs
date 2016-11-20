using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TradeDataApp
{
    static class AppSettingsProvider 
    {
        public static int CheckingIntervalMs
        {
            get
            {
                return Properties.Settings.Default.CheckingInterval * 1000;
            }
        }

        public static string InputDirectory
        {
            get
            {
                return Properties.Settings.Default.InputDirectory;
            }
        }

        public static string PluginsDirectory
        {
            get
            {
                return Properties.Settings.Default.PluginsDirectory;
            }
        }

    }
}
