using System.Text.Json.Serialization;
using System.Text.Json;

// Largely inspired by:
// https://stackoverflow.com/questions/57626878/the-json-value-could-not-be-converted-to-system-int32
namespace BookWebApi.Converters;
/// <summary>
/// Converts a <see cref="Guid"/> to and from its <see cref="System.String"/> representation.
/// </summary>
public class GuidConverter : JsonConverter<Guid>
{
    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>Returns <c>true</c> if this instance can convert the specified object type; otherwise <c>false</c>.</returns>
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsAssignableFrom(typeof(Guid));
    }


    /// <summary>
    /// Reads the JSON representation of the object.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            Guid returnRead = Guid.Empty;
            if (reader.TokenType == JsonTokenType.String && string.IsNullOrEmpty(reader.GetString()))
            {
                return Guid.Empty;
            }
            return reader.GetGuid();
        }
        catch
        {
            return Guid.Empty;
        }
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}