using System.Reflection;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using SlotMachine.Controllers;
using SlotMachine.Entities;

namespace SlotMachine.Tests;

public class MongoDatabaseFixture : IAsyncLifetime
{
    private readonly IMongoDatabase _database;

    public readonly IMongoCollection<Configuration> Configuration;
    public readonly IMongoCollection<Player> Players;
    public readonly string TestUserName = Guid.NewGuid().ToString();

    private readonly ObjectId _configId = ObjectId.GenerateNewId();

    public MongoDatabaseFixture()
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets(Assembly.GetAssembly(typeof(PlayerController))).Build();
        var connectionString = configuration.GetConnectionString("MongoDB");
        var databaseName = configuration.GetValue<string>("MongoDB:DatabaseName");

        var mongoClient = new MongoClient(connectionString);
        _database = mongoClient.GetDatabase(databaseName);

        Players = _database.GetCollection<Player>("players");
        Configuration = _database.GetCollection<Configuration>("configuration");

        var defaultConfig = new Configuration(5, 3)
        {
            Id = _configId,
        };
        Configuration.InsertOne(defaultConfig);

        var defaultPlayer = new Player(100, TestUserName);
        Players.InsertOne(defaultPlayer);
    }

    public IMongoDatabase GetDatabase()
    {
        return _database;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await Players.DeleteOneAsync(p => p.UserName == TestUserName);
        await Configuration.DeleteOneAsync(p => p.Id == _configId);
    }
}