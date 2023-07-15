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

    public static int CalculateWinAmount(this Configuration configuration, int[,] matrix, int bet)
    {
        var linesToWin = GenerateWinLines(configuration.Width, configuration.Height);
        var win = 0;

        foreach (var winLine in linesToWin)
        {
            if (matrix[winLine[0].Item1, winLine[0].Item2] != matrix[winLine[1].Item1, winLine[1].Item2])
            {
                continue;
            }

            var count = 1;
            for (var i = 1; i < winLine.Count; i++)
            {
                if (matrix[winLine[i - 1].Item1, winLine[i - 1].Item2] == matrix[winLine[i].Item1, winLine[i].Item2])
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            win += matrix[winLine[0].Item1, winLine[0].Item2] * count;
        }

        return win * bet;
    }


    public static List<List<(int, int)>> GenerateWinLines(int width, int height)
    {
        var winLines = new List<List<(int, int)>>();

        // Straight across each row
        for (var row = 0; row < height; row++)
        {
            var line = new List<(int, int)>();
            for (var col = 0; col < width; col++)
            {
                line.Add((row, col));
            }
            winLines.Add(line);
        }

        // Diagonal lines starting from the left column
        for (var i = 0; i < height; i++)
        {
            int row = i, col = 0, direction = 1;
            var linePoints = new List<(int, int)>();

            while (row >= 0 && row < height && col >= 0 && col < width)
            {
                linePoints.Add((row, col));

                row += direction;
                col++;

                if (row == height)
                {
                    direction = -1;
                    row -= 2;
                }
                else if (row == 0 && direction == -1)
                {
                    direction = 1;
                }
            }

            winLines.Add(linePoints);
        }

        return winLines;
    }
}