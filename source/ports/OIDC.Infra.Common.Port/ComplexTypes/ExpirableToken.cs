// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;

namespace OIDC.Infra.Common.Port.ComplexTypes;

/// <summary>
///   Represents an expirable token.
/// </summary>
/// <param name="value">The token value.</param>
/// <param name="expiresAt">The date and time when the token expires.</param>
public readonly struct ExpirableToken(string value, DateTime expiresAt) {
  /// <summary>
  ///   The token value.
  /// </summary>
  [Required(ErrorMessage = "The token value is required.")]
  public string Value { get; } = value;

  /// <summary>
  ///   The date and time when the token expires.
  /// </summary>
  [Required(ErrorMessage = "The token expiration date is required.")]
  public DateTime ExpiresAt { get; } = expiresAt;

  /// <summary>
  ///   Indicates whether the token is expired.
  /// </summary>
  public bool IsExpired => DateTime.UtcNow > ExpiresAt;

  /// <summary>
  ///   Implicitly converts the <see cref="ExpirableToken" /> to a <see cref="string" />.
  /// </summary>
  /// <param name="expirableToken">The <see cref="ExpirableToken" /> to be converted.</param>
  /// <returns>The token value.</returns>
  public static implicit operator string(ExpirableToken expirableToken)
    => expirableToken.Value;

  /// <summary>
  ///   Deconstructs the <see cref="ExpirableToken" /> into its properties.
  /// </summary>
  /// <param name="value">The token value.</param>
  /// <param name="expiresAt">The date and time when the token expires.</param>
  public void Deconstruct(out string value, out DateTime expiresAt) {
    value = Value;
    expiresAt = ExpiresAt;
  }
}
