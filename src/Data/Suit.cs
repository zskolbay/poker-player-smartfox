using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nancy.Simple
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Spades,
        Clubs
    }

    class SuitConverter : JsonConverter
    {
        private readonly Dictionary<Suit, string> _internalMap;

        public SuitConverter()
        {
            _internalMap = new Dictionary<Suit, string>();

            _internalMap[Suit.Clubs] = "clubs";
            _internalMap[Suit.Hearts] = "hearts";
            _internalMap[Suit.Diamonds] = "diamonds";
            _internalMap[Suit.Spades] = "spades";
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var readString = (string)reader.Value;

            if (_internalMap.ContainsValue(readString))
            {
                return _internalMap.First(pair => pair.Value == readString).Key;
            }

            throw new ArgumentOutOfRangeException(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string foundValue = null;
            if (value is Suit && _internalMap.TryGetValue((Suit)value, out foundValue))
            {
                writer.WriteValue(foundValue);
            }
        }
    }
}
