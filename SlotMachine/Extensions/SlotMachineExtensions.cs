using System;
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
}