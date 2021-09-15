using System;
using SampleApplication.Controllers;
using Serilog;

namespace SampleApplication
{
    public class WeatherForecast
    {

        private static Serilog.ILogger _logger = Log.ForContext<WeatherForecast>();
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; } = null!;
    }
}
