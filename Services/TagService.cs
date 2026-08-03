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

    public async Task<IEnumerable<Tag>> GetTagsAsync() => await _mongoRepository.GetTagsAsync();

    public async Task AddTagAsync(Tag tag) => await _mongoRepository.AddTagAsync(tag);

    public async Task<bool> UpdateTagAsync(ObjectId id, Tag tag) => await _mongoRepository.UpdateTagAsync(id, tag);

    public async Task<bool> DeleteTagAsync(ObjectId id) => await _mongoRepository.DeleteTagAsync(id);
}
