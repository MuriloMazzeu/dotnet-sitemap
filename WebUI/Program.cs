using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebUI
{
    public class Program
    {
        public static void Main(string[] args) => 
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(host =>
                {
                    host.UseStartup<Startup>();
                });

            return builder;
        }
    }
}
