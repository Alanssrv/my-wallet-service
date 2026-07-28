using MongoDB.Driver;
using MyWallet.Context;
using MyWallet.Models;

namespace MyWallet.Data;

public class MongoRepository
{
    private readonly IMongoCollection<Entries> _entriesCollection;
    private readonly MongoDbContext _mongoDbContext;
    public MongoRepository(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
        _entriesCollection = _mongoDbContext.GetCollection<Entries>("money-entries");
    }
    public async Task<IEnumerable<Entries>> GetEntries(int pageSize = 10, int index = 0)
    {
        return await _entriesCollection.Find(Builders<Entries>.Filter.Empty).Limit(pageSize).Skip(pageSize * index).ToListAsync();
    }
    public async Task AddEntryAsync(Entries document)
    {
        await _entriesCollection.InsertOneAsync(document);
    }
}
