using System;
using System.IO;
using System.Reflection;

namespace Plugin.Abstraction.HelpTopic
{
    public class HelpResourceProvider:IHelpResourceProvider
    {
        private readonly string pathToHelpFolder;

        public HelpResourceProvider()
        {
            pathToHelpFolder = AssemblyDirectory + "\\resources\\help\\";
        }
        public object GetResource(string name)
        {
            var fileName=Path.Combine(pathToHelpFolder, name);
            if (!File.Exists(fileName))
                return null;

            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                file.CopyTo(ms);

            return ms.ToArray();
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}