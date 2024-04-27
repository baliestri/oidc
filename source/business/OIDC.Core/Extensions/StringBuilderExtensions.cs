// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace OIDC.Core.Extensions;

/// <summary>
///   Extension methods for <see cref="StringBuilder" />.
/// </summary>
[ExcludeFromCodeCoverage]
public static class StringBuilderExtensions {
  /// <summary>
  ///   Appends a <see cref="string" /> to the end of the <see cref="StringBuilder" /> if the condition is <c>true</c>.
  /// </summary>
  /// <param name="builder">The <see cref="StringBuilder" /> to append to.</param>
  /// <param name="formattableString">The string to append.</param>
  /// <param name="condition">The condition to check.</param>
  /// <returns>The <see cref="StringBuilder" /> itself.</returns>
  public static StringBuilder TryAppend(this StringBuilder builder, FormattableString? formattableString, bool condition) {
    if (condition) {
      builder.Append(formattableString);
    }

    return builder;
  }

  /// <summary>
  ///   Appends a <see cref="string" /> to the end of the <see cref="StringBuilder" /> if the condition is <c>true</c>.
  /// </summary>
  /// <param name="builder">The <see cref="StringBuilder" /> to append to.</param>
  /// <param name="string">The string to append.</param>
  /// <param name="condition">The condition to check.</param>
  /// <returns>The <see cref="StringBuilder" /> itself.</returns>
  public static StringBuilder TryAppend(this StringBuilder builder, string? @string, bool condition) {
    if (condition) {
      builder.Append(@string);
    }

    return builder;
  }
}
