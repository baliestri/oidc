// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using OIDC.ResourceServer.Extensions.Delegates;

namespace OIDC.ResourceServer.Extensions;

/// <summary>
///   Extension methods for <see cref="WebApplication" />.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class WebApplicationExtensions {
  /// <summary>
  ///   Registers the components of the application.
  /// </summary>
  /// <param name="app">The <see cref="WebApplication" /> to register the components to.</param>
  /// <param name="configure">The action to configure the components.</param>
  /// <returns>The <see cref="WebApplication" /> itself.</returns>
  public static WebApplication RegisterComponents(this WebApplication app, ConfigureWebApplication configure) {
    var configuration = app.Configuration;
    var serviceProvider = app.Services;
    var environment = app.Environment;
    var logger = app.Logger;

    configure(app, app, configuration, serviceProvider, environment, logger);

    return app;
  }
}
