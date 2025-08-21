using System.Threading.Tasks;
using WeatherConsole.Models;

namespace WeatherConsole.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherResponse?> GetWeatherAsync(string cityName);
    }
}