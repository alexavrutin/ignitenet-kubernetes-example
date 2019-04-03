using Apache.Ignite.Core;
using Apache.Ignite.Core.Common;
using Apache.Ignite.Core.Discovery.Tcp;
using Apache.Ignite.Core.Discovery.Tcp.Static;
using Apache.Ignite.Log4Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtilityClick.ProductService
{
    public static class Initializer
    {
        /// <summary>
        /// Contains initialisation routines for the whole Product Service.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public async static Task Init(IServiceCollection serviceCollection)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true) // read settings from the JSON file
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables() // and the environment variables
                .Build();

            // Configure Apache Ignite
            var igniteConfig = new IgniteConfiguration
            {
                // Setup an Ingite node discovery mechanism for the K8s environment
                // Logger = new IgniteLog4NetLogger(),
                JvmOptions = new List<string>() {
                    "-Djava.net.preferIPv4Stack=true",
                    "-Xms512m",
                    "-Xmx512m",
                    "-DIGNITE_PERFORMANCE_SUGGESTIONS_DISABLED=true",
                    "-DIGNITE_QUIET=false" },
                UserAttributes = new Dictionary<string, object>()
                {
                    { "service", Program.SERVICE_NAME }
                }
            };

            Console.WriteLine("KUBERNETES configuration is active.");

            Console.WriteLine();
            Console.WriteLine("Directories in the working directory:");
            foreach (var dir in System.IO.Directory.GetDirectories(AppContext.BaseDirectory))
            {
                Console.WriteLine(dir);
            }
            Console.WriteLine();
            Console.WriteLine("Files in the working directory:");
            foreach (var file in System.IO.Directory.GetFiles(AppContext.BaseDirectory))
            {
                Console.WriteLine(file);
            }
            Console.WriteLine();

            igniteConfig.SpringConfigUrl = "./kubernetes.config";
            igniteConfig.JvmClasspath = string.Join(";", System.IO.Directory.GetFiles(
                System.IO.Path.Combine(AppContext.BaseDirectory, "libs"),
                "*.jar"));

            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

            // Set up dependencies
            serviceCollection
                .AddSingleton(sp => Ignition.TryGetIgnite() ?? Ignition.Start(igniteConfig));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var ignite = serviceProvider.GetService<IIgnite>();
        }
    }
}
