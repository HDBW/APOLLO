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

        /// <summary>
        /// Converts an expando object to a Training object.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>A Training object converted from the expando object.</returns>
        public static Training ToTraining(ExpandoObject expando)
        {
            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            Training tr = new Training();

            tr.ProviderId = dict.ContainsKey("ProviderId") ? (string)dict["ProviderId"] : "";
            tr.TrainingName = dict.ContainsKey("TrainingName") ? (string)dict["TrainingName"] : "";
            tr.Description = dict.ContainsKey("Description") ? (string)dict["Description"] : "";
            tr.ShortDescription = dict.ContainsKey("ShortDescription") ? (string)dict["ShortDescription"] : "";
            tr.Content = dict.ContainsKey("Content") ? (List<string>)dict["Content"] : new List<string>();
            tr.BenefitList = dict.ContainsKey("BenefitList") ? (List<string>)dict["BenefitList"] : new List<string>();
            tr.Certificate = dict.ContainsKey("Certificate") ? (List<string>)dict["Certificate"] : new List<string>();
            tr.Prerequisites = dict.ContainsKey("Prerequisites") ? (List<string>)dict["Prerequisites"] : new List<string>();
            tr.ProductUrl = dict.ContainsKey("ProductUrl") ? new Uri((string)dict["ProductUrl"]) : new Uri("about:blank");
            tr.Price = dict.ContainsKey("Price") ? (decimal)dict["Price"] : 0M;
            tr.Loans = dict.ContainsKey("Loans") ? ToEntityList<Loans>(dict["Loans"] as List<ExpandoObject>, ToLoans) : new List<Loans>();
            tr.TrainingProvider = dict.ContainsKey("TrainingProvider") ? ToTrainingProvider(dict["TrainingProvider"] as ExpandoObject) : null;
            // map other properties here
            // tr.Tags = dict.ContainsKey("Tags") ? (List<string>)dict["Tags"] : new List<string>();
            // tr.PublishingDate = dict.ContainsKey("PublishingDate") ? (DateTime)dict["PublishingDate"] : DateTime.MinValue;

            return tr;
        }



        /// <summary>
        /// Converts an expando object to a Loans object.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>A Loans object converted from the expando object.</returns>
        public static Loans ToLoans(ExpandoObject expando)
        {
            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            Loans loans = new Loans();

            loans.Id = (string)dict["Id"];
            loans.Name = (string)dict["Name"];
            loans.Description = (string)dict["Description"];
            loans.Url = new Uri((string)dict["Url"]);
            loans.LoanContact = ToContact(dict["LoanContact"] as ExpandoObject);

            return loans;
        }

        /// <summary>
        /// Converts an expando object to an AppointmentUrl object.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>An AppointmentUrl object converted from the expando object.</returns>
        public static Appointment ToAppointments(ExpandoObject expando)
        {
            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            Appointment appointment = new Appointment();

            appointment.Id = (string)dict["Id"];
            appointment.AppointmentUrl = new Uri((string)dict["AppointmentUrl"]);
            appointment.AppointmentType = (string)dict["AppointmentType"];
            appointment.AppointmentDescription = (string)dict["AppointmentDescription"];
            appointment.AppointmentLocation = ToContact(dict["AppointmentLocation"] as ExpandoObject);
            appointment.StartDate = (DateTime)dict["StartDate"];
            appointment.EndDate = (DateTime)dict["EndDate"];
            appointment.DurationDescription = (string)dict["DurationDescription"];
            appointment.Duration = (TimeSpan)dict["Duration"];
            appointment.Occurences = ToEntityList<Occurence>(dict["Occurences"] as List<ExpandoObject>, ToOccurence);
            appointment.IsGuaranteed = (bool)dict["IsGuaranteed"];
            //appointment.TrainingType = ToEntityList<TrainingType>(dict["TrainingType"] as List<ExpandoObject>, ToTrainingType);
            appointment.TrainingType = (TrainingType)dict["TrainingType"];
            appointment.TimeInvestAttendee = (TimeSpan)dict["TimeInvestAttendee"];
            appointment.TimeModel = (TrainingTimeModel)dict["TimeModel"];
            // Add other property mappings as needed

            return appointment;
        }

        //public static TrainingType ToTrainingType(ExpandoObject expandoObject)
        //{
        //    var trainingType = TrainingType.Unknown;

        //    if (expandoObject != null)
        //    {
        //        var expandoDict = expandoObject as IDictionary<string, object>;

        //        foreach (var keyValuePair in expandoDict)
        //        {
        //            if (Enum.TryParse<TrainingType>(keyValuePair.Key, out var value))
        //            {
        //                trainingType |= value;
        //            }
        //        }
        //    }

        //    return trainingType;
        //}

        /// <summary>
        /// Converts an expando object to an Occurence object.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>An Occurence object converted from the expando object.</returns>
        public static Occurence ToOccurence(ExpandoObject expando)
        {
            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            Occurence occurence = new Occurence();

            occurence.Id = (string)dict["Id"];
            occurence.StartDate = (DateTime)dict["StartDate"];
            occurence.EndDate = (DateTime)dict["EndDate"];
            occurence.Description = (string)dict["Description"];
            occurence.Location = ToContact(dict["Location"] as ExpandoObject);
            occurence.CorrelationId = (string)dict["CorrelationId"];

            // Calculate Duration based on StartDate and EndDate
            return occurence;
        }


        /// <summary>
        /// Converts an expando object to a Contact object.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>A Contact object converted from the expando object.</returns>
        public static Contact ToContact(ExpandoObject expando)
        {
            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            Contact contact = new Contact
            {
                Surname = dict.TryGetValue("Surname", out var surname) ? (string)surname! : null!,
                Mail = dict.TryGetValue("Mail", out var mail) ? (string)mail! : null!,
                Phone = dict.TryGetValue("Phone", out var phone) ? (string)phone! : null!,
                Organization = dict.TryGetValue("Organization", out var organization) ? (string)organization! : null!,
                Address = dict.TryGetValue("Address", out var address) ? (string)address! : null!,
                City = dict.TryGetValue("City", out var city) ? (string)city! : null!,
                ZipCode = dict.TryGetValue("ZipCode", out var zipCode) ? (string)zipCode! : null!,
                EAppointmentUrl = dict.TryGetValue("EAppointmentUrl", out var eAppointmentUrl) && eAppointmentUrl != null ?
                    new Uri((string)eAppointmentUrl)! : null
            };

            return contact;
        }

        /// <summary>
        /// Converts a list of expando objects to a list of a specific entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity to be converted to.</typeparam>
        /// <param name="expandos">The list of expando objects to be converted.</param>
        /// <param name="toEntity">The function to convert each expando object to the specified entity type.</param>
        /// <returns>A list of entities of type T converted from the list of expando objects.</returns>
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

        /// <summary>
        /// Converts an expando object to an EduProvider object.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>An EduProvider object converted from the expando object.</returns>
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


        /// <summary>
        /// Converts an expando object to a User object.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>A User object converted from the expando object.</returns>
        public static User ToUser(ExpandoObject expando)
        {
            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            User user = new User
            {
                Goal = dict.ContainsKey("Goal") ? (string)dict["Goal"] : "",
                FirstName = dict.ContainsKey("FirstName") ? (string)dict["FirstName"] : "",
                LastName = dict.ContainsKey("LastName") ? (string)dict["LastName"] : "",
                Image = dict.ContainsKey("Image") ? (string)dict["Image"] : "",
                Id = dict.ContainsKey("Id") ? (string)dict["Id"] : "",
                UserName = dict.ContainsKey("UserName") ? (string)dict["UserName"] : ""
            };

            return user;
        }


        /// <summary>
        /// Converts an Apollo API query filter to a Daenet query.
        /// </summary>
        /// <param name="apiQuery">The Apollo API query filter to be converted.</param>
        /// <returns>A Daenet query object converted from the Apollo API query filter.</returns>
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

        /// <summary>
        /// Converts an Apollo Common Entities QueryOperator to a Daenet QueryOperator.
        /// </summary>
        /// <param name="apiOperator">The Apollo Common Entities QueryOperator to be converted.</param>
        /// <returns>A Daenet QueryOperator converted from the Apollo Common Entities QueryOperator.</returns>
        public static Daenet.MongoDal.Entitties.QueryOperator ToOperator(Apollo.Common.Entities.QueryOperator apiOperator)
        {
            string? stringOperator = Enum.GetName(typeof(Apollo.Common.Entities.QueryOperator), apiOperator);
            var res = Enum.Parse<Daenet.MongoDal.Entitties.QueryOperator>(stringOperator!);
            return res;
        }

        public static Daenet.MongoDal.Entitties.SortExpression ToDaenetSortExpression(Common.Entities.SortExpression sortExpression)
        {
            if (sortExpression == null)
                return null;

            return  new Daenet.MongoDal.Entitties.SortExpression
            {
                FieldName = sortExpression.FieldName,
                Order = Enum.Parse<Daenet.MongoDal.Entitties.SortOrder>(Enum.GetName(typeof(Common.Entities.SortOrder), sortExpression.Order)!)                       
            };
        }
    }
}
