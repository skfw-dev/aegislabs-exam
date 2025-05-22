namespace AegisLabsExam.Common;

public enum HttpStatusCode
{
    Continue                      = 100,
    SwitchingProtocols            = 101,
    Processing                    = 102,
    EarlyHints                    = 103,
    Ok                            = 200,
    Created                       = 201,
    Accepted                      = 202,
    NonAuthoritativeInformation   = 203,
    NoContent                     = 204,
    ResetContent                  = 205,
    PartialContent                = 206,
    MultiStatus                   = 207,
    AlreadyReported               = 208,
    ImUsed                        = 226,
    MultipleChoices               = 300,
    MovedPermanently              = 301,
    Found                         = 302,
    SeeOther                      = 303,
    NotModified                   = 304,
    UseProxy                      = 305,
    Unused                        = 306,
    TemporaryRedirect             = 307,
    PermanentRedirect             = 308,
    BadRequest                    = 400,
    Unauthorized                  = 401,
    PaymentRequired               = 402,
    Forbidden                     = 403,
    NotFound                      = 404,
    MethodNotAllowed              = 405,
    NotAcceptable                 = 406,
    ProxyAuthenticationRequired   = 407,
    RequestTimeout                = 408,
    Conflict                      = 409,
    Gone                          = 410,
    LengthRequired                = 411,
    PreconditionFailed            = 412,
    PayloadTooLarge               = 413,
    RequestUriTooLong             = 414,
    UnsupportedMediaType          = 415,
    RequestedRangeNotSatisfiable  = 416,
    ExpectationFailed             = 417,
    ImATeapot                     = 418,
    InsufficientSpaceOnResource   = 419,
    MethodFailure                 = 420,
    MisdirectedRequest            = 421,
    UnprocessableEntity           = 422,
    Locked                        = 423,
    FailedDependency              = 424,
    UpgradeRequired               = 426,
    PreconditionRequired          = 428,
    TooManyRequests               = 429,
    RequestHeaderFieldsTooLarge   = 431,
    UnavailableForLegalReasons    = 451,
    InternalServerError           = 500,
    NotImplemented                = 501,
    BadGateway                    = 502,
    ServiceUnavailable            = 503,
    GatewayTimeout                = 504,
    HttpVersionNotSupported       = 505,
    VariantAlsoNegotiates         = 506,
    InsufficientStorage           = 507,
    LoopDetected                  = 508,
    NotExtended                   = 510,
    NetworkAuthenticationRequired = 511,
}

public static class HttpStatusCodeExtensions
{
    /// <summary>
    /// Determines if the given HTTP status code is an OK status code
    /// (i.e., in the 200-299 range).
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code to check.</param>
    /// <returns><see langword="true"/> if the status code is an OK status code, <see langword="false"/> otherwise.</returns>
    public static bool IsOk(this HttpStatusCode httpStatusCode) => httpStatusCode.ToValue() is >= 200 and < 300;
    
    /// <summary>
    /// Converts an HTTP status code to its corresponding integer value.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code to convert.</param>
    /// <returns>The integer value of the HTTP status code.</returns>
    public static int ToValue(this HttpStatusCode httpStatusCode) => (int)httpStatusCode;
    
    /// <summary>
    /// Converts an HTTP status code to its corresponding string representation.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code to convert.</param>
    /// <returns>The string representation of the HTTP status code.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the provided HTTP status code does not match any known HTTP status code.
    /// </exception>
    public static string ToDescriptionString(this HttpStatusCode httpStatusCode) => httpStatusCode switch
    {
        HttpStatusCode.Continue => "CONTINUE",
        HttpStatusCode.SwitchingProtocols => "SWITCHING_PROTOCOLS",
        HttpStatusCode.Processing => "PROCESSING",
        HttpStatusCode.EarlyHints => "EARLY_HINTS",
        HttpStatusCode.Ok => "OK",
        HttpStatusCode.Created => "CREATED",
        HttpStatusCode.Accepted => "ACCEPTED",
        HttpStatusCode.NonAuthoritativeInformation => "NON_AUTHORITATIVE_INFORMATION",
        HttpStatusCode.NoContent => "NO_CONTENT",
        HttpStatusCode.ResetContent => "RESET_CONTENT",
        HttpStatusCode.PartialContent => "PARTIAL_CONTENT",
        HttpStatusCode.MultiStatus => "MULTI_STATUS",
        HttpStatusCode.AlreadyReported => "ALREADY_REPORTED",
        HttpStatusCode.ImUsed => "IM_USED",
        HttpStatusCode.MultipleChoices => "MULTIPLE_CHOICES",
        HttpStatusCode.MovedPermanently => "MOVED_PERMANENTLY",
        HttpStatusCode.Found => "FOUND",
        HttpStatusCode.SeeOther => "SEE_OTHER",
        HttpStatusCode.NotModified => "NOT_MODIFIED",
        HttpStatusCode.UseProxy => "USE_PROXY",
        HttpStatusCode.Unused => "UNUSED",
        HttpStatusCode.TemporaryRedirect => "TEMPORARY_REDIRECT",
        HttpStatusCode.PermanentRedirect => "PERMANENT_REDIRECT",
        HttpStatusCode.BadRequest => "BAD_REQUEST",
        HttpStatusCode.Unauthorized => "UNAUTHORIZED",
        HttpStatusCode.PaymentRequired => "PAYMENT_REQUIRED",
        HttpStatusCode.Forbidden => "FORBIDDEN",
        HttpStatusCode.NotFound => "NOT_FOUND",
        HttpStatusCode.MethodNotAllowed => "METHOD_NOT_ALLOWED",
        HttpStatusCode.NotAcceptable => "NOT_ACCEPTABLE",
        HttpStatusCode.ProxyAuthenticationRequired => "PROXY_AUTHENTICATION_REQUIRED",
        HttpStatusCode.RequestTimeout => "REQUEST_TIMEOUT",
        HttpStatusCode.Conflict => "CONFLICT",
        HttpStatusCode.Gone => "GONE",
        HttpStatusCode.LengthRequired => "LENGTH_REQUIRED",
        HttpStatusCode.PreconditionFailed => "PRECONDITION_FAILED",
        HttpStatusCode.PayloadTooLarge => "PAYLOAD_TOO_LARGE",
        HttpStatusCode.RequestUriTooLong => "REQUEST_URI_TOO_LONG",
        HttpStatusCode.UnsupportedMediaType => "UNSUPPORTED_MEDIA_TYPE",
        HttpStatusCode.RequestedRangeNotSatisfiable => "REQUESTED_RANGE_NOT_SATISFIABLE",
        HttpStatusCode.ExpectationFailed => "EXPECTATION_FAILED",
        HttpStatusCode.ImATeapot => "IM_A_TEAPOT",
        HttpStatusCode.InsufficientSpaceOnResource => "INSUFFICIENT_SPACE_ON_RESOURCE",
        HttpStatusCode.MethodFailure => "METHOD_FAILURE",
        HttpStatusCode.MisdirectedRequest => "MISDIRECTED_REQUEST",
        HttpStatusCode.UnprocessableEntity => "UNPROCESSABLE_ENTITY",
        HttpStatusCode.Locked => "LOCKED",
        HttpStatusCode.FailedDependency => "FAILED_DEPENDENCY",
        HttpStatusCode.UpgradeRequired => "UPGRADE_REQUIRED",
        HttpStatusCode.PreconditionRequired => "PRECONDITION_REQUIRED",
        HttpStatusCode.TooManyRequests => "TOO_MANY_REQUESTS",
        HttpStatusCode.RequestHeaderFieldsTooLarge => "REQUEST_HEADER_FIELDS_TOO_LARGE",
        HttpStatusCode.UnavailableForLegalReasons => "UNAVAILABLE_FOR_LEGAL_REASONS",
        HttpStatusCode.InternalServerError => "INTERNAL_SERVER_ERROR",
        HttpStatusCode.NotImplemented => "NOT_IMPLEMENTED",
        HttpStatusCode.BadGateway => "BAD_GATEWAY",
        HttpStatusCode.ServiceUnavailable => "SERVICE_UNAVAILABLE",
        HttpStatusCode.GatewayTimeout => "GATEWAY_TIMEOUT",
        HttpStatusCode.HttpVersionNotSupported => "HTTP_VERSION_NOT_SUPPORTED",
        HttpStatusCode.VariantAlsoNegotiates => "VARIANT_ALSO_NEGOTIATES",
        HttpStatusCode.InsufficientStorage => "INSUFFICIENT_STORAGE",
        HttpStatusCode.LoopDetected => "LOOP_DETECTED",
        HttpStatusCode.NotExtended => "NOT_EXTENDED",
        HttpStatusCode.NetworkAuthenticationRequired => "NETWORK_AUTHENTICATION_REQUIRED",
        _ => "UNKNOWN_STATUS_CODE"
    };
}

public static class HttpStatusCodeUtils {
    public static bool IsOk(HttpStatusCode httpStatusCode) => httpStatusCode.IsOk();

    /// <summary>
    /// Converts the specified <see cref="HttpStatusCode"/> to its integer representation.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code to convert.</param>
    /// <returns>The integer value of the specified HTTP status code.</returns>
    public static int GetValue(HttpStatusCode httpStatusCode) => httpStatusCode.ToValue();
    
    /// <summary>
    /// Gets the string representation of the specified <see cref="HttpStatusCode"/>.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code to convert to a string.</param>
    /// <returns>A string representation of the specified HTTP status code.</returns>
    public static string GetDescriptionString(HttpStatusCode httpStatusCode) => httpStatusCode.ToDescriptionString();
    
    /// <summary>
    /// Converts the specified integer to its corresponding <see cref="HttpStatusCode"/>.
    /// </summary>
    /// <param name="httpStatusCode">The integer to convert to an HTTP status code.</param>
    /// <returns>The <see cref="HttpStatusCode"/> represented by the specified integer.</returns>
    public static HttpStatusCode GetHttpStatusCode(int httpStatusCode) => (HttpStatusCode)httpStatusCode;
    
    /// <summary>
    /// Converts a string description of an HTTP status code to its corresponding <see cref="HttpStatusCode"/>.
    /// </summary>
    /// <param name="description">The string description of the HTTP status code.</param>
    /// <returns>The corresponding <see cref="HttpStatusCode"/> for the given description.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the provided description does not match any known HTTP status code.
    /// </exception>
    public static HttpStatusCode GetHttpStatusCodeFromDescription(string description) => TransformText.ToUpperSnakeCase(description) switch
    {
        "CONTINUE" => HttpStatusCode.Continue,
        "SWITCHING_PROTOCOLS" => HttpStatusCode.SwitchingProtocols,
        "PROCESSING" => HttpStatusCode.Processing,
        "EARLY_HINTS" => HttpStatusCode.EarlyHints,
        "OK" => HttpStatusCode.Ok,
        "CREATED" => HttpStatusCode.Created,
        "ACCEPTED" => HttpStatusCode.Accepted,
        "NON_AUTHORITATIVE_INFORMATION" => HttpStatusCode.NonAuthoritativeInformation,
        "NO_CONTENT" => HttpStatusCode.NoContent,
        "RESET_CONTENT" => HttpStatusCode.ResetContent,
        "PARTIAL_CONTENT" => HttpStatusCode.PartialContent,
        "MULTI_STATUS" => HttpStatusCode.MultiStatus,
        "ALREADY_REPORTED" => HttpStatusCode.AlreadyReported,
        "IM_USED" => HttpStatusCode.ImUsed,
        "MULTIPLE_CHOICES" => HttpStatusCode.MultipleChoices,
        "MOVED_PERMANENTLY" => HttpStatusCode.MovedPermanently,
        "FOUND" => HttpStatusCode.Found,
        "SEE_OTHER" => HttpStatusCode.SeeOther,
        "NOT_MODIFIED" => HttpStatusCode.NotModified,
        "USE_PROXY" => HttpStatusCode.UseProxy,
        "UNUSED" => HttpStatusCode.Unused,
        "TEMPORARY_REDIRECT" => HttpStatusCode.TemporaryRedirect,
        "PERMANENT_REDIRECT" => HttpStatusCode.PermanentRedirect,
        "BAD_REQUEST" => HttpStatusCode.BadRequest,
        "UNAUTHORIZED" => HttpStatusCode.Unauthorized,
        "PAYMENT_REQUIRED" => HttpStatusCode.PaymentRequired,
        "FORBIDDEN" => HttpStatusCode.Forbidden,
        "NOT_FOUND" => HttpStatusCode.NotFound,
        "METHOD_NOT_ALLOWED" => HttpStatusCode.MethodNotAllowed,
        "NOT_ACCEPTABLE" => HttpStatusCode.NotAcceptable,
        "PROXY_AUTHENTICATION_REQUIRED" => HttpStatusCode.ProxyAuthenticationRequired,
        "REQUEST_TIMEOUT" => HttpStatusCode.RequestTimeout,
        "CONFLICT" => HttpStatusCode.Conflict,
        "GONE" => HttpStatusCode.Gone,
        "LENGTH_REQUIRED" => HttpStatusCode.LengthRequired,
        "PRECONDITION_FAILED" => HttpStatusCode.PreconditionFailed,
        "PAYLOAD_TOO_LARGE" => HttpStatusCode.PayloadTooLarge,
        "REQUEST_URI_TOO_LONG" => HttpStatusCode.RequestUriTooLong,
        "UNSUPPORTED_MEDIA_TYPE" => HttpStatusCode.UnsupportedMediaType,
        "REQUESTED_RANGE_NOT_SATISFIABLE" => HttpStatusCode.RequestedRangeNotSatisfiable,
        "EXPECTATION_FAILED" => HttpStatusCode.ExpectationFailed,
        "IM_A_TEAPOT" => HttpStatusCode.ImATeapot,
        "INSUFFICIENT_SPACE_ON_RESOURCE" => HttpStatusCode.InsufficientSpaceOnResource,
        "METHOD_FAILURE" => HttpStatusCode.MethodFailure,
        "MISDIRECTED_REQUEST" => HttpStatusCode.MisdirectedRequest,
        "UNPROCESSABLE_ENTITY" => HttpStatusCode.UnprocessableEntity,
        "LOCKED" => HttpStatusCode.Locked,
        "FAILED_DEPENDENCY" => HttpStatusCode.FailedDependency,
        "UPGRADE_REQUIRED" => HttpStatusCode.UpgradeRequired,
        "PRECONDITION_REQUIRED" => HttpStatusCode.PreconditionRequired,
        "TOO_MANY_REQUESTS" => HttpStatusCode.TooManyRequests,
        "REQUEST_HEADER_FIELDS_TOO_LARGE" => HttpStatusCode.RequestHeaderFieldsTooLarge,
        "UNAVAILABLE_FOR_LEGAL_REASONS" => HttpStatusCode.UnavailableForLegalReasons,
        "INTERNAL_SERVER_ERROR" => HttpStatusCode.InternalServerError,
        "NOT_IMPLEMENTED" => HttpStatusCode.NotImplemented,
        "BAD_GATEWAY" => HttpStatusCode.BadGateway,
        "SERVICE_UNAVAILABLE" => HttpStatusCode.ServiceUnavailable,
        "GATEWAY_TIMEOUT" => HttpStatusCode.GatewayTimeout,
        "HTTP_VERSION_NOT_SUPPORTED" => HttpStatusCode.HttpVersionNotSupported,
        "VARIANT_ALSO_NEGOTIATES" => HttpStatusCode.VariantAlsoNegotiates,
        "INSUFFICIENT_STORAGE" => HttpStatusCode.InsufficientStorage,
        "LOOP_DETECTED" => HttpStatusCode.LoopDetected,
        "NOT_EXTENDED" => HttpStatusCode.NotExtended,
        "NETWORK_AUTHENTICATION_REQUIRED" => HttpStatusCode.NetworkAuthenticationRequired,
        _ => throw new ArgumentOutOfRangeException(nameof(description), description, "The provided description does not match any known HTTP status code.")
    };
}