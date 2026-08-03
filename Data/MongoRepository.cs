using MongoDB.Bson;
using MongoDB.Driver;
using MyWallet.Context;
using MyWallet.Models;

namespace MyWallet.Data;

public class MongoRepository
{
    #region Collections

    private readonly MongoDbContext _mongoDbContext;
    private readonly IMongoCollection<Category> _categoriesCollection;
    private readonly IMongoCollection<Models.Tag> _tagsCollection;
    private readonly IMongoCollection<Limit> _limitsCollection;
    private readonly IMongoCollection<Entry> _entriesCollection;
    private readonly IMongoCollection<Account> _accountsCollection;

    #endregion

    public MongoRepository(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
        _categoriesCollection = _mongoDbContext.GetCollection<Category>("categories");
        _tagsCollection = _mongoDbContext.GetCollection<Models.Tag>("tags");
        _limitsCollection = _mongoDbContext.GetCollection<Limit>("limits");
        _entriesCollection = _mongoDbContext.GetCollection<Entry>("money-entries");
        _accountsCollection = _mongoDbContext.GetCollection<Account>("accounts");
    }

    #region Categories

    public async Task<Category?> GetCategoryByIdAsync(ObjectId id)
    {
        return await _categoriesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<PaginatedResult<Category>> GetCategoriesAsync(int pageSize = 10, int index = 0)
    {
        var filter = Builders<Category>.Filter.Empty;
        var countTask = _categoriesCollection.CountDocumentsAsync(filter);
        var dataTask = _categoriesCollection
            .Find(filter)
            .Skip(pageSize * index)
            .Limit(pageSize)
            .ToListAsync();

        await Task.WhenAll(countTask, dataTask);

        return new PaginatedResult<Category>
        {
            Count = countTask.Result,
            Data = dataTask.Result
        };
    }

    public async Task AddCategoryAsync(Category document)
    {
        await _categoriesCollection.InsertOneAsync(document);
    }

    public async Task<bool> UpdateCategoryAsync(ObjectId id, Category document)
    {
        var result = await _categoriesCollection.ReplaceOneAsync(x => x.Id == id, document);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteCategoryAsync(ObjectId id)
    {
        var result = await _categoriesCollection.DeleteOneAsync(x => x.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    #endregion

    #region Tags

    public async Task<IEnumerable<Models.Tag>> GetTagsAsync()
    {
        return await _tagsCollection.Find(Builders<Models.Tag>.Filter.Empty).ToListAsync();
    }

    public async Task AddTagAsync(Models.Tag document)
    {
        await _tagsCollection.InsertOneAsync(document);
    }

    public async Task<bool> UpdateTagAsync(ObjectId id, Models.Tag document)
    {
        var result = await _tagsCollection.ReplaceOneAsync(x => x.Id == id, document);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteTagAsync(ObjectId id)
    {
        var result = await _tagsCollection.DeleteOneAsync(x => x.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    #endregion

    #region Limits

    public async Task<PaginatedResult<Limit>> GetLimitsAsync(int pageSize = 10, int index = 0)
    {
        var filter = Builders<Limit>.Filter.Empty;
        var countTask = _limitsCollection.CountDocumentsAsync(filter);
        var dataTask = _limitsCollection
            .Find(filter)
            .Skip(pageSize * index)
            .Limit(pageSize)
            .ToListAsync();

        await Task.WhenAll(countTask, dataTask);

        return new PaginatedResult<Limit>
        {
            Count = countTask.Result,
            Data = dataTask.Result
        };
    }

    public async Task AddLimitAsync(Limit document)
    {
        await _limitsCollection.InsertOneAsync(document);
    }

    public async Task<bool> UpdateLimitAsync(ObjectId id, Limit document)
    {
        var result = await _limitsCollection.ReplaceOneAsync(x => x.Id == id, document);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteLimitAsync(ObjectId id)
    {
        var result = await _limitsCollection.DeleteOneAsync(x => x.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    #endregion

    #region Entries

    public async Task<PaginatedResult<Entry>> GetEntriesAsync(int pageSize = 10, int index = 0)
    {
        var filter = Builders<Entry>.Filter.Empty;
        var countTask = _entriesCollection.CountDocumentsAsync(filter);
        var dataTask = _entriesCollection
            .Find(filter)
            .Skip(pageSize * index)
            .Limit(pageSize)
            .ToListAsync();

        await Task.WhenAll(countTask, dataTask);

        return new PaginatedResult<Entry>
        {
            Count = countTask.Result,
            Data = dataTask.Result
        };
    }

    public async Task AddEntryAsync(Entry document)
    {
        await _entriesCollection.InsertOneAsync(document);
    }

    public async Task<bool> UpdateEntryAsync(ObjectId id, Entry document)
    {
        var result = await _entriesCollection.ReplaceOneAsync(x => x.Id == id, document);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteEntryAsync(ObjectId id)
    {
        var result = await _entriesCollection.DeleteOneAsync(x => x.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<Entry?> GetEntryByIdAsync(ObjectId id)
    {
        return await _entriesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Entry>> GetEntriesByMonthAsync(int year, int month)
    {
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        return await _entriesCollection
            .Find(x => x.Date >= start && x.Date < end)
            .ToListAsync();
    }

    #endregion

    #region Accounts

    public async Task<PaginatedResult<Account>> GetAccountsAsync(int pageSize = 10, int index = 0)
    {
        var filter = Builders<Account>.Filter.Empty;
        var countTask = _accountsCollection.CountDocumentsAsync(filter);
        var dataTask = _accountsCollection
            .Find(filter)
            .SortByDescending(x => x.Reference)
            .Skip(pageSize * index)
            .Limit(pageSize)
            .ToListAsync();

        await Task.WhenAll(countTask, dataTask);

        return new PaginatedResult<Account>
        {
            Count = countTask.Result,
            Data = dataTask.Result
        };
    }

    public async Task<Account?> GetAccountByReferenceAsync(DateTime? reference)
    {
        return await _accountsCollection.Find(x => x.Reference == reference).FirstOrDefaultAsync();
    }

    public async Task AddAccountAsync(Account document)
    {
        await _accountsCollection.InsertOneAsync(document);
    }

    public async Task<bool> UpdateAccountAsync(ObjectId id, Account document)
    {
        var result = await _accountsCollection.ReplaceOneAsync(x => x.Id == id, document);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    #endregion
}
