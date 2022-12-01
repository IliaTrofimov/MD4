using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MD4_app.ViewModels
{
    public class AboutWindowViewModel
    {
        public string BuildVersion { get; set; }
        public string BuildDate { get; set; }

        public AboutWindowViewModel()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            BuildVersion = fvi.FileVersion;
            BuildDate = System.IO.File.GetCreationTime(Assembly.GetEntryAssembly().Location).ToString("dd.MM.yyyy");
        }
    }
}
