using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace measurement_generator.Models;

    public class UtcDateTimeConverter :JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString(), null, DateTimeStyles.AdjustToUniversal);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
     }

