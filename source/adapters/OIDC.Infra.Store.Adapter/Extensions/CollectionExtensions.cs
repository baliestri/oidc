// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using OIDC.Core.Entities.Abstractions;
using OIDC.Infra.Store.Port.Expressions.ComplexTypes;
using OIDC.Infra.Store.Port.Expressions.Enums;

namespace OIDC.Infra.Store.Adapter.Extensions;

[ExcludeFromCodeCoverage]
internal static class CollectionExtensions {
  /// <summary>
  ///   Adds a key selector to the list.
  /// </summary>
  /// <param name="list">The list of key selectors.</param>
  /// <param name="keySelector">The key selector to be added.</param>
  /// <param name="direction">The order direction.</param>
  /// <typeparam name="TEntity">The entity type.</typeparam>
  public static void Add<TEntity>(this List<OrderByKeySelector<TEntity>> list, Expression<Func<TEntity, object?>> keySelector,
  OrderByDirection direction) where TEntity : class, IEntity
    => list.Add(new OrderByKeySelector<TEntity>(keySelector, direction));

  /// <summary>
  ///   Adds a key selector to the list.
  /// </summary>
  /// <param name="list">The list of key selectors.</param>
  /// <param name="navigationPropertyPath">The navigation property path.</param>
  /// <param name="includeType">The include type.</param>
  public static void Add(this List<IncludableKeySelector> list, LambdaExpression navigationPropertyPath, IncludableIncludeType includeType)
    => list.Add(new IncludableKeySelector(navigationPropertyPath, includeType));
}
