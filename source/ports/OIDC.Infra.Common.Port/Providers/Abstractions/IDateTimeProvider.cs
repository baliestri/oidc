// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.Infra.Common.Port.Providers.Abstractions;

/// <summary>
///   Provides a way to get the current date and time.
/// </summary>
public interface IDateTimeProvider {
  /// <summary>
  ///   Gets the current date and time.
  /// </summary>
  DateTime Now { get; }

  /// <summary>
  ///   Gets the current date and time with offset.
  /// </summary>
  DateTimeOffset NowOffset { get; }

  /// <summary>
  ///   Gets the current date and time in UTC.
  /// </summary>
  DateTime UtcNow { get; }

  /// <summary>
  ///   Gets the current date and time in UTC with offset.
  /// </summary>
  DateTimeOffset UtcNowOffset { get; }
}
