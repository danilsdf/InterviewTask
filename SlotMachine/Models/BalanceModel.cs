namespace SlotMachine.Models;

// The request will contain the amount to be added to the player balance
public class BalanceModel
{
    public string UserName { get; set; }
    public int Amount { get; set; }
}