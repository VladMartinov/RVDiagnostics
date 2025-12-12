using Core.Interfaces;
using Persistence.Contexts;

namespace Persistence;

public class UnitOfWork(DContext context) : IUnitOfWork
{
    public async Task StartTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync<T>(T entity, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(entity ?? throw new ArgumentNullException(nameof(entity)), cancellationToken);
    }

    public async Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        where T : class
    {
        await context.Set<T>().AddRangeAsync(entities, cancellationToken);
    }

    public void Remove<T>(T entity)
    {
        context.Remove(entity ?? throw new ArgumentNullException(nameof(entity)));
    }

    public void RemoveRange<T>(IEnumerable<T> entities) where T : class
    {
        context.Set<T>().RemoveRange(entities);
    }
}