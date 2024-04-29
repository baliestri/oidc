// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.Infra.Store.Port.Persistence.Abstractions;

/// <summary>
///   Defines a contract for unit of work.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable {
  /// <summary>
  ///   Asynchronously starts a new transaction.
  /// </summary>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A task that represents the asynchronous transaction initialization.</returns>
  /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
  /// <exception cref="InvalidOperationException">If there is already a transaction in progress.</exception>
  Task BeginAsync(CancellationToken cancellationToken = default);

  /// <summary>
  ///   Applies the outstanding operations in the current transaction to the database.
  /// </summary>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A Task representing the asynchronous operation.</returns>
  /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
  /// <exception cref="InvalidOperationException">If there is no transaction in progress.</exception>
  Task CommitAsync(CancellationToken cancellationToken = default);

  /// <summary>
  ///   Discards the outstanding operations in the current transaction.
  /// </summary>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A task that represents the asynchronous transaction initialization.</returns>
  /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
  Task RollbackAsync(CancellationToken cancellationToken = default);
}
