using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MyWallet.Models;
using MyWallet.Services;

namespace MyWallet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories([FromQuery] int pageSize = 10, [FromQuery] int index = 0)
    {
        var categories = await _categoryService.GetCategoriesAsync(pageSize, index);
        return Ok(categories);
    }

    [HttpPost("categories")]
    public async Task<IActionResult> AddCategory([FromBody] Category category)
    {
        if (category == null)
        {
            return BadRequest("Category cannot be null.");
        }

        await _categoryService.AddCategoryAsync(category);
        return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
    }

    [HttpPut("categories/{id}")]
    public async Task<IActionResult> UpdateCategory(string id, [FromBody] Category category)
    {
        if (category == null)
        {
            return BadRequest("Category cannot be null.");
        }

        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid id.");
        }

        category.Id = objectId;
        var updated = await _categoryService.UpdateCategoryAsync(objectId, category);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("categories/{id}")]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid id.");
        }

        var deleted = await _categoryService.DeleteCategoryAsync(objectId);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
