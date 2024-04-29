// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Linq.Expressions;
using OIDC.Core.Entities.Abstractions;
using OIDC.Infra.Store.Port.Expressions.Enums;

namespace OIDC.Infra.Store.Port.Expressions.ComplexTypes;

/// <summary>
///   Represents a key selector to be used in an order by clause.
/// </summary>
/// <param name="KeySelector">The key selector to be used in the order by clause.</param>
/// <param name="Direction">The direction of the order by clause.</param>
/// <typeparam name="TEntity">The type of the entity to be ordered.</typeparam>
public readonly record struct OrderByKeySelector<TEntity>(Expression<Func<TEntity, object?>> KeySelector, OrderByDirection Direction)
  where TEntity : class, IEntity;
