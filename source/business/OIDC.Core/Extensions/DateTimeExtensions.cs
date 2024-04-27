// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using OIDC.Core.Extensions.Utilities;

namespace OIDC.Core.Extensions;

/// <summary>
///   Extension methods for the <see cref="DateTime" />.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DateTimeExtensions {
  /// <summary>
  ///   Converts a <see cref="DateTime" /> to a Unix timestamp.
  /// </summary>
  /// <param name="dateTime">The <see cref="DateTime" /> to be converted.</param>
  /// <returns>The Unix timestamp.</returns>
  [Pure]
  public static long ToEpochTime(this DateTime dateTime) {
    var dateTimeUtc = dateTime;
    if (dateTime.Kind != DateTimeKind.Utc) {
      dateTimeUtc = dateTime.ToUniversalTime();
    }

    if (dateTimeUtc.ToUniversalTime() <= DateTimeUtilities.UnixEpoch) {
      return 0;
    }

    return (long)(dateTimeUtc - DateTimeUtilities.UnixEpoch).TotalSeconds;
  }

  /// <summary>
  ///   Converts a Unix timestamp to a <see cref="DateTime" />.
  /// </summary>
  /// <param name="secondsSinceUnixEpoch">The Unix timestamp to be converted.</param>
  /// <returns>The <see cref="DateTime" />.</returns>
  [Pure]
  public static DateTime FromEpochTime(this long secondsSinceUnixEpoch) {
    if (secondsSinceUnixEpoch <= 0) {
      return DateTimeUtilities.UnixEpoch;
    }

    return secondsSinceUnixEpoch > TimeSpan.MaxValue.TotalSeconds
      ? DateTimeUtilities.Add(DateTimeUtilities.UnixEpoch, TimeSpan.MaxValue).ToUniversalTime()
      : DateTimeUtilities.Add(DateTimeUtilities.UnixEpoch, TimeSpan.FromSeconds(secondsSinceUnixEpoch)).ToUniversalTime();
  }
}
