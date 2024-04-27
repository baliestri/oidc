// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using OIDC.ResourceServer.Extensions.Enrichers;

namespace OIDC.ResourceServer.Extensions;

/// <summary>
///   Extension methods for <see cref="HttpContext" />.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class HttpContextExtensions {
  /// <summary>
  ///   Gets the correlation ID from the HTTP context.
  /// </summary>
  /// <param name="httpContext">The <see cref="HttpContext" /> to get the correlation ID from.</param>
  /// <returns>The correlation ID.</returns>
  public static string GetCorrelationId(this HttpContext httpContext) {
    var header = string.Empty;

    if (httpContext.Request.Headers.TryGetValue(CorrelationIdEnricher.HEADER_NAME, out var values)) {
      header = values.FirstOrDefault();
    }
    else if (httpContext.Response.Headers.TryGetValue(CorrelationIdEnricher.HEADER_NAME, out values)) {
      header = values.FirstOrDefault();
    }

    var correlationId = string.IsNullOrEmpty(header)
      ? httpContext.TraceIdentifier
      : header;

    if (!httpContext.Response.Headers.ContainsKey(CorrelationIdEnricher.HEADER_NAME)) {
      httpContext.Response.Headers.Append(CorrelationIdEnricher.HEADER_NAME, correlationId);
    }

    return correlationId;
  }
}
