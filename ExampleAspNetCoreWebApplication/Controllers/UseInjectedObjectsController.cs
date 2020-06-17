using ExampleAspNetCoreWebApplication.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExampleAspNetCoreWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UseInjectedObjectsController : ControllerBase
    {
        private readonly ILogger<UseInjectedObjectsController> _logger;
        private readonly DoThing _doThing;
        private readonly ReadConfiguration _readConfiguration;

        public UseInjectedObjectsController(ILogger<UseInjectedObjectsController> logger, DoThing doThing, ReadConfiguration readConfiguration)
        {
            _logger = logger;
            _doThing = doThing;
            _readConfiguration = readConfiguration;
        }

        [HttpGet]
        public string Get()
        {
            _logger.LogInformation("Both ASP.Net service provider and compile-time types are successfully injected!");

            return $"{_doThing()}\n{_readConfiguration()}";
        }
    }
}
