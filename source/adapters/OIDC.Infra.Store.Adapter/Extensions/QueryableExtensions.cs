// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OIDC.Core.Entities.Abstractions;
using OIDC.Infra.Store.Port.ComplexTypes;

namespace OIDC.Infra.Store.Adapter.Extensions;

[ExcludeFromCodeCoverage]
internal static class QueryableExtensions {
  /// <summary>
  ///   Tries to ignore query filters given a condition.
  /// </summary>
  /// <param name="queryable">The queryable.</param>
  /// <param name="condition">The condition to ignore the query filters.</param>
  /// <typeparam name="TEntity">The entity type.</typeparam>
  /// <returns>The queryable itself.</returns>
  public static IQueryable<TEntity> TryIgnoreQueryFilters<TEntity>(this IQueryable<TEntity> queryable, bool condition = false)
    where TEntity : class, IEntity
    => condition ? queryable.IgnoreQueryFilters() : queryable;

  /// <summary>
  ///   Returns a subset of items from the queryable source.
  /// </summary>
  /// <param name="queryable">The queryable source.</param>
  /// <param name="currentPage">The current page number.</param>
  /// <param name="perPage">The number of items to be displayed per page.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <typeparam name="T">The type of items in the queryable source.</typeparam>
  /// <returns>A <see cref="PaginatedSet" /> instance.</returns>
  /// <exception cref="ArgumentOutOfRangeException">If <paramref name="currentPage" /> or <paramref name="perPage" /> is less than 1.</exception>
  public static async Task<PaginatedSet<T>> ToPaginatedSetAsync<T>(this IQueryable<T> queryable, int currentPage, int perPage,
  CancellationToken cancellationToken = default) where T : IEntity {
    ArgumentOutOfRangeException.ThrowIfLessThan(currentPage, 1, nameof(currentPage));
    ArgumentOutOfRangeException.ThrowIfLessThan(perPage, 1, nameof(perPage));

    var total = await queryable.CountAsync(cancellationToken);

    if (total <= 0) {
      return PaginatedSet.Empty<T>(perPage, currentPage);
    }

    var items = await queryable
      .Skip((currentPage - 1) * perPage)
      .Take(perPage)
      .ToListAsync(cancellationToken);

    var lastPage = (int)Math.Ceiling(total / (double)perPage);

    return new PaginatedSet<T>(total, perPage, currentPage, lastPage, items);
  }
}
