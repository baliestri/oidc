// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.Core.Entities.Abstractions;

/// <summary>
///   Represents an unique entity with soft delete capabilities.
/// </summary>
public abstract class EntitySoftDelete : Entity {
  /// <summary>
  ///   The date and time when the entity was deleted.
  /// </summary>
  public DateTime? DeletedAt { get; set; }

  /// <summary>
  ///   Indicates if the entity was deleted.
  /// </summary>
  public bool IsDeleted { get; set; } = false;
}
