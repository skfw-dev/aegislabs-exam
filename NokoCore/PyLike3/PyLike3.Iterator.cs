using JetBrains.Annotations;

namespace NokoCore.PyLike3;

/// <summary>
/// Interface that mimics Python's type "iterator".
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPyLike3Iterator<out T> : IEnumerable<T>, IPyLike3;

/// <summary>
/// Interface that mimics Python's default type "iterator".
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPyLikeIterator<out T> : IPyLike3Iterator<T>, IPyLike;

/// <summary>
/// Class that provides an implementation for a Python-like iterator.
/// It iterates over a collection of elements, associating each element with its index.
/// </summary>
/// <typeparam name="T">The type of elements in the wrapped collection.</typeparam>
/// <param name="data">The input collection to be iterated over.</param>
public class PyLike3Iterator<T>(IEnumerable<T> data) : IPyLike3Iterator<T>
{
    /// <summary>
    /// Returns an enumerator that iterates through the collection of elements.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerator{T}"/> for the collection of elements.
    /// </returns>
    public IEnumerator<T> GetEnumerator() => data.GetEnumerator();

    /// <inheritdoc cref="GetEnumerator"/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Class that mimics Python's default type "iterator."
/// </summary>
/// <typeparam name="T">The type of elements being iterated over, with an associated index.</typeparam>
/// <param name="data">The data collection to be wrapped by the iterator.</param>
public class PyLikeIterator<T>(IEnumerable<T> data) : PyLike3Iterator<T>(data), IPyLikeIterator<T>;

/// <summary>
/// Utility class that mimics Python's built-in functions.
/// </summary>
public static partial class PyLike3
{
    /// <summary>
    /// Like Python's <c>iter</c> - wraps an <see cref="IEnumerable{T}" /> into an <see cref="IPyLikeIterator{T}" />.
    /// </summary>
    /// <param name="data">The enumerable to iterate over.</param>
    /// <typeparam name="T">The type of each item in the enumerable.</typeparam>
    /// <returns>The enumerable wrapped into an <see cref="IPyLikeIterator{T}" />.</returns>
    [MustDisposeResource]
    public static IPyLikeIterator<T> Iterator<T>(IEnumerable<T> data)
    {
        // Like Python's "iter"
        return new PyLikeIterator<T>(data);
    }
}