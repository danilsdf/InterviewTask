using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SlotMachine.Entities;
using SlotMachine.Extensions;
using SlotMachine.Models;
using SlotMachine.Responses;

namespace SlotMachine.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IMongoCollection<Player> _players;
        private readonly IMongoCollection<Configuration> _configuration;

        public PlayerController(IMongoDatabase database)
        {
            _configuration = database.GetCollection<Configuration>("configuration");
            _players = database.GetCollection<Player>("players");
        }

        [HttpPost]
        public async Task<ActionResult<SpinResult>> Spin(SpinModel model)
        {
            var player = _players.Find(p => p.UserName == model.UserName).FirstOrDefault();
            if (player == null)
            {
                return NotFound("Player not found");
            }

            if (player.Balance < model.Bet)
            {
                return BadRequest("Insufficient balance");
            }

            var config = _configuration.Find(_ => true).FirstOrDefault();
            if (config == null)
            {
                return NotFound("Slot machine not configured yet");
            }

            player.Balance -= model.Bet;

            var result = config.GenerateRandomMatrix();
            var totalWin = config.CalculateWinAmount(result, model.Bet);

            player.Balance += totalWin;
            await _players.ReplaceOneAsync(p => p.Id == player.Id, player);

            var spinResult = new SpinResult(result, totalWin, player.Balance);

            return Ok(spinResult);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBalance(BalanceModel model)
        {
            var player = _players.Find(p => p.UserName == model.UserName).FirstOrDefault();
            if (player == null)
            {
                return NotFound("Player not found.");
            }

            if (model.Amount < 0)
            {
                return BadRequest("Negative amount");
            }

            //The amount will be added to the player balance and committed to DB
            player.Balance += model.Amount;
            await _players.ReplaceOneAsync(p => p.Id == player.Id, player);

            return Ok();
        }
    }
}
