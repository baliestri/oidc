// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace OIDC.Core.Extensions;

/// <summary>
///   Extension methods for base64 encoding and decoding.
/// </summary>
[ExcludeFromCodeCoverage]
public static class Base64Extensions {
  /// <summary>
  ///   Converts a <see cref="string" /> to a base64 <see cref="byte" /> array.
  /// </summary>
  /// <param name="value">The string to be converted.</param>
  /// <returns>The base64 <see cref="byte" /> array representation of the <see cref="string" />.</returns>
  [Pure]
  public static byte[] FromBase64(this string value)
    => Convert.FromBase64String(value);

  /// <summary>
  ///   Converts a base64 <see cref="byte" /> array to a <see cref="string" />.
  /// </summary>
  /// <param name="value">The base64 byte array.</param>
  /// <returns>The <see cref="string" /> representation of the base64 <see cref="byte" /> array.</returns>
  [Pure]
  public static string ToBase64(this byte[] value)
    => Convert.ToBase64String(value);
}
