// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.Core.ComplexTypes;

namespace OIDC.Core.UseCases.Abstractions;

/// <summary>
///   Defines the use case operation handler for a request.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IUseCaseOperationHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse> {
  /// <summary>
  ///   Handles the use case.
  /// </summary>
  /// <param name="request">The request.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A <see cref="ValueTask" /> representing the asynchronous operation.</returns>
  ValueTask<OperationResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
