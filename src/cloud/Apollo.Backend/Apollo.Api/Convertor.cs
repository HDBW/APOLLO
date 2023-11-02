using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Apollo.Api
{
    /// <summary>
    /// Provides methods to convert objects to expando objects.
    /// </summary>
    internal static class Convertor
    {
        /// <summary>
        ///  Converts the items to expando objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<ExpandoObject> Convert<T>(ICollection<T> items)
        {
            List<ExpandoObject> expandoItems = new List<ExpandoObject>();

            foreach (T item in items)
            {
                expandoItems.Add(Convertor.Convert(item!));
            }

            return expandoItems;
        }

        /// <summary>
        /// Converts the item to expando object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static ExpandoObject Convert(object item)
        {
            if (item == null)
                throw new ArgumentException("Item cannot be null!");

            ExpandoObject expo = new ExpandoObject();

            IDictionary<string, object> expoDict = expo as IDictionary<string, object>;

            foreach (var prop in item.GetType().GetProperties())
            {
                expoDict.Add(prop.Name, prop.GetValue(item)!);
            }

            return expo;
        }
    }
}
