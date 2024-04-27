// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.ResourceServer.Extensions.Enrichers;
using Serilog;
using Serilog.Configuration;

namespace OIDC.ResourceServer.Extensions;

/// <summary>
///   Extension methods for the <see cref="LoggerConfiguration" />.
/// </summary>
[ExcludeFromDescription]
internal static class LoggerConfigurationExtensions {
  /// <summary>
  ///   Adds the correlation ID enricher to the logger configuration.
  /// </summary>
  /// <param name="loggerConfiguration">The <see cref="LoggerEnrichmentConfiguration" /> to add the enricher to.</param>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to get the services from.</param>
  /// <returns>The <see cref="LoggerConfiguration" /> itself.</returns>
  public static LoggerConfiguration WithCorrelationId(this LoggerEnrichmentConfiguration loggerConfiguration, IServiceCollection serviceCollection)
    => loggerConfiguration.With(new CorrelationIdEnricher(serviceCollection));
}
