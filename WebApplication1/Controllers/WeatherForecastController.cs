using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HardIoCTests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Handlers;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DoThing _doThing;
        private readonly ReadConfiguration _readConfiguration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DoThing doThing, ReadConfiguration readConfiguration)
        {
            _logger = logger;
            _doThing = doThing;
            _readConfiguration = readConfiguration;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet]
        public string Get()
            => $"{_doThing()}\n{_readConfiguration()}";
    }
}
