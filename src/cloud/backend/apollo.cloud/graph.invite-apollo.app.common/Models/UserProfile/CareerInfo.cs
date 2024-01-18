// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    /// <summary>
    /// This is an experience within the Career Information of the User.
    /// It describes the Career, Position and Job description of a User.
    /// More Information can be found in the Documentation Endpoint.
    /// </summary>
    public class CareerInfo
    {
        /// <summary>
        /// Start date of the Experience
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// End date of the Experience
        /// Could be null if the User is still working there.
        /// </summary>
        public DateTime? End { get; set; }

        // Vorschlagsliste type as you go
        /// <summary>
        /// Job Title of the Experience represented as Occupation
        /// </summary>
        /// <remarks>
        /// This is a Taxonomy Object, however even though we use autosuggestions the User will be able to enter free text here.
        /// If the user decides to enter free text we will not be able to classify the Job Title in the client. So we will look up if the entered text is Taxonomy unknown and try to classify with ESCO.
        /// </remarks>
        /// <Note>
        /// CareerInfo_JobTtitle_filtered.txt
        /// </Note>
        public Occupation? Job { get; set; }

        /// <summary>
        /// This is the Career Type of the Experience.
        /// It indicates if the Experience is a Voluntary Service, a Professional Career or ...
        /// This is a Selection List in the Client UI.
        /// </summary>
        /// <note>
        /// CareerInfo_CareerType_filtered.txt
        /// </note>
        public CareerType CareerType { get; set; }

        /// <summary>
        /// Description of the Experience
        /// </summary>
        /// <note>
        /// CareerInfo_Description_filtered.txt
        /// </note>
        public string? Description { get; set; }

        /// <summary>
        /// This is the employer of the User.
        /// Freetext field.
        /// </summary>
        /// <note>
        /// CareerInfo_NameOfInstitution_filtered.txt
        /// </note>
        public string? NameOfInstitution { get; set; }

        /// <summary>
        /// The City of the Experience
        /// Autosuggestion fot the City of the Experience
        /// </summary>
        /// <note>
        /// CareerInfo_City_filtered.txt
        /// </note>
        public string? City { get; set; }

        /// <summary>
        /// This describes the Country of the Experience.
        /// Autosuggestion fot the Country of the Experience
        /// </summary>
        /// <note>
        /// CareerInfo_Country_filtered.txt
        /// </note>
        public string? Country { get; set; }

        /// <summary>
        /// This is the Service Type of the Experience.
        /// This is Selection List in the Client UI.
        /// </summary>
        /// <note>
        /// CareerInfo_ServiceType_filtered.txt
        /// </note>
        public ServiceType? ServiceType { get; set; }

        /// <summary>
        /// This is the Voluntary Service Type of the Experience.
        /// Selection List in the Client UI.
        /// </summary>
        /// <Note>
        /// CareerInfo_VoluntaryServiceType_filtered.txt
        /// </Note>
        public VoluntaryServiceType? VoluntaryServiceType { get; set; }

        /// <summary>
        /// This is the Working Time Model of the Experience.
        /// This indicates if the User worked Fulltime, Parttime, Shift, MiniJob or Home Office.
        /// </summary>
        /// <note>
        /// This is a reverse engineered Enum based on the Data we have in the BA Dataset.
        /// </note>
        public WorkingTimeModel? WorkingTimeModel { get; set; }
    }
}
