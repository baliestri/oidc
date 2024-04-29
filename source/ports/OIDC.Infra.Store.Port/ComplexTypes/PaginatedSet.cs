// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Collections.Immutable;

namespace OIDC.Infra.Store.Port.ComplexTypes;

/// <summary>
///   Represents a subset of data that is returned from a query and includes information about the entire set of data.
/// </summary>
/// <param name="total">The total number of items in the data source.</param>
/// <param name="perPage">The number of items to be displayed per page.</param>
/// <param name="currentPage">The current page number.</param>
/// <param name="lastPage">The last page number.</param>
/// <param name="items">The subset of items to be returned.</param>
/// <typeparam name="T">The type of items in the subset.</typeparam>
public readonly struct PaginatedSet<T>(int total, int perPage, int currentPage, int lastPage, IEnumerable<T> items) where T : notnull {
  /// <summary>
  ///   The total number of items in the data source.
  /// </summary>
  public int Total { get; } = total;

  /// <summary>
  ///   The number of items to be displayed per page.
  /// </summary>
  public int PerPage { get; } = perPage;

  /// <summary>
  ///   The current page number.
  /// </summary>
  public int CurrentPage { get; } = currentPage;

  /// <summary>
  ///   The last page number.
  /// </summary>
  public int LastPage { get; } = lastPage;

  /// <summary>
  ///   Whether there are more pages after the current page.
  /// </summary>
  public bool HasPreviousPage => CurrentPage > 1;

  /// <summary>
  ///   Whether there are more pages after the current page.
  /// </summary>
  public bool HasNextPage => CurrentPage < LastPage;

  /// <summary>
  ///   The subset of items to be returned.
  /// </summary>
  public IImmutableList<T> Items { get; } = items.ToImmutableList();

  /// <summary>
  ///   Gets the item at the specified index.
  /// </summary>
  /// <param name="index">The index of the item to get.</param>
  public T this[int index] => Items[index];
}

/// <summary>
///   Provides a set of methods for creating <see cref="PaginatedSet{T}" /> instances.
/// </summary>
public static class PaginatedSet {
  /// <summary>
  ///   Creates an empty <see cref="PaginatedSet{T}" />.
  /// </summary>
  /// <param name="perPage">The number of items to be displayed per page.</param>
  /// <param name="currentPage">The current page number.</param>
  /// <returns>An empty <see cref="PaginatedSet{T}" /> with the specified <paramref name="perPage" /> and <paramref name="currentPage" />.</returns>
  public static PaginatedSet<T> Empty<T>(int perPage, int currentPage) where T : notnull
    => new(0, perPage, currentPage, 0, []);
}
