// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OIDC.Core.Entities.Abstractions;
using OIDC.Infra.Store.Adapter.Context;
using OIDC.Infra.Store.Adapter.Persistence;
using OIDC.Infra.Store.Adapter.Persistence.Abstractions;
using OIDC.Infra.Store.Adapter.Persistence.Interceptors;
using OIDC.Infra.Store.Port.Abstractions;
using OIDC.Infra.Store.Port.Persistence.Abstractions;

namespace OIDC.Infra.Store.Adapter.Extensions;

/// <summary>
///   Extension methods for <see cref="IServiceCollection" />.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions {
  /// <summary>
  ///   Registers the store adapter services.
  /// </summary>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the store adapter services to.</param>
  /// <param name="configuration">The <see cref="IConfiguration" /> to get the connection string from.</param>
  /// <returns>The <see cref="IServiceCollection" /> itself.</returns>
  public static IServiceCollection RegisterStoreAdapter(this IServiceCollection serviceCollection, IConfiguration configuration)
    => serviceCollection
      .AddDbContextDependencies<IDatabaseContext, DbOidcContext>(configuration.GetDefaultConnectionString())
      .AddRepositories();

  /// <summary>
  ///   Adds the database context dependencies.
  /// </summary>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the dependencies to.</param>
  /// <param name="connectionString">The connection string.</param>
  /// <param name="optionsAction">The options action.</param>
  /// <typeparam name="TService">The type of the service.</typeparam>
  /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
  /// <returns>The <see cref="IServiceCollection" /> itself.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="connectionString" /> is null.</exception>
  internal static IServiceCollection AddDbContextDependencies<TService, TImplementation>(this IServiceCollection serviceCollection,
  string? connectionString, Action<DbContextOptionsBuilder>? optionsAction = null)
    where TService : class, IDatabaseContext
    where TImplementation : DbContext, TService {
    ArgumentNullException.ThrowIfNull(connectionString);

    return serviceCollection
      .AddPooledDbContextFactory<TImplementation>(options => {
        options
          .UseSqlServer(connectionString, builder => builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
          .UseSnakeCaseNamingConvention()
          .AddInterceptors(new EntitySoftDeleteInterceptor());

        optionsAction?.Invoke(options);
      })
      .AddSingleton<IDatabaseContextFactory>(serviceProvider => {
        var databaseContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<TImplementation>>();

        return new DatabaseContextFactory<TImplementation>(databaseContextFactory);
      })
      .AddScoped<TService, TImplementation>(serviceProvider => {
        var dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<TImplementation>>();
        var dbContext = dbContextFactory.CreateDbContext();

        return dbContext;
      })
      .AddScoped<IUnitOfWork, UnitOfWork>();
  }

  /// <summary>
  ///   Adds the repositories.
  /// </summary>
  /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add the repositories to.</param>
  /// <returns>The <see cref="IServiceCollection" /> itself.</returns>
  internal static IServiceCollection AddRepositories(this IServiceCollection serviceCollection) {
    var entities = typeof(Entity).Assembly
      .GetTypes()
      .Where(type => type is { IsClass: true, IsAbstract: false } && type.IsSubclassOf(typeof(Entity)));

    foreach (var entity in entities) {
      var repositoryType = typeof(Repository<>).MakeGenericType(entity);
      var repositoryInterfaceType = typeof(IRepository<>).MakeGenericType(entity);
      var readonlyRepositoryInterfaceType = typeof(IReadOnlyRepository<>).MakeGenericType(entity);

      serviceCollection.AddScoped(repositoryInterfaceType, repositoryType);
      serviceCollection.AddScoped(readonlyRepositoryInterfaceType, repositoryType);
    }

    return serviceCollection;
  }
}
