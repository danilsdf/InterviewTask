using SlotMachine.Entities;
using SlotMachine.Extensions;

namespace SlotMachine.Test;

public class SlotMachineExtensionsTests
{

    [Fact]
    public void GenerateWinLines_WithWidth3Height3_ReturnsCorrectWinLines()
    {
        // Arrange
        var expectedWinLines = new List<List<(int, int)>>
        {
            // Straight win lines across each row
            new() { (0,0), (0,1), (0, 2) },
            new() { (1,0), (1,1), (1, 2) },
            new() { (2,0), (2,1), (2, 2) },

            // Diagonal win lines
            new() { (0, 0), (1, 1), (2, 2) },
            new() { (1, 0), (2, 1), (1, 2) },
            new() { (2, 0), (1, 1), (0, 2) }
        };

        // Act
        var actualWinLines = new Configuration(3, 3).LinesToWin;
        // Assert
        Assert.Equal(expectedWinLines.Count, actualWinLines.Count);

        for (var i = 0; i < expectedWinLines.Count; i++)
        {
            Assert.Equal(expectedWinLines[i].Count, actualWinLines[i].Count);
            Assert.Equal(expectedWinLines[i].Count, actualWinLines[i].Count);

            for (var row = 0; row < expectedWinLines[i].Count; row++)
            {
                for (var col = 0; col < expectedWinLines[i].Count; col++)
                {
                    Assert.Equal(expectedWinLines[i], actualWinLines[i]);
                }
            }
        }
    }

    [Fact]
    public void GenerateWinLines_WithWidth3Height5_ReturnsCorrectWinLines()
    {
        // Arrange
        var expectedWinLines = new List<List<(int,int)>>
        {
            // Straight win lines across each row
            new() { (0,0), (0,1), (0, 2), (0, 3), (0, 4) },
            new() { (1,0), (1,1), (1, 2), (1, 3), (1, 4) },
            new() { (2,0), (2,1), (2, 2), (2, 3), (2, 4) },

            // Diagonal win lines
            new() { (0, 0), (1, 1), (2, 2), (1, 3), (0, 4) },
            new() { (1, 0), (2, 1), (1, 2), (0, 3), (1, 4) },
            new() { (2, 0), (1, 1), (0, 2), (1, 3), (2, 4) }
        };

        // Act
        var actualWinLines = new Configuration(5, 3).LinesToWin;
        // Assert
        Assert.Equal(expectedWinLines.Count, actualWinLines.Count);

        for (var i = 0; i < expectedWinLines.Count; i++)
        {
            Assert.Equal(expectedWinLines[i].Count, actualWinLines[i].Count);
            Assert.Equal(expectedWinLines[i].Count, actualWinLines[i].Count);

            for (var row = 0; row < expectedWinLines[i].Count; row++)
            {
                for (var col = 0; col < expectedWinLines[i].Count; col++)
                {
                    Assert.Equal(expectedWinLines[i], actualWinLines[i]);
                }
            }
        }
    }

    [Fact]
    public void CalculateTotalWin_Returns2700()
    {
        // Arrange
        var resultMatrix = new[,]
        {
            { 3, 3, 3, 4, 5 },
            { 2, 3, 2, 3, 3 },
            { 1, 2, 3, 3, 3 }
        };

        var configuration = new Configuration(5, 3);

        var bet = 100;

        // Act
        var result = configuration.CalculateWinAmount(resultMatrix, bet);

        // Assert
        Assert.Equal(2700, result);
    }

    [Fact]
    public void CalculateTotalWin_Returns27400()
    {
        // Arrange
        var resultMatrix = new[,]
        {
            { 1, 3, 3, 3, 1 },
            { 2, 1, 2, 1, 3 },
            { 1, 2, 1, 3, 3 }
        };

        var configuration = new Configuration(5, 3);

        var bet = 1;

        // Act
        var result = configuration.CalculateWinAmount(resultMatrix, bet); // 1+1+1+1+1 + 2+2+2 + 1+1

        // Assert
        Assert.Equal(13, result);
    }
}