using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nancy.Simple
{
    public enum PlayerStatus
    {
        Active,
        Folded,
        Out
    }

    class PlayerStatusConverter : JsonConverter
    {
        private readonly Dictionary<PlayerStatus, string> _internalMap;

        public PlayerStatusConverter()
        {
            _internalMap = new Dictionary<PlayerStatus, string>();

            _internalMap[PlayerStatus.Active] = "active";
            _internalMap[PlayerStatus.Folded] = "folded";
            _internalMap[PlayerStatus.Out] = "out";
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

            throw new ArgumentOutOfRangeException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string foundValue = null;
            if (value is PlayerStatus && _internalMap.TryGetValue((PlayerStatus)value, out foundValue))
            {
                writer.WriteValue(foundValue);
            }
        }
    }
}
