// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.Core.Entities.Abstractions;

namespace OIDC.Infra.Store.Port.Expressions.Abstractions;

/// <summary>
///   Defines the contract to extract includes from a queryable.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IIncludableExtract<TEntity> where TEntity : class, IEntity {
  /// <summary>
  ///   Extracts the includes from the queryable.
  /// </summary>
  /// <param name="queryable">The queryable to be included.</param>
  void Extract(out IQueryable<TEntity> queryable);
}
