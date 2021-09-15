using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoilerplateFree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using SampleApplication.Services;
using Serilog;

namespace SampleApplication.Controllers
{
    [AddNLog]
    [AutoGenerateConstructor]
    [ApiController]
    [Route("[controller]")]
    public partial class WeatherForecastController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.Warn("!!!!!!!!!!!!!Hello from NLOG weather forecast controller!!!");
            
            return _weatherService.Get();
        }
    }
}
