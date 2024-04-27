// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;

namespace OIDC.Core.Extensions.Utilities;

[ExcludeFromCodeCoverage]
internal static class DateTimeUtilities {
  public static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

  public static DateTime Add(DateTime time, TimeSpan timespan) {
    if (timespan == TimeSpan.Zero) {
      return time;
    }

    if (timespan > TimeSpan.Zero &&
        (DateTime.MaxValue - time) <= timespan) {
      return GetMaxValue(time.Kind);
    }

    if (timespan < TimeSpan.Zero &&
        (DateTime.MinValue - time) >= timespan) {
      return GetMinValue(time.Kind);
    }

    return time + timespan;
  }

  public static DateTime GetMaxValue(DateTimeKind kind)
    => kind == DateTimeKind.Unspecified
      ? new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc)
      : new DateTime(DateTime.MaxValue.Ticks, kind);

  public static DateTime GetMinValue(DateTimeKind kind)
    => kind == DateTimeKind.Unspecified
      ? new DateTime(DateTime.MinValue.Ticks, DateTimeKind.Utc)
      : new DateTime(DateTime.MinValue.Ticks, kind);
}
