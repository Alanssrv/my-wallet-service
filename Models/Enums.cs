namespace MyWallet.Models;

public enum EntryType
{
    Income = 1,
    Expense = 2,
}

public enum LimitType
{
    Percentage = 1,
    Absolute = 2,
}

public enum OriginType
{
    Nubank = 1,
    Cash = 2,
    C6 = 3,
    BB = 4,
}
