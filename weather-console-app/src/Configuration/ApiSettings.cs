namespace WeatherConsole.Configuration
{
    public class ApiSettings
    {
        // Base URL for Open-Meteo geocoding (no key required)
        public string GeocodingBaseUrl { get; set; } = "https://geocoding-api.open-meteo.com/v1/";
        // Base URL for Open-Meteo forecast (no key required)
        public string ForecastBaseUrl { get; set; } = "https://api.open-meteo.com/v1/";
    }
}