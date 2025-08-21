using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WeatherConsole.Configuration;
using WeatherConsole.Interfaces;
using WeatherConsole.Models;

namespace WeatherConsole.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public WeatherService(HttpClient httpClient, IOptions<ApiSettings> apiOptions)
        {
            _httpClient = httpClient;
            _apiSettings = apiOptions.Value;
        }

        public async Task<WeatherResponse?> GetWeatherAsync(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName)) return null;
            if (string.IsNullOrWhiteSpace(_apiSettings.ApiKey))
                throw new InvalidOperationException("API key is missing. Set ApiSettings:ApiKey in configuration.");

            // Build query for OpenWeatherMap (metric units & simple fields)
            var url = $"{_apiSettings.BaseUrl}?q={Uri.EscapeDataString(cityName)}&appid={_apiSettings.ApiKey}&units=metric";

            try
            {
                var dto = await _httpClient.GetFromJsonAsync<OpenWeatherDto>(url);
                if (dto == null || dto.Main == null || dto.Weather == null || dto.Weather.Length == 0) return null;

                return new WeatherResponse
                {
                    City = dto.Name ?? cityName,
                    Temperature = dto.Main.Temp,
                    Condition = dto.Weather[0].Description ?? dto.Weather[0].Main ?? "Unknown"
                };
            }
            catch (HttpRequestException)
            {
                return null; // Network or 404
            }
        }

        // Internal DTOs for minimal OpenWeatherMap mapping
        private sealed class OpenWeatherDto
        {
            [JsonPropertyName("name")] public string? Name { get; set; }
            [JsonPropertyName("weather")] public WeatherPart[]? Weather { get; set; }
            [JsonPropertyName("main")] public MainPart? Main { get; set; }
        }

        private sealed class WeatherPart
        {
            [JsonPropertyName("main")] public string? Main { get; set; }
            [JsonPropertyName("description")] public string? Description { get; set; }
        }

        private sealed class MainPart
        {
            [JsonPropertyName("temp")] public double Temp { get; set; }
        }
    }
}