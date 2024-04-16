using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prod.Domain.Common.Extentions;

public static class DateTimeExtensions
{
    public static JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new DateTimeConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    
    public static string ToRfcFormat(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd") + "T" +
               dateTime.ToString("HH:mm:ss") + "Z"
               + "00:00";
    }
}

public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && DateTime.TryParse(reader.GetString(), out DateTime dateTime))
        {
            return dateTime;
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var convertedDateTime = value.ToString("yyyy-MM-dd") + "T" + value.ToString("HH:mm:ss") + "Z" + "00:00";
        writer.WriteStringValue(convertedDateTime);
    }
}