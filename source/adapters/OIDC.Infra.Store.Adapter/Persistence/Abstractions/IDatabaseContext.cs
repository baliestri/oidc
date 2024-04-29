// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OIDC.Infra.Store.Adapter.Persistence.Abstractions;

/// <summary>
///   Defines a contract for a database context.
/// </summary>
public interface IDatabaseContext : IAsyncDisposable {
  /// <summary>
  ///   Provides access to database related information and operations for this context.
  /// </summary>
  DatabaseFacade Database { get; }

  /// <summary>
  ///   Creates a <see cref="DbSet{TEntity}" /> that can be used to query and save instances of TEntity.
  /// </summary>
  /// <typeparam name="TEntity">The type of entity for which a set should be returned.</typeparam>
  /// <returns>A set for the given entity type.</returns>
  DbSet<TEntity> Set<TEntity>() where TEntity : class;

  /// <summary>
  ///   Gets an <see cref="EntityEntry{TEntity}" /> for the given entity. The entry provides access to change tracking information and operations
  ///   for the entity.
  /// </summary>
  /// <param name="entity">The entity to get the entry for.</param>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  /// <returns>The entry for the given entity.</returns>
  EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

  /// <summary>
  ///   Saves all changes made in this context to the database.
  /// </summary>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>
  ///   A task that represents the asynchronous save operation. The task result contains the number of state entries written to the
  ///   database.
  /// </returns>
  /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
  /// <exception cref="DbUpdateConcurrencyException">
  ///   A concurrency violation is encountered while saving to the database. A concurrency violation occurs when an unexpected number of
  ///   rows are affected during save. This is usually because the data in the database has been modified since it was loaded into memory.
  /// </exception>
  /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
