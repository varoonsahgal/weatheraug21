using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherConsole.Interfaces;
using WeatherConsole.Services;
using WeatherConsole.Configuration;
using WeatherConsole.Rendering;

namespace WeatherConsole
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.Configure<ApiSettings>(context.Configuration.GetSection("ApiSettings"));
                    services.AddHttpClient<IWeatherService, WeatherService>();
                })
                .Build();

            var weatherService = host.Services.GetRequiredService<IWeatherService>();

            Console.Write("Enter the city name: ");
            var city = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(city))
            {
                Console.WriteLine("City name is required.");
                return 1;
            }

            try
            {
                var weatherResponse = await weatherService.GetWeatherAsync(city.Trim());
                if (weatherResponse == null)
                {
                    Console.WriteLine("No weather data returned.");
                }
                else
                {
                    Console.WriteLine($"Weather in {weatherResponse.City}:");
                    Console.WriteLine($"Temperature: {weatherResponse.Temperature:F1}°C");
                    Console.WriteLine($"Condition: {weatherResponse.Condition}");
                    Console.WriteLine();
                    Console.WriteLine(AsciiWeatherArt.Get(weatherResponse.Condition));
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 2;
            }
        }
    }
}