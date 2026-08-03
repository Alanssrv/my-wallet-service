using MongoDB.Bson;
using MyWallet.Data;
using MyWallet.Models;

namespace MyWallet.Services;

public class EntryService
{
    private readonly MongoRepository _mongoRepository;
    private readonly AccountService _accountService;

    public EntryService(MongoRepository mongoRepository, AccountService accountService)
    {
        _mongoRepository = mongoRepository;
        _accountService = accountService;
    }

    public async Task<PaginatedResult<Entry>> GetEntries(int pageSize = 10, int index = 0) => await _mongoRepository.GetEntriesAsync(pageSize, index);

    public async Task AddEntryAsync(Entry entry)
    {
        await _mongoRepository.AddEntryAsync(entry);
        await _accountService.IncrementAccountsForEntryAsync(entry);
    }

    public async Task<bool> UpdateEntryAsync(ObjectId id, Entry entry)
    {
        var currentEntry = await _mongoRepository.GetEntryByIdAsync(id);
        if (currentEntry is null)
        {
            return false;
        }

        var updated = await _mongoRepository.UpdateEntryAsync(id, entry);
        if (!updated)
        {
            return false;
        }

        await _accountService.DecrementAccountsForEntryAsync(currentEntry);
        await _accountService.IncrementAccountsForEntryAsync(entry);

        return true;
    }

    public async Task<bool> DeleteEntryAsync(ObjectId id)
    {
        var currentEntry = await _mongoRepository.GetEntryByIdAsync(id);
        if (currentEntry is null)
        {
            return false;
        }

        var deleted = await _mongoRepository.DeleteEntryAsync(id);
        if (!deleted)
        {
            return false;
        }

        await _accountService.DecrementAccountsForEntryAsync(currentEntry);
        return true;
    }

    public async Task<IEnumerable<Entry>> GetEntriesByMonthAsync(int year, int month) => await _mongoRepository.GetEntriesByMonthAsync(year, month);
}
