// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.Infra.Store.Port.Expressions.Enums;

/// <summary>
///   Represents the type of include to be performed in a query.
/// </summary>
public enum IncludableIncludeType {
  /// <summary>
  ///   Represents an include operation.
  /// </summary>
  Include,

  /// <summary>
  ///   Represents a then include operation.
  /// </summary>
  ThenInclude
}
