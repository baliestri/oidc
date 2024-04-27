// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Microsoft.Extensions.Options;
using OIDC.Core.Options;

namespace OIDC.Infra.Common.Port.OptionsSetups;

/// <summary>
///   Setup for the CORS options.
/// </summary>
public interface ICorsOptionsSetup : IConfigureOptions<CorsOptions>, IValidateOptions<CorsOptions>;
