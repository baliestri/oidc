// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.Core.Entities.Abstractions;

/// <summary>
///   Represents an unique entity.
/// </summary>
public abstract class Entity : IEntity {
  /// <summary>
  ///   The unique identifier of the entity.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  ///   The date and time when the entity was created.
  /// </summary>
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

  /// <summary>
  ///   The date and time when the entity was last updated.
  /// </summary>
  public DateTime? UpdatedAt { get; set; }

  /// <inheritdoc />
  public int CompareTo(object? obj)
    => obj switch {
      null => 1,
      Entity other => Id.CompareTo(other.Id),
      var _ => throw new ArgumentException("Object is not an Entity")
    };

  /// <inheritdoc />
  public bool Equals(Guid x, Guid y)
    => x.Equals(y);

  /// <inheritdoc />
  public int GetHashCode(Guid obj)
    => obj.GetHashCode();

  /// <inheritdoc />
  public bool Equals(Guid other)
    => Id.Equals(other);
}
