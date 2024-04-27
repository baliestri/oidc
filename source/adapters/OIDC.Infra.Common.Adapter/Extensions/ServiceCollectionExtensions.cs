// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OIDC.Infra.Common.Adapter.OptionsSetups;
using OIDC.Infra.Common.Adapter.Providers;
using OIDC.Infra.Common.Port.Providers.Abstractions;

namespace OIDC.Infra.Common.Adapter.Extensions;

/// <summary>
///   Extension methods for <see cref="IServiceCollection" />.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions {
  /// <summary>
  ///   Registers the common adapter services.
  /// </summary>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the common adapter services to.</param>
  /// <returns>The <see cref="IServiceCollection" /> itself.</returns>
  public static IServiceCollection RegisterCommonAdapter(this IServiceCollection serviceCollection)
    => serviceCollection
      .ConfigureOptions<CorsOptionsSetup>()
      .ConfigureOptions<SwaggerGenOptionsSetup>()
      .ConfigureOptions<SwaggerUIOptionsSetup>()
      .AddScoped<IDateTimeProvider, DateTimeProvider>();

  /// <summary>
  ///   Adds the validation provider to the service collection.
  /// </summary>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the validation provider to.</param>
  /// <typeparam name="T">The type of the instance to validate.</typeparam>
  /// <typeparam name="TValidationProvider">The type of the implementation of the validation provider.</typeparam>
  /// <returns>The <see cref="IServiceCollection" /> itself.</returns>
  public static IServiceCollection AddValidationProvider<T, TValidationProvider>(this IServiceCollection serviceCollection)
    where T : class
    where TValidationProvider : class, IValidationProvider<T>, IValidator<T>
    => serviceCollection
      .AddScoped<IValidationProvider<T>, TValidationProvider>()
      .AddScoped<IValidator<T>, TValidationProvider>();
}
