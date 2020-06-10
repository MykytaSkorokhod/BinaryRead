using BinaryRead.Core;
using BinaryRead.Core.Services.Implementations;
using BinaryRead.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BinaryRead.EndPoint
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();

            if (args.Length != 0)
            {

            }
            else
            {
                var task = serviceProvider.GetService<App>().Start();
                task.Wait();
            }

            Console.ReadKey();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<App>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IOutputService, OutputService>();
            services.AddTransient<InputManager>();

            return services;
        }
    }
}
