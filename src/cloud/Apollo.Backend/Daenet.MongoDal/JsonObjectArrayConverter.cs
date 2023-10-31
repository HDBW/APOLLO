using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Daenet.MongoDal.Entitties
{
    /// <summary>
    /// Takes a control of deserialization of Arays of objects. Currentlly used for deserailization of JSON array proeprty <see cref="FieldExpression.Array"/>.
    /// </summary>
    public class JsonObjectArrayConverter : JsonConverter<ICollection<object>>
    {
        /// <summary>
        /// Reads the JSON value and returns the array.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public override ICollection<object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<object> lst = new List<object>();

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                return ReadArray(ref reader, lst);
            }

            else
                throw new NotSupportedException("The reading element must be an array!");
        }

        /// <summary>
        /// Reads the array of values.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="lst"></param>
        /// <returns></returns>
        private static ICollection<object> ReadArray(ref Utf8JsonReader reader, List<object> lst)
        {
            while (reader.Read())
            {
                object val = null;

                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                val = ReadOfObject(ref reader);

                lst.Add(val);
            }
            return lst;
        }

        /// <summary>
        /// Reads the single value from JSON.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static object ReadOfObject(ref Utf8JsonReader reader)
        {
            object val = null;

            if (reader.TokenType == JsonTokenType.Number)
            {
                val = reader.GetDouble();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string st = reader.GetString();
                DateTime dt;
                var formats = new string[] { "dd/MM/yyyy", "dd/MM/yy", "dd.MM.yyyy", "dd.MM.yy", "yyyy-MM-dd", "yyyy-MM" };
                if (DateTime.TryParseExact(st, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out dt))
                    val = dt;
                else
                    val = st;
            }
            else if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
            {
                val = reader.GetBoolean();
            }
            else if (reader.TokenType == JsonTokenType.Null)
            {
                val = null;
            }
            else if (reader.TokenType == JsonTokenType.StartArray)
            {
                val = new List<Object>();
                val = ReadArray(ref reader, val as List<Object>);
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                ExpandoObject dynamicObject = new ExpandoObject();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                        break;
                    var property = ReadProperty(ref reader);
                    if (!property.Equals(default(KeyValuePair<string, object>)))
                    {
                        dynamicObject.TryAdd(property.Key, property.Value);
                    }
                }
                val = dynamicObject;
            }

            return val;
        }

        private static KeyValuePair<string, object> ReadProperty(ref Utf8JsonReader reader)
        {
            string propName = null;
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                propName = reader.GetString();
                reader.Read();
            }
            var propValue = ReadOfObject(ref reader);
            if (propName != null)
            {
                return new KeyValuePair<string, object>(propName, propValue);
            }
            return default;

        }

        public override void Write(Utf8JsonWriter writer, ICollection<object> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}


