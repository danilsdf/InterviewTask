using MongoDB.Bson;

namespace SlotMachine.Entities;

public class Configuration
{
    public ObjectId Id { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public List<List<(int, int)>> LinesToWin { get; set; }

    public Configuration(int width, int height)
    {
        Id = new ObjectId();
        Width = width;
        Height = height;
        LinesToWin = GenerateWinLines(width, height);
    }

    private static List<List<(int, int)>> GenerateWinLines(int width, int height)
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