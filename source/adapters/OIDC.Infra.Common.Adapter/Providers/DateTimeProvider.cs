// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.Infra.Common.Port.Providers.Abstractions;

namespace OIDC.Infra.Common.Adapter.Providers;

/// <inheritdoc cref="IDateTimeProvider" />
internal sealed class DateTimeProvider : IDateTimeProvider {
  /// <inheritdoc />
  public DateTime Now => DateTime.Now;

  /// <inheritdoc />
  public DateTimeOffset NowOffset => DateTimeOffset.Now;

  /// <inheritdoc />
  public DateTime UtcNow => DateTime.UtcNow;

  /// <inheritdoc />
  public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
}
