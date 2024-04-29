// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.Core.Entities.Abstractions;
using OIDC.Infra.Store.Port.Expressions.Abstractions;

namespace OIDC.Infra.Store.Port.Delegates;

/// <summary>
///   Expression to build the including used in the ORM fluently.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public delegate IIncludable<TEntity> IncludableExpression<TEntity>(IIncludable<TEntity> includable)
  where TEntity : class, IEntity;
