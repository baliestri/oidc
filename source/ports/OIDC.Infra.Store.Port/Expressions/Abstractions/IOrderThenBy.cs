// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Linq.Expressions;
using OIDC.Core.Entities.Abstractions;

namespace OIDC.Infra.Store.Port.Expressions.Abstractions;

/// <summary>
///   Defines a contract for ordering query results.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IOrderThenBy<TEntity> where TEntity : class, IEntity {
  /// <summary>
  ///   Orders the query result by the given key selector in ascending order.
  /// </summary>
  /// <param name="keySelector">The key selector to be ordered by.</param>
  /// <returns>The order by itself.</returns>
  IOrderThenBy<TEntity> ThenByAscending(Expression<Func<TEntity, object?>> keySelector);

  /// <summary>
  ///   Orders the query result by the given key selector in descending order.
  /// </summary>
  /// <param name="keySelector">The key selector to be ordered by.</param>
  /// <returns>The order by itself.</returns>
  IOrderThenBy<TEntity> ThenByDescending(Expression<Func<TEntity, object?>> keySelector);
}
