// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.Core.ComplexTypes;
using OIDC.Core.Exceptions;
using OIDC.Core.Exceptions.Abstractions;
using OIDC.Infra.Common.Port.Exceptions;

namespace OIDC.Infra.Common.Port.ComplexTypes;

/// <summary>
///   Represents a result of an operation.
/// </summary>
public readonly struct OperationResult {
  /// <summary>
  ///   Creates a successful operation result.
  /// </summary>
  /// <param name="value">The value of the operation.</param>
  /// <typeparam name="TValue">The type of the value of the operation.</typeparam>
  /// <returns>The successful operation result.</returns>
  public static OperationResult<TValue> Success<TValue>(TValue value)
    => OperationResult<TValue>.Success(value);

  /// <summary>
  ///   Creates a failed operation result.
  /// </summary>
  /// <param name="httpException">The HTTP exception of the issue.</param>
  /// <typeparam name="TValue">The type of the value of the operation.</typeparam>
  /// <returns>The failed operation result.</returns>
  public static OperationResult<TValue> Failure<TValue>(HttpException httpException)
    => OperationResult<TValue>.Failure(httpException);

  /// <summary>
  ///   Creates a failed operation result.
  /// </summary>
  /// <param name="validationResult">The validation result of the issue.</param>
  /// <typeparam name="TValue">The type of the value of the operation.</typeparam>
  /// <returns>The failed operation result.</returns>
  public static OperationResult<TValue> Failure<TValue>(ValidationResult validationResult) {
    var exception = validationResult.Exception;

    return exception is null
      ? OperationResult<TValue>.Failure(new UnprocessableEntityHttpException(new Dictionary<string, string[]>()))
      : OperationResult<TValue>.Failure(new UnprocessableEntityHttpException(exception.Errors));
  }

  /// <summary>
  ///   Creates a failed operation result.
  /// </summary>
  /// <param name="validationException">The validation exception of the issue.</param>
  /// <typeparam name="TValue">The type of the value of the operation.</typeparam>
  /// <returns>The failed operation result.</returns>
  public static OperationResult<TValue> Failure<TValue>(ValidationException validationException)
    => OperationResult<TValue>.Failure(new UnprocessableEntityHttpException(validationException.Errors));
}
