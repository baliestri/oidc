// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.IdentityServer.Extensions.Delegates;

/// <summary>
///   Delegate to configure the components of the application.
/// </summary>
internal delegate void ConfigureWebApplication(IApplicationBuilder applicationBuilder, IEndpointRouteBuilder endpointRouteBuilder,
IConfiguration configuration, IServiceProvider serviceProvider, IWebHostEnvironment environment, ILogger logger);
