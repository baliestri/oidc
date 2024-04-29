// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OIDC.Core.Entities.Abstractions;
using OIDC.Core.Extensions;
using OIDC.Infra.Store.Adapter.Extensions;
using OIDC.Infra.Store.Adapter.Persistence.Abstractions;
using OIDC.Infra.Store.Port.Abstractions;
using OIDC.Infra.Store.Port.ComplexTypes;
using OIDC.Infra.Store.Port.Delegates;

namespace OIDC.Infra.Store.Adapter.Persistence;

/// <summary>
///   Represents a repository.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
internal sealed class Repository<TEntity>(IDatabaseContext databaseContext) : IRepository<TEntity> where TEntity : class, IEntity {
  private bool _shouldIgnoreQueryFilters;

  /// <inheritdoc />
  public async ValueTask DisposeAsync()
    => await databaseContext.DisposeAsync();

  /// <inheritdoc />
  public IReadOnlyRepository<TEntity> AsReadOnly()
    => this;

  /// <inheritdoc />
  public async Task ReAttach(TEntity entity) {
    ArgumentNullException.ThrowIfNull(entity);

    var entry = databaseContext.Entry(entity);

    if (entry.State == EntityState.Detached) {
      databaseContext.Set<TEntity>().Attach(entity);
    }

    await Task.CompletedTask;
  }

  /// <inheritdoc />
  public async Task ReAttach(IEnumerable<TEntity> entities) {
    ArgumentNullException.ThrowIfNull(entities);

    var enumerable = entities.ToList();

    var entries = enumerable.Select(databaseContext.Entry);
    var detachedEntities = entries
      .Where(entry => entry.State == EntityState.Detached)
      .Select(entry => entry.Entity)
      .ToList();

    databaseContext.Set<TEntity>().AttachRange(detachedEntities);

    await Task.CompletedTask;
  }

  /// <inheritdoc />
  public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entity);

    await databaseContext.Set<TEntity>().AddAsync(entity, cancellationToken);
  }

  /// <inheritdoc />
  public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entities);

    await databaseContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
  }

  /// <inheritdoc />
  public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entity);

    await Task.Run(
      async () => {
        await ReAttach(entity);
        entity.UpdatedAt = DateTime.UtcNow;
        databaseContext.Set<TEntity>().Update(entity);
      }, cancellationToken
    );
  }

  /// <inheritdoc />
  public async Task UpdateAsync(Guid id, Action<TEntity> action, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(id);
    ArgumentNullException.ThrowIfNull(action);

    var entity = await GetAsync(id, cancellationToken);

    if (entity is not null) {
      action(entity);
      await UpdateAsync(entity, cancellationToken);
    }
  }

  /// <inheritdoc />
  public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entities);

    await Task.Run(
      async () => {
        var enumerable = entities.ToList();
        await ReAttach(enumerable);
        enumerable.ForEach(entity => entity.UpdatedAt = DateTime.UtcNow);
        databaseContext.Set<TEntity>().UpdateRange(enumerable);
      }, cancellationToken
    );
  }

  /// <inheritdoc />
  public async Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);
    ArgumentNullException.ThrowIfNull(action);

    var entities = await FindAsync(predicate, cancellationToken).ToImmutableListAsync();

    if (entities.Any()) {
      entities.ForEach(action);
      await UpdateAsync(entities, cancellationToken);
    }
  }

  /// <inheritdoc />
  public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entity);

    await Task.Run(
      async () => {
        await ReAttach(entity);
        databaseContext.Set<TEntity>().Remove(entity);
      }, cancellationToken
    );
  }

  /// <inheritdoc />
  public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(entities);

    await Task.Run(
      async () => {
        var enumerable = entities.ToList();
        await ReAttach(enumerable);
        databaseContext.Set<TEntity>().RemoveRange(enumerable);
      }, cancellationToken
    );
  }

  /// <inheritdoc />
  public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);

    var entities = await FindAsync(predicate, cancellationToken).ToImmutableListAsync();

    if (entities.Any()) {
      await DeleteAsync(entities, cancellationToken);
    }
  }

  /// <inheritdoc />
  public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(id);

    var entity = await GetAsync(entity => entity.Id == id, cancellationToken);

    if (entity is not null) {
      await DeleteAsync(entity, cancellationToken);
    }
  }

  /// <inheritdoc />
  public async Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(ids);

    var entities = await FindAsync(entity => ids.Contains(entity.Id), cancellationToken).ToImmutableListAsync();

    if (entities.Any()) {
      await DeleteAsync(entities, cancellationToken);
    }
  }

  /// <inheritdoc />
  public IReadOnlyRepository<TEntity> IgnoreQueryFilters() {
    _shouldIgnoreQueryFilters = true;

    return this;
  }

  /// <inheritdoc />
  public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    => await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .CountAsync(cancellationToken);

  /// <inheritdoc />
  public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .CountAsync(predicate, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .AnyAsync(predicate, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(id);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .AnyAsync(entity => entity.Id == id, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<TEntity?> GetAsync(Guid id, IncludableExpression<TEntity> includable, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(id);
    ArgumentNullException.ThrowIfNull(includable);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .UseIncludable(includable)
      .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .FirstOrDefaultAsync(predicate, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, IncludableExpression<TEntity> includable,
  CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);
    ArgumentNullException.ThrowIfNull(includable);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .UseIncludable(includable)
      .FirstOrDefaultAsync(predicate, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(id);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IEnumerable<TEntity>> FindAsync(CancellationToken cancellationToken = default)
    => await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .ToListAsync(cancellationToken);

  /// <inheritdoc />
  public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .Where(predicate)
      .ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, OrderByExpression<TEntity> orderBy,
  CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);
    ArgumentNullException.ThrowIfNull(orderBy);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .Where(predicate)
      .UseOrderBy(orderBy)
      .ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IEnumerable<TEntity>> FindAsync(OrderByExpression<TEntity> orderBy, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(orderBy);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .UseOrderBy(orderBy)
      .ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IEnumerable<TEntity>> FindAsync(IncludableExpression<TEntity> includable, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(includable);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .UseIncludable(includable)
      .ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, IncludableExpression<TEntity> includable,
  CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);
    ArgumentNullException.ThrowIfNull(includable);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .Where(predicate)
      .UseIncludable(includable)
      .ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, OrderByExpression<TEntity> orderBy,
  IncludableExpression<TEntity> includable, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);
    ArgumentNullException.ThrowIfNull(orderBy);
    ArgumentNullException.ThrowIfNull(includable);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .Where(predicate)
      .UseOrderBy(orderBy)
      .UseIncludable(includable)
      .ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IEnumerable<TEntity>> FindAsync(OrderByExpression<TEntity> orderBy, IncludableExpression<TEntity> includable,
  CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(orderBy);
    ArgumentNullException.ThrowIfNull(includable);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .UseOrderBy(orderBy)
      .UseIncludable(includable)
      .ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<PaginatedSet<TEntity>> PaginateAsync(int currentPage, int perPage, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(currentPage);
    ArgumentNullException.ThrowIfNull(perPage);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .ToPaginatedSetAsync(currentPage, perPage, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<PaginatedSet<TEntity>> PaginateAsync(Expression<Func<TEntity, bool>> predicate, int currentPage, int perPage,
  CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);
    ArgumentNullException.ThrowIfNull(currentPage);
    ArgumentNullException.ThrowIfNull(perPage);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .Where(predicate)
      .ToPaginatedSetAsync(currentPage, perPage, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<PaginatedSet<TEntity>> PaginateAsync(OrderByExpression<TEntity> orderBy, int currentPage, int perPage,
  CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(orderBy);
    ArgumentNullException.ThrowIfNull(currentPage);
    ArgumentNullException.ThrowIfNull(perPage);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .UseOrderBy(orderBy)
      .ToPaginatedSetAsync(currentPage, perPage, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<PaginatedSet<TEntity>> PaginateAsync(Expression<Func<TEntity, bool>> predicate, OrderByExpression<TEntity> orderBy,
  int currentPage, int perPage, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);
    ArgumentNullException.ThrowIfNull(orderBy);
    ArgumentNullException.ThrowIfNull(currentPage);
    ArgumentNullException.ThrowIfNull(perPage);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .Where(predicate)
      .UseOrderBy(orderBy)
      .ToPaginatedSetAsync(currentPage, perPage, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<PaginatedSet<TEntity>> PaginateAsync(IncludableExpression<TEntity> includable, int currentPage, int perPage,
  CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(includable);
    ArgumentNullException.ThrowIfNull(currentPage);
    ArgumentNullException.ThrowIfNull(perPage);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .UseIncludable(includable)
      .ToPaginatedSetAsync(currentPage, perPage, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<PaginatedSet<TEntity>> PaginateAsync(Expression<Func<TEntity, bool>> predicate, IncludableExpression<TEntity> includable,
  int currentPage, int perPage, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);
    ArgumentNullException.ThrowIfNull(includable);
    ArgumentNullException.ThrowIfNull(currentPage);
    ArgumentNullException.ThrowIfNull(perPage);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .Where(predicate)
      .UseIncludable(includable)
      .ToPaginatedSetAsync(currentPage, perPage, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<PaginatedSet<TEntity>> PaginateAsync(OrderByExpression<TEntity> orderBy, IncludableExpression<TEntity> includable,
  int currentPage, int perPage, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(orderBy);
    ArgumentNullException.ThrowIfNull(includable);
    ArgumentNullException.ThrowIfNull(currentPage);
    ArgumentNullException.ThrowIfNull(perPage);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .UseOrderBy(orderBy)
      .UseIncludable(includable)
      .ToPaginatedSetAsync(currentPage, perPage, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<PaginatedSet<TEntity>> PaginateAsync(Expression<Func<TEntity, bool>> predicate, OrderByExpression<TEntity> orderBy,
  IncludableExpression<TEntity> includable, int currentPage, int perPage, CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(predicate);
    ArgumentNullException.ThrowIfNull(orderBy);
    ArgumentNullException.ThrowIfNull(includable);
    ArgumentNullException.ThrowIfNull(currentPage);
    ArgumentNullException.ThrowIfNull(perPage);

    return await databaseContext
      .Set<TEntity>()
      .AsNoTracking()
      .TryIgnoreQueryFilters(_shouldIgnoreQueryFilters)
      .Where(predicate)
      .UseOrderBy(orderBy)
      .UseIncludable(includable)
      .ToPaginatedSetAsync(currentPage, perPage, cancellationToken);
  }
}
