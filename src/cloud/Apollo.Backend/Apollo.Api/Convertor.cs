using System;
using System.Collections.Generic;
using System.Dynamic;
using Apollo.Common.Entities;
using MongoDB.Driver.Core.Events.Diagnostics;

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

        public static Training ToTraining(ExpandoObject expando)
        {
            IDictionary<string,object> dict = expando as IDictionary<string, object>;
         
            Training tr = new Training();

            tr.TrainingProvider = ToTrainingProvider(dict["TrainingProvider"] as ExpandoObject);
         //   tr.Tags
            tr.PublishingDate = (DateTime)dict["PublishingDate"];
           // tr.Prerequisites
            tr.ProductUrl = new Uri(dict["ProductUrl"] as string);
            tr.Price = (decimal)dict["Price"];
            //tr. = (string)dict["PictureUrl"];

            tr.Loans = dict.ContainsKey("Loans") ? ToEntityList<Loans>(dict["Loans"] as List<ExpandoObject>, ToLoans) : null;

            return tr;
        }

        public static Loans ToLoans(ExpandoObject expando)
        {
            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            Loans loans = new Loans();
            //..
            return loans;
        }

        public static List<T> ToEntityList<T>(IList<ExpandoObject>? expandos, Func<ExpandoObject,T> toEntity)
        {
            if (expandos == null)
                throw new ArgumentNullException("Argument 'expandos' cannot be null!");

            List<T> list = new List<T>();

            foreach (ExpandoObject item in expandos)
            {
                list.Add(toEntity(item));
            }

            return list;
        }

        public static EduProvider ToTrainingProvider(ExpandoObject? expando)
        {
            EduProvider provider = new EduProvider();

            IDictionary<string, object>? dict = expando as IDictionary<string, object>;

            if (dict != null)
            {
                provider.Name = dict.ContainsKey("Name") ? (string)dict["Name"] : null;
                provider.Url = new Uri((string)dict["Url"]);
                //TODO...
            }

            return provider;
        }

        public static Daenet.MongoDal.Entitties.Query ToDaenetQuery(Apollo.Common.Entities.Filter apiQuery)
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

        public static Daenet.MongoDal.Entitties.SortExpression ToDaenetSortExpression(Common.Entities.SortExpression sortExpression)
        {
            return  new Daenet.MongoDal.Entitties.SortExpression
            {
                FieldName = sortExpression.FieldName,
                Order = Enum.Parse<Daenet.MongoDal.Entitties.SortOrder>(Enum.GetName(typeof(Common.Entities.SortOrder), sortExpression.Order)!)                       
            };
        }
    }
}
