using I2C.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace verne.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LedController : ControllerBase
    {
        private readonly ILogger<LedController> _logger;
        private readonly ILedStrip _ledStrip;

        public LedController(ILogger<LedController> logger, ILedStrip ledStrip)
        {
            _logger = logger;
            _ledStrip = ledStrip;
        }

        [HttpGet]
        public IEnumerable<Led> Get()
        {
            return _ledStrip.Leds;
        }

        [HttpGet("{id}")]
        public int Get(int id)
        {
            return _ledStrip[id];
        }

        [HttpPost("{id}/{brightness}")]
        public void HttpPost(int id, int brightness)
        {
            _logger.LogInformation($"Setting {id} to {brightness}");
            _ledStrip[id] = (byte)brightness;
            _logger.LogInformation(_ledStrip.ToString());
        }

        [HttpPost]
        public IEnumerable<Led> HttpPost([FromBody] Led[] leds)
        {
            foreach (var led in leds)
            {
                _ledStrip[led.Id] = led.Brightness;
            }
            return leds;
        }
    }
}
