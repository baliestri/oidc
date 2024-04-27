// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Text;
using OIDC.Core.Utilities.Builders;

namespace OIDC.Core.Response.Abstractions;

/// <summary>
///   Represents a response that can be redirected.
/// </summary>
/// <param name="Uri">The URL to redirect to.</param>
public abstract record RedirectableResponse(
  [property: StringSyntax(StringSyntaxAttribute.Uri)]
  string Uri
) : CommonResponse {
  /// <summary>
  ///   Converts the response to a query string.
  /// </summary>
  /// <returns>The query string representation of the response.</returns>
  public virtual string ToQueryParams() {
    var urlQueryParams = new QueryParamsBuilder(this, nameof(Uri));

    return new StringBuilder()
      .Append(Uri)
      .Append('?')
      .Append(urlQueryParams)
      .ToString();
  }
}
