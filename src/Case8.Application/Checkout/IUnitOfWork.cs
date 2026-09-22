namespace Case8.Application.Checkout;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
}
