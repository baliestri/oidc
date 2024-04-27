// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;

namespace OIDC.Infra.Common.Port.Exceptions;

/// <summary>
///   Represents an exception that occurs when a validation fails.
/// </summary>
/// <param name="message">The message that describes the error.</param>
/// <param name="errors">The errors that occurred during the validation.</param>
[ExcludeFromCodeCoverage]
public sealed class ValidationException(string message, Dictionary<string, string[]> errors) : Exception(message) {
  /// <summary>
  ///   Gets the errors that occurred during the validation.
  /// </summary>
  public Dictionary<string, string[]> Errors { get; } = errors;
}
