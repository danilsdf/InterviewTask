using MongoDB.Bson;

namespace SlotMachine.Entities;

public class Player
{
    public ObjectId Id { get; set; }
    public string UserName { get; set; }
    public int Balance { get; set; }

    public Player(int balance, string userName)
    {
        Balance = balance;
        UserName = userName;
    }
}