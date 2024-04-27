// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using OIDC.Infra.Common.Port.ComplexTypes;

namespace OIDC.Infra.Common.Port.Providers.Abstractions;

/// <summary>
///   Provides a way to validate an instance or a collection of instances.
/// </summary>
/// <typeparam name="T">The type of the instance to validate.</typeparam>
public interface IValidationProvider<in T> {
  /// <summary>
  ///   Validates an instance asynchronously.
  /// </summary>
  /// <param name="instance">The instance to validate.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A task that represents the asynchronous validation operation. The task result contains the validation result.</returns>
  Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Validates a collection of instances asynchronously.
  /// </summary>
  /// <param name="instances">The instances to validate.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
  /// <returns>A task that represents the asynchronous validation operation. The task result contains the validation result.</returns>
  Task<ValidationResult> ValidateAsync(IEnumerable<T> instances, CancellationToken cancellationToken = default);

  /// <summary>
  ///   Validates an instance synchronously.
  /// </summary>
  /// <param name="instance">The instance to validate.</param>
  /// <returns>The validation result.</returns>
  ValidationResult Validate(T instance);

  /// <summary>
  ///   Validates a collection of instances.
  /// </summary>
  /// <param name="instances">The instances to validate.</param>
  /// <returns>The validation result.</returns>
  ValidationResult Validate(IEnumerable<T> instances);
}
