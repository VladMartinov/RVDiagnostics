namespace Core.Interfaces
{
    /// <summary>
    /// Represents a Unit of Work pattern that manages database transactions
    /// and coordinates the work of multiple repositories by ensuring that
    /// changes are committed as a single atomic operation.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Starts a new database transaction. 
        /// Use this before performing a sequence of operations that must be committed together.
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task StartTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the current transaction.
        /// All pending changes tracked by the context will be saved permanently.
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the current transaction.
        /// Discards all pending changes made during the current transactional scope.
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves all changes made in the current context to the database.
        /// Should be used when transaction is not manually controlled,
        /// or inside a transaction before committing.
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an entity to the context to be inserted into the database on save.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">Entity instance to add.</param>
        /// <param name="cancellationToken">Token for cancelling the operation.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        Task AddAsync<T>(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a collection of entities to the context for bulk insertion.
        /// All entities will be inserted during the next save or transaction commit.
        /// </summary>
        /// <typeparam name="T">Type of the entities.</typeparam>
        /// <param name="entities">Collection of entities to add.</param>
        /// <param name="cancellationToken">Token for cancelling the operation.</param>
        /// <returns>A task that represents the asynchronous add range operation.</returns>
        Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class;

        /// <summary>
        /// Marks an entity for removal from the database.
        /// The actual delete will occur on save or transaction commit.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">Entity instance to remove.</param>
        void Remove<T>(T entity);

        /// <summary>
        /// Marks a collection of entities for removal.
        /// The actual deletion will occur on save or transaction commit.
        /// </summary>
        /// <typeparam name="T">Type of the entities.</typeparam>
        /// <param name="entities">Collection of entities to remove.</param>
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;
    }
}
