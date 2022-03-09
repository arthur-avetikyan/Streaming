using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace KioskStream.Web.Client.Extensions
{
    public static class QueryExtensions
    {
        public static string GetReturnUrlFromQuery(this string query)
        {
            string queryOption = "returnUrl=";
            int lastIndex = query.IndexOf(queryOption);
            if (lastIndex > 0)
                return query.Substring(lastIndex + queryOption.Length);
            else
                return null;
        }

        public static int GetNumberFromQuery(this string query, string key)
        {
            int id = 0;
            if (QueryHelpers.ParseQuery(query).TryGetValue(key, out StringValues tempId))
            {
                int.TryParse(tempId, out int idResult);
                id = idResult;
            }
            return id;
        }

        public static bool GetBoolFromQuery(this string query, string key)
        {
            bool current = false;
            if (QueryHelpers.ParseQuery(query).TryGetValue(key, out StringValues currentValue))
            {
                bool.TryParse(currentValue, out bool currentResult);
                current = currentResult;
            }
            return current;
        }
    }
}