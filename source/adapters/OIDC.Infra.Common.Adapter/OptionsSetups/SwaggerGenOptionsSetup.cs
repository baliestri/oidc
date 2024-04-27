// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Text;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OIDC.Core.Common;
using OIDC.Core.Options;
using OIDC.Infra.Common.Port.OptionsSetups;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OIDC.Infra.Common.Adapter.OptionsSetups;

/// <summary>
///   Setup for the Swagger generator options.
/// </summary>
/// <param name="descriptionProvider">The <see cref="IApiVersionDescriptionProvider" /> to get the API version descriptions from.</param>
/// <param name="swaggerUiOptionsInstance">The <see cref="IOptions{TOptions}" /> to get the Swagger UI options from.</param>
internal sealed class SwaggerGenOptionsSetup(IApiVersionDescriptionProvider descriptionProvider, IOptions<SwaggerUIOptions> swaggerUiOptionsInstance)
  : ISwaggerGenOptionsSetup {
  /// <inheritdoc />
  public void Configure(SwaggerGenOptions options) {
    var apiVersionDescriptions = descriptionProvider.ApiVersionDescriptions;
    var swaggerUiOptions = swaggerUiOptionsInstance.Value;

    foreach (var apiVersionDescription in apiVersionDescriptions) {
      var description = new StringBuilder(swaggerUiOptions.Description);

      if (apiVersionDescription.IsDeprecated) {
        description.AppendLine().AppendLine("This API version has been deprecated.");
      }

      if (apiVersionDescription.SunsetPolicy is { } policy) {
        if (policy.Date is { } when) {
          description
            .Append(" The API will be sunset on ")
            .Append(when.Date.ToShortDateString())
            .Append('.');
        }

        if (policy.HasLinks) {
          description.AppendLine();

          foreach (var link in policy.Links) {
            if (link.Type == "text/html") {
              description.AppendLine();

              if (link.Title.HasValue) {
                description.Append(link.Title.Value).Append(": ");
              }

              description.Append(link.LinkTarget.OriginalString);
            }
          }
        }
      }

      options.SwaggerDoc(apiVersionDescription.GroupName.ToLowerInvariant(), new OpenApiInfo {
        Title = swaggerUiOptions.Title,
        Version = apiVersionDescription.ApiVersion.ToString(),
        Description = description.ToString(),
        Contact = new OpenApiContact {
          Name = swaggerUiOptions.Contact.Name,
          Email = swaggerUiOptions.Contact.Email,
          Url = new Uri(swaggerUiOptions.Contact.Url)
        },
        License = new OpenApiLicense {
          Name = swaggerUiOptions.License.Name,
          Url = new Uri(swaggerUiOptions.License.Url)
        }
      });

      options.EnableAnnotations();

      var xmlCommentsPaths = getXmlCommentsPaths();

      foreach (var xmlCommentsPath in xmlCommentsPaths) {
        options.IncludeXmlComments(xmlCommentsPath);
      }
    }
  }

  /// <inheritdoc />
  public void Configure(string? name, SwaggerGenOptions options)
    => Configure(options);

  private static IEnumerable<string> getXmlCommentsPaths() {
    const string XML_FILE_PATTERN = $"{OIDCConstants.Common.MAIN_ASSEMBLY_NAME}*.xml";
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, XML_FILE_PATTERN, SearchOption.TopDirectoryOnly);

    return xmlFiles;
  }
}
