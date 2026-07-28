using MongoDB.Driver;

namespace MyWallet.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public MongoDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("MongoDb");
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("MongoDB connection string is not configured.");
        }
        var databaseName = _configuration["MongoSettings:DatabaseName"];
        var client = new MongoClient(_connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
