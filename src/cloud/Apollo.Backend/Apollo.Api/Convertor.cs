using System;
using System.Collections;
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
            appointment.Duration = (int)dict["DurationInMin"];
            appointment.Occurences = ToEntityList<Occurence>(dict["Occurences"] as List<ExpandoObject>, ToOccurence);
            appointment.IsGuaranteed = (bool)dict["IsGuaranteed"];
            //appointment.TrainingMode = ToEntityList<TrainingMode>(dict["TrainingMode"] as List<ExpandoObject>, ToTrainingType);
            // appointment.TrainingType = (TrainingType)dict["TrainingType"];
            appointment.TimeInvestAttendee = (int)dict["TimeInvestAttendeeInMin"];
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
            if (expando == null)
                throw new ArgumentException($"The argument {nameof(expando)} cannot be null!");

            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            Profile profile = new Profile
            {
                Id = dict.ContainsKey(nameof(Profile.Id)) ? (string)dict[nameof(Profile.Id)] : null,
                CareerInfos = dict.ContainsKey(nameof(Profile.CareerInfos)) ? ConvertToList<CareerInfo>(dict[nameof(Profile.CareerInfos)], conversionFunc: ConvertToCareerInfo!) : new List<CareerInfo>(),
                EducationInfos = dict.ContainsKey(nameof(Profile.EducationInfos)) ? ConvertToList<EducationInfo>(dict[nameof(Profile.EducationInfos)], conversionFunc: ConvertToEducationInfo!) : new List<EducationInfo>(),
                LanguageSkills = dict.ContainsKey(nameof(Profile.LanguageSkills)) ? ConvertToList<LanguageSkill>(dict[nameof(Profile.LanguageSkills)],ConvertToLanguageSkills!) : new List<LanguageSkill>(),
                Licenses = dict.ContainsKey(nameof(Profile.Licenses)) ? ConvertToList<License>(dict[nameof(Profile.Licenses)], ConvertToLicenses!) : new List<License>(),
                WebReferences = dict.ContainsKey("WebReferences") ? ConvertToList<WebReference>(dict[nameof(Profile.WebReferences)], ConvertToWebReference!) : new List<WebReference>(),
                MobilityInfo = dict.ContainsKey("MobilityInfo") ? ConvertToType<Mobility>(dict[nameof(Profile.MobilityInfo)], ConvertToMobilityInfo!)  : null,
                LeadershipSkills = dict.ContainsKey("LeadershipSkills") ? ConvertToType<LeadershipSkills>(dict[nameof(Profile.LeadershipSkills)], ConvertToLeaderShipSkills!) : null,
            };

            return profile;
        }

        /// <summary>
        /// Converts an object, typically an ExpandoObject, to a nullable CareerInfo instance.
        /// </summary>
        /// <param name="item">The object to be converted.</param>
        /// <returns>
        /// A CareerInfo instance with properties populated from the input object, or null if the input is null or an empty string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input type is not supported or does not match the expected structure for CareerInfo.
        /// </exception>
        private static CareerInfo? ConvertToCareerInfo(object item)
        {
            if (item is ExpandoObject expando)
            {
                IDictionary<string, object> dict = expando as IDictionary<string, object>;

                var careerInfo = new CareerInfo
                {
                    Id = dict.ContainsKey(nameof(CareerInfo.Id)) ? (string)dict[nameof(CareerInfo.Id)] : null,
                    Start = dict.ContainsKey(nameof(CareerInfo.Start)) ? (DateTime)dict[nameof(CareerInfo.Start)] : default,
                    End = dict.ContainsKey(nameof(CareerInfo.End)) ? (DateTime?)dict[nameof(CareerInfo.End)] : null,
                    Description = dict.ContainsKey(nameof(CareerInfo.Description)) ? (string)dict[nameof(CareerInfo.Description)] : null,
                    NameOfInstitution = dict.ContainsKey(nameof(CareerInfo.NameOfInstitution)) ? (string)dict[nameof(CareerInfo.NameOfInstitution)] : null,
                    City = dict.ContainsKey(nameof(CareerInfo.City)) ? (string)dict[nameof(CareerInfo.City)] : null,
                    Country = dict.ContainsKey(nameof(CareerInfo.Country)) ? (string)dict[nameof(CareerInfo.Country)] : null,
                    VoluntaryServiceType = dict.ContainsKey(nameof(CareerInfo.VoluntaryServiceType))? ConvertToObject<VoluntaryServiceType>(dict[nameof(CareerInfo.VoluntaryServiceType)]) : null,
                    WorkingTimeModel = dict.ContainsKey(nameof(CareerInfo.WorkingTimeModel)) ? ConvertToObject<WorkingTimeModel>(dict[nameof(CareerInfo.WorkingTimeModel)]) : null,
                    CareerType = dict.ContainsKey(nameof(CareerInfo.CareerType)) ? ConvertToObject<CareerType>(dict[nameof(CareerInfo.CareerType)]) : null,
                    ServiceType = dict.ContainsKey(nameof(CareerInfo.ServiceType)) ? ConvertToObject<ServiceType>(dict[nameof(CareerInfo.ServiceType)]) : null,
                };

                return careerInfo;
            }

            if (item is null || (item is string str && string.IsNullOrEmpty(str)))
            {
                return null;
            }
            else
            // Handle other cases or throw an exception
            throw new ArgumentException($"Unsupported type: {item.GetType()} for property name: {nameof(Profile.CareerInfos)}");
        }

        /// <summary>
        /// Converts an object, typically an ExpandoObject, to a nullable EducationInfo instance.
        /// </summary>
        /// <param name="item">The object to be converted.</param>
        /// <returns>
        /// An EducationInfo instance with properties populated from the input object, or null if the input is null or an empty string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input type is not supported or does not match the expected structure for EducationInfo.
        /// </exception>
        private static EducationInfo? ConvertToEducationInfo(object item)
        {
            if (item is ExpandoObject expando)
            {
                IDictionary<string, object> dict = expando as IDictionary<string, object>;

                var educationInfo = new EducationInfo
                {
                    Start = dict.ContainsKey(nameof(EducationInfo.Start)) ? (DateTime)dict[nameof(EducationInfo.Start)] : default,
                    End = dict.ContainsKey(nameof(EducationInfo.End)) ? (DateTime?)dict[nameof(EducationInfo.End)] : null,
                    Description = dict.ContainsKey(nameof(EducationInfo.Description)) ? (string)dict[nameof(EducationInfo.Description)] : null,
                    NameOfInstitution = dict.ContainsKey(nameof(EducationInfo.NameOfInstitution)) ? (string)dict[nameof(EducationInfo.NameOfInstitution)] : null,
                    City = dict.ContainsKey(nameof(EducationInfo.City)) ? (string)dict[nameof(EducationInfo.City)] : null,
                    Country = dict.ContainsKey(nameof(EducationInfo.Country)) ? (string)dict[nameof(EducationInfo.Country)] : null,
                    Graduation = dict.ContainsKey(nameof(EducationInfo.Graduation)) ? ConvertToObject<SchoolGraduation>(dict[nameof(EducationInfo.Graduation)]) : null,
                    TypeOfSchool = dict.ContainsKey(nameof(EducationInfo.TypeOfSchool)) ? ConvertToObject<TypeOfSchool>(dict[nameof(EducationInfo.TypeOfSchool)]) : null,
                    UniversityDegree = dict.ContainsKey(nameof(EducationInfo.UniversityDegree)) ? ConvertToObject<UniversityDegree>(dict[nameof(EducationInfo.UniversityDegree)]) : null,
                    EducationType = dict.ContainsKey(nameof(EducationInfo.EducationType)) ? ConvertToObject<EducationType>(dict[nameof(EducationInfo.EducationType)]) : null,
                    Recognition = dict.ContainsKey(nameof(EducationInfo.Recognition)) ? ConvertToObject<RecognitionType>(dict[nameof(EducationInfo.Recognition)]) : null,
                    CompletionState = dict.ContainsKey(nameof(EducationInfo.CompletionState)) ? ConvertToObject<CompletionState>(dict[nameof(EducationInfo.CompletionState)]) : null,
                };

                return educationInfo;

            }

            if (item is null || (item is string str && string.IsNullOrEmpty(str)))
            {
                return null;
            }
            else
                // Handle other cases or throw an exception
                throw new ArgumentException($"Unsupported type: {item.GetType()} for property name: {nameof(Profile.EducationInfos)}");
        }

        /// <summary>
        /// Converts an object, typically an ExpandoObject, to a nullable LanguageSkill instance.
        /// </summary>
        /// <param name="item">The object to be converted.</param>
        /// <returns>
        /// A LanguageSkill instance with properties populated from the input object, or null if the input is null or an empty string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input type is not supported or does not match the expected structure for LanguageSkill.
        /// </exception>
        private static LanguageSkill? ConvertToLanguageSkills(object item)
        {
            if (item is ExpandoObject expando)
            {
                IDictionary<string, object> dict = expando as IDictionary<string, object>;

                var languageSkill = new LanguageSkill
                {
                    Id = dict.ContainsKey(nameof(LanguageSkill.Id)) ? (string)dict[nameof(LanguageSkill.Id)] : null,
                    Code = dict.ContainsKey(nameof(LanguageSkill.Code)) ? (string)dict[nameof(LanguageSkill.Code)] : "",
                    Name = dict.ContainsKey(nameof(LanguageSkill.Name)) ? (string)dict[nameof(LanguageSkill.Name)] : "",
                    Niveau = dict.ContainsKey(nameof(LanguageSkill.Niveau)) ? ConvertToObject<LanguageNiveau>(dict[nameof(LanguageSkill.Niveau)]) : null,
                };

               return languageSkill;
            }
            if (item is null || (item is string str && string.IsNullOrEmpty(str)))
            {
                return null;
            }
            else
            // Handle other cases or throw an exception
            throw new ArgumentException($"Unsupported type: {item.GetType()} for property name: {nameof(Profile.LanguageSkills)}");
        }

        /// <summary>
        /// Converts an object, typically an ExpandoObject, to a nullable License instance.
        /// </summary>
        /// <param name="item">The object to be converted.</param>
        /// <returns>
        /// A License instance with properties populated from the input object, or null if the input is null or an empty string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input type is not supported or does not match the expected structure for License.
        /// </exception>
        private static License? ConvertToLicenses(object item)
        {
            if (item is ExpandoObject expando)
            {
                IDictionary<string, object> dict = expando as IDictionary<string, object>;

                var license = new License
                {
                    Granted = dict.ContainsKey(nameof(License.Granted)) ? (DateTime?)dict[nameof(License.Granted)] : null,
                    Expires = dict.ContainsKey(nameof(License.Expires)) ? (DateTime?)dict[nameof(License.Expires)] : null,
                    IssuingAuthority = dict.ContainsKey(nameof(License.IssuingAuthority)) ? (string)dict[nameof(License.IssuingAuthority)] : null,
                    ListItemId = (int)dict[nameof(ApolloListItem.ListItemId)]!,
                    Value = (string)dict[nameof(ApolloListItem.Value)]!,
                    Lng = dict.ContainsKey(nameof(License.Lng)) ? (string)dict[nameof(ApolloListItem.Lng)] : null,
                    Description = dict.ContainsKey(nameof(License.Description)) ? (string)dict[nameof(ApolloListItem.Description)] : null
            };

                return license;
            }

            if (item is null || (item is string str && string.IsNullOrEmpty(str)))
            {
                return null;
            }
            else
            // Handle other cases or throw an exception
            throw new ArgumentException($"Unsupported type: {item.GetType()} for property name: {nameof(Profile.Licenses)}");
        }

        /// <summary>
        /// Converts an object, typically an ExpandoObject, to a nullable WebReference instance.
        /// </summary>
        /// <param name="item">The object to be converted.</param>
        /// <returns>
        /// A WebReference instance with properties populated from the input object, or null if the input is null or an empty string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input type is not supported or does not match the expected structure for WebReference.
        /// </exception>
        private static WebReference? ConvertToWebReference(object item)
        {
            if (item is ExpandoObject expando)
            {
                IDictionary<string, object> dict = expando as IDictionary<string, object>;

                var webReference = new WebReference
                {
                    Id = dict.ContainsKey(nameof(WebReference.Id)) ? (string?)dict[nameof(WebReference.Id)] : null,
                    Title = dict.ContainsKey(nameof(WebReference.Title)) ? (string?)dict[nameof(WebReference.Title)] : null,
                    Url = dict.ContainsKey(nameof(WebReference.Url)) ? new Uri((string)dict[nameof(WebReference.Url)]): null,
                };

                return webReference;
            }

            if (item is null || (item is string str && string.IsNullOrEmpty(str)))
            {
                return null;
            }
            else

            // Handle other cases or throw an exception
            throw new ArgumentException($"Unsupported type: {item.GetType()} for property name: {nameof(Profile.WebReferences)}");
        }

        /// <summary>
        /// Converts an object, typically an ExpandoObject, to a nullable Mobility instance.
        /// </summary>
        /// <param name="item">The object to be converted.</param>
        /// <returns>
        /// A Mobility instance with properties populated from the input object, or null if the input is null or an empty string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input type is not supported or does not match the expected structure for Mobility.
        /// </exception>
        private static Mobility? ConvertToMobilityInfo(object item)
        {
            if (item is ExpandoObject expando)
            {
                IDictionary<string, object> dict = expando as IDictionary<string, object>;

                var mobilityInfo = new Mobility
                {
                    WillingToTravel = dict.ContainsKey(nameof(Mobility.WillingToTravel)) ? ConvertToObject<Willing>(dict[nameof(Mobility.WillingToTravel)]) : null,
                    HasVehicle = dict.ContainsKey(nameof(Mobility.HasVehicle)) ? (bool)dict[nameof(Mobility.HasVehicle)] : false,
                };

                return mobilityInfo;
            }

            if (item is null || (item is string str && string.IsNullOrEmpty(str)))
            {
                return null;
            }
            else
                // Handle other cases or throw an exception
                throw new ArgumentException($"Unsupported type: {item.GetType()} for property name: {nameof(Profile.MobilityInfo)}");
        }

        /// <summary>
        /// Converts an object, typically an ExpandoObject, to a nullable LeadershipSkills instance.
        /// </summary>
        /// <param name="item">The object to be converted.</param>
        /// <returns>
        /// A LeadershipSkills instance with properties populated from the input object, or null if the input is null or an empty string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input type is not supported or does not match the expected structure for LeadershipSkills.
        /// </exception>
        private static LeadershipSkills? ConvertToLeaderShipSkills(object item)
        {
            if (item is ExpandoObject expando)
            {
                IDictionary<string, object> dict = expando as IDictionary<string, object>;

                var leaderShipSkills = new LeadershipSkills
                {
                    StaffResponsibility = dict.ContainsKey(nameof(LeadershipSkills.StaffResponsibility)) ? ConvertToObject<StaffResponsibility>(dict[nameof(LeadershipSkills.StaffResponsibility)]) : null,
                    YearsofLeadership = dict.ContainsKey(nameof(LeadershipSkills.YearsofLeadership)) ? ConvertToObject<YearRange>(dict[nameof(LeadershipSkills.YearsofLeadership)]) : null,
                    BudgetResponsibility = dict.ContainsKey(nameof(LeadershipSkills.BudgetResponsibility)) ? (bool)dict[nameof(LeadershipSkills.BudgetResponsibility)] : false,
                    PowerOfAttorney = dict.ContainsKey(nameof(LeadershipSkills.PowerOfAttorney)) ? (bool)dict[nameof(LeadershipSkills.PowerOfAttorney)] : false,
                };

                return leaderShipSkills;
            }

            if (item is null || (item is string str && string.IsNullOrEmpty(str)))
            {
                return null;
            }
            else
                // Handle other cases or throw an exception
                throw new ArgumentException($"Unsupported type: {item.GetType()} for property name: {nameof(Profile.LeadershipSkills)}");
        }

        /// <summary>
        /// Converts a generic object to a specified type using a conversion function.
        /// </summary>
        /// <typeparam name="T">The type to convert the object to.</typeparam>
        /// <param name="value">The object to be converted.</param>
        /// <param name="conversionFunc">A function that performs the conversion to type T.</param>
        /// <returns>
        /// The result of applying the conversion function to the input object, resulting in an object of type T.
        /// </returns>
        private static T ConvertToType<T>(object value, Func<object, T> conversionFunc)
        {
            return conversionFunc(value);
        }

        /// <summary>
        /// Converts a generic object, typically a List of objects, to a List of a specified type using a conversion function.
        /// </summary>
        /// <typeparam name="T">The type of elements in the resulting List.</typeparam>
        /// <param name="value">The object to be converted, usually a List of objects.</param>
        /// <param name="conversionFunc">A function that converts individual objects to the desired type.</param>
        /// <returns>
        /// A List of elements of type T, where each element is obtained by applying the conversion function to the original elements.
        /// If the input is not a List or is null, returns an empty List of type T.
        /// </returns>
        private static List<T> ConvertToList<T>(object value, Func<object, T> conversionFunc)
        {
            if (value is List<object> list)
            {
                return list.Select(conversionFunc).ToList();
            }
            return new List<T>();
        }

        /// <summary>
        /// Converts an object, typically an ExpandoObject, to a specified subtype of ApolloListItem.
        /// </summary>
        /// <typeparam name="T">The type of the ApolloListItem subclass to convert to.</typeparam>
        /// <param name="item">The object to be converted.</param>
        /// <returns>
        /// An instance of the specified ApolloListItem subtype, populated with values from the input object.
        /// If the input is null or an empty string, returns null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input type is not supported or does not match the expected subtype.
        /// </exception>
        private static T? ConvertToObject<T>(object item) where T : ApolloListItem
        {
            if (item is ExpandoObject expando)
            {
                IDictionary<string, object> dict = expando as IDictionary<string, object>;

                T result = Activator.CreateInstance<T>();
                result.ListItemId = (int)dict[nameof(ApolloListItem.ListItemId)]!;
                result.Value = (string)dict[nameof(ApolloListItem.Value)]!;
                result.Lng = dict.ContainsKey(nameof(License.Lng)) ? (string)dict[nameof(ApolloListItem.Lng)] : null;
                result.Description = dict.ContainsKey(nameof(License.Description)) ? (string)dict[nameof(ApolloListItem.Description)] : null;
                return result;
            }
            if (item is null || (item is string str && string.IsNullOrEmpty(str)))
            {
                return item as T;
            }
            else
            // Handle other cases or throw an exception
            throw new ArgumentException($"Unsupported type: {item.GetType()} for property name: {typeof(T).Name}");
        }


        /// <summary>
        /// Converts an expando object to a List of strings.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>A list object converted from the expando object.</returns>
        //public static string ToListValue(ExpandoObject expando)
        //{
        //    IDictionary<string, object> dict = expando as IDictionary<string, object>;

        //    string val = dict["Value"] != null ? (string)dict["Value"] : string.Empty;

        //    return val;
        //}

        /// <summary>
        /// Converts an expando object to a ApolloList.
        /// </summary>
        /// <param name="expando">The expando object to be converted.</param>
        /// <returns>A list object converted from the expando object.</returns>
        public static ApolloList ToApolloList(ExpandoObject expando)
        {
            if (expando == null)
                throw new ArgumentException($"The argument {nameof(expando)} cannot be null!");

            IDictionary<string, object> dict = expando as IDictionary<string, object>;

            ApolloList lst = new ApolloList
            {
                Id = dict.ContainsKey("Id") ? (string)dict["Id"] : "",
                ItemType = dict.ContainsKey("ItemType") ? (string)dict["ItemType"] : "",
                Description = dict.ContainsKey("Description") ? (string)dict["Description"] : "",
                Items = new List<ApolloListItem>(),
            };

            List<Object> expandoList = dict.ContainsKey(nameof(ApolloList.Items)) ? (List<Object>)dict[nameof(ApolloList.Items)] : new List<Object>();

            foreach (var expandoItem in expandoList)
            {
                IDictionary<string, object> dictList = (IDictionary<string, object>)expandoItem;

                lst.Items.Add(new ApolloListItem
                {
                    ListItemId = (int)dictList![nameof(ApolloListItem.ListItemId)],
                    Value = (string)dictList![nameof(ApolloListItem.Value)],
                    Description = (string)dictList![nameof(ApolloListItem.Description)],
                    Lng = (string)dictList![nameof(ApolloListItem.Lng)]
                });

            }

            return lst;
        }

        private static List<ApolloListItem> ToApolloListItem(List<ExpandoObject> items)
        {
            List<ApolloListItem> list = new List<ApolloListItem>();

            foreach (var expando in items)
            {
                IDictionary<string, object> dict = expando as IDictionary<string, object>;

                list.Add(new ApolloListItem
                {
                    ListItemId = dict.ContainsKey("ListItemId") ? (int)dict["ListItemId"] : throw new ApolloApiException(-1, $"The ListItem in the ApolloList has no {nameof(ApolloListItem.ListItemId)} identifier."),
                    Lng = dict.ContainsKey("Lng") ? (string)dict["Lng"] : "",
                    Value = dict.ContainsKey("Value") ? (string)dict["Value"] : ""
                });
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

            //Qualification quali = new Qualification
            //{
            //    Id = dict.ContainsKey("Id") ? (string)dict["Id"] : "",
            //    //to do mapp all properties in profile entity ... #Mukit


            //};

            throw new NotImplementedException();
        }


        /// <summary>
        /// Converts an Apollo API query filter to a Daenet query.
        /// </summary>
        /// <param name="filter">The Apollo API query filter to be converted.</param>
        /// <returns>A Daenet query object converted from the Apollo API query filter.</returns>
        public static Daenet.MongoDal.Entitties.Query ToDaenetQuery(Apollo.Common.Entities.Filter filter)
        {
            Daenet.MongoDal.Entitties.Query daenetQuery = new();
            if (filter != null)
            {
                daenetQuery.IsOrOperator = filter.IsOrOperator;
                daenetQuery.Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>();
                foreach (var item in filter.Fields)
                {
                    daenetQuery.Fields.Add(new Daenet.MongoDal.Entitties.FieldExpression
                    {
                        FieldName = item.FieldName,
                        Operator = ToOperator(item.Operator),
                        Argument = item.Argument!,
                        Distinct = item.Distinct
                    });
                }
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

        public static Daenet.MongoDal.Entitties.SortExpression? ToDaenetSortExpression(Common.Entities.SortExpression? sortExpression)
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
