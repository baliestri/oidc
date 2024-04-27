// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace OIDC.Core.Common;

/// <summary>
///   Contains constants for HTTP status codes.
/// </summary>
public static class HttpStatusCode {
  /// <summary>
  ///   HTTP status code 100.
  /// </summary>
  public const int CONTINUE = 100;

  /// <summary>
  ///   HTTP status code 101.
  /// </summary>
  public const int SWITCHING_PROTOCOLS = 101;

  /// <summary>
  ///   HTTP status code 102.
  /// </summary>
  public const int PROCESSING = 102;

  /// <summary>
  ///   HTTP status code 200.
  /// </summary>
  public const int OK = 200;

  /// <summary>
  ///   HTTP status code 201.
  /// </summary>
  public const int CREATED = 201;

  /// <summary>
  ///   HTTP status code 202.
  /// </summary>
  public const int ACCEPTED = 202;

  /// <summary>
  ///   HTTP status code 203.
  /// </summary>
  public const int NON_AUTHORITATIVE = 203;

  /// <summary>
  ///   HTTP status code 204.
  /// </summary>
  public const int NO_CONTENT = 204;

  /// <summary>
  ///   HTTP status code 205.
  /// </summary>
  public const int RESET_CONTENT = 205;

  /// <summary>
  ///   HTTP status code 206.
  /// </summary>
  public const int PARTIAL_CONTENT = 206;

  /// <summary>
  ///   HTTP status code 207.
  /// </summary>
  public const int MULTI_STATUS = 207;

  /// <summary>
  ///   HTTP status code 208.
  /// </summary>
  public const int ALREADY_REPORTED = 208;

  /// <summary>
  ///   HTTP status code 226.
  /// </summary>
  public const int IM_USED = 226;

  /// <summary>
  ///   HTTP status code 300.
  /// </summary>
  public const int MULTIPLE_CHOICES = 300;

  /// <summary>
  ///   HTTP status code 301.
  /// </summary>
  public const int MOVED_PERMANENTLY = 301;

  /// <summary>
  ///   HTTP status code 302.
  /// </summary>
  public const int FOUND = 302;

  /// <summary>
  ///   HTTP status code 303.
  /// </summary>
  public const int SEE_OTHER = 303;

  /// <summary>
  ///   HTTP status code 304.
  /// </summary>
  public const int NOT_MODIFIED = 304;

  /// <summary>
  ///   HTTP status code 305.
  /// </summary>
  public const int USE_PROXY = 305;

  /// <summary>
  ///   HTTP status code 306.
  /// </summary>
  public const int SWITCH_PROXY = 306;

  /// <summary>
  ///   HTTP status code 307.
  /// </summary>
  public const int TEMPORARY_REDIRECT = 307;

  /// <summary>
  ///   HTTP status code 308.
  /// </summary>
  public const int PERMANENT_REDIRECT = 308;

  /// <summary>
  ///   HTTP status code 400.
  /// </summary>
  public const int BAD_REQUEST = 400;

  /// <summary>
  ///   HTTP status code 401.
  /// </summary>
  public const int UNAUTHORIZED = 401;

  /// <summary>
  ///   HTTP status code 402.
  /// </summary>
  public const int PAYMENT_REQUIRED = 402;

  /// <summary>
  ///   HTTP status code 403.
  /// </summary>
  public const int FORBIDDEN = 403;

  /// <summary>
  ///   HTTP status code 404.
  /// </summary>
  public const int NOT_FOUND = 404;

  /// <summary>
  ///   HTTP status code 405.
  /// </summary>
  public const int METHOD_NOT_ALLOWED = 405;

  /// <summary>
  ///   HTTP status code 406.
  /// </summary>
  public const int NOT_ACCEPTABLE = 406;

  /// <summary>
  ///   HTTP status code 407.
  /// </summary>
  public const int PROXY_AUTHENTICATION_REQUIRED = 407;

  /// <summary>
  ///   HTTP status code 408.
  /// </summary>
  public const int REQUEST_TIMEOUT = 408;

  /// <summary>
  ///   HTTP status code 409.
  /// </summary>
  public const int CONFLICT = 409;

  /// <summary>
  ///   HTTP status code 410.
  /// </summary>
  public const int GONE = 410;

  /// <summary>
  ///   HTTP status code 411.
  /// </summary>
  public const int LENGTH_REQUIRED = 411;

  /// <summary>
  ///   HTTP status code 412.
  /// </summary>
  public const int PRECONDITION_FAILED = 412;

  /// <summary>
  ///   HTTP status code 413.
  /// </summary>
  public const int REQUEST_ENTITY_TOO_LARGE = 413;

  /// <summary>
  ///   HTTP status code 413.
  /// </summary>
  public const int PAYLOAD_TOO_LARGE = 413;

  /// <summary>
  ///   HTTP status code 414.
  /// </summary>
  public const int REQUEST_URI_TOO_LONG = 414;

  /// <summary>
  ///   HTTP status code 414.
  /// </summary>
  public const int URI_TOO_LONG = 414;

  /// <summary>
  ///   HTTP status code 415.
  /// </summary>
  public const int UNSUPPORTED_MEDIA_TYPE = 415;

  /// <summary>
  ///   HTTP status code 416.
  /// </summary>
  public const int REQUESTED_RANGE_NOT_SATISFIABLE = 416;

  /// <summary>
  ///   HTTP status code 416.
  /// </summary>
  public const int RANGE_NOT_SATISFIABLE = 416;

  /// <summary>
  ///   HTTP status code 417.
  /// </summary>
  public const int EXPECTATION_FAILED = 417;

  /// <summary>
  ///   HTTP status code 418.
  /// </summary>
  public const int IM_A_TEAPOT = 418;

  /// <summary>
  ///   HTTP status code 419.
  /// </summary>
  public const int AUTHENTICATION_TIMEOUT = 419;

  /// <summary>
  ///   HTTP status code 421.
  /// </summary>
  public const int MISDIRECTED_REQUEST = 421;

  /// <summary>
  ///   HTTP status code 422.
  /// </summary>
  public const int UNPROCESSABLE_ENTITY = 422;

  /// <summary>
  ///   HTTP status code 423.
  /// </summary>
  public const int LOCKED = 423;

  /// <summary>
  ///   HTTP status code 424.
  /// </summary>
  public const int FAILED_DEPENDENCY = 424;

  /// <summary>
  ///   HTTP status code 426.
  /// </summary>
  public const int UPGRADE_REQUIRED = 426;

  /// <summary>
  ///   HTTP status code 428.
  /// </summary>
  public const int PRECONDITION_REQUIRED = 428;

  /// <summary>
  ///   HTTP status code 429.
  /// </summary>
  public const int TOO_MANY_REQUESTS = 429;

  /// <summary>
  ///   HTTP status code 431.
  /// </summary>
  public const int REQUEST_HEADER_FIELDS_TOO_LARGE = 431;

  /// <summary>
  ///   HTTP status code 451.
  /// </summary>
  public const int UNAVAILABLE_FOR_LEGAL_REASONS = 451;

  /// <summary>
  ///   HTTP status code 499. This is an unofficial status code originally defined by Nginx and is commonly used
  ///   in logs when the client has disconnected.
  /// </summary>
  public const int CLIENT_CLOSED_REQUEST = 499;

  /// <summary>
  ///   HTTP status code 500.
  /// </summary>
  public const int INTERNAL_SERVER_ERROR = 500;

  /// <summary>
  ///   HTTP status code 501.
  /// </summary>
  public const int NOT_IMPLEMENTED = 501;

  /// <summary>
  ///   HTTP status code 502.
  /// </summary>
  public const int BAD_GATEWAY = 502;

  /// <summary>
  ///   HTTP status code 503.
  /// </summary>
  public const int SERVICE_UNAVAILABLE = 503;

  /// <summary>
  ///   HTTP status code 504.
  /// </summary>
  public const int GATEWAY_TIMEOUT = 504;

  /// <summary>
  ///   HTTP status code 505.
  /// </summary>
  public const int HTTP_VERSION_NOTSUPPORTED = 505;

  /// <summary>
  ///   HTTP status code 506.
  /// </summary>
  public const int VARIANT_ALSO_NEGOTIATES = 506;

  /// <summary>
  ///   HTTP status code 507.
  /// </summary>
  public const int INSUFFICIENT_STORAGE = 507;

  /// <summary>
  ///   HTTP status code 508.
  /// </summary>
  public const int LOOP_DETECTED = 508;

  /// <summary>
  ///   HTTP status code 510.
  /// </summary>
  public const int NOT_EXTENDED = 510;

  /// <summary>
  ///   HTTP status code 511.
  /// </summary>
  public const int NETWORK_AUTHENTICATION_REQUIRED = 511;
}
