// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace OIDC.Infra.Store.Adapter.Extensions;

[ExcludeFromCodeCoverage]
internal static class ConfigurationExtensions {
  /// <summary>
  ///   Gets the default connection string from the configuration.
  /// </summary>
  /// <param name="configuration">The configuration.</param>
  /// <returns>The default connection string.</returns>
  public static string? GetDefaultConnectionString(this IConfiguration configuration) {
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    return connectionString;
  }
}
