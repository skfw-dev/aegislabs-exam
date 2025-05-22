namespace AegisLabsExam.Tests;

public interface IConsoleTestOutputHelper : ITestOutputHelper
{
    public Stream Stdout { get; }
    public StreamWriter Writer { get; }
} 

public class ConsoleTestOutputHelper : IConsoleTestOutputHelper
{
    public Stream Stdout { get; }
    public StreamWriter Writer { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleTestOutputHelper"/> class,
    /// which enables real-time console output by setting the standard output stream
    /// to a writer with an auto-flush enabled.
    /// </summary>
    public ConsoleTestOutputHelper()
    {
        // Enable real-time console output
        Stdout = Console.OpenStandardOutput();
        Writer = new StreamWriter(Stdout) { AutoFlush = true };
        Console.SetOut(Writer);
    }

    /// <summary>
    /// Gets the output associated with the console test helper.
    /// This property is intended to represent any text or data produced
    /// during the execution of test helper methods, but currently returns an empty string.
    /// </summary>
    public string Output => string.Empty;

    /// <summary>
    /// Writes the specified message to the console output using the writer.
    /// </summary>
    /// <param name="message">The message to write to the console output.</param>
    public void Write(string message) => Writer.Write(message);

    /// <summary>
    /// Writes the specified message to the console output using the writer.
    /// </summary>
    /// <param name="format">The format string to use for writing the message.</param>
    /// <param name="args">The arguments to use in the format string.</param>
    public void Write(string format, params object[] args) => Writer.Write(format, args);
    
    /// <summary>
    /// Writes the specified message to the console output, followed by a line
    /// terminator, using the writer.
    /// </summary>
    /// <param name="message">The message to write to the console output.</param>
    public void WriteLine(string message) => Writer.WriteLine(message);

    /// <summary>
    /// Writes the specified message to the console output, followed by a line
    /// terminator, using the writer. The message is formatted using the
    /// specified format string and arguments.
    /// </summary>
    /// <param name="format">The format string to use for writing the message.</param>
    /// <param name="args">The arguments to use in the format string.</param>
    public void WriteLine(string format, params object[] args) => Writer.WriteLine(format, args);
}
