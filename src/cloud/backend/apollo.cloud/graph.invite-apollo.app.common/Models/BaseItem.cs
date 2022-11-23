using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Esco;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models
{
    [DataContract]
    [ProtoContract]
    [ProtoInclude(5, typeof(AssessmentItem))]
    
    [ProtoInclude(6, typeof(QuestionItem))]
    [ProtoInclude(7, typeof(AnswerItem))]
    [ProtoInclude(8, typeof(AssessmentCategory))]
    [ProtoInclude(9, typeof(AnswerMetaDataRelation))]
    [ProtoInclude(10, typeof(QuestionMetaDataRelation))]
    [ProtoInclude(11, typeof(MetaDataMetaDataRelation))]

    [ProtoInclude(12, typeof(UserProfileItem))]
    [ProtoInclude(13, typeof(AssessmentScore))]
    [ProtoInclude(14, typeof(AnswerItemResult))]
    [ProtoInclude(15, typeof(AssessmentCategoryResult))]
    [ProtoInclude(16, typeof(UserPreferences))]

    [ProtoInclude(17, typeof(Skill))]
    [ProtoInclude( 18, typeof(MetaDataItem))]
    [ProtoInclude(19, typeof(DocumentItem))]
    [ProtoInclude(20, typeof(ApolloLabel))]
    [ProtoInclude(21, typeof(QnAItem))]

    [ProtoInclude(22, typeof(Occupation))]
    [ProtoInclude(23, typeof(PaymentInfo))]
    [ProtoInclude(24, typeof(OccupationHasSkill))]
    [ProtoInclude(25, typeof(OccupationMetaDataRelation))]


    [ProtoInclude(26, typeof(CourseItem))]
    [ProtoInclude(27, typeof(LoanOption))]
    [ProtoInclude(28, typeof(SimilarCourses))]
    [ProtoInclude(29, typeof(CourseContact))]
    [ProtoInclude(30, typeof(CourseBenefits))]
    [ProtoInclude(31, typeof(CourseQnAItem))]

    [ProtoInclude(32, typeof(CoursePriceItem))]
    [ProtoInclude(33, typeof(EduProviderItem))]
    [ProtoInclude(34, typeof(CourseMetaData))]

    [ProtoInclude(35, typeof(CourseDocuments))]
    [ProtoInclude(36, typeof(CourseModuleItem))]
    [ProtoInclude(37, typeof(CourseApolloLabels))]


    [ProtoInclude(38, typeof(CourseAppointment))]
    [ProtoInclude(39, typeof(CourseAppointmentQnA))]
    [ProtoInclude(40, typeof(CourseAppointmentOffer))]
    [ProtoInclude(41, typeof(CourseLearningObjectives))]
    [ProtoInclude(42, typeof(CourseAppointmentMetaData))]




    //[ProtoInclude(100,typeof(MetaDataItem))]
    //[ProtoInclude(200, typeof(AssessmentItem))]
    public class BaseItem: IEntity, IBackendEntity
    {
        #region Implementation of IEntity

        [Key, DataMember(Order = 1), ProtoMember(1)]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        [ProtoMember(2)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity

        [DataMember(Order = 3, IsRequired = true)]
        [ProtoMember(3)]
        public Uri Schema { get; set; } = null!;

        #endregion
    }
}
