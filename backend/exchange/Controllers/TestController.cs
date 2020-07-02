using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace exchange.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        public TestController(ILoggerManager logger)
        {
            _logger = logger;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogDebug("Test debug message from the controller");
            _logger.LogError("Test error message from the controller");
            _logger.LogWarning("Test warning message from the controller");
            _logger.LogInformation("Test information message from the controller");

            return new string[] { "value1", "value2" };
        }
    }
}
