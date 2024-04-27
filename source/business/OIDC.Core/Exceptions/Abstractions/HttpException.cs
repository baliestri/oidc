// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.Core.Exceptions.Abstractions;

/// <summary>
///   Exception thrown when an HTTP request is invalid.
/// </summary>
/// <param name="code">The error code.</param>
/// <param name="message">The error message.</param>
/// <param name="status">The HTTP status code.</param>
public abstract class HttpException(string code, string message, int status) : Exception(message) {
  /// <summary>
  ///   The error code.
  /// </summary>
  public string Code { get; } = code;

  /// <summary>
  ///   The HTTP status code.
  /// </summary>
  public int Status { get; } = status;

  /// <summary>
  ///   The error type.
  /// </summary>
  public string? Type { get; init; }

  /// <summary>
  ///   Additional information about the error.
  /// </summary>
  public Dictionary<string, object?> Extensions { get; } = new();
}
