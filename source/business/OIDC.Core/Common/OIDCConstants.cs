// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;

namespace OIDC.Core.Common;

/// <summary>
///   OIDC constants.
/// </summary>
[ExcludeFromCodeCoverage]
public static class OIDCConstants {
  /// <summary>
  ///   The common constants.
  /// </summary>
  public static class Common {
    /// <summary>
    ///   The main assembly name.
    /// </summary>
    public const string MAIN_ASSEMBLY_NAME = "OIDC";
  }

  /// <summary>
  ///   The CORS constants.
  /// </summary>
  public static class Cors {
    /// <summary>
    ///   The CORS allowed origins policy.
    /// </summary>
    public const string ALLOWED_ORIGINS_POLICY = "CORS_POLICY:ALLOWED_ORIGINS_ONLY";

    /// <summary>
    ///   The CORS any origin policy.
    /// </summary>
    public const string ANY_ORIGIN_POLICY = "CORS_POLICY:ALLOW_ANY_ORIGIN";
  }

  /// <summary>
  ///   The Store (Entity Framework Core) constants.
  /// </summary>
  public static class Store;

  /// <summary>
  ///   The OAuth constants.
  /// </summary>
  public static class OAuth {
    /// <summary>
    ///   Response type constants.
    /// </summary>
    public static class ResponseType {
      /// <summary>
      ///   The authorization code grant type.
      /// </summary>
      public const string CODE = "code";

      /// <summary>
      ///   The implicit grant type.
      /// </summary>
      public const string TOKEN = "token";

      /// <summary>
      ///   The valid response types.
      /// </summary>
      public static readonly string[] AsCollection = [CODE, TOKEN];
    }
  }
}
