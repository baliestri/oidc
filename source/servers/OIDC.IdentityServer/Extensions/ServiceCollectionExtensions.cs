// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using OIDC.Core.Common;
using OIDC.Core.Extensions;
using OIDC.Core.Options;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Templates;
using Serilog.Templates.Themes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OIDC.IdentityServer.Extensions;

/// <summary>
///   Extension methods for the <see cref="IServiceCollection" />.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions {
  /// <summary>
  ///   Configures the OpenAPI generator.
  /// </summary>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the OpenAPI generator to.</param>
  /// <returns>The <see cref="IServiceCollection" /> itself.</returns>
  public static IServiceCollection ConfigureOpenApiGenerator(this IServiceCollection serviceCollection) {
    serviceCollection
      .AddEndpointsApiExplorer()
      .AddSwaggerGen();

    return serviceCollection;
  }

  /// <summary>
  ///   Configures the Minimal API versioning.
  /// </summary>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the versioning to.</param>
  /// <returns>The <see cref="IServiceCollection" /> itself.</returns>
  public static IServiceCollection ConfigureApiVersioning(this IServiceCollection serviceCollection) {
    serviceCollection
      .AddApiVersioning(options => {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1);
        options.ApiVersionReader = ApiVersionReader.Combine(
          new UrlSegmentApiVersionReader(),
          new QueryStringApiVersionReader("api-version"),
          new HeaderApiVersionReader("X-Version"),
          new MediaTypeApiVersionReader("x-version"));
      })
      .AddApiExplorer(options => {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
      })
      .EnableApiVersionBinding();

    return serviceCollection;
  }

  /// <summary>
  ///   Configures the API router options.
  /// </summary>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the routing to.</param>
  /// <param name="configuration">The <see cref="IConfiguration" /> to get the allowed hosts from.</param>
  /// <returns>The <see cref="IServiceCollection" /> itself.</returns>
  /// <exception cref="InvalidOperationException">Thrown when the allowed hosts are not found in the configuration.</exception>
  public static IServiceCollection ConfigureApiRouter(this IServiceCollection serviceCollection, IConfiguration configuration) {
    using var scope = serviceCollection.BuildServiceProvider().CreateScope();
    var corsOptions = scope.ServiceProvider.GetRequiredService<IOptions<CorsOptions>>().Value;

    serviceCollection
      .AddRouting(options => {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
      })
      .AddProblemDetails()
      .AddCors(options => {
        options
          .AddPolicy(OIDCConstants.Cors.ANY_ORIGIN_POLICY, policyBuilder => policyBuilder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

        options.AddPolicy(OIDCConstants.Cors.ALLOWED_ORIGINS_POLICY, policyBuilder => policyBuilder
          .WithOrigins(corsOptions.AllowedOrigins)
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials());
      });

    return serviceCollection;
  }

  /// <summary>
  ///   Configures the JSON serializer options.
  /// </summary>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the JSON options to.</param>
  /// <returns>The <see cref="IServiceCollection" /> itself.</returns>
  public static IServiceCollection ConfigureJsonSerializerOptions(this IServiceCollection serviceCollection) {
    serviceCollection
      .ConfigureHttpJsonOptions(options => {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
      })
      .AddTransient<ISerializerDataContractResolver>(_ =>
        new JsonSerializerDataContractResolver(new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower }));

    return serviceCollection;
  }

  /// <summary>
  ///   Configures the MVC options.
  /// </summary>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the MVC options to.</param>
  /// <returns>The <see cref="IServiceCollection" /> itself.</returns>
  public static IServiceCollection ConfigureMvcOptions(this IServiceCollection serviceCollection) {
    serviceCollection
      .AddControllers()
      .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
      })
      .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

    return serviceCollection;
  }

  /// <summary>
  ///   Configures the authentication and authorization.
  /// </summary>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the authentication and authorization to.</param>
  /// <returns>The <see cref="IServiceCollection" /> itself.</returns>
  public static IServiceCollection ConfigureAuth(this IServiceCollection serviceCollection) {
    serviceCollection
      .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
      .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
        options.Cookie.HttpOnly = true;
      });
    serviceCollection
      .AddAuthorization();

    return serviceCollection;
  }

  public static IServiceCollection ConfigureLogging(this IServiceCollection serviceCollection, IConfiguration configuration) {
#if DEBUG
    const LogEventLevel MINIMUM_LEVEL = LogEventLevel.Debug;
#else
    const LogEventLevel MINIMUM_LEVEL = LogEventLevel.Information;
#endif

    const string TEMPLATE
      = "[{@t:HH:mm:ss} {@l:u3} ({Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)})] [{MachineName} ({UserId})] ] {@m}\n{@x}";

    var path = Path.Combine(AppContext.BaseDirectory, "logs", ".log");
    var expressionTemplate = new ExpressionTemplate(TEMPLATE, theme: TemplateTheme.Code);
    var loki = configuration.GetConnectionString("Loki");
    var lokiLabels = new[] { new LokiLabel { Key = "app", Value = "identity-server" } };
    var loggerConfiguration = new LoggerConfiguration()
#if DEBUG
      .MinimumLevel.Debug()
#endif
      .WriteTo.Async(
        sink => sink.File(
          new RenderedCompactJsonFormatter(),
          path,
          buffered: true,
          rollingInterval: RollingInterval.Day,
          rollOnFileSizeLimit: true,
          restrictedToMinimumLevel: MINIMUM_LEVEL
        )
      )
      .WriteTo.Console(expressionTemplate, MINIMUM_LEVEL)
      .Enrich.FromLogContext()
      .Enrich.WithCorrelationId(serviceCollection)
      .Enrich.WithMachineName()
      .Enrich.WithThreadName()
      .Enrich.WithProcessName()
      .Enrich.WithExceptionDetails()
      .Enrich.WithEnvironmentName();

    if (!loki.IsNullOrEmpty()) {
      loggerConfiguration.WriteTo.GrafanaLoki(loki, lokiLabels, restrictedToMinimumLevel: MINIMUM_LEVEL);
    }

    serviceCollection
      .AddSerilog(loggerConfiguration.CreateLogger());

    return serviceCollection;
  }
}
