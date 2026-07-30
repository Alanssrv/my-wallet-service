using MongoDB.Bson;
using MyWallet.Data;
using MyWallet.Models;

namespace MyWallet.Services;

public class EntryService
{
    private readonly MongoRepository _mongoRepository;
    public EntryService(MongoRepository mongoRepository)
    {
        _mongoRepository = mongoRepository;
    }

    public async Task<PaginatedResult<Entries>> GetEntries(int pageSize = 10, int index = 0) => await _mongoRepository.GetEntriesAsync(pageSize, index);

    public async Task AddEntryAsync(Entries entry) => await _mongoRepository.AddEntryAsync(entry);

    public async Task<bool> UpdateEntryAsync(ObjectId id, Entries entry) => await _mongoRepository.UpdateEntryAsync(id, entry);

    public async Task<bool> DeleteEntryAsync(ObjectId id) => await _mongoRepository.DeleteEntryAsync(id);

    public async Task<IEnumerable<Entries>> GetEntriesByMonthAsync(int year, int month) => await _mongoRepository.GetEntriesByMonthAsync(year, month);

    public async Task<IEnumerable<OriginEntriesSummary>> GetEntriesSummaryByOriginAsync() => await _mongoRepository.GetEntriesSummaryByOriginAsync();

    public async Task<IEnumerable<OriginMonthlyEntriesSummary>> GetEntriesMonthlySummaryByOriginAsync() => await _mongoRepository.GetEntriesMonthlySummaryByOriginAsync();
}
