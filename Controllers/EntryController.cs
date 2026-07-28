using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetEntries()
    {
        var entries = await _entryService.GetEntries();
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
}
