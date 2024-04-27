// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.Core.Common;
using OIDC.Core.Exceptions.Abstractions;

namespace OIDC.Core.Exceptions;

/// <summary>
///   Exception thrown when an HTTP request is invalid, and the status code is 422.
/// </summary>
/// <param name="issues">The issues that caused the request to be invalid.</param>
public sealed class UnprocessableEntityHttpException(Dictionary<string, string[]> issues)
  : HttpException("UNPROCESSABLE_ENTITY", "One or more issues ocurred", HttpStatusCode.UNPROCESSABLE_ENTITY) {
  /// <summary>
  ///   The issues that caused the request to be invalid.
  /// </summary>
  public Dictionary<string, string[]> Issues { get; } = issues;
}
