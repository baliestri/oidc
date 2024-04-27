// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Runtime.CompilerServices;
using FluentValidation;
using OIDC.Core.Extensions;
using OIDC.Infra.Common.Port.ComplexTypes;
using OIDC.Infra.Common.Port.Providers.Abstractions;
using ValidationException = OIDC.Infra.Common.Port.Exceptions.ValidationException;

namespace OIDC.Infra.Common.Adapter.Providers;

/// <inheritdoc cref="IValidationProvider{T}" />
public abstract class ValidationProvider<T> : AbstractValidator<T>, IValidationProvider<T> {
  /// <summary>
  ///   Initializes a new instance of the <see cref="ValidationProvider{T}" /> class.
  /// </summary>
  protected ValidationProvider()
    => RuleLevelCascadeMode = CascadeMode.Stop;

  /// <inheritdoc />
  async Task<ValidationResult> IValidationProvider<T>.ValidateAsync(T instance, CancellationToken cancellationToken) {
    var validate = await base.ValidateAsync(instance, cancellationToken);

    var validationResult = new ValidationResult(validate.IsValid);

    if (validate.IsValid) {
      return validationResult;
    }

    var errors = validate.Errors
      .GroupBy(failure => failure.PropertyName)
      .ToDictionary(
        grouping => grouping.Key.ToSnakeCase(),
        grouping => grouping.Select(failure => failure.ErrorMessage).ToArray()
      );

    validationResult = new ValidationResult(false, new ValidationException("Validation failed.", errors));

    return validationResult;
  }

  /// <inheritdoc />
  ValidationResult IValidationProvider<T>.Validate(T instance) {
    var validate = base.Validate(instance);

    var validationResult = new ValidationResult(validate.IsValid);

    if (validate.IsValid) {
      return validationResult;
    }

    var errors = validate.Errors
      .GroupBy(failure => failure.PropertyName)
      .ToDictionary(
        grouping => grouping.Key.ToSnakeCase(),
        grouping => grouping.Select(failure => failure.ErrorMessage).ToArray()
      );

    validationResult = new ValidationResult(false, new ValidationException("Validation failed.", errors));

    return validationResult;
  }

  /// <inheritdoc />
  public ValidationResult Validate(IEnumerable<T> instances) {
    var validationExceptions = validateMany(instances).ToList();
    var isValid = validationExceptions.Count == 0;
    var errors = validationExceptions
      .SelectMany((validationException, index) => validationException.Errors.Select(
        error => new KeyValuePair<string, string[]>($"[{index}].{error.Key}", error.Value)
      ))
      .ToDictionary(
        grouping => grouping.Key,
        grouping => grouping.Value
      );

    var validationException = isValid
      ? null
      : new ValidationException("Validation failed.", errors);

    return new ValidationResult(isValid, validationException);
  }

  /// <inheritdoc />
  public async Task<ValidationResult> ValidateAsync(IEnumerable<T> instances, CancellationToken cancellationToken) {
    var validationExceptions = new List<ValidationException>();

    await foreach (var exception in validateManyAsync(instances, cancellationToken)) {
      validationExceptions.Add(exception);
    }

    var isValid = validationExceptions.Count == 0;
    var errors = validationExceptions
      .SelectMany((validationException, index) => validationException.Errors.Select(
        error => new KeyValuePair<string, string[]>($"[{index}].{error.Key}", error.Value)
      ))
      .ToDictionary(
        grouping => grouping.Key,
        grouping => grouping.Value
      );

    var validationException = isValid
      ? null
      : new ValidationException("Validation failed.", errors);

    return new ValidationResult(isValid, validationException);
  }

  private IEnumerable<ValidationException> validateMany(IEnumerable<T> instances)
    => from instance in instances
      select base.Validate(instance)
      into validate
      where !validate.IsValid
      select validate.Errors
        .GroupBy(failure => failure.PropertyName)
        .ToDictionary(
          grouping => grouping.Key.ToSnakeCase(),
          grouping => grouping.Select(failure => failure.ErrorMessage).ToArray()
        )
      into errors
      select new ValidationException("Validation failed.", errors);

  private async IAsyncEnumerable<ValidationException> validateManyAsync(IEnumerable<T> instances,
  [EnumeratorCancellation] CancellationToken cancellationToken = default) {
    foreach (var instance in instances) {
      var validate = await base.ValidateAsync(instance, cancellationToken);

      if (validate.IsValid) {
        continue;
      }

      var errors = validate.Errors
        .GroupBy(failure => failure.PropertyName)
        .ToDictionary(
          grouping => grouping.Key.ToSnakeCase(),
          grouping => grouping.Select(failure => failure.ErrorMessage).ToArray()
        );

      yield return new ValidationException("Validation failed.", errors);
    }
  }
}
