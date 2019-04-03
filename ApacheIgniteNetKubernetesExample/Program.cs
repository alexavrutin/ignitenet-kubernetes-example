using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;

namespace UtilityClick.ProductService
{
    class Program
    {
        public const string SERVICE_NAME = "ProductService";

        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-GB");

            /*
            // Wait until stopped

            var done = new ManualResetEventSlim(false);
            done.Wait();
            */

            // Run the web api 
            CreateWebHostBuilder(args).Build().Run();

            /*
            ignite.Dispose();
            */
        }


        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://0.0.0.0:5100")
                .UseStartup<ApiStartup>();
    }
}
