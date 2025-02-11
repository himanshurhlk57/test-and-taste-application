using System;
using System.Collections.Generic;
using System.Linq;
namespace Test_Taste_Console_Application.Domain.Services
{
public class PlanetProcessingService
{
    public List<PlanetTemperatureInfo> GetPlanetsWithMoonTemperatures(List<Planet> planets)
    {
        return planets
            .Where(p => p.HasMoons())
            .Select(p => new PlanetTemperatureInfo
            {
                Name = p.Name,
                AverageMoonTemperature = p.GetAverageMoonTemperature()
            })
            .ToList();
    }
}

public class PlanetTemperatureInfo
{
    public string Name { get; set; }
    public double AverageMoonTemperature { get; set; }
}
}