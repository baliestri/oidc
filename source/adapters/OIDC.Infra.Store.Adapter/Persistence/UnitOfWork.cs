// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OIDC.Infra.Store.Adapter.Persistence.Abstractions;
using OIDC.Infra.Store.Port.Persistence.Abstractions;

namespace OIDC.Infra.Store.Adapter.Persistence;

internal sealed class UnitOfWork(ILogger<UnitOfWork> logger, IDatabaseContext databaseContext) : IUnitOfWork {
  private bool _disposed;
  private IDbContextTransaction? _transaction;

  /// <inheritdoc />
  public async ValueTask DisposeAsync() {
    logger.LogDebug("Disposing the unit of work...");

    if (_disposed) {
      return;
    }

    if (_transaction is not null) {
      await _transaction.RollbackAsync();
      await _transaction.DisposeAsync();

      _transaction = null;
    }

    _disposed = true;
    await databaseContext.DisposeAsync();
  }

  /// <inheritdoc />
  public async Task BeginAsync(CancellationToken cancellationToken = default) {
    logger.LogInformation("Starting a new transaction...");

    if (_transaction is not null) {
      throw new InvalidOperationException("The transaction has already been started.");
    }

    _transaction = await databaseContext.Database.BeginTransactionAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task CommitAsync(CancellationToken cancellationToken = default) {
    logger.LogInformation("Committing the transaction...");

    if (_transaction is null) {
      throw new InvalidOperationException("The transaction has not been started.");
    }

    await databaseContext.SaveChangesAsync(cancellationToken);
    await _transaction.CommitAsync(cancellationToken);
    await _transaction.DisposeAsync();
    _transaction = null;
  }

  /// <inheritdoc />
  public async Task RollbackAsync(CancellationToken cancellationToken = default) {
    logger.LogInformation("Rolling back the transaction...");

    if (_transaction is null) {
      logger.LogWarning("Nothing to do, the transaction has not been started");

      return;
    }

    await _transaction.RollbackAsync(cancellationToken);
    await _transaction.DisposeAsync();
    _transaction = null;
  }
}
