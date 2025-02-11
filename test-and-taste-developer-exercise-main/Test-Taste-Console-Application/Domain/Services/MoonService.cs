using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Text.Json;
using System.Threading.Tasks;

namespace Test_Taste_Console_Application.Domain.Services
{

public class MoonService : IMoonService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public MoonService(HttpClient httpClient, string apiBaseUrl)
    {
        _httpClient = httpClient;
        _apiBaseUrl = apiBaseUrl;
    }

    public async Task<List<Moon>> GetMoonsForPlanetAsync(string planetName)
    {
        try
        {
            string url = $"{_apiBaseUrl}/planets/{planetName}/moons";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            var moonDtos = JsonSerializer.Deserialize<List<MoonDto>>(json, new Json


}
