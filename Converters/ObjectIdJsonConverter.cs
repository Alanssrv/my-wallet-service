using MongoDB.Bson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyWallet.Converters;

public class ObjectIdJsonConverter : JsonConverter<ObjectId>
{
    public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("ObjectId must be a string.");
        }

        var value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value) || !ObjectId.TryParse(value, out var objectId))
        {
            throw new JsonException("Invalid ObjectId value.");
        }

        return objectId;
    }

    public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
