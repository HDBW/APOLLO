using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace Invite.Apollo.App.Graph.Common.Models
{
    /// <summary>
    /// TESTING PURPOSE ONLY
    /// USECASES FOR PROTOTYPE TESTING
    /// </summary>
    [DataContract]
    public class UseCaseCollections
    {
        /// <summary>
        /// Assessment Overload
        /// </summary>
        /// <param name="assessmentItems"></param>
        /// <param name="questionItems"></param>
        /// <param name="answerItems"></param>
        /// <param name="metaDataItems"></param>
        /// <param name="questionMetaDataRelations"></param>
        /// <param name="answerMetaDataRelations"></param>
        /// <param name="metaDataMetaDataRelations"></param>
        public UseCaseCollections(Collection<AssessmentItem> assessmentItems, Collection<QuestionItem> questionItems, Collection<AnswerItem> answerItems, Collection<MetaDataItem> metaDataItems, Collection<QuestionMetaDataRelation> questionMetaDataRelations, Collection<AnswerMetaDataRelation> answerMetaDataRelations, Collection<MetaDataMetaDataRelation> metaDataMetaDataRelations)
        {
            AssessmentItems = assessmentItems;
            QuestionItems = questionItems;
            AnswerItems = answerItems;
            MetaDataItems = metaDataItems;
            QuestionMetaDataRelations = questionMetaDataRelations;
            AnswerMetaDataRelations = answerMetaDataRelations;
            MetaDataMetaDataRelations = metaDataMetaDataRelations;
        }

        /// <summary>
        /// UseCase Overload
        /// </summary>
        /// <param name="assessmentItems"></param>
        /// <param name="questionItems"></param>
        /// <param name="answerItems"></param>
        /// <param name="metaDataItems"></param>
        /// <param name="questionMetaDataRelations"></param>
        /// <param name="answerMetaDataRelations"></param>
        /// <param name="metaDataMetaDataRelations"></param>
        /// <param name="userProfile"></param>
        /// <param name="eduProviderItems"></param>
        /// <param name="courseContacts"></param>
        /// <param name="courseAppointments"></param>
        /// <param name="courseItems"></param>
        public UseCaseCollections(Collection<AssessmentItem> assessmentItems, Collection<QuestionItem> questionItems,
            Collection<AnswerItem> answerItems, Collection<MetaDataItem> metaDataItems,
            Collection<QuestionMetaDataRelation> questionMetaDataRelations,
            Collection<AnswerMetaDataRelation> answerMetaDataRelations,
            Collection<MetaDataMetaDataRelation> metaDataMetaDataRelations, UserProfileItem userProfile,
            Collection<EduProviderItem> eduProviderItems, Collection<CourseContact> courseContacts,
            Collection<CourseAppointment> courseAppointments, Collection<CourseItem> courseItems)
        {
            AssessmentItems = assessmentItems;
            QuestionItems = questionItems;
            AnswerItems = answerItems;
            MetaDataItems = metaDataItems;
            QuestionMetaDataRelations = questionMetaDataRelations;
            AnswerMetaDataRelations = answerMetaDataRelations;
            MetaDataMetaDataRelations = metaDataMetaDataRelations;
            UserProfile = userProfile;
            EduProviderItems = eduProviderItems;
            CourseContacts = courseContacts;
            CourseAppointments = courseAppointments;
            CourseItems = courseItems;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UseCaseCollections()
        {
            UserProfile = new UserProfileItem();
            AssessmentItems = new Collection<AssessmentItem>();
            QuestionItems = new Collection<QuestionItem>();
            AnswerItems = new Collection<AnswerItem>();
            MetaDataItems = new Collection<MetaDataItem>();
            QuestionMetaDataRelations = new Collection<QuestionMetaDataRelation>();
            AnswerMetaDataRelations = new Collection<AnswerMetaDataRelation>();
            MetaDataMetaDataRelations = new Collection<MetaDataMetaDataRelation>();
            EduProviderItems = new Collection<EduProviderItem>();
            CourseContacts = new Collection<CourseContact>();
            CourseAppointments = new Collection<CourseAppointment>();
            CourseItems = new Collection<CourseItem>();
        }

        [DataMember(Order = 1, IsRequired = false)]
        public Collection<AssessmentItem> AssessmentItems { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public Collection<QuestionItem> QuestionItems { get; set; }

        [DataMember(Order = 3, IsRequired = false)]
        public Collection<AnswerItem> AnswerItems { get; set; }

        [DataMember(Order = 4, IsRequired = false)]
        public Collection<MetaDataItem> MetaDataItems { get; set; }

        [DataMember(Order = 5, IsRequired = false)]
        public Collection<QuestionMetaDataRelation> QuestionMetaDataRelations { get; set; }

        [DataMember(Order = 6, IsRequired = false)]
        public Collection<AnswerMetaDataRelation> AnswerMetaDataRelations { get; set; }

        [DataMember(Order = 7, IsRequired = false)]
        public Collection<MetaDataMetaDataRelation> MetaDataMetaDataRelations { get; set; }

        [DataMember(Order = 8, IsRequired = false)]
        public Collection<CourseItem> CourseItems { get; set; }

        [DataMember(Order = 9, IsRequired = false)]
        public Collection<CourseAppointment> CourseAppointments { get; set; }

        [DataMember(Order = 10, IsRequired = false)]
        public Collection<CourseContact> CourseContacts { get; set; }

        [DataMember(Order = 11, IsRequired = false)]
        public Collection<EduProviderItem> EduProviderItems { get; set; }

        [DataMember(Order = 12, IsRequired = false)]
        public UserProfileItem UserProfile { get; set; }
    }
}
