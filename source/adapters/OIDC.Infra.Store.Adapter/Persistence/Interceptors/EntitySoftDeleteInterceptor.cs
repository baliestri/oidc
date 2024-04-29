// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OIDC.Core.Entities.Abstractions;

namespace OIDC.Infra.Store.Adapter.Persistence.Interceptors;

internal sealed class EntitySoftDeleteInterceptor : SaveChangesInterceptor {
  /// <inheritdoc />
  public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
  CancellationToken cancellationToken = default) {
    if (eventData.Context is null) {
      return ValueTask.FromResult(result);
    }

    foreach (var entry in eventData.Context.ChangeTracker.Entries()) {
      if (entry is not { State: EntityState.Deleted, Entity: EntitySoftDelete softDelete }) {
        continue;
      }

      entry.State = EntityState.Modified;
      softDelete.DeletedAt = DateTime.UtcNow;
      softDelete.IsDeleted = true;
    }

    return base.SavedChangesAsync(eventData, result, cancellationToken);
  }
}
