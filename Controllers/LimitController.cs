using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MyWallet.Models;
using MyWallet.Services;

namespace MyWallet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LimitController : ControllerBase
{
    private readonly LimitService _limitService;

    public LimitController(LimitService limitService)
    {
        _limitService = limitService;
    }

    [HttpGet("limits")]
    public async Task<IActionResult> GetLimits([FromQuery] int pageSize = 10, [FromQuery] int index = 0)
    {
        var limits = await _limitService.GetLimitsAsync(pageSize, index);
        return Ok(limits);
    }

    [HttpPost("limits")]
    public async Task<IActionResult> AddLimit([FromBody] Limit limit)
    {
        if (limit == null)
        {
            return BadRequest("Limit cannot be null.");
        }

        await _limitService.AddLimitAsync(limit);
        return CreatedAtAction(nameof(GetLimits), new { id = limit.Id }, limit);
    }

    [HttpPut("limits/{id}")]
    public async Task<IActionResult> UpdateLimit(string id, [FromBody] Limit limit)
    {
        if (limit == null)
        {
            return BadRequest("Limit cannot be null.");
        }

        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid id.");
        }

        limit.Id = objectId;
        var updated = await _limitService.UpdateLimitAsync(objectId, limit);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("limits/{id}")]
    public async Task<IActionResult> DeleteLimit(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid id.");
        }

        var deleted = await _limitService.DeleteLimitAsync(objectId);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
