using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.DependencyInjection;

namespace AECCWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.ConfigureLogging(logging => logging.AddAzureWebAppDiagnostics())
                //.ConfigureServices(serviceCollection => serviceCollection.Configure<AzureFileLoggerOptions>(options =>
                //                                                                                            {
                //                                                                                                options.FileName = "azure-diagnostics-";
                //                                                                                                options.FileSizeLimit = 50 * 1024;
                //                                                                                                options.RetainedFileCountLimit = 5;
                //                                                                                            })).UseStartup<Startup>()
                .ConfigureLogging(
                    (hostingContext, logging) =>
                    {
                        // Providing an instrumentation key here is required if you're using
                        // standalone package Microsoft.Extensions.Logging.ApplicationInsights
                        // or if you want to capture logs from early in the application startup
                        // pipeline from Startup.cs or Program.cs itself.
                        var appInsightKey = hostingContext.Configuration["ApplicationInsightsInstrumentationkey"];
                        logging.AddApplicationInsights(appInsightKey);

                    }
                ).UseStartup<Startup>();

      
    }
}
