// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OIDC.Core.Entities.Abstractions;
using OIDC.Infra.Store.Adapter.Extensions;
using OIDC.Infra.Store.Port.Expressions.Abstractions;
using OIDC.Infra.Store.Port.Expressions.ComplexTypes;
using OIDC.Infra.Store.Port.Expressions.Enums;

namespace OIDC.Infra.Store.Adapter.Expressions;

internal sealed class IncludableExpressionImpl<TEntity, TProperty> : IIncludableApply<TEntity>, IIncludableExtract<TEntity>,
  IIncludable<TEntity, TProperty> where TEntity : class, IEntity where TProperty : class, IEntity {
  private readonly List<IncludableKeySelector> _items = [];
  private IQueryable<TEntity> _queryable = default!;

  public IncludableExpressionImpl() { }

  private IncludableExpressionImpl(List<IncludableKeySelector> items)
    => _items = items;

  /// <inheritdoc />
  public IIncludable<TEntity, TCurrentProperty> ThenInclude<TCurrentProperty>(Expression<Func<TProperty, TCurrentProperty>> keySelector)
    where TCurrentProperty : class, IEntity {
    _items.Add(keySelector, IncludableIncludeType.ThenInclude);

    return new IncludableExpressionImpl<TEntity, TCurrentProperty>(_items);
  }

  /// <inheritdoc />
  public IIncludable<TEntity, TCurrentProperty> ThenInclude<TCurrentProperty>(Expression<Func<TProperty, IEnumerable<TCurrentProperty>>> keySelector)
    where TCurrentProperty : class, IEntity {
    _items.Add(keySelector, IncludableIncludeType.ThenInclude);

    return new IncludableExpressionImpl<TEntity, TCurrentProperty>(_items);
  }

  /// <inheritdoc />
  public IIncludable<TEntity, TCurrentProperty> Include<TCurrentProperty>(Expression<Func<TEntity, TCurrentProperty>> keySelector)
    where TCurrentProperty : class, IEntity {
    _items.Add(keySelector, IncludableIncludeType.Include);

    return new IncludableExpressionImpl<TEntity, TCurrentProperty>(_items);
  }

  /// <inheritdoc />
  public IIncludable<TEntity, TCurrentProperty> Include<TCurrentProperty>(Expression<Func<TEntity, IEnumerable<TCurrentProperty>>> keySelector)
    where TCurrentProperty : class, IEntity {
    _items.Add(keySelector, IncludableIncludeType.Include);

    return new IncludableExpressionImpl<TEntity, TCurrentProperty>(_items);
  }

  /// <inheritdoc />
  public IIncludableExtract<TEntity> Apply(IQueryable<TEntity> queryable) {
    _queryable = _items
      .Aggregate(
        queryable,
        (current, item) => item.Type switch {
          IncludableIncludeType.Include => EntityFrameworkQueryableExtensions.Include(current, (dynamic)item.KeySelector),
          IncludableIncludeType.ThenInclude => EntityFrameworkQueryableExtensions.ThenInclude(current, (dynamic)item.KeySelector),
          var _ => throw new InvalidOperationException("Invalid include type.")
        }
      );

    return this;
  }

  /// <inheritdoc />
  public void Extract(out IQueryable<TEntity> queryable)
    => queryable = _queryable;
}
