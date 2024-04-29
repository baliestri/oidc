// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OIDC.Core.Extensions;

namespace OIDC.Infra.Store.Adapter.Extensions;

[ExcludeFromCodeCoverage]
internal static class ModelBuilderExtensions {
  /// <summary>
  ///   Configures a query filter for all entities that implement the specified interface.
  /// </summary>
  /// <param name="modelBuilder">The model builder.</param>
  /// <param name="predicate">The predicate to filter the entities.</param>
  /// <typeparam name="TAssignableTo">The type to filter the entities.</typeparam>
  /// <returns>The model builder itself.</returns>
  public static ModelBuilder ConfigureQueryFilter<TAssignableTo>(this ModelBuilder modelBuilder, Expression<Predicate<TAssignableTo>> predicate)
    where TAssignableTo : notnull {
    modelBuilder.Model
      .GetEntityTypes()
      .Where(entity => entity.ClrType.IsAssignableTo(typeof(TAssignableTo)))
      .Select(entity => entity.ClrType)
      .ForEach(clrType => {
        var parameterExpression = Expression.Parameter(clrType);
        var expression = ReplacingExpressionVisitor.Replace(predicate.Parameters[0], parameterExpression, predicate.Body);

        modelBuilder
          .Entity(clrType)
          .HasQueryFilter(Expression.Lambda(expression, parameterExpression));
      });

    return modelBuilder;
  }
}
