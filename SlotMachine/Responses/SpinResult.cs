namespace SlotMachine.Responses;

/*The response will contain the result of the spin in the form of a multi-dimensional integer array
where each cell in the array represents a reel in our slot machine,
and the content of the cell is the symbol that was selected for that reel.
The response will also contain a field that represents the player win from the specific spin and the current player balance.*/
public class SpinResult
{
    public int[,] SpinMatrix { get; set; }
    public int WinAmount { get; set; }
    public int CurrentBalance { get; set; }

    public SpinResult(int[,] spinMatrix, int winAmount, int currentBalance)
    {
        SpinMatrix = spinMatrix;
        WinAmount = winAmount;
        CurrentBalance = currentBalance;
    }
}