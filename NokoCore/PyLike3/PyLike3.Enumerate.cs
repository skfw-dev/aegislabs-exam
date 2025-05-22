namespace NokoCore.PyLike3;

/// <summary>
/// Interface that mimics Python's type "enumerate".
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPyLike3Enumerate<T> : IPyLike3Iterator<(int Index, T Item)>;

/// <summary>
/// Interface that mimics Python's default type "enumerate".
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPyLikeEnumerate<T> : IPyLike3Enumerate<T>, IPyLike;

/// <summary>
/// A class that provides functionality similar to Python's built-in "enumerate",
/// associating an Index with each Item in the given collection.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public class PyLike3Enumerate<T>(IEnumerable<T> data) : IPyLike3Enumerate<T>
{
    /// <summary>
    /// Returns an enumerator that iterates through the collection of elements,
    /// with each element being a tuple containing the zero-based index of the
    /// element and the element itself.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerator{T}"/> for the collection of elements.
    /// </returns>
    public IEnumerator<(int Index, T Item)> GetEnumerator()
    {
        return data.Select((item, index) => (index, item)).GetEnumerator();
    }

    /// <inheritdoc cref="GetEnumerator"/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// A class that mimics Python's "enumerate" built-in function, extending functionality
/// to provide both the index and corresponding item from a sequence, adhering to
/// Python-like interfaces.
/// </summary>
/// <typeparam name="T">The type of elements in the sequence.</typeparam>
/// <param name="data">The sequence of elements to enumerate.</param>
public class PyLikeEnumerate<T>(IEnumerable<T> data) : PyLike3Enumerate<T>(data), IPyLikeEnumerate<T>;

/// <summary>
/// Utility class that mimics Python's built-in functions.
/// </summary>
public static partial class PyLike3
{
    /// <summary>
    /// Like Python's <c>enumerate</c> - wraps an <see cref="IEnumerable{T}" /> into an <see cref="IPyLikeEnumerate{T}" />, 
    /// pairing each item with its index.
    /// </summary>
    /// <param name="data">The enumerable to iterate over.</param>
    /// <typeparam name="T">The type of each item in the enumerable.</typeparam>
    /// <returns>
    /// The enumerable wrapped into an <see cref="IPyLikeEnumerate{T}" />, where each item is a tuple containing the index 
    /// and the item.
    /// </returns>
    public static IPyLikeEnumerate<T> Enumerate<T>(IEnumerable<T> data)
    {
        // Like Python's "enumerate"
        return new PyLikeEnumerate<T>(data);
    }
}