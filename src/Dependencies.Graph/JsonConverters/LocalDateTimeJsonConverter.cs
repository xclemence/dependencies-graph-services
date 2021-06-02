using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Neo4j.Driver;

namespace Dependencies.Graph.JsonConverters
{
    public class LocalDateTimeJsonConverter : JsonConverter<LocalDateTime>
    {
        public override LocalDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(DateTime.Parse(reader.GetString()));

        public override void Write(Utf8JsonWriter writer, LocalDateTime value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToDateTime().ToString());
    }
}
