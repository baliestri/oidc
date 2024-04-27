// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OIDC.Core.Options;
using OIDC.Infra.Common.Port.OptionsSetups;

namespace OIDC.Infra.Common.Adapter.OptionsSetups;

/// <summary>
///   Setup for the CORS options.
/// </summary>
/// <param name="configuration">The <see cref="IConfiguration" /> to bind the options from.</param>
internal sealed class CorsOptionsSetup(IConfiguration configuration) : ICorsOptionsSetup {
  /// <inheritdoc />
  public void Configure(CorsOptions options)
    => configuration.Bind(CorsOptions.KEY, options);

  /// <inheritdoc />
  public ValidateOptionsResult Validate(string? name, CorsOptions options) {
    var context = new ValidationContext(options);
    var results = new List<ValidationResult>();

    return Validator.TryValidateObject(options, context, results, true)
      ? ValidateOptionsResult.Success
      : ValidateOptionsResult.Fail(results.Select(result => result.ErrorMessage)!);
  }
}
