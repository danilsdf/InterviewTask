using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SlotMachine.Entities;
using SlotMachine.Models;

namespace SlotMachine.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IMongoCollection<Player> _players;

        public PlayerController(IMongoDatabase database)
        {
            _players = database.GetCollection<Player>("players");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBalance(BalanceModel model)
        {
            var player = _players.Find(p => p.Id == model.PlayerId).FirstOrDefault();

            //The amount will be added to the player balance and committed to DB
            player.Balance += model.Amount;
            await _players.ReplaceOneAsync(p => p.Id == player.Id, player);

            return Ok();
        }
    }
}
