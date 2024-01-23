// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using Invite.Apollo.App.Graph.Common.Models.Esco;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    /// <summary>
    /// This is the Profileinformation of the User.
    /// This information contains the basic CV Information of the User.
    /// This also contains the Skills and Classification associated to a Profile.
    /// Please note that this is not PII relevant.
    /// If a user deletes his account this information will not be deleted.
    /// This will be used for the Machine Learning and the User Profile Classification.
    /// A user can not restore this information.
    /// So if a user deletes his account and creates a new one he has to fill out the profile again.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// This is the Unique Identifier set by Apollo for the User.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///  This is the Professional career information of the User.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>2</value>
        /// </remarks>
        public List<CareerInfo> CareerInfos { get; set; } = new List<CareerInfo>();

        /// <summary>
        /// This is the education and apprenticeship information of the User.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>1</value>
        /// </remarks>
        public List<EducationInfo> EducationInfos { get; set; } = new List<EducationInfo>();

        /// <summary>
        /// This is the qualifications a User has.
        /// Qualifications are somewhat regulated by the government.
        /// But basically an Institution can give a Qualification to a User.
        /// This can be done by a training or test.
        /// This is relevant for the classification of the User Profile.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>2</value>
        /// </remarks>
        public List<Qualification> Qualifications { get; set; } = new List<Qualification>();

        /// <summary>
        /// This is the driving license information of the User.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>0</value>
        /// </remarks>
        public Mobility MobilityInfo { get; set; } = new Mobility();

        /// <summary>
        /// This is the Language Skills of the User.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>2</value>
        /// </remarks>
        public List<Language> LanguageSkills { get; set; } = new List<Language>();

        /// <summary>
        /// Skills that are directly associated to the User.
        /// This is done by classification of the User Profile Data.
        /// This is not shared with the User or mobile App.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>0</value>
        /// </remarks>
        /// <Note>Not part of UI</Note>
        /// <Note>Backend only</Note>
        public List<Skill> Skills { get; set; } = new List<Skill>();


        /// <summary>
        /// This is for the Backend only to classify the Occupations a User has.
        /// Should not be shared with the Client or the User.
        /// </summary>
        public List<Occupation> Occupations { get; set; } = new List<Occupation>();

        /// <summary>
        /// Knowledge is part of the original BA Dataset for Machine Learning.
        /// This is part of the classification of the User Profile Data.
        /// This is not shared with the User or mobile App.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>0</value>
        /// </remarks>
        /// <Note>Not part of UI</Note>
        /// <Note>Backend only</Note>
        public Knowledge Knowledge { get; set; } = new Knowledge();

        /// <summary>
        /// Apprenticeship is part of the original BA Dataset for Machine Learning.
        /// This is part of the classification of the User Profile Data.
        /// This is not shared with the User or mobile App.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>0</value>
        /// </remarks>
        /// <Note>Not part of UI</Note>
        /// <Note>Backend only</Note>
        public List<Apprenticeship> Apprenticeships { get; set; } = new List<Apprenticeship>();

        /// <summary>
        /// License that are directly associated to the User.
        /// This is relevant for the classification of the User Profile.
        /// These seem to be more regulated by the government.
        /// Only certain Institutions mainly government and federal entities can give a License to a User.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>2</value>
        /// </remarks>
        public List<License> Licenses { get; set; } = new List<License>();

        // not in ui
        public LeadershipSkills? LeadershipSkills { get; set; }

        /// <summary>
        /// Honestly this is a KeyValuePair<Uri,String>.
        /// May the gods of the dotnet maui platform explain to me why this is a list.
        /// Yeah, probably cause DataBinding does not like KeyValuePairs?
        /// </summary>
        /// <remarks>
        ///     Relevance <value>1</value>
        /// </remarks>
        public List<WebReference> WebReferences { get; set; } = new List<WebReference>();
    }
}
