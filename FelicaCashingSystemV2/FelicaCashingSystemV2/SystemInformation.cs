using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2
{
    class SystemInformation
    {
        static SystemInformation()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version.ToString();
            var name = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(
                assembly, typeof(System.Reflection.AssemblyTitleAttribute))).Title;
            var copyright = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(
                assembly, typeof(AssemblyCopyrightAttribute))).Copyright;

            SystemInformation.version = version;
            SystemInformation.appName = name;
            SystemInformation.copyright = copyright;
        }

        private static readonly string version;
        public static string Version
        {
            get { return SystemInformation.version; }
        }


        private static readonly string appName;
        public static string AppName
        {
            get { return SystemInformation.appName; }
        }

        private static readonly string copyright;
        public static string Copyright
        {
            get { return SystemInformation.copyright; }
        }
    }
}
