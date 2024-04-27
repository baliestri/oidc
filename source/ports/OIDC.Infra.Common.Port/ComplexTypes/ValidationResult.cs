// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using OIDC.Infra.Common.Port.Exceptions;

namespace OIDC.Infra.Common.Port.ComplexTypes;

/// <summary>
///   Represents the result of a validation.
/// </summary>
/// <param name="isValid">Whether the validation is valid or not.</param>
/// <param name="exception">The exception that occurred during the validation.</param>
public readonly struct ValidationResult(bool isValid, ValidationException? exception = null) {
  /// <summary>
  ///   Gets whether the validation is valid or not.
  /// </summary>
  [MemberNotNullWhen(false, nameof(Exception))]
  public bool IsValid { get; } = isValid;

  /// <summary>
  ///   Gets the exception that occurred during the validation.
  /// </summary>
  public ValidationException? Exception { get; } = exception;
}
