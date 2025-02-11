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

namespace Test_Taste_Console_Application
{
   using System;

public class Moon
{
    public string Name { get; set; }
    public double Mass { get; set; }
    public double Temperature { get; set; }

    public Moon(string name, double mass, double temperature)
    {
        Name = name;
        Mass = mass;
        Temperature = temperature;
    }

    public override string ToString()
    {
        return $"{Name} - Mass: {Mass}kg, Temperature: {Temperature}°C";
    }
}

public class Planet
{
    public string Name { get; set; }
    public double Mass { get; set; }
    public List<Moon> Moons { get; set; }

    public Planet(string name, double mass)
    {
        Name = name;
        Mass = mass;
        Moons = new List<Moon>();  
    }

    public void AddMoon(Moon moon)
    {
        Moons.Add(moon);
    }

    public double? AverageTemperatureOfMoons()
    {
        if (Moons.Count == 0)
        {
            return null;  
        }

        double totalTemp = 0;
        foreach (var moon in Moons)
        {
            totalTemp += moon.Temperature;
        }

        return totalTemp / Moons.Count;
    }

    public override string ToString()
    {
        return $"{Name} - Mass: {Mass}kg";
    }
}

public class Program
{
    
    public static async Task<List<Planet>> FetchPlanetsAsync(string apiUrl)
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync(apiUrl);
            var planets = JsonConvert.DeserializeObject<List<Planet>>(response);
            return planets;
        }
    }

    public static List<(Planet, double?)> ListPlanetsWithMoonsAndAvgTemperature(List<Planet> planets)
    {
        var planetsWithMoons = new List<(Planet, double?)>();

        foreach (var planet in planets)
        {
            if (planet.Moons.Count > 0) 
            {
                double? avgTemp = planet.AverageTemperatureOfMoons();
                planetsWithMoons.Add((planet, avgTemp));
            }
        }

        return planetsWithMoons;
    }

    public static async Task Main(string[] args)
    {
        string apiUrl = "https://api.le-systeme-solaire.net";

        try
        {
      
            var planets = await FetchPlanetsAsync(apiUrl);

            var planetsWithAvgTemp = ListPlanetsWithMoonsAndAvgTemperature(planets);

            foreach (var entry in planetsWithAvgTemp)
            {
                var planet = entry.Item1;
                var avgTemp = entry.Item2;
                Console.WriteLine($"{planet.Name} has an average moon temperature of {avgTemp}°C");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching data: {ex.Message}");
        }
    }
}

