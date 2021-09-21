
namespace SampleApplication.Controllers
{
    using System.Collections.Generic;
    using BoilerplateFree;
    using Microsoft.AspNetCore.Mvc;
    using SampleApplication.Services;
    
    [AddNLog]
    [AutoGenerateConstructor]
    [ApiController]
    [Route("[controller]")]
    public partial class WeatherForecastControllerInside : ControllerBase
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
