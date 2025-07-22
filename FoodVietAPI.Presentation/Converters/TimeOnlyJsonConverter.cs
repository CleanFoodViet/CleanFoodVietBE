using System.Text.Json;
using System.Text.Json.Serialization;

namespace CleanFoodVietAPI.Presentation.Converters
{
    public class TimeOnlyJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Automatically convert to UTC DateTime with the right Kind
            var dt = reader.GetDateTime();
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Write it in ISO 8601 with "Z"
            writer.WriteStringValue(value.ToUniversalTime().ToString("o")); // e.g., 2025-07-27T02:59:00.0000000Z
        }
    }
}
