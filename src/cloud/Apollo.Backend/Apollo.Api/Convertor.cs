using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
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

            if (item.GetType().IsClass == false)
                throw new ArgumentException("Item must be a class!");

            ExpandoObject expo = new ExpandoObject();

            IDictionary<string, object?> expoDict = expo as IDictionary<string, object?>;

            foreach (var prop in item.GetType().GetProperties())
            {
                if (prop.Name == "Id")
                {
                    // We put both Id and _id to be able to deserialize automatically from Id.
                    expoDict.Add("Id", prop.GetValue(item)!);
                    expoDict.Add("_id", prop.GetValue(item)!);
                }
                else
                {
                    if (IsList(prop.PropertyType))
                    {
                        expoDict.Add(prop.Name, ConvertFromList(prop, prop.GetValue(item))!);
                    }
                    else if (prop.PropertyType == typeof(Uri))
                    {
                        var val = prop.GetValue(item);

                        expoDict.Add(prop.Name, val == null! ? null : ((Uri)val).ToString());
                    }
                    else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                    {
                        var val = prop.GetValue(item);

                        expoDict.Add(prop.Name, val == null! ? null : Convert(val)!);
                    }
                    else
                        expoDict.Add(prop.Name, prop.GetValue(item)!);
                }
            }

            return expo;
        }

        private static bool IsList(Type type)
        {
            return type.GetInterfaces().Count(i => i.Name.Contains("List")) > 0 || type.Name.Contains("List") || type.IsArray;
        }


        /// <summary>
        /// Converts the list of objects to a list of expando objects.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private static IList<object> ConvertFromList(PropertyInfo propertyInfo, object? val)
        {
            if (val == null)
                return null;

            if (propertyInfo == null)
                throw new NullReferenceException();

            var listType = typeof(List<>);

            List<object?> list = new List<object?>();

            var arrElements = val as IEnumerable<object>;

            if (arrElements != null)
            {
                foreach (var listItem in arrElements)
                {
                    object? convertedValue = null;

                    if (listItem == null)
                    {
                        list.Add(null);
                        continue;
                    }

                    var tp = listItem.GetType();

                    if (tp.IsClass && tp != typeof(string))
                    {
                        convertedValue = Convert(listItem);
                    }
                    else if (tp.IsPrimitive || tp.IsValueType || tp == typeof(string))
                        convertedValue = listItem;
                    else
                        throw new ArgumentException("Unsuported itm type of list item!");

                    list.Add(convertedValue);
                }
            }

            return list!;
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
            tr.Price = dict.ContainsKey("Price") ? (double)dict["Price"] : 0;
            tr.Loans = dict.ContainsKey("Loans") ? ToEntityList<Loans>(dict["Loans"] as List<ExpandoObject>, ToLoans) : new List<Loans>();
            tr.TrainingProvider = dict.ContainsKey("TrainingProvider") ? ToTrainingProvider(dict["TrainingProvider"] as ExpandoObject) : null;
            // map other properties here
            // tr.Tags = dict.ContainsKey("Tags") ? (List<string>)dict["Tags"] : new List<string>();
            // tr.PublishingDate = dict.ContainsKey("PublishingDate") ? (DateTime)dict["PublishingDate"] : DateTime.MinValue;
            //tr.Appointment = dict.ContainsKey("Appointment") ? ToEntityList<Appointment>(dict["Appoinment"] as List<ExpandoObject>, ToAppointments) : new List<Appointment>();

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
            //appointment.TrainingMode = ToEntityList<TrainingMode>(dict["TrainingMode"] as List<ExpandoObject>, ToTrainingType);
           // appointment.TrainingType = (TrainingType)dict["TrainingType"];
            appointment.TimeInvestAttendee = (TimeSpan)dict["TimeInvestAttendee"];
            appointment.TimeModel = (TrainingTimeModel)dict["TimeModel"];
            // Add other property mappings as needed

            return appointment;
        }

        //public static TrainingMode ToTrainingType(ExpandoObject expandoObject)
        //{
        //    var trainingType = TrainingMode.Unknown;

        //    if (expandoObject != null)
        //    {
        //        var expandoDict = expandoObject as IDictionary<string, object>;

        //        foreach (var keyValuePair in expandoDict)
        //        {
        //            if (Enum.TryParse<TrainingMode>(keyValuePair.Key, out var value))
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
        public static List<T> ToEntityList<T>(IList<ExpandoObject>? expandos, Func<ExpandoObject, T> toEntity)
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
                Id = dict.ContainsKey("Id") ? (string)dict["Id"] : "",
                ObjectId = dict.ContainsKey("ObjectId") ? (string)dict["ObjectId"] : "",
                Upn = dict.ContainsKey("Upn") ? (string)dict["Upn"] : null,
                Email = dict.ContainsKey("Email") ? (string)dict["Email"] : null,
                Name = dict.ContainsKey("Name") ? (string)dict["Name"] : "",

                // Should add ContactInfos,  Birthdate,  Disabilities, Profile?
            };

            return user;
        }

        /// <summary>
        /// Converts an expando object to a Profile object.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>A Profile object converted from the expando object.</returns>
        public static Profile ToProfile(ExpandoObject expando)
        {
            // TODO

            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            Profile profile = new Profile
            {
                Id = dict.ContainsKey("Id") ? (string)dict["Id"] : "",
                //to do mapp all properties in profile entity ... #Mukit

                
            };

            return profile;
        }


        /// <summary>
        /// Converts an expando object to a List object.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>A list object converted from the expando object.</returns>
        public static List ToList(ExpandoObject expando)
        {
            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            List list = new List
            {
                Id = dict.ContainsKey("Id") ? (string)dict["Id"] : "",

                Items = new List<ListItem>(),
            };

            if (dict.ContainsKey("Items"))
            {
                var expLst = dict["Items"] as IList<ExpandoObject>;

                if (expLst != null)
                {
                    foreach (var item in expLst)
                    {
                        ListItem listItem = new ListItem();

                        IDictionary<string, object> expItem = item as IDictionary<string, object>;

                        listItem.Name = dict.ContainsKey(nameof(ListItem.Name)) ? (string)dict[nameof(ListItem.Name)] : "";
                        listItem.Name = dict.ContainsKey(nameof(ListItem.Description)) ? (string)dict[nameof(ListItem.Description)] : "";

                        list.Items.Add(listItem);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Converts an expando object to a Qualification object.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>A Profile object converted from the expando object.</returns>
        public static Qualification ToQualification(ExpandoObject expando)
        {
            // TODO

            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            Qualification quali = new Qualification
            {
                Id = dict.ContainsKey("Id") ? (string)dict["Id"] : "",
                //to do mapp all properties in profile entity ... #Mukit


            };

            return quali;
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

            return new Daenet.MongoDal.Entitties.SortExpression
            {
                FieldName = sortExpression.FieldName,
                Order = Enum.Parse<Daenet.MongoDal.Entitties.SortOrder>(Enum.GetName(typeof(Common.Entities.SortOrder), sortExpression.Order)!)
            };
        }
    }
}
