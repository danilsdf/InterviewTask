using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace SlotMachine.Infrastructure;

public static class MongoExtensions
{
    public static void AddCustomMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnectionString = configuration.GetConnectionString("MongoDB");
        services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
        services.AddScoped<IMongoDatabase>(sp =>
        {
            var mongoClient = sp.GetService<IMongoClient>();
            var databaseName = configuration.GetValue<string>("MongoDB:DatabaseName");
            return mongoClient.GetDatabase(databaseName);
        });
    }
}