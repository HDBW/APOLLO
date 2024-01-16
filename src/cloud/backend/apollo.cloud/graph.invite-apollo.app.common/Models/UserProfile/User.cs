using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Esco;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    /// <summary>
    /// The User Profile is the main Data Object for the User.
    /// It contains all the Information about the User.
    /// This Object contains PII and is GDPR relevant.
    /// </summary>
    [DataContract]
    public class User
    {
        /// <summary>
        /// This is the Unique Identifier of the User.
        /// Object ID
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// This is the User principal name given by AADB2C.
        /// </summary>
        public string? Upn { get; set; }

        /// <summary>
        /// The Name is the Display Name of the User
        /// It is aquired via the Identity Provider and can be changed by the User during Registration as well as in the Profile.
        /// PII
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ContactInfo> ContactInfos { get; set; }

        /// <summary>
        /// Indicates the Birthdate of the User
        /// Optional Information
        /// PII
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// This indicates if the User has a Disability
        /// We do not classify disabilities but only indicate if the User has one or not.
        /// This is relevant for the User to get the right Information and for the Admins to know if the User needs special treatment.
        /// </summary>
        public bool? Disabilities { get; set; }

        public Profile? Profile { get; set; }
    }

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
        ///  This is the Professional career information of the User.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>2</value>
        /// </remarks>
        public List<CareerInfo> CareerInfos { get; set; }

        /// <summary>
        /// This is the education and apprenticeship information of the User.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>1</value>
        /// </remarks>
        public List<EducationInfo> EducationInfos { get; set; }

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
        public List<Qualification> Qualifications { get; set; }

        /// <summary>
        /// This is the driving license information of the User.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>0</value>
        /// </remarks>
        public Mobility MobilityInfo { get; set; }

        /// <summary>
        /// This is the Language Skills of the User.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>2</value>
        /// </remarks>
        public List<Language> LanguageSkills { get; set; }

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
        public List<Skill> Skills { get; set; }

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
        public Knowledge Knowledge { get; set; }

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
        public List<Apprenticeship> Apprenticeships { get; set; }

        /// <summary>
        /// License that are directly associated to the User.
        /// This is relevant for the classification of the User Profile.
        /// These seem to be more regulated by the government.
        /// Only certain Institutions mainly government and federal entities can give a License to a User.
        /// </summary>
        /// <remarks>
        ///     Relevance <value>2</value>
        /// </remarks>
        public List<License> Licenses { get; set; }

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
        public List<WebReference> WebReferences { get; set; }
    }
}
