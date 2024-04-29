// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.Core.Entities.Abstractions;

namespace OIDC.Infra.Store.Port.Expressions.Abstractions;

/// <summary>
///   Defines the contract to apply includes to a queryable.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IIncludableApply<TEntity> where TEntity : class, IEntity {
  /// <summary>
  ///   Applies the includes to the queryable.
  /// </summary>
  /// <param name="queryable">The queryable to be included.</param>
  /// <returns>The includable queryable extractor.</returns>
  IIncludableExtract<TEntity> Apply(IQueryable<TEntity> queryable);
}
