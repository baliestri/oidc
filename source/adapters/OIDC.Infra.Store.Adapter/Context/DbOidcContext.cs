// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OIDC.Infra.Store.Adapter.Persistence;

namespace OIDC.Infra.Store.Adapter.Context;

/// <summary>
///   Represents the database context for OIDC.
/// </summary>
/// <param name="logger">The <see cref="ILogger" /> to log messages.</param>
/// <param name="options">The <see cref="DbContextOptions{TContext}" /> to configure the context.</param>
public sealed class DbOidcContext(ILogger<DbOidcContext> logger, DbContextOptions<DbOidcContext> options)
  : DatabaseContext<DbOidcContext>(logger, options);
