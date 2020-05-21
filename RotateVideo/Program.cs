using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RotateVideo
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            if (args.Length == 0)
                return; // return if no file was dragged onto exe

            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            await serviceProvider.GetService<App>().Run(args[0]);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            // Add app
            serviceCollection.AddTransient<App>();
            serviceCollection.AddTransient<IReadMeta, ReadMeta>();
            serviceCollection.AddTransient<IRotate, Rotate>();
        }
    }
}
