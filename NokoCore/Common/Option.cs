namespace NokoCore.Common;

/// <summary>
/// Represents an interface for an optional value, which may exist (be present) or not exist (be absent).
/// </summary>
/// <typeparam name="T">The type of the value that the option may contain.</typeparam>
public interface IOption<T>
{
    public bool Ok { get; }
    public bool IsNone { get; }
    public T? Value { get; set; }
    public IOption<T> None { get; }
    public T Unwrap();
}

/// <summary>
/// Represents an optional value that can either be present with a specific value or absent.
/// </summary>
/// <typeparam name="T">The type of the value that this option can hold.</typeparam>
public struct Option<T>(T? value = default) : IOption<T>
{
    public bool Ok => Value != null;
    public bool IsNone => !Ok;
    
    public T? Value { get; set; } = value;
    
    public IOption<T> None => new Option<T>();

    /// <summary>
    /// Unwrap the value of this option, throwing an exception if this option is <see cref="IOption{T}.None"/>.
    /// </summary>
    /// <returns>The value of this option, if it is not <see cref="IOption{T}.None"/>.</returns>
    /// <exception cref="NullReferenceException">This option is <see cref="IOption{T}.None"/>.</exception>
    public T Unwrap()
    {
        if (!Ok) throw new NullReferenceException("Option is None");
        return Value!;
    }
}
