using System.Text.Json;
using System.Text.Json.Serialization;
using AegisLabsExam.Common;

namespace AegisLabsExam.Schemas;

public class MessageBody
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }
    
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; } = 200;
    
    [JsonPropertyName("statusText")]
    public string StatusText { get; set; } = "OK";
    
    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;
    
    [JsonPropertyName("errors")]
    public List<object>? Errors { get; set; }
    
    [JsonPropertyName("data")]
    public object? Data { get; set; }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(MessageBody))]
public partial class MessageBodyJsonSerializerContext : JsonSerializerContext;

public static class MessageBodyExtensions
{
    /// <summary>
    /// Updates the properties of a <see cref="MessageBody"/> instance.
    /// </summary>
    /// <param name="messageBody">The message body schema to build.</param>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="message">The message.</param>
    /// <param name="errors">The errors.</param>
    /// <param name="data">The data.</param>
    /// <returns>The built message body schema.</returns>
    public static MessageBody Update(this MessageBody messageBody, HttpStatusCode httpStatusCode, string message, List<object>? errors, object? data)
    {
        messageBody.Ok = HttpStatusCodeUtils.IsOk(httpStatusCode);
        messageBody.StatusCode = HttpStatusCodeUtils.GetValue(httpStatusCode);
        messageBody.StatusText = HttpStatusCodeUtils.GetDescriptionString(httpStatusCode);
        messageBody.Message = message;
        messageBody.Errors = errors;
        messageBody.Data = data;
        return messageBody;
    }
}