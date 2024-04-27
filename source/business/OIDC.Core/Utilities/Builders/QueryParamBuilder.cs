// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Collections;
using System.Web;
using OIDC.Core.Extensions;

namespace OIDC.Core.Utilities.Builders;

/// <summary>
///   Builder for query parameters. It can be used to build query strings from objects or dictionaries.
/// </summary>
public sealed class QueryParamsBuilder {
  private readonly Dictionary<string, string> _parameters = new();

  /// <summary>
  ///   Initialize a new instance of the <see cref="QueryParamsBuilder" /> class.
  /// </summary>
  /// <param name="queryString">The query string to parse.</param>
  public QueryParamsBuilder(string queryString)
    => appendQueryString(queryString);

  /// <summary>
  ///   Initialize a new instance of the <see cref="QueryParamsBuilder" /> class.
  /// </summary>
  /// <param name="obj">The object to parse.</param>
  public QueryParamsBuilder(object obj)
    => Append(obj, []);

  /// <summary>
  ///   Initialize a new instance of the <see cref="QueryParamsBuilder" /> class.
  /// </summary>
  /// <param name="obj">The object to parse.</param>
  /// <param name="exclude">The properties to exclude from the object.</param>
  public QueryParamsBuilder(object obj, params string[] exclude)
    => Append(obj, exclude);

  /// <summary>
  ///   Adds a new parameter to the search parameters.
  /// </summary>
  /// <param name="model">The model to add.</param>
  /// <param name="excluded">The properties to exclude from the model.</param>
  /// <param name="parentKey">The parent key to use for the model.</param>
  public void Append(object model, string[] excluded, string? parentKey = null) {
    switch (model) {
      case IDictionary dictionary: {
        appendDictionary(dictionary, parentKey?.ToSnakeCase());

        return;
      }
      case IEnumerable enumerable: {
        appendEnumerable(enumerable, parentKey?.ToSnakeCase());

        return;
      }
    }

    var properties = model.GetType().GetProperties();

    foreach (var property in properties) {
      if (excluded.Contains(property.Name)) {
        continue;
      }

      var propertyName = property.Name.ToSnakeCase();
      var propertyValue = property.GetValue(model);
      var key = parentKey != null ? $"{parentKey.ToSnakeCase()}.{propertyName}" : propertyName;

      switch (propertyValue) {
        case null:
          continue;
        case IDictionary:
          Append(propertyValue, excluded, key);

          break;
        case IEnumerable and not string:
          Append(propertyValue, excluded, key);

          break;
        default: {
          _parameters[key] = propertyValue.ToString() ?? string.Empty;

          break;
        }
      }
    }
  }

  /// <summary>
  ///   Removes the parameter with the specified key from the search parameters.
  /// </summary>
  /// <param name="key">The key of the parameter to remove.</param>
  /// <returns><c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>.</returns>
  public bool Remove(string key)
    => _parameters.Remove(key.ToSnakeCase());

  /// <summary>
  ///   Returns the first value associated with the given search parameter.
  /// </summary>
  /// <param name="key">The name of the parameter to get.</param>
  /// <returns>The first value associated with the given search parameter.</returns>
  public string? Get(string key)
    => _parameters.GetValueOrDefault(key.ToSnakeCase());

  /// <inheritdoc />
  public override string ToString() {
    var queryString = string.Join("&", _parameters.Select(keyValuePair => $"{keyValuePair.Key}={HttpUtility.UrlEncode(keyValuePair.Value)}"));

    return queryString;
  }

  /// <summary>
  ///   Implicitly converts the <see cref="QueryParamsBuilder" /> to a <see cref="string" />.
  /// </summary>
  /// <param name="queryParamsBuilder">The query parameters builder to convert.</param>
  /// <returns>The query string representation of the <see cref="QueryParamsBuilder" />.</returns>
  public static implicit operator string(QueryParamsBuilder queryParamsBuilder)
    => queryParamsBuilder.ToString();

  private void appendDictionary(IDictionary dictionary, string? parentKey = null) {
    foreach (var key in dictionary.Keys) {
      var value = dictionary[key];
      var keyString = key.ToString() ?? string.Empty;

      var fullKey = parentKey is not null
        ? $"{parentKey.ToSnakeCase()}.{keyString.ToSnakeCase()}"
        : keyString;

      switch (value) {
        case null:
          continue;
        case IEnumerable and not string:
          Append(value, [], fullKey);

          break;
        default:
          _parameters[fullKey] = value.ToString() ?? string.Empty;

          break;
      }
    }
  }

  private void appendEnumerable(IEnumerable enumerable, string? parentKey = null) {
    var i = 0;

    foreach (var item in enumerable) {
      Append(item, [], $"{parentKey?.ToSnakeCase()}[{i}]");
      i++;
    }
  }

  private void appendQueryString(string queryString) {
    if (queryString.StartsWith('?')) {
      queryString = queryString[1..];
    }

    var pairs = queryString.Split('&');

    foreach (var pair in pairs) {
      var keyValue = pair.Split('=');

      if (keyValue.Length == 2) {
        _parameters[keyValue[0].ToSnakeCase()] = HttpUtility.UrlDecode(keyValue[1]);
      }
    }
  }
}
