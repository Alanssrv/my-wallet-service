using MongoDB.Bson;
using MyWallet.Data;
using MyWallet.Models;

namespace MyWallet.Services;

public class LimitService
{
    private readonly MongoRepository _mongoRepository;

    public LimitService(MongoRepository mongoRepository)
    {
        _mongoRepository = mongoRepository;
    }

    public async Task<PaginatedResult<Limits>> GetLimitsAsync(int pageSize = 10, int index = 0) => await _mongoRepository.GetLimitsAsync(pageSize, index);

    public async Task AddLimitAsync(Limits limit) => await _mongoRepository.AddLimitAsync(limit);

    public async Task<bool> UpdateLimitAsync(ObjectId id, Limits limit) => await _mongoRepository.UpdateLimitAsync(id, limit);

    public async Task<bool> DeleteLimitAsync(ObjectId id) => await _mongoRepository.DeleteLimitAsync(id);
}
