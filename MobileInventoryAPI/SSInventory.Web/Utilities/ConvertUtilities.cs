using Newtonsoft.Json;
using SSInventory.Share.Models.Dto.ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSInventory.Web.Utilities
{
    public static class ConvertUtilities
    {
        public static string ParseArrayToString(this object data)
        {
            var dictionary = new List<Dictionary<string, object>>();
            try
            {
                dictionary = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(data.ToString());
            }
            catch
            {
                var json = JsonConvert.SerializeObject(data);
                dictionary = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
            }

            return dictionary.Count > 0 ? string.Join(", ", dictionary.Select(x => x.Values.FirstOrDefault())) : "";
        }

        public static List<SelectItemOptionModel> ParseSelectedDropdownValueOption(this object value)
        {
            try
            {
                var dictionary = new List<SelectItemOptionModel>();
                try
                {
                    dictionary = JsonConvert.DeserializeObject<List<SelectItemOptionModel>>(value.ToString());
                }
                catch
                {
                    var json = JsonConvert.SerializeObject(value);
                    var data = JsonConvert.DeserializeObject<List<SelectItemOptionModel>>(json);
                    if (data.Count > 0)
                    {
                        return data;
                    }
                }

                return dictionary.Count > 0 ? dictionary : new List<SelectItemOptionModel>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<SelectedItemOptionModel> ParseSelectedItemOptions(this object value)
        {
            try
            {
                var dictionary = new List<Dictionary<string, SelectedItemOptionModel>>();
                try
                {
                    dictionary = JsonConvert.DeserializeObject<List<Dictionary<string, SelectedItemOptionModel>>>(value.ToString());
                }
                catch
                {
                    var json = JsonConvert.SerializeObject(value);
                    var data = JsonConvert.DeserializeObject<List<SelectedItemOptionModel>>(json);
                    if (data.Count > 0)
                    {
                        return data;
                    }
                }

                return dictionary.Count > 0 ? dictionary.Select(x => x.Values.FirstOrDefault()).ToList() : new List<SelectedItemOptionModel>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static decimal ConvertReturnValueToDecimal(this object value)
        {
            return decimal.TryParse(ParseItemTypeOptionReturnValueSingleValue(value), out decimal result)
                    ? result
                    : 0;
        }

        public static int ConvertReturnValueToInt(this object itemTypeOptionReturnValue)
        {
            return int.TryParse(itemTypeOptionReturnValue.ParseItemTypeOptionReturnValueSingleValue(), out int result)
                    ? result
                    : 0;
        }

        public static string ParseItemTypeOptionReturnValueSingleValue(this object itemTypeOptionReturnValue, string valueKey = "returnValue")
        {
            try
            {
                var dictionary = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(itemTypeOptionReturnValue.ToString());
                return dictionary.Count == 0 ? "" : dictionary[0].GetValueOrDefault(valueKey);
            }
            catch
            {
                return "";
            }
        }

        public static (string value1, string value2) ParseCoupleValues(this object itemTypeOptionReturnValue)
        {
            var data = new List<Dictionary<string, object>>();
            try
            {
                data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(itemTypeOptionReturnValue.ToString());
            }
            catch
            {
                var json = JsonConvert.SerializeObject(itemTypeOptionReturnValue);
                data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
            }
            try
            {
                if (data.Count == 0) return (null, null);
                if (data.Count == 1)
                {
                    return (data[0].FirstOrDefault().Value.ToString(), null);
                }

                return (data[0].FirstOrDefault().Value.ToString(), data[1].FirstOrDefault().Value.ToString());
            }
            catch
            {
                return (itemTypeOptionReturnValue.ParseItemTypeOptionReturnValueSingleValue(), null);
            }
        }

    }
}
