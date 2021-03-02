using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Remedium.Web
{
    public static class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}