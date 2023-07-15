using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SlotMachine.Entities;
using SlotMachine.Models;
using SlotMachine.Responses;

namespace SlotMachine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SlotMachineController : Controller
    {
        private readonly IMongoCollection<Configuration> _configuration;

        public SlotMachineController(IMongoDatabase database)
        {
            _configuration = database.GetCollection<Configuration>("configuration");
        }

        [HttpPost]
        public async Task<ActionResult<SpinResult>> AddConfiguration(ConfigurationModel model)
        {
            if (model.Height <= 0 || model.Width < 0)
            {
                return BadRequest("Invalid input");
            }

            if (model.Height > model.Width)
            {
                return BadRequest("Width must be more or equal than height");
            }

            var config = _configuration.Find(_ => true).FirstOrDefault();
            if (config != null)
            {
                return BadRequest("Slot machine already configured");
            }

            var configuration = new Configuration(model.Width, model.Height);

            await _configuration.InsertOneAsync(configuration);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateConfiguration(ConfigurationModel model)
        {
            if (model.Height <= 0 || model.Width < 0)
            {
                return BadRequest("Invalid input");
            }

            if (model.Height > model.Width)
            {
                return BadRequest("Width must be more or equal than height");
            }

            var config = _configuration.Find(_ => true).FirstOrDefault();
            if (config == null)
            {
                return NotFound("Slot machine not configured yet");
            }

            if (model.Height == config.Height && model.Width == config.Width)
            {
                return Ok();
            }

            var configuration = new Configuration(model.Width, model.Height)
            {
                Id = config.Id
            };

            await _configuration.ReplaceOneAsync(p => p.Id == config.Id, configuration);

            return Ok();
        }
    }
}
