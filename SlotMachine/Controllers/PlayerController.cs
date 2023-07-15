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

            var config = _configuration.Find(_ => true).FirstOrDefault();

            player.Balance -= model.Bet;

            var result = config.GenerateRandomMatrix();
            var totalWin = 0; // todo calculate win amount

            player.Balance += totalWin;
            await _players.ReplaceOneAsync(p => p.Id == player.Id, player);

            var spinResult = new SpinResult(result, totalWin, player.Balance);

            return Ok(spinResult);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBalance(BalanceModel model)
        {
            var player = _players.Find(p => p.UserName == model.UserName).FirstOrDefault();

            //The amount will be added to the player balance and committed to DB
            player.Balance += model.Amount;
            await _players.ReplaceOneAsync(p => p.Id == player.Id, player);

            return Ok();
        }
    }
}
