// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.Core.Common;
using OIDC.Core.Response.Abstractions;

namespace OIDC.Core.Exceptions;

/// <summary>
///   Http exception prelude.
///   Contains helper methods to create http exceptions.
/// </summary>
public static class Prelude {
  /// <summary>
  ///   Creates a bad request http exception with the given code and message.
  /// </summary>
  /// <param name="code">The http exception error code.</param>
  /// <param name="message">The http exception error message.</param>
  /// <returns>The bad request http exception.</returns>
  public static BadRequestHttpException BadRequestHttpException(string code, string message)
    => new(code, message);

  /// <summary>
  ///   Creates a bad request http exception with the given message.
  /// </summary>
  /// <param name="message">The http exception error message.</param>
  /// <returns>The bad request http exception.</returns>
  public static BadRequestHttpException BadRequestHttpException(string message)
    => new(nameof(HttpStatusCode.BAD_REQUEST), message);

  /// <summary>
  ///   Creates a forbidden http exception with the given code and message.
  /// </summary>
  /// <param name="code">The http exception error code.</param>
  /// <param name="message">The http exception error message.</param>
  /// <returns>The forbidden http exception.</returns>
  public static ForbiddenHttpException ForbiddenHttpException(string code, string message)
    => new(code, message);

  /// <summary>
  ///   Creates a forbidden http exception with the given message.
  /// </summary>
  /// <param name="message">The http exception error message.</param>
  /// <returns>The forbidden http exception.</returns>
  public static ForbiddenHttpException ForbiddenHttpException(string message)
    => new(nameof(HttpStatusCode.FORBIDDEN), message);

  /// <summary>
  ///   Creates a not found http exception with the given code and message.
  /// </summary>
  /// <param name="code">The http exception error code.</param>
  /// <param name="message">The http exception error message.</param>
  /// <returns>The not found http exception.</returns>
  public static NotFoundHttpException NotFoundHttpException(string code, string message)
    => new(code, message);

  /// <summary>
  ///   Creates a not found http exception with the given message.
  /// </summary>
  /// <param name="message">The http exception error message.</param>
  /// <returns>The not found http exception.</returns>
  public static NotFoundHttpException NotFoundHttpException(string message)
    => new(nameof(HttpStatusCode.NOT_FOUND), message);

  /// <summary>
  ///   Creates an unauthorized http exception with the given code and message.
  /// </summary>
  /// <param name="code">The http exception error code.</param>
  /// <param name="message">The http exception error message.</param>
  /// <returns>The unauthorized http exception.</returns>
  public static UnauthorizedHttpException UnauthorizedHttpException(string code, string message)
    => new(code, message);

  /// <summary>
  ///   Creates an unauthorized http exception with the given message.
  /// </summary>
  /// <param name="message">The http exception error message.</param>
  /// <returns>The unauthorized http exception.</returns>
  public static UnauthorizedHttpException UnauthorizedHttpException(string message)
    => new(nameof(HttpStatusCode.UNAUTHORIZED), message);

  /// <summary>
  ///   Creates an unprocessable entity http exception with the given issues.
  /// </summary>
  /// <param name="issues">The issues that caused the request to be invalid.</param>
  /// <returns>The unprocessable entity http exception.</returns>
  public static UnprocessableEntityHttpException UnprocessableEntityHttpException(Dictionary<string, string[]> issues)
    => new(issues);

  /// <summary>
  ///   Creates a redirectable http exception with the given response.
  /// </summary>
  /// <param name="response">The response value.</param>
  /// <typeparam name="TResponse">The response type.</typeparam>
  /// <returns>The redirectable http exception.</returns>
  public static RedirectableHttpException<TResponse> RedirectableHttpException<TResponse>(TResponse response) where TResponse : RedirectableResponse
    => new(response);
}
