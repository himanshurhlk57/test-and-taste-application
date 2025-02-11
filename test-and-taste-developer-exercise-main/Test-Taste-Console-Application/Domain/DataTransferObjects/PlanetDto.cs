using System.Collections.Generic;
using Newtonsoft.Json;

namespace Test_Taste_Console_Application.Domain.DataTransferObjects
{

public class PlanetDto
{
    public string Name { get; set; }
    public List<MoonDto> Moons { get; set; } = new List<MoonDto>();
}
}