using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WeatherConsole.Configuration;
using WeatherConsole.Interfaces;
using WeatherConsole.Models;

namespace WeatherConsole.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _http;
        private readonly ApiSettings _settings;

        public WeatherService(HttpClient http, IOptions<ApiSettings> settings)
        {
            _http = http;
            _settings = settings.Value;
        }

        public async Task<WeatherResponse?> GetWeatherAsync(string city)
        {
            var geo = await GeocodeAsync(city);
            if (geo == null)
            {
                return null;
            }

            var forecast = await GetCurrentForecastAsync(geo.Value.lat, geo.Value.lon);
            if (forecast == null)
            {
                return null;
            }

            return new WeatherResponse
            {
                City = geo.Value.name,
                Temperature = forecast.Value.temperature,
                Condition = MapWeatherCode(forecast.Value.code)
            };
        }

        private async Task<(string name, double lat, double lon)?> GeocodeAsync(string city)
        {
            var url = $"{_settings.GeocodingBaseUrl}search?name={Uri.EscapeDataString(city)}&count=1";
            using var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return null;

            using var stream = await resp.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);
            if (!doc.RootElement.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
                return null;

            var first = results[0];
            var name = first.GetProperty("name").GetString() ?? city;
            var lat = first.GetProperty("latitude").GetDouble();
            var lon = first.GetProperty("longitude").GetDouble();
            return (name, lat, lon);
        }

        private async Task<(double temperature, int code)?> GetCurrentForecastAsync(double lat, double lon)
        {
            var url = $"{_settings.ForecastBaseUrl}forecast?latitude={lat}&longitude={lon}&current=temperature_2m,weather_code";
            using var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return null;

            using var stream = await resp.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);
            if (!doc.RootElement.TryGetProperty("current", out var current))
                return null;

            if (!current.TryGetProperty("temperature_2m", out var tempProp) ||
                !current.TryGetProperty("weather_code", out var codeProp))
                return null;

            var temp = tempProp.GetDouble();
            var code = codeProp.GetInt32();
            return (temp, code);
        }

        private static string MapWeatherCode(int code)
        {
            // Mapping per Open-Meteo weather codes
            return code switch
            {
                0 => "Clear sky",
                1 => "Mainly clear",
                2 => "Partly cloudy",
                3 => "Overcast",
                45 or 48 => "Fog",
                51 or 53 or 55 => "Drizzle",
                56 or 57 => "Freezing drizzle",
                61 or 63 or 65 => "Rain",
                66 or 67 => "Freezing rain",
                71 or 73 or 75 => "Snowfall",
                77 => "Snow grains",
                80 or 81 or 82 => "Rain showers",
                85 or 86 => "Snow showers",
                95 => "Thunderstorm",
                96 or 99 => "Thunderstorm with hail",
                _ => $"Unknown (code {code})"
            };
        }
    }
}