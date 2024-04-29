// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using OIDC.Core.Entities.Abstractions;
using OIDC.Infra.Store.Adapter.Expressions;
using OIDC.Infra.Store.Port.Delegates;
using OIDC.Infra.Store.Port.Expressions.Abstractions;

namespace OIDC.Infra.Store.Adapter.Extensions;

[ExcludeFromCodeCoverage]
internal static class OrderByExpressionExtensions {
  /// <summary>
  ///   Use an order by expression for the entity.
  /// </summary>
  /// <param name="queryable">The queryable to apply the order by expression.</param>
  /// <param name="orderByExpression">The order by expression.</param>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  /// <returns>The queryable with the order by expression applied.</returns>
  public static IQueryable<TEntity> UseOrderBy<TEntity>(this IQueryable<TEntity> queryable, OrderByExpression<TEntity> orderByExpression)
    where TEntity : class, IEntity {
    var orderByExpressionImpl = new OrderByExpressionImpl<TEntity>();
    var orderBy = orderByExpression(orderByExpressionImpl);
    var orderByApply = orderBy as IOrderByApply<TEntity> ??
                       throw new InvalidOperationException("The order by expression must be ready to be applied.");

    orderByApply
      .Apply(queryable)
      .Extract(out var orderedQueryable);

    return orderedQueryable;
  }
}
