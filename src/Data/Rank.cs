using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nancy.Simple
{
    public enum Rank
    {
        Number2 = 2,
        Number3 = 3,
        Number4 = 4,
        Number5 = 5,
        Number6 = 6,
        Number7 = 7,
        Number8 = 8,
        Number9 = 9,
        Number10 = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }

    class RankConverter : JsonConverter
    {
        private readonly Dictionary<Rank, string> _internalMap;

        public RankConverter()
        {
            _internalMap = new Dictionary<Rank, string>();

            _internalMap[Rank.Number2] = "2";
            _internalMap[Rank.Number3] = "3";
            _internalMap[Rank.Number4] = "4";
            _internalMap[Rank.Number5] = "5";
            _internalMap[Rank.Number6] = "6";
            _internalMap[Rank.Number7] = "7";
            _internalMap[Rank.Number8] = "8";
            _internalMap[Rank.Number9] = "9";
            _internalMap[Rank.Number10] = "10";
            _internalMap[Rank.Jack] = "J";
            _internalMap[Rank.Queen] = "Q";
            _internalMap[Rank.King] = "K";
            _internalMap[Rank.Ace] = "A";
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var readString = (string)reader.Value;

            if(_internalMap.ContainsValue(readString))
            {
                return _internalMap.First(pair => pair.Value == readString).Key;
            }

            throw new ArgumentOutOfRangeException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string foundValue = null;
            if (value is Rank && _internalMap.TryGetValue((Rank)value, out foundValue))
            {
                writer.WriteValue(foundValue);
            }
        }
    }
}
