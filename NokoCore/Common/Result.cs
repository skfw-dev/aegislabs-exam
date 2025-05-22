namespace NokoCore.Common;

/// <summary>
/// Defines an interface representing the outcome of an operation, where the result can either succeed with a value of a specified type or fail.
/// </summary>
/// <typeparam name="T">The type of the result value for successful operations.</typeparam>
public interface IResult<T> : IOption<T>
{
    public IError? Error { get; }
}

/// <summary>
/// Represents the result of an operation, encapsulating whether the operation was successful or not,
/// along with an optional value of a specified type for successful results.
/// </summary>
/// <typeparam name="T">The type of the result value for successful operations.</typeparam>
public struct Result<T>(T? value, IError? error = null) : IResult<T>
{
    public IError? Error => error;
    
    public bool Ok => error == null;
    public bool IsNone => !Ok;
    
    public T? Value { get; set; } = value;
    
    public IOption<T> None => new Option<T>();

    /// <summary>
    /// Creates a new result indicating a successful operation with the specified value.
    /// </summary>
    /// <param name="value">The value to be encapsulated in the result, indicating success.</param>
    /// <returns>A result object representing a successful operation containing the given value.</returns>
    public static IResult<T> Set(T? value = default) => new Result<T>(value);
    
    /// <summary>
    /// Creates a new result indicating a failed operation with the specified error.
    /// </summary>
    /// <param name="error">The error to be encapsulated in the result, indicating failure.</param>
    /// <returns>A result object representing a failed operation containing the given error.</returns>
    public static IResult<T> Err(IError? error = null) => new Result<T>(default, error ?? new SystemError("exited with error"));
    
    /// <summary>
    /// Creates a new result indicating a failed operation with the specified error message and optional inner error.
    /// </summary>
    /// <param name="message">The error message to be encapsulated in the result, indicating failure.</param>
    /// <param name="inner">An optional inner error to be encapsulated in the result, indicating failure.</param>
    /// <returns>A result object representing a failed operation containing the given error message and optional inner error.</returns>
    public static IResult<T> Err(string message, Error? inner) => new Result<T>(default, new Error(message, inner));

    /// <summary>
    /// Unwraps the value of this result, throwing an exception if this result is not successful (i.e., <see cref="IOption{T}.None"/>).
    /// </summary>
    /// <returns>The value of this result, if it is not <see cref="IOption{T}.None"/>.</returns>
    /// <exception cref="NullReferenceException">This result is <see cref="IOption{T}.None"/>.</exception>
    public T Unwrap()
    {
        if (!Ok) throw new NullReferenceException("Result is None");
        return Value!;
    }
}
