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

public class PlanetService : IPlanetService
{
    private readonly HttpClient _httpClient;

    public PlanetService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Planet>> GetPlanetsAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("planets");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var planetDtos = JsonSerializer.Deserialize<List<PlanetDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return planetDtos?.ConvertAll(MapPlanetDtoToDomain) ?? new List<Planet>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching planets from API: {ex.Message}");
            return new List<Planet>();
        }
    }

    private Planet MapPlanetDtoToDomain(PlanetDto dto)
    {
        return new Planet
        {
            Name = dto.Name,
            Moons = dto.Moons?.ConvertAll(m => new Moon
            {
                Name = m.Name,
                Temperature = m.Temperature
            }) ?? new List<Moon>()
        };
    }
}


}
