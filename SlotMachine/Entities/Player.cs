using MongoDB.Bson;

namespace SlotMachine.Entities;

public class Player
{
    public ObjectId Id { get; set; }
    public int Balance { get; set; }

    public Player(int balance)
    {
        Balance = balance;
    }
}