using System.Collections.Generic;
using System.Collections.Specialized;

namespace Serilog.Enrichers.HttpHeader.Extensions
{
    internal static class NameValueCollectionExtensions
    {
        public static bool TryGetValue(this NameValueCollection collection, string key, out IEnumerable<string> values)
        {
            values = collection.GetValues(key);

            return values != null;
        }
    }
}
