using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class OsmService : IOsmService
    {
        private readonly HttpClient _httpClient;

        public OsmService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

        }

        public async Task<LocationData?> GetCoordinatesFromAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                Console.WriteLine("Invalid address input.");
                return null;
            }

            string formattedAddress = $"{address}, Vietnam";
            Console.WriteLine($"Fetching coordinates for: {formattedAddress}");

            string requestUrl = $"https://nominatim.openstreetmap.org/search?q={formattedAddress}&format=json";
            Console.WriteLine($"Final Request URL: {requestUrl}");

            try
            {
                await Task.Delay(1500); // Delay để tránh bị rate-limit
                Console.WriteLine($"Fetching coordinates for: {address}");

                var response = await _httpClient.GetStringAsync(requestUrl);
                Console.WriteLine($"API Response: {response}");

                if (string.IsNullOrWhiteSpace(response) || response == "[]")
                {
                    Console.WriteLine("No results found.");
                    return null;
                }

                var locationData = JsonSerializer.Deserialize<List<LocationData>>(response);
                if (locationData == null || !locationData.Any())
                {
                    Console.WriteLine("No valid coordinates found.");
                    return null;
                }

                var validLocation = locationData.FirstOrDefault(loc => loc.latitude.HasValue && loc.longitude.HasValue);
                if (validLocation != null)
                {
                    Console.WriteLine($"Valid Location: {validLocation.latitude}, {validLocation.longitude}");
                    return validLocation;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return null;
        }
    }
}
