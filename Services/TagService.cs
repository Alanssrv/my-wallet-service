using MongoDB.Bson;
using MyWallet.Data;
using MyWallet.Models;

namespace MyWallet.Services;

public class TagService
{
    private readonly MongoRepository _mongoRepository;

    public TagService(MongoRepository mongoRepository)
    {
        _mongoRepository = mongoRepository;
    }

    public async Task<IEnumerable<Tags>> GetTagsAsync() => await _mongoRepository.GetTagsAsync();

    public async Task AddTagAsync(Tags tag) => await _mongoRepository.AddTagAsync(tag);

    public async Task<bool> UpdateTagAsync(ObjectId id, Tags tag) => await _mongoRepository.UpdateTagAsync(id, tag);

    public async Task<bool> DeleteTagAsync(ObjectId id) => await _mongoRepository.DeleteTagAsync(id);
}
