using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UsbInfo.Extensions
{
    static class EnumerableExtensions
    {
        internal static IEnumerable<T> NotOfType<T>(this IEnumerable<T> source, Type type)
        {
            return source.Where(arg => arg.GetType() != type);
        }
    }
}
