using MyWallet.Data;
using MyWallet.Models;

namespace MyWallet.Services;

public class AccountService
{
    private readonly MongoRepository _mongoRepository;

    public AccountService(MongoRepository mongoRepository)
    {
        _mongoRepository = mongoRepository;
    }

    public async Task<PaginatedResult<Account>> GetAccountsAsync(int pageSize = 10, int index = 0)
        => await _mongoRepository.GetAccountsAsync(pageSize, index);

    public async Task<Account?> GetAccountByReferenceAsync(int? year, int? month)
    {
        var reference = BuildReference(year, month);
        return await _mongoRepository.GetAccountByReferenceAsync(reference);
    }

    public async Task IncrementAccountsForEntryAsync(Entry entry)
    {
        var monthlyReference = BuildReference(entry.Date.Year, entry.Date.Month);

        var category = await _mongoRepository.GetCategoryByIdAsync(entry.Category);

        await IncrementAccountAsync(monthlyReference, entry.Origin, category, entry.Value);
        await IncrementAccountAsync(null, entry.Origin, category, entry.Value);
    }

    public async Task DecrementAccountsForEntryAsync(Entry entry)
    {
        var monthlyReference = BuildReference(entry.Date.Year, entry.Date.Month);

        var category = await _mongoRepository.GetCategoryByIdAsync(entry.Category);

        await DecrementAccountAsync(monthlyReference, entry.Origin, category, entry.Value);
        await DecrementAccountAsync(null, entry.Origin, category, entry.Value);
    }

    private async Task IncrementAccountAsync(DateTime? reference, OriginType origin, Category category, decimal value)
    {
        var account = await GetOrCreateAccountByReferenceAsync(reference);

        if (category.Type == EntryType.Income)
        {
            account.FinancialSummaries.First(x => x.Origin == null).Income += value;
            account.FinancialSummaries.First(x => x.Origin == origin).Income += value;
        }
        else
        {
            account.FinancialSummaries.First(x => x.Origin == null).Expense += value;
            account.FinancialSummaries.First(x => x.Origin == origin).Expense += value;
        }
        
        account.FinancialSummaries.First(x => x.Origin == null).Balance += value;
        await _mongoRepository.UpdateAccountAsync(account.Id, account);
    }

    private async Task DecrementAccountAsync(DateTime? reference, OriginType origin, Category category, decimal value)
    {
        var account = await GetOrCreateAccountByReferenceAsync(reference);

        if (category.Type == EntryType.Income)
        {
            account.FinancialSummaries.First(x => x.Origin == null).Income -= value;
            account.FinancialSummaries.First(x => x.Origin == origin).Income -= value;
        }
        else
        {
            account.FinancialSummaries.First(x => x.Origin == null).Expense -= value;
            account.FinancialSummaries.First(x => x.Origin == origin).Expense -= value;
        }

        account.FinancialSummaries.First(x => x.Origin == null).Balance -= value;
        account.FinancialSummaries.First(x => x.Origin == origin).Balance -= value;
        await _mongoRepository.UpdateAccountAsync(account.Id, account);
    }

    private async Task<Account> GetOrCreateAccountByReferenceAsync(DateTime? reference)
    {
        var account = await _mongoRepository.GetAccountByReferenceAsync(reference);

        if (account is not null)
        {
            return account;
        }

        List<OriginType> origins = Enum.GetValues<OriginType>().ToList();

        account = new Account
        {
            Reference = reference,
            FinancialSummaries = new List<Account.FinancialSummary>
            {
                new() {
                    Origin = null,
                    Income = 0,
                    Expense = 0,
                    Balance = 0
                }
            }.Concat(origins.Select(origin => new Account.FinancialSummary
            {
                Origin = origin,
                Income = 0,
                Expense = 0,
                Balance = 0
            })).ToList()
        };

        await _mongoRepository.AddAccountAsync(account);
        return account;
    }

    private static DateTime? BuildReference(int? year, int? month)
    {
        if (!year.HasValue || !month.HasValue)
        {
            return null;
        }

        return new DateTime(year.Value, month.Value, 1);
    }
}
