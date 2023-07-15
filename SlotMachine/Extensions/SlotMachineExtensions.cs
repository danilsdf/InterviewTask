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
        var win = CalculateWinSum(configuration.LinesToWin, matrix, bet);

        return win * bet;
    }

    public static int CalculateWinAmountParallel(this Configuration configuration, int[,] matrix, int bet, int taskCount)
    {
        var tasks = new Task[taskCount];
        var results = new int[taskCount];
        var portionSize = configuration.LinesToWin.Count / taskCount;

        for (var i = 0; i < taskCount; i++)
        {
            var i1 = i;
            tasks[i] = Task.Run(() => results[i1] = CalculateWinSum(configuration.LinesToWin.Skip(i1 * portionSize).Take(portionSize).ToList(), matrix, bet));
        }

        Task.WaitAll(tasks);

        return results.Sum() * bet;
    }

    private static int CalculateWinSum(List<List<(int,int)>> linesToWin, int[,] matrix, int bet)
    {
        var sum = 0;

        foreach (var winLine in linesToWin)
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

            sum += matrix[winLine[0].Item1, winLine[0].Item2] * count;
        }

        return sum;
    }
}