using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MD4_app.ViewModels
{
    public class AboutWindowViewModel
    {
        public string BuildVersion { get; set; }
        public string BuildDate { get; set; }

        public AboutWindowViewModel()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().MainModule is not null)
            {
                try
                {
                    var exe = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    BuildVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(exe).ProductVersion;
                    BuildDate = new FileInfo(exe).CreationTime.ToString("dd.MM.yyyy");
                }
                catch
                {
                }
                
            }
        }
    }
}
