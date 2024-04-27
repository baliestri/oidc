// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;

namespace OIDC.Core.Options;

/// <summary>
///   Configuration options for the Swagger UI.
/// </summary>
public sealed class SwaggerUIOptions {
  /// <summary>
  ///   The key used to store the settings.
  /// </summary>
  public const string KEY = "Swagger";

  /// <summary>
  ///   The title of the API.
  /// </summary>
  [Required(ErrorMessage = "The title of the API is required.")]
  public required string Title { get; init; }

  /// <summary>
  ///   The description of the API.
  /// </summary>
  public string? Description { get; init; }

  /// <summary>
  ///   The information about the contact of the API.
  /// </summary>
  [Required(ErrorMessage = "The contact information of the API is required.")]
  public required SwaggerContactInfo Contact { get; init; }

  /// <summary>
  ///   The information about the license of the API.
  /// </summary>
  [Required(ErrorMessage = "The license information of the API is required.")]
  public required SwaggerLicenseInfo License { get; init; }
}

/// <summary>
///   Information about the contact of the API.
/// </summary>
/// <param name="Name">The name of the contact person/organization.</param>
/// <param name="Url">The URL pointing to the contact information.</param>
/// <param name="Email">The email address of the contact person/organization.</param>
public readonly record struct SwaggerContactInfo(
  [property: Required(ErrorMessage = "The name of the contact person/organization is required.")]
  string Name,
  [property: Required(ErrorMessage = "The URL pointing to the contact information is required.")]
  [property: Url(ErrorMessage = "The URL pointing to the contact information is invalid.")]
  string Url,
  [property: Required(ErrorMessage = "The email address of the contact person/organization is required.")]
  [property: EmailAddress(ErrorMessage = "The email address of the contact person/organization is invalid.")]
  string Email
);

/// <summary>
///   Information about the license of the API.
/// </summary>
/// <param name="Name">The license name used for the API.</param>
/// <param name="Url">A URL to the license used for the API.</param>
public readonly record struct SwaggerLicenseInfo(
  [property: Required(ErrorMessage = "The license name used for the API is required.")]
  string Name,
  [property: Required(ErrorMessage = "The URL to the license used for the API is required.")]
  [property: Url(ErrorMessage = "The URL to the license used for the API is invalid.")]
  string Url
);
