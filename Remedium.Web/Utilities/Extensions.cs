using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Remedium.Web.Utilities
{
    public static class Extensions
    {
        public static async Task ForEachAsync<T>(this IEnumerable<T> collection, Func<T, Task> func)
        {
            foreach (var value in collection)
            {
                await func(value);
            }
        }
    }
}