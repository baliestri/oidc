// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using OIDC.Core.Exceptions.Abstractions;

namespace OIDC.Core.ComplexTypes;

/// <summary>
///   Represents a result of an operation.
/// </summary>
/// <typeparam name="TValue">The type of the value of the operation.</typeparam>
public readonly struct OperationResult<TValue> {
  /// <summary>
  ///   The value of the operation.
  /// </summary>
  public TValue? Value { get; }

  /// <summary>
  ///   The HTTP exception of the issue.
  /// </summary>
  public HttpException? HttpException { get; }

  /// <summary>
  ///   Indicates if the operation was successful.
  /// </summary>
  [MemberNotNullWhen(true, nameof(Value))]
  [MemberNotNullWhen(false, nameof(HttpException))]
  public bool IsSuccess { get; }

  /// <summary>
  ///   Indicates if the operation was a failure.
  /// </summary>
  [MemberNotNullWhen(true, nameof(HttpException))]
  [MemberNotNullWhen(false, nameof(Value))]
  public bool IsFailure => !IsSuccess;

  private OperationResult(TValue? value) {
    IsSuccess = true;
    Value = value;
    HttpException = null;
  }

  private OperationResult(HttpException httpException) {
    IsSuccess = false;
    Value = default;
    HttpException = httpException;
  }

  /// <summary>
  ///   Matches the result of the operation.
  /// </summary>
  /// <param name="onSuccess">The action to execute if the operation was successful.</param>
  /// <param name="onFailure">The action to execute if the operation was a failure.</param>
  /// <typeparam name="T">The type of the result of the operation.</typeparam>
  /// <returns>A task that represents the asynchronous operation.</returns>
  [Pure]
  public async Task<T> MatchAsync<T>(Func<TValue, Task<T>> onSuccess, Func<HttpException, Task<T>> onFailure) {
    if (IsSuccess) {
      return await onSuccess(Value).ConfigureAwait(false);
    }

    return await onFailure(HttpException).ConfigureAwait(false);
  }

  /// <summary>
  ///   Matches the result of the operation.
  /// </summary>
  /// <param name="onSuccess">The action to execute if the operation was successful.</param>
  /// <param name="onFailure">The action to execute if the operation was a failure.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  public async Task MatchAsync(Func<TValue, Task> onSuccess, Func<HttpException, Task> onFailure) {
    if (IsSuccess) {
      await onSuccess(Value).ConfigureAwait(false);

      return;
    }

    await onFailure(HttpException).ConfigureAwait(false);
  }

  /// <summary>
  ///   Matches the result of the operation.
  /// </summary>
  /// <param name="onSuccess">The action to execute if the operation was successful.</param>
  /// <param name="onFailure">The action to execute if the operation was a failure.</param>
  /// <typeparam name="T">The type of the result of the operation.</typeparam>
  /// <returns>The result of the operation.</returns>
  public T Match<T>(Func<TValue, T> onSuccess, Func<HttpException, T> onFailure)
    => IsSuccess ? onSuccess(Value) : onFailure(HttpException);

  /// <summary>
  ///   Converts the operation result to a string.
  /// </summary>
  /// <param name="value">The value of the operation.</param>
  /// <returns>The string representation of the operation result.</returns>
  public static implicit operator OperationResult<TValue>(TValue value)
    => new(value);

  /// <summary>
  ///   Converts the HTTP exception to a failed operation result.
  /// </summary>
  /// <param name="httpException">The HTTP exception of the issue.</param>
  /// <returns>The failed operation result.</returns>
  public static implicit operator OperationResult<TValue>(HttpException httpException)
    => new(httpException);

  /// <summary>
  ///   Converts the operation result to a boolean.
  /// </summary>
  /// <param name="operationResult">The operation result.</param>
  /// <returns>The boolean representation of the operation result.</returns>
  public static implicit operator bool(OperationResult<TValue> operationResult)
    => operationResult.IsSuccess;

  /// <summary>
  ///   Creates a successful operation result.
  /// </summary>
  /// <param name="value">The value of the operation.</param>
  /// <returns>The successful operation result.</returns>
  public static OperationResult<TValue> Success(TValue value)
    => new(value);

  /// <summary>
  ///   Creates a failed operation result.
  /// </summary>
  /// <param name="httpException">The HTTP exception of the issue.</param>
  /// <returns>The failed operation result.</returns>
  public static OperationResult<TValue> Failure(HttpException httpException)
    => new(httpException);
}
