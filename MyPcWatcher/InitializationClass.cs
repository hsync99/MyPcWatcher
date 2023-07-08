using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPcWatcher
{
    internal class InitializationClass
    {
        public InitializationClass()
        {

        }
        public bool SetAutoRunValue(bool isautorun, string path)
        {
            const string name = "MyPcWatcher";
            string ExePath = path;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (isautorun)
                {
                    reg.SetValue(name, ExePath);
                }
                else
                {
                    reg.DeleteValue(name);
                }
                reg.Flush();
                reg.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
