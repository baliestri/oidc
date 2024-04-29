// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OIDC.Core.Entities.Abstractions;

namespace OIDC.Infra.Store.Adapter.Extensions;

[ExcludeFromCodeCoverage]
internal static class EntityTypeBuilderExtensions {
  /// <summary>
  ///   Use a schema for the entity.
  /// </summary>
  /// <param name="builder">The entity type builder.</param>
  /// <param name="schema">The schema to use.</param>
  /// <typeparam name="TEntity">The entity type.</typeparam>
  /// <returns>The entity type builder.</returns>
  public static EntityTypeBuilder<TEntity> UseSchema<TEntity>(this EntityTypeBuilder<TEntity> builder, string schema) where TEntity : class, IEntity {
    var tableName = builder.Metadata.GetTableName();

    builder.ToTable(tableName!, schema);

    return builder;
  }

  /// <summary>
  ///   Use a schema for the entity.
  /// </summary>
  /// <param name="builder">The entity type builder.</param>
  /// <param name="table">The table name.</param>
  /// <param name="schema">The schema to use.</param>
  /// <typeparam name="TEntity">The entity type.</typeparam>
  /// <returns>The entity type builder.</returns>
  public static EntityTypeBuilder<TEntity> UseSchema<TEntity>(this EntityTypeBuilder<TEntity> builder, string table, string schema)
    where TEntity : class, IEntity {
    builder.ToTable(table, schema);

    return builder;
  }

  /// <summary>
  ///   Use a schema for the entity.
  /// </summary>
  /// <param name="builder">The entity type builder.</param>
  /// <param name="schema">The schema to use.</param>
  /// <param name="tableBuilder">The table builder.</param>
  /// <typeparam name="TEntity">The entity type.</typeparam>
  /// <returns>The entity type builder.</returns>
  public static EntityTypeBuilder<TEntity> UseSchema<TEntity>(this EntityTypeBuilder<TEntity> builder, string schema,
  Action<TableBuilder<TEntity>> tableBuilder) where TEntity : class, IEntity {
    var tableName = builder.Metadata.GetTableName();

    builder.ToTable(tableName!, schema, tableBuilder);

    return builder;
  }

  /// <summary>
  ///   Use a schema for the entity.
  /// </summary>
  /// <param name="builder">The entity type builder.</param>
  /// <param name="table">The table name.</param>
  /// <param name="schema">The schema to use.</param>
  /// <param name="tableBuilder">The table builder.</param>
  /// <typeparam name="TEntity">The entity type.</typeparam>
  /// <returns>The entity type builder.</returns>
  public static EntityTypeBuilder<TEntity> UseSchema<TEntity>(this EntityTypeBuilder<TEntity> builder, string table, string schema,
  Action<TableBuilder<TEntity>> tableBuilder) where TEntity : class, IEntity {
    builder.ToTable(table, schema, tableBuilder);

    return builder;
  }

  /// <summary>
  ///   Use the default properties for the entity.
  /// </summary>
  /// <param name="builder">The entity type builder.</param>
  /// <typeparam name="TEntity">The entity type.</typeparam>
  /// <returns>The entity type builder.</returns>
  public static EntityTypeBuilder<TEntity> UseDefaults<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IEntity {
    builder
      .HasKey(entity => entity.Id);

    builder
      .Property(entity => entity.CreatedAt)
      .IsRequired();

    builder
      .Property(entity => entity.UpdatedAt);

    if (builder.Metadata.ClrType.IsAssignableTo(typeof(EntitySoftDelete))) {
      builder
        .Property(entity => (entity as EntitySoftDelete)!.DeletedAt);

      builder
        .Property(entity => (entity as EntitySoftDelete)!.IsDeleted);

      builder
        .HasIndex(entity => (entity as EntitySoftDelete)!.IsDeleted);
    }

    return builder;
  }
}
