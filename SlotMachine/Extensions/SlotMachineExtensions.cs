using SlotMachine.Entities;

namespace SlotMachine.Extensions;

public static class SlotMachineExtensions
{
    public static int[,] GenerateRandomMatrix(this Configuration configuration)
    {
        var random = new Random();
        var result = new int[configuration.Height, configuration.Width];

        for (var row = 0; row < configuration.Height; row++)
        {
            for (var col = 0; col < configuration.Width; col++)
            {
                result[row, col] = random.Next(0, 10);
            }
        }

        return result;
    }

    public static int CalculateWinAmount(this Configuration configuration, int[,] matrix, int bet)
    {
        var win = 0;

        foreach (var winLine in configuration.LinesToWin)
        {
            if (matrix[winLine[0].Item1, winLine[0].Item2] != matrix[winLine[1].Item1, winLine[1].Item2])
            {
                continue;
            }

            var count = 1;
            for (var i = 1; i < winLine.Count; i++)
            {
                if (matrix[winLine[i - 1].Item1, winLine[i - 1].Item2] != matrix[winLine[i].Item1, winLine[i].Item2])
                {
                    break;
                }

                count++;
            }

            win += matrix[winLine[0].Item1, winLine[0].Item2] * count;
        }

        return win * bet;
    }
}