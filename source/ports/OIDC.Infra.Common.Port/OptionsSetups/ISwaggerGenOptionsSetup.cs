// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OIDC.Infra.Common.Port.OptionsSetups;

/// <summary>
///   Setup for the Swagger generator options.
/// </summary>
public interface ISwaggerGenOptionsSetup : IConfigureNamedOptions<SwaggerGenOptions>;
