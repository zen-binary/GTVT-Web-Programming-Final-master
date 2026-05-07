using Project_Final.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Project_Final.Utils
{
    public class ProductJsonConverter : JsonConverter<Metadata>
    {
        public override Metadata? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Đọc JSON thành JsonDocument để có thể xử lý tùy chỉnh
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = document.RootElement;

                // Kiểm tra nếu có thuộc tính "Type" để xác định kiểu con cụ thể
                if (root.TryGetProperty("Type", out JsonElement typeProperty))
                {
                    string? type = typeProperty.GetString();
                    Console.WriteLine("Root: " + root);
                    // Xử lý các trường hợp dựa trên loại con
                    return type switch
                    {
                        nameof(ClothesMetadata) => JsonSerializer.Deserialize<ClothesMetadata>(root.GetRawText(), options),
                        nameof(ShoeMetadata) => JsonSerializer.Deserialize<ShoeMetadata>(root.GetRawText(), options),
                        _ => throw new NotSupportedException("Type: " + type + " is not supported deserializer")
                    };
                }

                throw new JsonException("Missing field 'Type' property in JSON.");
            }
        }

        public override void Write(Utf8JsonWriter writer, Metadata value, JsonSerializerOptions options)
        {
            // Serialize với thông tin đầy đủ của lớp con
            JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
        }
    }
}
