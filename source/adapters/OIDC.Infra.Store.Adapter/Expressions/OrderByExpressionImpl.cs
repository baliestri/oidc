// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Linq.Expressions;
using OIDC.Core.Entities.Abstractions;
using OIDC.Infra.Store.Adapter.Extensions;
using OIDC.Infra.Store.Port.Expressions.Abstractions;
using OIDC.Infra.Store.Port.Expressions.ComplexTypes;
using OIDC.Infra.Store.Port.Expressions.Enums;

namespace OIDC.Infra.Store.Adapter.Expressions;

internal sealed class OrderByExpressionImpl<TEntity> : IOrderByApply<TEntity>, IOrderByExtract<TEntity>, IOrderBy<TEntity>,
  IOrderThenBy<TEntity> where TEntity : class, IEntity {
  private readonly List<OrderByKeySelector<TEntity>> _items = [];
  private IQueryable<TEntity> _orderedQueryable = default!;

  /// <inheritdoc />
  public IOrderThenBy<TEntity> ByAscending(Expression<Func<TEntity, object?>> keySelector) {
    _items.Add(keySelector, OrderByDirection.Ascending);

    return this;
  }

  /// <inheritdoc />
  public IOrderThenBy<TEntity> ByDescending(Expression<Func<TEntity, object?>> keySelector) {
    _items.Add(keySelector, OrderByDirection.Descending);

    return this;
  }

  /// <inheritdoc />
  public IOrderByExtract<TEntity> Apply(IQueryable<TEntity> queryable) {
    if (_items.Count == 0) {
      throw new InvalidOperationException("No ordering was specified.");
    }

    var orderBy = _items[0];

    var orderByKeySelector = orderBy.KeySelector.As<Expression<Func<TEntity, object?>>>();

    var orderedQueryable = orderBy.Direction == OrderByDirection.Ascending
      ? queryable.OrderBy(orderByKeySelector)
      : queryable.OrderByDescending(orderByKeySelector);

    _orderedQueryable = _items
      .Skip(1)
      .Aggregate(
        orderedQueryable,
        (current, item) => {
          var thenByKeySelector = item.KeySelector.As<Expression<Func<TEntity, object?>>>();

          return item.Direction == OrderByDirection.Ascending
            ? current.ThenBy(thenByKeySelector)
            : current.ThenByDescending(thenByKeySelector);
        }
      );

    return this;
  }

  /// <inheritdoc />
  public void Extract(out IQueryable<TEntity> orderedQueryable)
    => orderedQueryable = _orderedQueryable;

  /// <inheritdoc />
  public IOrderThenBy<TEntity> ThenByAscending(Expression<Func<TEntity, object?>> keySelector) {
    _items.Add(keySelector, OrderByDirection.Ascending);

    return this;
  }

  /// <inheritdoc />
  public IOrderThenBy<TEntity> ThenByDescending(Expression<Func<TEntity, object?>> keySelector) {
    _items.Add(keySelector, OrderByDirection.Descending);

    return this;
  }
}
