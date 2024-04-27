// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using OIDC.Core.Common;
using OIDC.Core.ComplexTypes;
using OIDC.Core.Exceptions;
using OIDC.Core.Exceptions.Abstractions;
using OIDC.Core.Response.Abstractions;
using OIDC.Infra.Common.Port.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace OIDC.ResourceServer.Controllers.Abstractions;

/// <summary>
///   Represents the base controller for all endpoints.
/// </summary>
[ApiController]
[Route("/api/v{version:apiVersion}/[controller]")]
[SwaggerResponse(HttpStatusCode.UNPROCESSABLE_ENTITY, "The request is invalid.", typeof(ValidationProblemDetails),
  ContentTypes = [MediaTypeNames.Application.ProblemJson])]
[SwaggerResponse(HttpStatusCode.INTERNAL_SERVER_ERROR, "An unexpected error occurred.", typeof(ProblemDetails),
  ContentTypes = [MediaTypeNames.Application.ProblemJson])]
public abstract class EndpointController : ControllerBase {
  /// <summary>
  ///   Handles an exception and returns the appropriate response.
  /// </summary>
  /// <param name="redirectableHttpException">The exception to be handled.</param>
  /// <typeparam name="TResponse">The response type.</typeparam>
  /// <returns>The response of the exception.</returns>
  protected IActionResult HandleRedirectableException<TResponse>(RedirectableHttpException<TResponse> redirectableHttpException)
    where TResponse : RedirectableResponse
    => Redirect(redirectableHttpException.Value.ToQueryParams());

  /// <summary>
  ///   Handles an exception and returns the appropriate response.
  /// </summary>
  /// <param name="exception">The exception to be handled.</param>
  /// <returns>The response of the exception.</returns>
  protected IActionResult HandleException(Exception exception) {
    switch (exception) {
      case UnprocessableEntityHttpException unprocessableEntityHttpException: {
        var unprocessableEntityHttpExceptionProblemDetails = new ValidationProblemDetails {
          Status = unprocessableEntityHttpException.Status,
          Title = "One or more issues occurred.",
          Detail = unprocessableEntityHttpException.Message,
          Type = unprocessableEntityHttpException.Type,
          Instance = HttpContext.Request.Path,
          Extensions = unprocessableEntityHttpException.Extensions,
          Errors = unprocessableEntityHttpException.Issues
        };

        return new ObjectResult(unprocessableEntityHttpExceptionProblemDetails) {
          StatusCode = unprocessableEntityHttpExceptionProblemDetails.Status,
          ContentTypes = { "application/problem+json" }
        };
      }
      case ValidationException validationException: {
        var validationProblemDetails = new ValidationProblemDetails {
          Status = HttpStatusCode.UNPROCESSABLE_ENTITY,
          Title = "One or more issues occurred.",
          Detail = validationException.Message,
          Instance = HttpContext.Request.Path,
          Errors = validationException.Errors
        };

        return new ObjectResult(validationProblemDetails) {
          StatusCode = validationProblemDetails.Status,
          ContentTypes = { "application/problem+json" }
        };
      }
      case HttpException httpException: {
        var httpExceptionProblemDetails = new ProblemDetails {
          Status = httpException.Status,
          Title = httpException.Code,
          Detail = httpException.Message,
          Instance = HttpContext.Request.Path,
          Extensions = httpException.Extensions
        };

        return new ObjectResult(httpExceptionProblemDetails) {
          StatusCode = httpExceptionProblemDetails.Status,
          ContentTypes = { "application/problem+json" }
        };
      }
    }

    var environment = HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();
    var isDevelopment = environment.IsDevelopment();

    var problemDetails = new ProblemDetails {
      Status = HttpStatusCode.INTERNAL_SERVER_ERROR,
      Title = "An error occurred while processing your request.",
      Detail = exception.Message,
      Instance = HttpContext.Request.Path
    };

    if (isDevelopment) {
      problemDetails.Extensions.Add("exception", exception);
    }

    return new ObjectResult(problemDetails) {
      StatusCode = problemDetails.Status,
      ContentTypes = { "application/problem+json" }
    };
  }

  /// <summary>
  ///   Handles a <see cref="RedirectableHttpException{TResponse}" /> and returns the appropriate response.
  /// </summary>
  /// <param name="httpException">The exception to be handled.</param>
  /// <typeparam name="TResponse">The response type.</typeparam>
  /// <returns>The response of the exception.</returns>
  protected IActionResult HandleRedirectableException<TResponse>(HttpException httpException)
    where TResponse : RedirectableResponse {
    var redirectableHttpException = (RedirectableHttpException<TResponse>)httpException;

    return Redirect(redirectableHttpException.Value.ToQueryParams());
  }

  /// <summary>
  ///   Transforms an <see cref="OperationResult{TResponse}" /> into a <c>200 OK</c> response.
  /// </summary>
  /// <param name="operationResult">The result to transform.</param>
  /// <typeparam name="TResponse">The type of the response.</typeparam>
  /// <returns>The transformed response.</returns>
  [NonAction]
  protected IActionResult ToOk<TResponse>(OperationResult<TResponse> operationResult) where TResponse : notnull
    => operationResult.Match(
      response => response is NoContentResponse
        ? Ok()
        : Ok(response),
      HandleException
    );

  /// <summary>
  ///   Transforms an <see cref="OperationResult{TResponse}" /> into a <c>204 No Content</c> response.
  /// </summary>
  /// <param name="operationResult">The result to transform.</param>
  /// <typeparam name="TResponse">The type of the response.</typeparam>
  /// <returns>The transformed response.</returns>
  [NonAction]
  protected IActionResult ToNoContent<TResponse>(OperationResult<TResponse> operationResult) where TResponse : NoContentResponse
    => operationResult.Match(_ => NoContent(), HandleException);

  /// <summary>
  ///   Transforms an <see cref="OperationResult{TResponse}" /> into a <c>201 Created</c> response.
  /// </summary>
  /// <param name="operationResult">The result to transform.</param>
  /// <typeparam name="TResponse">The type of the response.</typeparam>
  /// <returns>The transformed response.</returns>
  [NonAction]
  protected IActionResult ToCreated<TResponse>(OperationResult<TResponse> operationResult) where TResponse : notnull
    => operationResult.Match(
      response => response is NoContentResponse
        ? Created(HttpContext.Request.Path, null)
        : Created(HttpContext.Request.Path, response),
      HandleException
    );

  /// <summary>
  ///   Transforms an <see cref="OperationResult{TResponse}" /> into a <c>201 Created</c> at action response.
  /// </summary>
  /// <param name="operationResult">The result to transform.</param>
  /// <param name="actionName">The name of the action to redirect to.</param>
  /// <typeparam name="TResponse">The type of the response.</typeparam>
  /// <returns>The transformed response.</returns>
  [NonAction]
  protected IActionResult ToCreatedAtAction<TResponse>(OperationResult<TResponse> operationResult, string actionName) where TResponse : notnull
    => operationResult.Match(response => CreatedAtAction(actionName, response), HandleException);

  /// <summary>
  ///   Transforms an <see cref="OperationResult{TResponse}" /> into a <c>301 Moved Permanently</c> response.
  /// </summary>
  /// <param name="operationResult">The result to transform.</param>
  /// <typeparam name="TResponse">The type of the response.</typeparam>
  /// <returns>The transformed response.</returns>
  [NonAction]
  protected IActionResult ToRedirect<TResponse>(OperationResult<TResponse> operationResult) where TResponse : RedirectableResponse
    => operationResult.Match(response => Redirect(response.ToQueryParams()), HandleException);

  /// <summary>
  ///   Transforms an <see cref="OperationResult{TResponse}" /> into a <c>301 Moved Permanently</c> response.
  /// </summary>
  /// <param name="operationResult">The result to transform.</param>
  /// <typeparam name="TResponse">The type of the response.</typeparam>
  /// <returns>The transformed response.</returns>
  /// <remarks>
  ///   If the operation result is a failure, it will redirect to the appropriate URL with the exception details.
  /// </remarks>
  protected IActionResult ToRedirectWithFailure<TResponse>(OperationResult<TResponse> operationResult) where TResponse : RedirectableResponse
    => operationResult.Match(response => Redirect(response.ToQueryParams()), HandleRedirectableException<TResponse>);
}
