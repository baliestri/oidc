// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Linq.Expressions;
using OIDC.Core.Entities.Abstractions;
using OIDC.Infra.Store.Port.ComplexTypes;
using OIDC.Infra.Store.Port.Delegates;

namespace OIDC.Infra.Store.Port.Abstractions;

/// <summary>
///   Defines a contract for a read-only repository.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IReadOnlyRepository<TEntity> : IAsyncDisposable where TEntity : class, IEntity {
  /// <summary>
  ///   Ignore query filters.
  /// </summary>
  /// <returns>The repository with query filters ignored.</returns>
  IReadOnlyRepository<TEntity> IgnoreQueryFilters();

  /// <summary>
  ///   Counts all entities in the repository.
  /// </summary>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The number of entities.</returns>
  Task<int> CountAsync(CancellationToken cancellationToken = default);

  /// <summary>
  ///   Counts all entities in the repository by a predicate.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The number of entities.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" /> is <c>null</c>.</exception>
  Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Checks if an entity exists in the repository by a predicate.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns><c>true</c> if the entity exists, <c>false</c> otherwise.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" /> is <c>null</c>.</exception>
  Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Checks if an entity exists in the repository by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the entity to find.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns><c>true</c> if the entity exists, <c>false</c> otherwise.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="id" /> is <c>null</c>.</exception>
  Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Gets an entity in the repository by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the entity to find.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entity if found, null otherwise.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="id" /> is <c>null</c>.</exception>
  Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Gets an entity in the repository by its identifier and includes related entities.
  /// </summary>
  /// <param name="id">The identifier of the entity to find.</param>
  /// <param name="includable">The expression to use for including related entities.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entity if found, null otherwise.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="id" /> or <paramref name="includable" /> is <c>null</c>.</exception>
  Task<TEntity?> GetAsync(Guid id, IncludableExpression<TEntity> includable, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Gets an entity in the repository by a predicate.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entity if found, null otherwise.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" /> is <c>null</c>.</exception>
  Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Gets an entity in the repository by a predicate and includes related entities.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="includable">The expression to use for including related entities.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entity if found, null otherwise.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" /> or <paramref name="includable" /> is <c>null</c>.</exception>
  Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, IncludableExpression<TEntity> includable,
  CancellationToken cancellationToken = default);

  /// <summary>
  ///   Finds all entities in the repository.
  /// </summary>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entities.</returns>
  Task<IEnumerable<TEntity>> FindAsync(CancellationToken cancellationToken = default);

  /// <summary>
  ///   Finds all entities in the repository by a predicate.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entities found by the predicate.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" /> is <c>null</c>.</exception>
  Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Finds all entities in the repository and orders them.
  /// </summary>
  /// <param name="orderBy">The expression to use for ordering the entities.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entities ordered by the expression.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="orderBy" /> is <c>null</c>.</exception>
  Task<IEnumerable<TEntity>> FindAsync(OrderByExpression<TEntity> orderBy, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Finds all entities in the repository by a predicate and orders them.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="orderBy">The expression to use for ordering the entities.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entities found by the predicate and ordered by the expression.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" /> or <paramref name="orderBy" /> is <c>null</c>.</exception>
  Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, OrderByExpression<TEntity> orderBy,
  CancellationToken cancellationToken = default);

  /// <summary>
  ///   Finds all entities in the repository and includes related entities.
  /// </summary>
  /// <param name="includable">The expression to use for including related entities.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entities with related entities included.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="includable" /> is <c>null</c>.</exception>
  Task<IEnumerable<TEntity>> FindAsync(IncludableExpression<TEntity> includable, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Finds all entities in the repository by a predicate and includes related entities.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="includable">The expression to use for including related entities.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entities found by the predicate with related entities included.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" /> or <paramref name="includable" /> is <c>null</c>.</exception>
  Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, IncludableExpression<TEntity> includable,
  CancellationToken cancellationToken = default);

  /// <summary>
  ///   Finds all entities in the repository by a predicate, orders them and includes related entities.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="orderBy">The expression to use for ordering the entities.</param>
  /// <param name="includable">The expression to use for including related entities.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>The entities found by the predicate and ordered by the expression with related entities included.</returns>
  /// <exception cref="ArgumentNullException">
  ///   If the <paramref name="predicate" />, <paramref name="orderBy" /> or <paramref name="includable" />
  ///   is <c>null</c>.
  /// </exception>
  Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, OrderByExpression<TEntity> orderBy,
  IncludableExpression<TEntity> includable, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Finds all entities in the repository, orders them and includes related entities.
  /// </summary>
  /// <param name="orderBy">The expression to use for ordering the entities.</param>
  /// <param name="includable">The expression to use for including related entities.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns></returns>
  Task<IEnumerable<TEntity>> FindAsync(OrderByExpression<TEntity> orderBy, IncludableExpression<TEntity> includable,
  CancellationToken cancellationToken = default);

  /// <summary>
  ///   Paginates all entities in the repository.
  /// </summary>
  /// <param name="currentPage">The current page number.</param>
  /// <param name="perPage">The number of items to be displayed per page.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A paginated set of entities.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="currentPage" /> or <paramref name="perPage" /> is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="currentPage" /> or <paramref name="perPage" /> is less than 1.</exception>
  Task<PaginatedSet<TEntity>> PaginateAsync(int currentPage, int perPage, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Paginates all entities in the repository by a predicate.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="currentPage">The current page number.</param>
  /// <param name="perPage">The number of items to be displayed per page.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A paginated set of entities.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="predicate" />, <paramref name="currentPage" /> or <paramref name="perPage" /> is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="currentPage" /> or <paramref name="perPage" /> is less than 1.</exception>
  Task<PaginatedSet<TEntity>> PaginateAsync(Expression<Func<TEntity, bool>> predicate, int currentPage, int perPage,
  CancellationToken cancellationToken = default);

  /// <summary>
  ///   Paginates all entities in the repository and orders them.
  /// </summary>
  /// <param name="orderBy">The expression to use for ordering the entities.</param>
  /// <param name="currentPage">The current page number.</param>
  /// <param name="perPage">The number of items to be displayed per page.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A paginated set of entities.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="orderBy" />, <paramref name="currentPage" /> or <paramref name="perPage" /> is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="currentPage" /> or <paramref name="perPage" /> is less than 1.</exception>
  Task<PaginatedSet<TEntity>> PaginateAsync(OrderByExpression<TEntity> orderBy, int currentPage, int perPage,
  CancellationToken cancellationToken = default);

  /// <summary>
  ///   Paginates all entities in the repository by a predicate and orders them.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="orderBy">The expression to use for ordering the entities.</param>
  /// <param name="currentPage">The current page number.</param>
  /// <param name="perPage">The number of items to be displayed per page.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A paginated set of entities.</returns>
  /// <exception cref="ArgumentNullException">
  ///   If the <paramref name="predicate" />, <paramref name="orderBy" />, <paramref name="currentPage" /> or
  ///   <paramref name="perPage" /> is null.
  /// </exception>
  /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="currentPage" /> or <paramref name="perPage" /> is less than 1.</exception>
  Task<PaginatedSet<TEntity>> PaginateAsync(Expression<Func<TEntity, bool>> predicate, OrderByExpression<TEntity> orderBy, int currentPage,
  int perPage, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Paginates all entities in the repository and includes related entities.
  /// </summary>
  /// <param name="includable">The expression to use for including related entities.</param>
  /// <param name="currentPage">The current page number.</param>
  /// <param name="perPage">The number of items to be displayed per page.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A paginated set of entities.</returns>
  /// <exception cref="ArgumentNullException">
  ///   If the <paramref name="includable" />, <paramref name="currentPage" /> or <paramref name="perPage" /> is
  ///   null.
  /// </exception>
  /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="currentPage" /> or <paramref name="perPage" /> is less than 1.</exception>
  Task<PaginatedSet<TEntity>> PaginateAsync(IncludableExpression<TEntity> includable, int currentPage, int perPage,
  CancellationToken cancellationToken = default);

  /// <summary>
  ///   Paginates all entities in the repository by a predicate and includes related entities.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="includable">The expression to use for including related entities.</param>
  /// <param name="currentPage">The current page number.</param>
  /// <param name="perPage">The number of items to be displayed per page.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A paginated set of entities.</returns>
  /// <exception cref="ArgumentNullException">
  ///   If the <paramref name="predicate" />, <paramref name="includable" />, <paramref name="currentPage" /> or
  ///   <paramref name="perPage" /> is null.
  /// </exception>
  /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="currentPage" /> or <paramref name="perPage" /> is less than 1.</exception>
  Task<PaginatedSet<TEntity>> PaginateAsync(Expression<Func<TEntity, bool>> predicate, IncludableExpression<TEntity> includable, int currentPage,
  int perPage, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Paginates all entities in the repository by a predicate, orders them and includes related entities.
  /// </summary>
  /// <param name="orderBy">The expression to use for ordering the entities.</param>
  /// <param name="includable">The expression to use for including related entities.</param>
  /// <param name="currentPage">The current page number.</param>
  /// <param name="perPage">The number of items to be displayed per page.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A paginated set of entities.</returns>
  /// <exception cref="ArgumentNullException">
  ///   If the <paramref name="orderBy" />, <paramref name="includable" />, <paramref name="currentPage" /> or
  ///   <paramref name="perPage" /> is null.
  /// </exception>
  /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="currentPage" /> or <paramref name="perPage" /> is less than 1.</exception>
  Task<PaginatedSet<TEntity>> PaginateAsync(OrderByExpression<TEntity> orderBy, IncludableExpression<TEntity> includable, int currentPage,
  int perPage, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Paginates all entities in the repository by a predicate, orders them and includes related entities.
  /// </summary>
  /// <param name="predicate">The predicate to use.</param>
  /// <param name="orderBy">The expression to use for ordering the entities.</param>
  /// <param name="includable">The expression to use for including related entities.</param>
  /// <param name="currentPage">The current page number.</param>
  /// <param name="perPage">The number of items to be displayed per page.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A paginated set of entities.</returns>
  /// <exception cref="ArgumentNullException">
  ///   If the <paramref name="predicate" />, <paramref name="orderBy" />, <paramref name="includable" />,
  ///   <paramref name="currentPage" /> or <paramref name="perPage" /> is null.
  /// </exception>
  /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="currentPage" /> or <paramref name="perPage" /> is less than 1.</exception>
  Task<PaginatedSet<TEntity>> PaginateAsync(Expression<Func<TEntity, bool>> predicate, OrderByExpression<TEntity> orderBy,
  IncludableExpression<TEntity> includable, int currentPage, int perPage, CancellationToken cancellationToken = default);
}
