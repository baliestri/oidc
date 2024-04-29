// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

await WebApplication
  .CreateBuilder(args)
  .RegisterPipelines((configurationManager, serviceCollection, environment, loggingBuilder, metricsBuilder) => {
    serviceCollection
      .ConfigureApiRouter(configurationManager)
      .ConfigureMvcOptions()
      .ConfigureJsonSerializerOptions()
      .ConfigureOpenApiGenerator()
      .ConfigureApiVersioning()
      .ConfigureAuth()
      .ConfigureLogging(configurationManager);

    serviceCollection
      .RegisterCommonAdapter()
      .RegisterStoreAdapter(configurationManager);
  })
  .RegisterComponents((applicationBuilder, endpointRouteBuilder, configuration, serviceProvider, environment, logger) => {
    applicationBuilder
      .UseSecureMiddlewares(serviceProvider)
      .UseSerilogRequestLogging()
      .UseAuth()
      .UseEndpoints(routeBuilder => routeBuilder
        .MapExceptionHandlingRoute(environment)
        .MapControllers())
      .UseOpenApiForDevelopment(endpointRouteBuilder, environment);
  })
  .RunAsync();

/// <summary>
///   The entry point of the application.
///   <remarks>
///     Explicitly declared the Program class to use in end-to-end tests.
///   </remarks>
/// </summary>
public partial class Program;
