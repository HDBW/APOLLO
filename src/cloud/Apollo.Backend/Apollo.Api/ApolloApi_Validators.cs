// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Common.Entities;

namespace Apollo.Api
{
    /// <summary>
    /// Implements various validation methods.
    /// </summary>
    public partial class ApolloApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="throwIfInvalid"></param>
        /// <returns></returns>
        /// <exception cref="ApolloApiException"></exception>
        public  bool IsValidId(string value, bool throwIfInvalid = false)
        {

            if (throwIfInvalid)
                throw new ApolloApiException(ErrorCodes.GeneralErrors.InvalidId, "Invalid Id.");

            return false;
        }


        /// <summary>
        /// Validates the query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="throwIfInvalid"></param>
        /// <returns></returns>
        /// <exception cref="ApolloApiException"></exception>
        public bool IsQueryValid(Query query, bool throwIfInvalid = false)
        {
            if (query == null)
            {
                if (throwIfInvalid) throw new ApolloApiException(ErrorCodes.GeneralErrors.InvalidQuery, "Invalid query: The query object is null.");
                return false;
            }

            // Filter validation
            if (query.Filter == null || !IsFilterValid(query.Filter, throwIfInvalid))
            {
                // Error handling occurs in IsFilterValid if throwIfInvalid is true
                return false;
            }

            // Field validation
            if (query.Fields != null && !AreFieldsValid(query.Fields, throwIfInvalid))
            {
                return false;
            }

            // Sorting
            if (query.SortExpression != null && !IsSortExpressionValid(query.SortExpression, throwIfInvalid))
            {
                return false;
            }

            // More reviews can be added here.

            return true;
        }

        private bool IsFilterValid(Filter filter, bool throwIfInvalid)
        {

            foreach (var fieldExpression in filter.Fields)
            {
                // Here check if the field name is valid
                if (string.IsNullOrEmpty(fieldExpression.FieldName))
                {
                    if (throwIfInvalid) throw new ApolloApiException(ErrorCodes.GeneralErrors.InvalidQuery, "Invalid filter: FieldName is null or empty.");
                    return false;
                }

                // Further specific tests could be carried out here
            }

            return true;
        }

        private bool AreFieldsValid(List<string> fields, bool throwIfInvalid)
        {

            foreach (var field in fields)
            {
                // Check the existence and admissibility of the fields
                if (!IsValidFieldName(field))
                {
                    if (throwIfInvalid) throw new ApolloApiException(ErrorCodes.GeneralErrors.InvalidQuery, $"Invalid field: {field} is not a valid field name.");
                    return false;
                }
            }

            return true;
        }

        private bool IsSortExpressionValid(SortExpression sortExpression, bool throwIfInvalid)
        {

            if (!IsValidFieldName(sortExpression.FieldName))
            {
                if (throwIfInvalid) throw new ApolloApiException(ErrorCodes.GeneralErrors.InvalidQuery, $"Invalid sort expression: {sortExpression.FieldName} is not a valid field name.");
                return false;
            }

            return true;
        }

        private bool IsValidFieldName(string fieldName)
        {
            //  Combined list of valid field names from the User and Training classes
            var validFieldNames = new List<string>
            {
                // Fields of User class
                "ObjectId", "IdentityProvicer", "Upn", "Email", "Name", "ContactInfos", "Birthdate", "Disabilities", "Profile",
                // Fields of Training class
                "ProviderId", "ExternalTrainingId", "TrainingType", "TrainingName", "Image", "SubTitle", "Description",
                "ShortDescription", "Content", "BenefitList", "Certificate", "Prerequisites", "Loans", "TrainingProvider",
                "CourseProvider", "Appointment", "TargetAudience", "ProductUrl", "Contacts", "TrainingMode",
                "IndividualStartDate", "Price", "PriceDescription", "AccessibilityAvailable", "Tags", "Categories",
                "SimilarTrainings", "RecommendedTrainings", "PublishingDate", "UnpublishingDate", "Successor", "Predecessor"
            };

                return validFieldNames.Contains(fieldName, StringComparer.OrdinalIgnoreCase);
        }

    }
}

