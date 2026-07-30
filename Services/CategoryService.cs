using MongoDB.Bson;
using MyWallet.Data;
using MyWallet.Models;

namespace MyWallet.Services;

public class CategoryService
{
    private readonly MongoRepository _mongoRepository;

    public CategoryService(MongoRepository mongoRepository)
    {
        _mongoRepository = mongoRepository;
    }

    public async Task<PaginatedResult<Categories>> GetCategoriesAsync(int pageSize = 10, int index = 0) => await _mongoRepository.GetCategoriesAsync(pageSize, index);

    public async Task AddCategoryAsync(Categories category) => await _mongoRepository.AddCategoryAsync(category);

    public async Task<bool> UpdateCategoryAsync(ObjectId id, Categories category) => await _mongoRepository.UpdateCategoryAsync(id, category);

    public async Task<bool> DeleteCategoryAsync(ObjectId id) => await _mongoRepository.DeleteCategoryAsync(id);
}
