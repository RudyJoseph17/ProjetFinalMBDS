using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Converters
{
    public class OuiNonBoolConverter : JsonConverter<bool?>
    {
        public override void WriteJson(JsonWriter writer, bool? value, JsonSerializer serializer)
        {
            if (value.HasValue)
                writer.WriteValue(value.Value ? "oui" : "non");
            else
                writer.WriteNull();
        }

        public override bool? ReadJson(JsonReader reader, Type objectType, bool? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var val = reader.Value?.ToString()?.ToLower();
            if (val == "oui") return true;
            if (val == "non") return false;
            return null;
        }
    }

}
