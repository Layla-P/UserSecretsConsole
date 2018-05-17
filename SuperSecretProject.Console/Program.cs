using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserSecrets.Console
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static void Main()
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == "development";
            //Determines the working environment as IHostingEnvironment is unavailable in a console app

            var builder = new ConfigurationBuilder();

            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //only add secrets in development
            if (isDevelopment) 
            {
                builder.AddUserSecrets<Program>();
            }

            Configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();

            //Map the implementations of your classes here ready for DI
            services
                .Configure<SecretStuff>(Configuration.GetSection(nameof(SecretStuff)))
                .AddOptions()
                .AddSingleton<ISecretRevealer, SecretRevealer>()
                .BuildServiceProvider();

            var serviceProvider = services.BuildServiceProvider();

            // Get the service you need - DI will handle any dependencies - in this case our User Secrets
            var revealer = serviceProvider.GetService<ISecretRevealer>();

            revealer.Reveal();

            System.Console.ReadKey();

        }
    }
}