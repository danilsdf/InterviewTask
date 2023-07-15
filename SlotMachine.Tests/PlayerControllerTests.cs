using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SlotMachine.Controllers;
using SlotMachine.Entities;
using SlotMachine.Models;
using SlotMachine.Responses;

namespace SlotMachine.Tests;

public class PlayerControllerTests : IClassFixture<MongoDatabaseFixture>
{
    private readonly PlayerController _playerController;
    private readonly MongoDatabaseFixture _fixture;

    public PlayerControllerTests(MongoDatabaseFixture fixture)
    {
        _fixture = fixture;
        _playerController = new PlayerController(_fixture.GetDatabase());
    }

    [Fact]
    public async Task Spin_InsufficientBalance_ReturnsBadRequest()
    {
        var model = new SpinModel { UserName = _fixture.TestUserName, Bet = 10005 };

        var result = await _playerController.Spin(model);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Spin_NewId_ReturnsNotFound()
    {
        var model = new SpinModel { UserName = Guid.NewGuid().ToString(), Bet = 105 };

        var result = await _playerController.Spin(model);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task Spin_ValidRequest_ReturnsSpinResult()
    {
        var model = new SpinModel { UserName = _fixture.TestUserName, Bet = 25 };

        var response = await _playerController.Spin(model);

        Assert.IsType<OkObjectResult>(response.Result);
        var result = (SpinResult)((OkObjectResult)response.Result).Value!;
        Assert.NotNull(result.SpinMatrix);
        Assert.True(result.WinAmount >= 0);
        Assert.True(result.CurrentBalance >= 0);
    }

    [Fact]
    public async Task UpdateBalance_ValidRequest_ReturnsUpdatedBalance()
    {
        var userName = Guid.NewGuid().ToString();
        var initialBalance = 1000;
        var amountToAdd = 500;

        var player = new Player(initialBalance, userName);
        await _fixture.Players.InsertOneAsync(player);
        var request = new BalanceModel
        {
            UserName = userName,
            Amount = amountToAdd
        };

        var response = await _playerController.UpdateBalance(request);
        var updPlayer = _fixture.Players.Find(p => p.UserName == userName).FirstOrDefault();

        Assert.IsType<OkResult>(response);
        Assert.Equal(initialBalance + amountToAdd, updPlayer.Balance);
        await _fixture.Players.DeleteOneAsync(p => p.UserName == userName);
    }

    [Fact]
    public async Task UpdateBalance_InvalidPlayerId_ReturnsNotFound()
    {
        var amountToAdd = 500;

        var request = new BalanceModel
        {
            UserName = Guid.NewGuid().ToString(),
            Amount = amountToAdd
        };

        var response = await _playerController.UpdateBalance(request);

        Assert.IsType<NotFoundObjectResult>(response);
    }

    [Fact]
    public async Task UpdateBalance_NegativeAmount_ReturnsBadRequest()
    {
        var amountToAdd = -500;

        var request = new BalanceModel
        {
            UserName = _fixture.TestUserName,
            Amount = amountToAdd
        };

        var response = await _playerController.UpdateBalance(request);

        Assert.IsType<BadRequestObjectResult>(response);
    }
}