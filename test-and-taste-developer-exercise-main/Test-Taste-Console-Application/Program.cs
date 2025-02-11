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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Test_Taste_Console_Application
{

class Program
{
    static async Task Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddHttpClient<IPlanetService, PlanetService>(client =>
                {
                    client.BaseAddress = new Uri("https://api.le-systeme-solaire.net"); 
                });

                services.AddHttpClient<IMoonService, MoonService>(client =>
                {
                    client.BaseAddress = new Uri("https://api.le-systeme-solaire.net"); 
                });

                services.AddSingleton<PlanetProcessingService>();
            })
            .Build();

        var planetService = host.Services.GetRequiredService<IPlanetService>();
        var moonService = host.Services.GetRequiredService<IMoonService>();
        var processingService = host.Services.GetRequiredService<PlanetProcessingService>();

        List<Planet> planets = await planetService.GetPlanetsAsync();

        foreach (var planet in planets)
        {
            planet.Moons = await moonService.GetMoonsForPlanetAsync(planet.Name);
        }

  
        List<PlanetTemperatureInfo> result = processingService.GetPlanetsWithMoonTemperatures(planets);

        Console.WriteLine("Planets with at least one moon and their moons' average temperature:");
        foreach (var planet in result)
        {
            Console.WriteLine($"{planet.Name}: {planet.AverageMoonTemperature:F2}°C");
        }
    }
}

}

