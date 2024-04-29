// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;

namespace OIDC.Infra.Store.Adapter.Extensions;

[ExcludeFromCodeCoverage]
internal static class CastExtensions {
  /// <summary>
  ///   Cast an object to a specific type.
  /// </summary>
  /// <param name="obj">The object to cast.</param>
  /// <typeparam name="T">The type to cast the object.</typeparam>
  /// <returns>The object casted to the specific type.</returns>
  public static T As<T>(this object? obj) where T : class
    => obj as T ?? throw new InvalidCastException($"Cannot cast the object to {typeof(T).Name}.");
}
