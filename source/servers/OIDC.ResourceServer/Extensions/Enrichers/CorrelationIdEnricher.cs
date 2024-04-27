// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Serilog.Core;
using Serilog.Events;

namespace OIDC.ResourceServer.Extensions.Enrichers;

/// <summary>
///   Enriches the log event with the correlation ID.
/// </summary>
/// <param name="serviceCollection">The <see cref="IServiceCollection" /> to get the services from.</param>
internal sealed class CorrelationIdEnricher(IServiceCollection serviceCollection) : ILogEventEnricher {
  public const string PROPERTY_NAME = "CorrelationId";
  public const string HEADER_NAME = "X-Correlation-Id";

  /// <inheritdoc />
  public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
    using var scope = serviceCollection.BuildServiceProvider().CreateScope();
    var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

    var correlationId = httpContextAccessor.HttpContext?.GetCorrelationId();
    var correlationIdProperty = new LogEventProperty(PROPERTY_NAME, new ScalarValue(correlationId));

    logEvent.AddPropertyIfAbsent(correlationIdProperty);
  }
}
