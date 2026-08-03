using Microsoft.AspNetCore.Mvc;
using MyWallet.Services;

namespace MyWallet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccounts([FromQuery] int pageSize = 10, [FromQuery] int index = 0)
    {
        var accounts = await _accountService.GetAccountsAsync(pageSize, index);
        return Ok(accounts);
    }

    [HttpGet("accounts/by-reference")]
    public async Task<IActionResult> GetAccountByReference([FromQuery] int? year = null, [FromQuery] int? month = null)
    {
        if (year.HasValue != month.HasValue)
        {
            return BadRequest("year and month must be sent together, or both omitted for general account.");
        }

        if (month is < 1 or > 12)
        {
            return BadRequest("month must be between 1 and 12.");
        }

        var account = await _accountService.GetAccountByReferenceAsync(year, month);
        if (account is null)
        {
            return NotFound();
        }

        return Ok(account);
    }
}
