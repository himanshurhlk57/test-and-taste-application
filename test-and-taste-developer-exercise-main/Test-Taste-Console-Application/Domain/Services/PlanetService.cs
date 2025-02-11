using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.DataTransferObjects;
using Test_Taste_Console_Application.Domain.DataTransferObjects.JsonObjects;
using Test_Taste_Console_Application.Domain.Objects;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test_Taste_Console_Application.Domain.Services
{

public class MoonService : IMoonService
{
    private readonly HttpClient _httpClient;

    public MoonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Moon>> GetMoonsForPlanetAsync(string planetName)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"moons?planet={planetName}");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var moonDtos = JsonSerializer.Deserialize<List<MoonDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return moonDtos?.ConvertAll(m => new Moon
            {
                Name = m.Name,
                Temperature = m.Temperature
            }) ?? new List<Moon>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching moons for {planetName} from API: {ex.Message}");
            return new List<Moon>();
        }
    }
}

}
