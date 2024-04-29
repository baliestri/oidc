// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Linq.Expressions;
using OIDC.Core.Entities.Abstractions;

namespace OIDC.Infra.Store.Port.Expressions.Abstractions;

/// <summary>
///   Defines a contract for including related entities in a query.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IIncludable<TEntity> where TEntity : class, IEntity {
  /// <summary>
  ///   Specifies related entities to include in the query results.
  /// </summary>
  /// <param name="keySelector">A lambda expression representing the navigation property to be included.</param>
  /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
  /// <returns>The includable itself.</returns>
  IIncludable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> keySelector) where TProperty : class, IEntity;

  /// <summary>
  ///   Specifies related entities to include in the query results.
  /// </summary>
  /// <param name="keySelector">A lambda expression representing the navigation property to be included.</param>
  /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
  /// <returns>The includable itself.</returns>
  IIncludable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> keySelector) where TProperty : class, IEntity;
}

/// <summary>
///   Defines a contract for including related entities in a query.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
public interface IIncludable<TEntity, TProperty> : IIncludable<TEntity> where TEntity : class, IEntity where TProperty : class, IEntity {
  /// <summary>
  ///   Specifies additional related entities to include in the query results.
  /// </summary>
  /// <param name="keySelector">A lambda expression representing the navigation property to be included.</param>
  /// <typeparam name="TCurrentProperty">The type of the related entity to be included.</typeparam>
  /// <returns>The includable itself.</returns>
  IIncludable<TEntity, TCurrentProperty> ThenInclude<TCurrentProperty>(Expression<Func<TProperty, TCurrentProperty>> keySelector)
    where TCurrentProperty : class, IEntity;

  /// <summary>
  ///   Specifies additional related entities to include in the query results.
  /// </summary>
  /// <param name="keySelector">A lambda expression representing the navigation property to be included.</param>
  /// <typeparam name="TCurrentProperty">The type of the related entity to be included.</typeparam>
  /// <returns>The includable itself.</returns>
  IIncludable<TEntity, TCurrentProperty> ThenInclude<TCurrentProperty>(Expression<Func<TProperty, IEnumerable<TCurrentProperty>>> keySelector)
    where TCurrentProperty : class, IEntity;
}
