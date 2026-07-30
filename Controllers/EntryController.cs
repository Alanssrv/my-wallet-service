using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MyWallet.Models;
using MyWallet.Services;

namespace MyWallet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EntryController : ControllerBase
{
    private readonly EntryService _entryService;

    public EntryController(EntryService entryService) => _entryService = entryService;

    [HttpGet("entries")]
    public async Task<IActionResult> GetEntries([FromQuery] int pageSize = 10, [FromQuery] int index = 0)
    {
        var entries = await _entryService.GetEntries(pageSize, index);
        return Ok(entries);
    }

    [HttpPost("entries")]
    public async Task<IActionResult> AddEntry([FromBody] Entries entry)
    {
        if (entry == null)
        {
            return BadRequest("Entry cannot be null.");
        }
        await _entryService.AddEntryAsync(entry);
        return CreatedAtAction(nameof(GetEntries), new { id = entry.Id }, entry);
    }

    [HttpPut("entries/{id}")]
    public async Task<IActionResult> UpdateEntry(string id, [FromBody] Entries entry)
    {
        if (entry == null)
        {
            return BadRequest("Entry cannot be null.");
        }

        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid id.");
        }

        entry.Id = objectId;
        var updated = await _entryService.UpdateEntryAsync(objectId, entry);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("entries/{id}")]
    public async Task<IActionResult> DeleteEntry(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid id.");
        }

        var deleted = await _entryService.DeleteEntryAsync(objectId);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("entries/month")]
    public async Task<IActionResult> GetEntriesByMonth([FromQuery] int year, [FromQuery] int month)
    {
        if (year <= 0 || month is < 1 or > 12)
        {
            return BadRequest("Invalid year or month.");
        }

        var entries = await _entryService.GetEntriesByMonthAsync(year, month);
        return Ok(entries);
    }

    [HttpGet("entries/summary/origin")]
    public async Task<IActionResult> GetEntriesSummaryByOrigin()
    {
        var summary = await _entryService.GetEntriesSummaryByOriginAsync();
        return Ok(summary);
    }

    [HttpGet("entries/summary/origin/by-month")]
    public async Task<IActionResult> GetEntriesMonthlySummaryByOrigin()
    {
        var summary = await _entryService.GetEntriesMonthlySummaryByOriginAsync();
        return Ok(summary);
    }
}
