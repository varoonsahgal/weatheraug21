namespace WeatherConsole.Interfaces
{
    using System.Threading.Tasks;
    using WeatherConsole.Models;

    public interface IWeatherService
    {
        Task<WeatherResponse?> GetWeatherAsync(string city);
    }
}