using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.Services;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace Test_Taste_Console_Application
{
class Program
{
    static async Task Main()
    {
        string apiUrl = "https://api.le-systeme-solaire.net"; 

        using HttpClient httpClient = new HttpClient();
        IPlanetService planetService = new PlanetService(httpClient, apiUrl);
        IMoonService moonService = new MoonService(httpClient, apiUrl);
        PlanetProcessingService processingService = new PlanetProcessingService();

        List<Planet> planets = await planetService.GetPlanetsAsync();

        // Fetch moon data for each planet
        foreach (var planet in planets)
        {
            planet.Moons = await moonService.GetMoonsForPlanetAsync(planet.Name);
        }

        List<PlanetTemperatureInfo> result = processingService.GetPlanetsWithMoonTemperatures(planets);

        if (result.Count == 0)
        {
            Console.WriteLine("No planets with moons found.");
            return;
        }

        Console.WriteLine("Planets with at least one moon and their moons' average temperature:");
        foreach (var planet in result)
        {
            Console.WriteLine($"{planet.Name}: {planet.AverageMoonTemperature:F2}°C");
        }
    }
}


}

