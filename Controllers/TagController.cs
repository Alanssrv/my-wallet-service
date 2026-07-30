using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MyWallet.Models;
using MyWallet.Services;

namespace MyWallet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly TagService _tagService;

    public TagController(TagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet("tags")]
    public async Task<IActionResult> GetTags()
    {
        var tags = await _tagService.GetTagsAsync();
        return Ok(tags);
    }

    [HttpPost("tags")]
    public async Task<IActionResult> AddTag([FromBody] Tags tag)
    {
        if (tag == null)
        {
            return BadRequest("Tag cannot be null.");
        }

        await _tagService.AddTagAsync(tag);
        return CreatedAtAction(nameof(GetTags), new { id = tag.Id }, tag);
    }

    [HttpPut("tags/{id}")]
    public async Task<IActionResult> UpdateTag(string id, [FromBody] Tags tag)
    {
        if (tag == null)
        {
            return BadRequest("Tag cannot be null.");
        }

        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid id.");
        }

        tag.Id = objectId;
        var updated = await _tagService.UpdateTagAsync(objectId, tag);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("tags/{id}")]
    public async Task<IActionResult> DeleteTag(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid id.");
        }

        var deleted = await _tagService.DeleteTagAsync(objectId);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
