// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.Infra.Store.Adapter.Persistence.Abstractions;

/// <summary>
///   Defines a contract for a database context factory.
/// </summary>
public interface IDatabaseContextFactory {
  /// <summary>
  ///   Creates a new <see cref="IDatabaseContext" /> instance in an async context.
  /// </summary>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A task containing the created context that represents the asynchronous operation.</returns>
  /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
  Task<IDatabaseContext> CreateAsync(CancellationToken cancellationToken = default);
}
