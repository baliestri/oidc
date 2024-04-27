// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.Core.Entities.Abstractions;

/// <summary>
///   Represents an entity.
/// </summary>
public interface IEntity : IComparable, IEquatable<Guid>, IEqualityComparer<Guid> {
  /// <summary>
  ///   The unique identifier of the entity.
  /// </summary>
  Guid Id { get; init; }

  /// <summary>
  ///   The date and time when the entity was created.
  /// </summary>
  DateTime CreatedAt { get; init; }

  /// <summary>
  ///   The date and time when the entity was last updated.
  /// </summary>
  DateTime? UpdatedAt { get; set; }
}
