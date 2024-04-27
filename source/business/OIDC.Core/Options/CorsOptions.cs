// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;

namespace OIDC.Core.Options;

/// <summary>
///   Options for CORS.
/// </summary>
public sealed class CorsOptions {
  /// <summary>
  ///   The key used to store the settings.
  /// </summary>
  public const string KEY = "Cors";

  /// <summary>
  ///   The flag to enable CORS.
  /// </summary>
  [Required(ErrorMessage = "The flag to enable CORS is required.")]
  public bool IsEnabled { get; set; }

  /// <summary>
  ///   The flag to allow any origin for CORS.
  /// </summary>
  [Required(ErrorMessage = "The flag to allow any origin for CORS is required.")]
  public bool AllowAnyOrigin { get; set; }

  /// <summary>
  ///   The allowed origins for CORS.
  /// </summary>
  [Required(ErrorMessage = "The allowed origins for CORS are required.")]
  public string[] AllowedOrigins { get; set; } = [];
}
