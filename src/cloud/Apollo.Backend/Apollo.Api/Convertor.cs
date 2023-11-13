using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Apollo.Api
{
    /// <summary>
    /// Provides methods to convert objects to expando objects.
    /// </summary>
    public static class Convertor
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
                if (prop.Name == "Id")
                    expoDict.Add("_id", prop.GetValue(item)!);
                else
                    expoDict.Add(prop.Name, prop.GetValue(item)!);
            }

            return expo;
        }

        public static Daenet.MongoDal.Entitties.Query ToDaenetQuery(Apollo.Common.Entities.Query apiQuery)
        {
            Daenet.MongoDal.Entitties.Query daenetQuery = new();
            daenetQuery.IsOrOperator = apiQuery.IsOrOperator;
            daenetQuery.Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>();
            foreach (var item in apiQuery.Fields)
            {
                daenetQuery.Fields.Add(new Daenet.MongoDal.Entitties.FieldExpression
                {
                    FieldName = item.FieldName,
                    Operator = ToOperator(item.Operator),
                    Argument = item.Argument,
                    Distinct = item.Distinct
                });
            }
            return daenetQuery;
        }

        public static Daenet.MongoDal.Entitties.QueryOperator ToOperator(Apollo.Common.Entities.QueryOperator apiOperator)
        {
            string? stringOperator = Enum.GetName(typeof(Apollo.Common.Entities.QueryOperator), apiOperator);
            var res = Enum.Parse<Daenet.MongoDal.Entitties.QueryOperator>(stringOperator!);
            return res;
        }
    }
}
