// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Linq.Expressions;
using OIDC.Infra.Store.Port.Expressions.Enums;

namespace OIDC.Infra.Store.Port.Expressions.ComplexTypes;

/// <summary>
///   Represents a key selector that can be included in a query.
/// </summary>
/// <param name="KeySelector">The key selector to be included.</param>
/// <param name="Type">The type of include to be performed.</param>
public readonly record struct IncludableKeySelector(LambdaExpression KeySelector, IncludableIncludeType Type);
