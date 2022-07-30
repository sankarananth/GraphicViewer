using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonReader
{
    public class ShapesConverter : JsonCreationConverter<Shapes>
    {
		protected override Shapes Create(Type objectType, JObject jObject)
        {
            if (FieldExists("a", jObject) && FieldExists("b", jObject) && !FieldExists("c", jObject))
            {
                return new Line();
            }
            else if (FieldExists("center", jObject))
            {
                return new Circle();
            }
            else
            {
                return new Triangle();
            }
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
