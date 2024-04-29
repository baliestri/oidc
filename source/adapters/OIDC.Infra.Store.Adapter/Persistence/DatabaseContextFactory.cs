// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Microsoft.EntityFrameworkCore;
using OIDC.Infra.Store.Adapter.Persistence.Abstractions;

namespace OIDC.Infra.Store.Adapter.Persistence;

internal sealed class DatabaseContextFactory<TContext>(IDbContextFactory<TContext> databaseContextFactory)
  : IDatabaseContextFactory where TContext : DbContext, IDatabaseContext {
  /// <inheritdoc />
  public async Task<IDatabaseContext> CreateAsync(CancellationToken cancellationToken = default)
    => await databaseContextFactory.CreateDbContextAsync(cancellationToken);
}
