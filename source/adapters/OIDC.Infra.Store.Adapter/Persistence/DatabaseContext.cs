// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OIDC.Core.Entities.Abstractions;
using OIDC.Infra.Store.Adapter.Extensions;
using OIDC.Infra.Store.Adapter.Persistence.Abstractions;

namespace OIDC.Infra.Store.Adapter.Persistence;

/// <summary>
///   Base class for all database contexts.
/// </summary>
/// <param name="logger">The logger.</param>
/// <param name="options">The options.</param>
[ExcludeFromCodeCoverage]
public abstract class DatabaseContext<TContext>(ILogger<DatabaseContext<TContext>> logger, DbContextOptions<TContext> options)
  : DbContext(options), IDatabaseContext where TContext : DbContext {
  /// <summary>
  ///   Determines whether the context has been disposed.
  /// </summary>
  protected bool IsDisposed { get; private set; }

  /// <inheritdoc />
  public override ValueTask DisposeAsync() {
    if (IsDisposed) {
      return new ValueTask();
    }

    if (ChangeTracker.HasChanges()) {
      foreach (var entry in ChangeTracker.Entries()) {
        if (entry.State is EntityState.Modified or EntityState.Added or EntityState.Deleted) {
          logger.LogWarning("Entity {ClrTypeName} has been modified and not yet committed", entry.Metadata.ClrType.Name);
        }
      }
    }

    IsDisposed = true;

    GC.SuppressFinalize(this);
    return base.DisposeAsync();
  }

  /// <inheritdoc />
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder
      .ApplyConfigurationsFromAssembly(typeof(TContext).Assembly)
      .ConfigureQueryFilter<EntitySoftDelete>(softDelete => !softDelete.IsDeleted);

    base.OnModelCreating(modelBuilder);
  }
}
