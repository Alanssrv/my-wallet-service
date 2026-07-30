using MongoDB.Bson;
using MongoDB.Driver;
using MyWallet.Context;
using MyWallet.Models;

namespace MyWallet.Data;

public class MongoRepository
{
    #region Collections

    private readonly MongoDbContext _mongoDbContext;
    private readonly IMongoCollection<Categories> _categoriesCollection;
    private readonly IMongoCollection<Tags> _tagsCollection;
    private readonly IMongoCollection<Limits> _limitsCollection;
    private readonly IMongoCollection<Entries> _entriesCollection;

    #endregion

    public MongoRepository(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
        _categoriesCollection = _mongoDbContext.GetCollection<Categories>("categories");
        _tagsCollection = _mongoDbContext.GetCollection<Tags>("tags");
        _limitsCollection = _mongoDbContext.GetCollection<Limits>("limits");
        _entriesCollection = _mongoDbContext.GetCollection<Entries>("money-entries");
    }

    #region Categories

    public async Task<PaginatedResult<Categories>> GetCategoriesAsync(int pageSize = 10, int index = 0)
    {
        var filter = Builders<Categories>.Filter.Empty;
        var countTask = _categoriesCollection.CountDocumentsAsync(filter);
        var dataTask = _categoriesCollection
            .Find(filter)
            .Skip(pageSize * index)
            .Limit(pageSize)
            .ToListAsync();

        await Task.WhenAll(countTask, dataTask);

        return new PaginatedResult<Categories>
        {
            Count = countTask.Result,
            Data = dataTask.Result
        };
    }

    public async Task AddCategoryAsync(Categories document)
    {
        await _categoriesCollection.InsertOneAsync(document);
    }

    public async Task<bool> UpdateCategoryAsync(ObjectId id, Categories document)
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

    public async Task<IEnumerable<Tags>> GetTagsAsync()
    {
        return await _tagsCollection.Find(Builders<Tags>.Filter.Empty).ToListAsync();
    }

    public async Task AddTagAsync(Tags document)
    {
        await _tagsCollection.InsertOneAsync(document);
    }

    public async Task<bool> UpdateTagAsync(ObjectId id, Tags document)
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

    public async Task<PaginatedResult<Limits>> GetLimitsAsync(int pageSize = 10, int index = 0)
    {
        var filter = Builders<Limits>.Filter.Empty;
        var countTask = _limitsCollection.CountDocumentsAsync(filter);
        var dataTask = _limitsCollection
            .Find(filter)
            .Skip(pageSize * index)
            .Limit(pageSize)
            .ToListAsync();

        await Task.WhenAll(countTask, dataTask);

        return new PaginatedResult<Limits>
        {
            Count = countTask.Result,
            Data = dataTask.Result
        };
    }

    public async Task AddLimitAsync(Limits document)
    {
        await _limitsCollection.InsertOneAsync(document);
    }

    public async Task<bool> UpdateLimitAsync(ObjectId id, Limits document)
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

    public async Task<PaginatedResult<Entries>> GetEntriesAsync(int pageSize = 10, int index = 0)
    {
        var filter = Builders<Entries>.Filter.Empty;
        var countTask = _entriesCollection.CountDocumentsAsync(filter);
        var dataTask = _entriesCollection
            .Find(filter)
            .Skip(pageSize * index)
            .Limit(pageSize)
            .ToListAsync();

        await Task.WhenAll(countTask, dataTask);

        return new PaginatedResult<Entries>
        {
            Count = countTask.Result,
            Data = dataTask.Result
        };
    }

    public async Task AddEntryAsync(Entries document)
    {
        await _entriesCollection.InsertOneAsync(document);
    }

    public async Task<bool> UpdateEntryAsync(ObjectId id, Entries document)
    {
        var result = await _entriesCollection.ReplaceOneAsync(x => x.Id == id, document);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteEntryAsync(ObjectId id)
    {
        var result = await _entriesCollection.DeleteOneAsync(x => x.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<IEnumerable<Entries>> GetEntriesByMonthAsync(int year, int month)
    {
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        return await _entriesCollection
            .Find(x => x.Date >= start && x.Date < end)
            .ToListAsync();
    }

    public async Task<IEnumerable<OriginEntriesSummary>> GetEntriesSummaryByOriginAsync()
    {
        var entries = await _entriesCollection.Find(Builders<Entries>.Filter.Empty).ToListAsync();

        return entries
            .GroupBy(x => x.OriginType)
            .Select(group => new OriginEntriesSummary
            {
                Origin = group.Key,
                Income = group.Where(x => x.Value >= 0).Sum(x => x.Value),
                Expense = group.Where(x => x.Value < 0).Sum(x => Math.Abs(x.Value))
            })
            .OrderBy(x => x.Origin)
            .ToList();
    }

    public async Task<IEnumerable<OriginMonthlyEntriesSummary>> GetEntriesMonthlySummaryByOriginAsync()
    {
        var entries = await _entriesCollection.Find(Builders<Entries>.Filter.Empty).ToListAsync();

        return entries
            .GroupBy(x => new { x.Date.Year, x.Date.Month, x.OriginType })
            .Select(group => new OriginMonthlyEntriesSummary
            {
                Year = group.Key.Year,
                Month = group.Key.Month,
                Origin = group.Key.Origin,
                Income = group.Where(x => x.Value >= 0).Sum(x => x.Value),
                Expense = group.Where(x => x.Value < 0).Sum(x => Math.Abs(x.Value))
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ThenBy(x => x.Origin)
            .ToList();
    }

    #endregion
}
