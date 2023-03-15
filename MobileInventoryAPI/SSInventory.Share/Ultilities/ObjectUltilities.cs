using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SSInventory.Share.Ultilities
{
    public static class ObjectUltilities
    {
        public static Dictionary<string, object> DictionaryFromType(this object atype)
        {
            if (atype == null) return new Dictionary<string, object>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(atype, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }

        public static string ConvertObjectToString(this object data)
        {
            var returnVal = new StringBuilder("");
            var objectProperties = data.DictionaryFromType();
            var index = 0;
            foreach (var property in objectProperties)
            {
                returnVal.AppendFormat("{0}:{1}{2}", property.Key, property.Value, index < objectProperties.Count ? "," : "");
                index++;
            }

            return returnVal.ToString();
        }
    }
}
