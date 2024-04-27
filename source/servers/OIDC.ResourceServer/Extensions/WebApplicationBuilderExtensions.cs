// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using OIDC.ResourceServer.Extensions.Delegates;

namespace OIDC.ResourceServer.Extensions;

/// <summary>
///   Extension methods for <see cref="WebApplicationBuilder" />.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class WebApplicationBuilderExtensions {
  /// <summary>
  ///   Registers the components of the application builder.
  /// </summary>
  /// <param name="builder">The <see cref="WebApplicationBuilder" /> to register the components to.</param>
  /// <param name="configure">The action to configure the components.</param>
  /// <returns>The configured <see cref="WebApplication" /> instance.</returns>
  public static WebApplication RegisterPipelines(this WebApplicationBuilder builder, ConfigureWebApplicationBuilder configure) {
    var configuration = builder.Configuration;
    var serviceCollection = builder.Services;
    var hostEnvironment = builder.Environment;
    var loggingBuilder = builder.Logging;
    var metricsBuilder = builder.Metrics;

    configure(configuration, serviceCollection, hostEnvironment, loggingBuilder, metricsBuilder);

    return builder.Build();
  }
}
