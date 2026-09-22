using Case8.Application.Checkout;

namespace Case8.Infrastructure;

public class EfUnitOfWork(StoreDbContext db) : IUnitOfWork
{
    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await db.Database.BeginTransactionAsync();
        try
        {
            await action();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
