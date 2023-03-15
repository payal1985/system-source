using Newtonsoft.Json;
using SSInventory.Share.Models.Dto.ItemTypes;
using System;
using System.Collections.Generic;

namespace SSInventory.Share.Ultilities
{
    public static class ConvertUltilities
    {
        public static Dictionary<string, TValue> ToDictionary<TValue>(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json);
            return dictionary;
        }

        public static T ConvertTo<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        //public static T CastObject<T>(this object input)
        //{
        //    return (T)input;
        //}

        public static T CastObject<T>(this object value)
        {
            var json = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
