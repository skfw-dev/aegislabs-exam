namespace NokoCore.Common;

/// <summary>
/// Represents a contract for handling or identifying error-related functionalities.
/// Implementations of this interface are used to define specific error types or structures
/// that can be used for error management scenarios.
/// </summary>
public interface IError;

/// <summary>
/// Defines a contract for exceptions, providing a structure for handling and identifying
/// error scenarios that integrate with the error-handling framework.
/// Implementations typically extend error functionalities to represent exceptions
/// with additional context or structured nesting.
/// </summary>
public interface IException : IError;

/// <summary>
/// Represents an error type that encapsulates specific error details and supports nested error structures.
/// This class is typically used to define and propagate standardized errors within an application.
/// </summary>
public class Error(string message, Error? inner = null) : Exception(message, inner), IError;

/// <summary>
/// Represents a specialized error type extending the base error functionalities.
/// This class is typically used to signify system-level errors, offering a structured
/// approach to define and manage errors specific to system operations.
/// </summary>
public class SystemError(string message, Error? inner = null) : Error(message, inner);
