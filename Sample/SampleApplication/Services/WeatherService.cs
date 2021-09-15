using System;
using System.Collections.Generic;
using System.Linq;
using BoilerplateFree;

namespace SampleApplication.Services
{
    [AddSerilog]
    public partial class WeatherService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.Information("Hello from SERILOG WeatherService!!");
            
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}