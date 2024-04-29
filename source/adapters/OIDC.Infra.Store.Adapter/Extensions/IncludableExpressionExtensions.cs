// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using OIDC.Core.Entities.Abstractions;
using OIDC.Infra.Store.Adapter.Expressions;
using OIDC.Infra.Store.Port.Delegates;
using OIDC.Infra.Store.Port.Expressions.Abstractions;

namespace OIDC.Infra.Store.Adapter.Extensions;

[ExcludeFromCodeCoverage]
internal static class IncludableExpressionExtensions {
  /// <summary>
  ///   Use an includable expression for the entity.
  /// </summary>
  /// <param name="queryable">The queryable to apply the includable expression.</param>
  /// <param name="includableExpression">The includable expression.</param>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  /// <returns>The queryable with the includable expression applied.</returns>
  public static IQueryable<TEntity> UseIncludable<TEntity>(this IQueryable<TEntity> queryable, IncludableExpression<TEntity> includableExpression)
    where TEntity : class, IEntity {
    var includableExpressionImpl = new IncludableExpressionImpl<TEntity, TEntity>();
    var includable = includableExpression(includableExpressionImpl);
    var includableApply = includable as IIncludableApply<TEntity> ??
                          throw new InvalidCastException("The includable expression is not ready to be applied.");

    includableApply
      .Apply(queryable)
      .Extract(out var queryableWithIncludes);

    return queryableWithIncludes;
  }
}
