// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.Core.Common;
using OIDC.Core.Exceptions.Abstractions;

namespace OIDC.Core.Exceptions;

/// <summary>
///   Exception thrown when an HTTP request is invalid, and the status code is 401.
/// </summary>
/// <param name="code">The error code.</param>
/// <param name="message">The error message.</param>
public sealed class UnauthorizedHttpException(string code, string message) : HttpException(code, message, HttpStatusCode.UNAUTHORIZED);
