namespace WeatherConsole.Models
{
    public class WeatherResponse
    {
        public string City { get; set; } = string.Empty;
        public double Temperature { get; set; } // Celsius from Open-Meteo current.temperature_2m
        public string Condition { get; set; } = string.Empty; // Mapped from Open-Meteo weather_code
    }
}