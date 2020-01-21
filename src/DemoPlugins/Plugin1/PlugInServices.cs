using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Abstraction;
using Plugin.Abstraction.Controllers;
using Plugin.Abstraction.HelpTopic;

namespace AECC.Plugin1
{
    public class PlugInServices
    {
        readonly bool _standalone;
        public PlugInServices()
        {

        }

        public PlugInServices(bool standalone)
        {
            _standalone = standalone;
        }
        public void RegisteredPlugInServices(IServiceCollection services)
        {
            if (_standalone)
            {
                var assembly = typeof(PluginBaseController).Assembly;
                var myAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(AssemblyDirectory + "\\" + assembly.GetName().Name + ".Views.dll");
                var viewsApplicationPart = new CompiledRazorAssemblyPart(myAssembly);
                services.AddMvc().ConfigureApplicationPartManager(manager => manager.ApplicationParts.Add(viewsApplicationPart));
            }
            services.AddSingleton<HelpTopicsProvider>();
            services.AddSingleton<IHelpResourceProvider>(new HelpResourceProvider());
            services.AddScoped<Tracker>();
        }

        public string[] GetAddAssemblies()
        {
            var ass = typeof(PluginBaseController).Assembly;
            var viewAssembly = ass.GetName().Name + ".Views";
            return new[] { viewAssembly };
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