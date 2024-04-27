// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace OIDC.Core.Extensions;

/// <summary>
///   Extension methods for assertions.
/// </summary>
[ExcludeFromCodeCoverage]
public static class AssertionExtensions {
  /// <summary>
  ///   Checks if a collection is null or empty.
  /// </summary>
  /// <param name="source">The collection to be checked.</param>
  /// <typeparam name="T">The type of the collection.</typeparam>
  /// <returns><c>true</c> if the collection is null or empty; otherwise, <c>false</c>.</returns>
  [Pure]
  public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? source)
    => source is null || !source.Any();

  /// <summary>
  ///   Checks if a <see cref="string" /> is null or empty.
  /// </summary>
  /// <param name="value">The string to be checked.</param>
  /// <returns><c>true</c> if the string is null or empty; otherwise, <c>false</c>.</returns>
  [Pure]
  public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value)
    => string.IsNullOrEmpty(value);
}
