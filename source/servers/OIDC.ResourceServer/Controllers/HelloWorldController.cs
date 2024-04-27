// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OIDC.ResourceServer.Controllers.Abstractions;

namespace OIDC.ResourceServer.Controllers;

/// <summary>
///   The hello world controller.
/// </summary>
[ApiVersion(1)]
public sealed class HelloWorldController : EndpointController {
  /// <summary>
  ///   The index action.
  /// </summary>
  [HttpGet]
  [MapToApiVersion(1)]
  public IActionResult Index()
    => Ok(new { Message = "Hello World" });
}
