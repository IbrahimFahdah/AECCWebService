using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AECCWebAPI;
using McMaster.NETCore.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace AECCWebAPI
{
}
public class Startup
{
    IHostingEnvironment _env;
    public Startup(IConfiguration configuration, IHostingEnvironment env)
    {
        Configuration = configuration;
        _env = env;


    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var mvcBuilder = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        services.AddMvc().AddJsonOptions(options =>
        {
            options.SerializerSettings.Formatting = Formatting.Indented;
        });


        LoadPlugins(services);

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        //app.UseStaticFiles();
        app.UseMvc();

    }

    private void LoadPlugins(IServiceCollection services)
    {
        var mvcBuilder = services.AddMvc();

        var libDicPath = Path.Combine(_env.WebRootPath, "libs");
//#if DEBUG
//           libDicPath =@"C:\Users\ixf.SCI\Desktop\OneDrive\SciJobRelated\Repos\AECC\Published\wwwroot\Libs";
//#endif
        
        if (!Directory.Exists(libDicPath))
        {
            throw new Exception("Libs folder was not found.");
        }

        string[] directories = Directory.GetDirectories(libDicPath);
        foreach (var dir in directories)
        {
            var pluginName = Path.GetFileName(dir);

            var plugin = PluginLoader.CreateFromAssemblyFile(
                Path.Combine(dir, pluginName + ".dll"), // create a plugin from for the .dll file
                config =>
                    // this ensures that the version of MVC is shared between this app and the plugin
                    config.PreferSharedTypes = true);

            var pluginAssembly = plugin.LoadDefaultAssembly();
            Console.WriteLine($"Loading application parts from plugin {pluginName}");

            // This loads MVC application parts from plugin assemblies
            var partFactory = ApplicationPartFactory.GetApplicationPartFactory(pluginAssembly);
            foreach (var part in partFactory.GetApplicationParts(pluginAssembly))
            {
                Console.WriteLine($"* {part.Name}");
                mvcBuilder.PartManager.ApplicationParts.Add(part);
            }

            // This piece finds and loads related parts, such as MvcAppPlugin1.Views.dll.
            var relatedAssembliesAttrs = pluginAssembly.GetCustomAttributes<RelatedAssemblyAttribute>();
            foreach (var attr in relatedAssembliesAttrs)
            {
                ////======
                //if(attr.AssemblyFileName.Contains($"{pluginName}.Views"))
                //{
                //    //IXF: to avoid changing the plugin to 'Class Library' 
                //    //See https://github.com/natemcmaster/DotNetCorePlugins/issues/21
                //    continue;
                //}
                ////\=====
                //==== IXF: not need to bother if the assembly doesn't exist (e.g. plugin.views.dll)
                if (!File.Exists(Path.Combine(dir, attr.AssemblyFileName + ".dll")))
                    continue;
                ////\=====

                var assembly = plugin.LoadAssembly(attr.AssemblyFileName);
                partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
                foreach (var part in partFactory.GetApplicationParts(assembly))
                {
                    Console.WriteLine($"  * {part.Name}");
                    mvcBuilder.PartManager.ApplicationParts.Add(part);
                }
            }

            //==ixf: register plugin services
            var plugInServicesType = pluginAssembly.GetTypes().FirstOrDefault(t => t.Name == "PlugInServices");
            if (plugInServicesType != null)
            {
                var plugInServicesIntsance = pluginAssembly.CreateInstance(plugInServicesType.FullName);
                MethodInfo methodInfo = plugInServicesType.GetMethod("RegisteredPlugInServices");
                if (methodInfo != null)
                {
                    methodInfo.Invoke(plugInServicesIntsance, new object[] { services });
                }

                //load additional related parts (e.g. views) referenced by the plugin assembly 
                methodInfo = plugInServicesType.GetMethod("GetAddAssemblies");
                if (methodInfo != null)
                {
                    string[] lst = (string[])methodInfo.Invoke(plugInServicesIntsance, null);
                    foreach (var ass in lst)
                    {
                        var assembly = plugin.LoadAssembly(ass);
                        partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
                        foreach (var part in partFactory.GetApplicationParts(assembly))
                        {
                            Console.WriteLine($"  * {part.Name}");
                            mvcBuilder.PartManager.ApplicationParts.Add(part);
                        }
                    }
                }


            }

            //\\===
        }
    }


}

