// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Diagnostics;

namespace OIDC.IdentityServer.Extensions;

/// <summary>
///   Extension methods for <see cref="IEndpointRouteBuilder" />.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class EndpointRouteBuilderExtensions {
  /// <summary>
  ///   Maps the exception handling route.
  /// </summary>
  /// <param name="endpointRouteBuilder">The <see cref="IEndpointRouteBuilder" /> to map the route to.</param>
  /// <param name="environment">The <see cref="IWebHostEnvironment" /> to check the environment.</param>
  /// <returns>The <see cref="IEndpointRouteBuilder" /> itself.</returns>
  public static IEndpointRouteBuilder MapExceptionHandlingRoute(this IEndpointRouteBuilder endpointRouteBuilder, IWebHostEnvironment environment) {
    var isDevelopment = environment.IsDevelopment();

    endpointRouteBuilder
      .Map("/api/error", (HttpContext httpContext) => {
        var context = httpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context?.Error;
        var extensions = new Dictionary<string, object?>();

        if (exception is not null) {
          extensions.Add("innerException", exception.InnerException?.Message);
        }

        extensions.Add("traceId", Activity.Current?.Id ?? httpContext.TraceIdentifier);

        return isDevelopment
          ? Results.Problem(
            exception?.Message,
            statusCode: 500,
            title: "An error occurred while processing your request.",
            instance: context?.Path,
            extensions: extensions
          )
          : Results.Problem(title: "An error occurred while processing your request.", statusCode: 500);
      });

    return endpointRouteBuilder;
  }
}
