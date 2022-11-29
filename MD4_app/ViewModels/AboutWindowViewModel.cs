using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD4_app.ViewModels
{
    public class AboutWindowViewModel
    {
        private readonly string buildVersion;
        public string BuildVersion => buildVersion;

        public AboutWindowViewModel()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            buildVersion = fvi.FileVersion;
        }
    }
}
