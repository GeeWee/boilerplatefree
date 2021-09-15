using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using Serilog;
using Serilog.Events;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace SampleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} <{SourceContext}> {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();


            try
            {
                logger.Warn("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .UseSerilog()
                .UseNLog()
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        // .ConfigureLogging(logging =>
        // {
        //     // logging.ClearProviders();
        //     logging.SetMinimumLevel(LogLevel.Trace);
        // });
        // .UseNLog();  // NLog: Setup NLog for Dependency injection
    }
}