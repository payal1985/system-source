using System;

namespace SSInventory.Share.Ultilities
{
    public static class ItemTypeHelpers
    {
        public static string ConvertItemTypeName(this string itemTypeCode, string itemTypeName = null)
        {
            if (!string.IsNullOrWhiteSpace(itemTypeCode)) return itemTypeCode;

            return !string.IsNullOrWhiteSpace(itemTypeName) ? itemTypeName.Replace(" ", "") : string.Empty;
        }

        public static string ConvertNameToCode(this string name)
        {
            return name.Replace(" ", "");
        }

        public static string CombineDateAndTime(DateTime? date, string time, string timeSeparator = ":")
        {
            if (date is null) return string.Empty;

            var dateString = date.Value.ToLongDateString();
            //var dateString = $"{date.Day}{date.Month}{date.Year}";
            //if (!string.IsNullOrWhiteSpace(time))
            //{
            //    var timeString = $"{time.Replace(timeSeparator, "")}";
            //    return dateString + timeString;
            //}

            return dateString;
        }

        public static DateTime? ConvertStringToDatetime(this string dateString, string dateFormat = "")
        {
            if (string.IsNullOrWhiteSpace(dateString)) return null;

            try
            {
                if (!string.IsNullOrWhiteSpace(dateFormat))
                {
                    DateTime.TryParseExact(dateString, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date);
                    return date;
                }
                DateTime.TryParse(dateString, out var result);
                return result;
            }
            catch
            {
                return null;
            }
        }

    }
}
