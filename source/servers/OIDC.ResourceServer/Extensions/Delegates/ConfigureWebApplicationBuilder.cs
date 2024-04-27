// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Microsoft.Extensions.Diagnostics.Metrics;

namespace OIDC.ResourceServer.Extensions.Delegates;

/// <summary>
///   Delegate to configure the components of the application builder.
/// </summary>
internal delegate void ConfigureWebApplicationBuilder(IConfigurationManager configuration, IServiceCollection services, IHostEnvironment environment,
ILoggingBuilder loggingBuilder, IMetricsBuilder metricsBuilder);
