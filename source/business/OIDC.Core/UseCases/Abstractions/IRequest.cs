// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.Core.UseCases.Abstractions;

/// <summary>
///   Defines a request for an use case.
/// </summary>
public interface IRequest;

/// <summary>
///   Defines a request for an use case.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IRequest<out TResponse>;
