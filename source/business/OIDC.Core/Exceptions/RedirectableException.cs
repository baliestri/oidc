// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.Core.Common;
using OIDC.Core.Exceptions.Abstractions;
using OIDC.Core.Response.Abstractions;

namespace OIDC.Core.Exceptions;

/// <summary>
///   Represents an exception that can be redirected.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public sealed class RedirectableHttpException<TResponse>(TResponse response)
  : HttpException("REDIRECTED_ERROR", "An error occurred and the response will be redirected.", HttpStatusCode.MOVED_PERMANENTLY)
  where TResponse : RedirectableResponse {
  /// <summary>
  ///   The response value.
  /// </summary>
  public TResponse Value { get; } = response;
}
