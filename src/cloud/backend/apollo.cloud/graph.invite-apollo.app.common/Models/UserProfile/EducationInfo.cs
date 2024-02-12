// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using Invite.Apollo.App.Graph.Common.Models.Lists;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    /// <summary>
    /// This is the Education Information of the User
    /// This indicates the Highest Education Level of the User as well as his education history.
    /// This is an Instance of List because a User can have multiple Education Information
    /// Education and Graduation as well as Recognition of foreign Diplomas is a big topic in Germany.
    /// Apollo does not have enough Data to make a good prediction on this.
    /// We are not able to get a logical conclusion about the Education Level, school graduations and university degrees.
    /// In germany this topic is defined by the 16 states and changes each year.
    /// A historical and complete automation of the classification is impossible for the scope of the project.
    /// </summary>
    /// <remarks>
    /// We identified 5 different Education Types as stated in the EducationType Enum.
    /// These Education Type definitions are based on the BA Dataset for Machine Learning.
    /// <note>We added University of Applied Science to List of TypeOfSchool to comply if regulatory aspects of vocational training.</note>
    /// This uses the following Taxonomy:
    ///     <remarks>
    ///     1. Education = 1
    ///     We decided to base this information on the "graduation" and select the School.
    ///     Being aware of the associated problems and lack of confidence we have about the school system the ability to check the user input for correctness.
    ///     In this case the following fields will be used:
    ///     <param name="StartDate"></param>
    ///     <param name="EndDate"></param>
    ///     <param name="City"></param>
    ///     <param name="Country"></param>
    ///     <param name="CompletionState"></param>
    ///     <param name="SchoolGraduation"></param>
    ///     <param name="TypeOfSchool">VocationalSchool, TechnicalAcademy, UniversityOfAppliedScience are excluded in this case</param>
    ///     </remarks>
    ///     2. CompanyBasedVocationalTraining = 2
    ///     This is the main focus audience of apollo. People who have a company based vocational training.
    ///     The issue here is that we do have two parts of the training. The school part and the company part.
    ///     The school part is relevant cause it defines that graduation of the vocational school and work experience qualifies for study in university of applied sciences.
    ///     The company part is relevant cause it defines the work experience as well as the occupation and is typically attested by the chamber of commerce or craftsmanship or equivalent institutes for government, military, arts or other.
    ///     So basically this will generate two entries in vita represented by the profile.
    ///     First it will generate the Education entry with the school and the graduation as well as the occupation.
    ///     It will also generate the career entry with the company and the occupation.
    ///     <param name="StartDate"></param>
    ///     <param name="EndDate"></param>
    ///     <param name="City"></param>
    ///     <param name="Country"></param>
    ///     <param name="CompletionState"></param>
    ///     <param name="SchoolGraduation"></param>
    ///     <param name="TypeOfSchool"></param>
    ///     <param name="NameOfInstitution"></param>
    ///     <param name="SchoolGraduation"></param>
    ///     <param name="ProfessionalTitle"></param>
    ///     <remarks>
    ///     
    /// </remarks>
    /// </remarks>
    public class EducationInfo
    {
        public string? Id { get; set; }

        /// <summary>
        /// Start Date of the education
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// End date of the Education
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// City of the Education
        /// Autocomplete in the UI
        /// </summary>
        /// <note>
        /// EducationInfo_City_filtered.txt
        /// </note>
        public string? City { get; set; }

        /// <summary>
        /// Country of the Education
        /// Autocomplete in the UI
        /// </summary>
        /// <note>EducationInfo_Country_filtered.txt</note>
        public string? Country { get; set; }

        /// <summary>
        /// Description of the Education
        /// Allows the User to describe his education
        /// </summary>
        /// <note>
        /// EducationInfo_Description_filtered.txt
        /// </note>
        public string? Description { get; set; }

        /// <summary>
        /// Education Title of the Education represented as Occupation
        /// Autocomplete in the UI
        /// </summary>
        /// <note>
        /// EducationInfo_ProfessionalTitle_filtered.txt
        /// </note>
        public Occupation? ProfessionalTitle { get; set; }

        // Auswahlliste
        public ApolloListItem CompletionState { get; set; }

        // EducationInfo_Graduation_filtered.txt
        // Auswahlliste
        public ApolloListItem? Graduation { get; set; }

        // EducationInfo_UniversityDegree_filtered.txt
        // Auswahlliste
        public ApolloListItem? UniversityDegree { get; set; }

        // EducationInfo_TypeOfSchool_filtered.txt
        // Auswahlliste
        public ApolloListItem? TypeOfSchool { get; set; }

        // EducationInfo_NameOfInstitution_filtered.txt
        // Freitext
        public string? NameOfInstitution { get; set; }

        // EducationInfo_EducationType_filtered.txt
        // Auswahlliste
        public ApolloListItem EducationType { get; set; }

        // EducationInfo_Recognition_filtered.txt
        // Auswahlliste
        public ApolloListItem? Recognition { get; set; }

    }
}
