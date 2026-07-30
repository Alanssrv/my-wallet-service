namespace MyWallet.Models;

public class PaginatedResult<T>
{
    public long Count { get; set; }
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
}
