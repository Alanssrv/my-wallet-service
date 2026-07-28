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
    public async Task<IEnumerable<Entries>> GetEntries() => await _mongoRepository.GetEntries();
    public async Task AddEntryAsync(Entries entry) => await _mongoRepository.AddEntryAsync(entry);
}
