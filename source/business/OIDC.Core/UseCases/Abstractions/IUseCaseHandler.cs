// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.Core.UseCases.Abstractions;

/// <summary>
///   Defines the use case handler for a request.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
public interface IUseCaseHandler<in TRequest> where TRequest : IRequest {
  /// <summary>
  ///   Handles the use case.
  /// </summary>
  /// <param name="request">The request.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A <see cref="ValueTask" /> representing the asynchronous operation.</returns>
  ValueTask HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
///   Defines the use case handler for a request.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IUseCaseHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse> {
  /// <summary>
  ///   Handles the use case.
  /// </summary>
  /// <param name="request">The request.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A <see cref="ValueTask" /> representing the asynchronous operation.</returns>
  ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
