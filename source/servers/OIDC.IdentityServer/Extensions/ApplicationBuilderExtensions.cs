// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using OIDC.Core.Common;
using OIDC.Core.Options;
using Serilog;
using Serilog.AspNetCore;

namespace OIDC.IdentityServer.Extensions;

/// <summary>
///   Extension methods for <see cref="IApplicationBuilder" />.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class ApplicationBuilderExtensions {
  /// <summary>
  ///   Uses secure middlewares for the application.
  /// </summary>
  /// <param name="applicationBuilder">The <see cref="IApplicationBuilder" /> to add the secure middlewares to.</param>
  /// <param name="serviceProvider">The <see cref="IServiceProvider" /> to get the services from.</param>
  /// <returns>The <see cref="IApplicationBuilder" /> itself.</returns>
  public static IApplicationBuilder UseSecureMiddlewares(this IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider) {
    var corsOptions = serviceProvider.GetRequiredService<IOptions<CorsOptions>>().Value;

    applicationBuilder
      .UseExceptionHandler()
      .UseHsts()
      .UseRouting();

    if (corsOptions.IsEnabled) {
      applicationBuilder.UseCors(corsOptions.AllowAnyOrigin
        ? OIDCConstants.Cors.ANY_ORIGIN_POLICY
        : OIDCConstants.Cors.ALLOWED_ORIGINS_POLICY);
    }

    return applicationBuilder;
  }

  /// <summary>
  ///   Uses the authentication and authorization middlewares.
  /// </summary>
  /// <param name="applicationBuilder">The <see cref="IApplicationBuilder" /> to add the authentication and authorization middlewares to.</param>
  /// <returns>The <see cref="IApplicationBuilder" /> itself.</returns>
  public static IApplicationBuilder UseAuth(this IApplicationBuilder applicationBuilder) {
    applicationBuilder
      .UseAuthentication()
      .UseAuthorization();

    return applicationBuilder;
  }

  /// <summary>
  ///   Uses the OpenAPI options for development.
  /// </summary>
  /// <param name="applicationBuilder">The <see cref="IApplicationBuilder" /> to add the OpenAPI options to.</param>
  /// <param name="endpointRouteBuilder">The <see cref="IEndpointRouteBuilder" /> to get the API versions.</param>
  /// <param name="environment">The <see cref="IWebHostEnvironment" /> to check the environment.</param>
  /// <returns>The <see cref="IApplicationBuilder" /> itself.</returns>
  public static IApplicationBuilder UseOpenApiForDevelopment(this IApplicationBuilder applicationBuilder, IEndpointRouteBuilder endpointRouteBuilder,
  IWebHostEnvironment environment) {
    var isDevelopment = environment.IsDevelopment();

    if (isDevelopment) {
      var apiVersionDescriptions = endpointRouteBuilder.DescribeApiVersions();

      applicationBuilder.UseSwagger();
      applicationBuilder.UseSwaggerUI(options => {
        foreach (var apiVersionDescription in apiVersionDescriptions) {
          var name = apiVersionDescription.GroupName;
          var url = $"/swagger/{name}/swagger.json";

          options.SwaggerEndpoint(url, name);
        }
      });
    }

    return applicationBuilder;
  }

  /// <inheritdoc cref="SerilogApplicationBuilderExtensions.UseSerilogRequestLogging(Microsoft.AspNetCore.Builder.IApplicationBuilder,string)" />
  public static IApplicationBuilder UseSerilogRequestLogging(this IApplicationBuilder applicationBuilder,
  Action<RequestLoggingOptions>? configureOptions = null)
    => SerilogApplicationBuilderExtensions.UseSerilogRequestLogging(applicationBuilder, configureOptions);
}
