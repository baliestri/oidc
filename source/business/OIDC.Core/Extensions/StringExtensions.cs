// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace OIDC.Core.Extensions;

/// <summary>
///   Extension methods for <see cref="string" /> type.
/// </summary>
[ExcludeFromCodeCoverage]
public static partial class StringExtensions {
  /// <summary>
  ///   Transforms a string to snake case.
  /// </summary>
  /// <param name="value">The string to be transformed.</param>
  /// <returns>The string in snake case.</returns>
  public static string ToSnakeCase(this string value)
    => string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();

  [GeneratedRegex(@"{{\s*(.*?)\s*}}")]
  private static partial Regex replaceKeyInStringRegex();
}
