using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Forms;

namespace NasaBackgroundWPF.Utils
{
    public static class Installer
    {
        public static void SetAutaticStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rk.GetValue("NasaBackgroundWPF") == null)
            {
                rk.SetValue("NasaBackgroundWPF", System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }
    }
}
