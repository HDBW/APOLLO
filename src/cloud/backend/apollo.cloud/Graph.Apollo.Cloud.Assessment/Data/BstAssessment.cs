using System.Reflection.Metadata.Ecma335;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Data
{
    internal class BstAssessment
    {
        public int UseCaseId { get; set; } = 1;

        public string HTMLDistractorPrimary_1;
        public string HTMLDistractorPrimary_2;
        public string HTMLDistractorPrimary_3;
        public string HTMLDistractorPrimary_4;
        public string ScoringOption_1;
        public string HTMLDistractorSecondary_1;
        public string HTMLDistractorSecondary_2;
        public string HTMLDistractorSecondary_3;
        public string HTMLDistractorSecondary_4;
        public int Credit_ScoringOption_1;


        /// <summary>
        /// Unique Identifier of the Skill Assessment
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Identifier of the itemType
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// ItemStem Text
        /// </summary>
        public string ItemStem { get; set; }

        /// <summary>
        /// The Image of the Resource
        /// </summary>
        public string ImageResourceName1 { get; set; }

        /// <summary>
        /// Instruction map to MetaDataType.Hint on Apollo
        /// </summary>
        public string Instruction { get; set; }

        public string NumberOfPrimaryDisctrators { get; set; }
        public int NumberSelectable { get; set; }
        public string DescriptionOfProfession { get; set; }
        public string Kldb { get; set; }
        public string DescriptionOfPartialQualification { get; set; }
        public string DescriptionOfWorkingProcess { get; set; }

        public string Description { get; set; }
        public string AssessmentType { get; set; }
        public string Disclaimer { get; set; }
        public string Duration { get; set; }
        public string EscoOccupationId { get; set; }
        public string EscoSkills { get; set; }
        public string Publisher { get; set; }
        public string Title { get; set; }
        public long CourseId { get; set; }
        public int Limit { get; set; }
        public int QuestionHasPictures { get; set; }
        public int AnswerHasPicture { get; set; }
        public int AmountAnswers { get; set; }
        public string DescriptionOfSkills { get; set; }
        public string EscoId { get; set; }
        public string SubjectArea { get; set; }

        //TODO: Verify Mapping between Bst Data and DTO Schema
        public AnswerType GetAnswerType()
        {
            AnswerType result;
            switch (ItemType.ToUpper())
            {
                case "SORT":
                    result = AnswerType.Integer;
                    break;
                case "CHOICE":
                    result = AnswerType.Boolean;
                    break;
                case "CHOICE_AP":
                    result = AnswerType.Boolean;
                    break;
                case "CHOICE_QP":
                    result = AnswerType.Boolean;
                    break;
                case "ASSOCIATE":
                    result = AnswerType.Long;
                    break;
                case "IMAGEMAP":
                    result = AnswerType.Boolean;
                    break;
                case "RATING":
                    result = AnswerType.Integer;
                    break;
                case "USER":
                    result = AnswerType.TextBox;
                    break;
                default:
                    result = AnswerType.Unknown;
                    break;
            }

            return result;
        }

        public string GetHTMLDistractorPrimary(int answerIndex)
        {
            string result;
            switch (answerIndex)
            {
                case 0:
                    result = HTMLDistractorPrimary_1;
                    break;
                case 1:
                    result = HTMLDistractorPrimary_2;
                    break;
                case 2:
                    result = HTMLDistractorPrimary_3;
                    break;
                case 3:
                    result = HTMLDistractorPrimary_4;
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        public string GetHTMLDistractorSecondary(int answerIndex)
        {
            string result;
            switch (answerIndex)
            {
                case 0:
                    result = HTMLDistractorSecondary_1;
                    break;
                case 1:
                    result = HTMLDistractorSecondary_2;
                    break;
                case 2:
                    result = HTMLDistractorSecondary_3;
                    break;
                case 3:
                    result = HTMLDistractorSecondary_4;
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        public QuestionType GetQuestionType()
        {
            QuestionType result;
            switch (ItemType.ToUpper())
            {
                case "SORT":
                    result = QuestionType.Sort;
                    break;
                case "CHOICE":
                    result = QuestionType.Choice;
                    break;
                case "CHOICE_AP":
                    result = QuestionType.Choice;
                    break;
                case "CHOICE_QP":
                    result = QuestionType.Choice; 
                    break;
                case "ASSOCIATE":
                    result = QuestionType.Associate;
                    break;
                case "IMAGEMAP":
                    result = QuestionType.Imagemap;
                    break;
                case "RATING":
                    result = QuestionType.Rating;
                    break;
                case "SURVEY":
                    result = QuestionType.Survey;
                    break;
                default:
                    result = QuestionType.Unknown;
                    break;
            }

            return result;
        }

        public AssessmentType GetAssessmentType()
        {
            AssessmentType result;
            switch (AssessmentType.ToUpper())
            {
                case "SKILL":
                    result = Common.Models.Assessment.Enums.AssessmentType.SkillAssessment;
                    break;
                case "SOFTSKILL":
                    result = Common.Models.Assessment.Enums.AssessmentType.SoftSkillAssessment;
                    break;
                case "EXPERIENCE":
                    result = Common.Models.Assessment.Enums.AssessmentType.ExperienceAssessment;
                    break;
                case "CLOZE":
                    result = Common.Models.Assessment.Enums.AssessmentType.Cloze;
                    break;
                case "SURVEY":
                    result = Common.Models.Assessment.Enums.AssessmentType.Survey;
                    break;
                default:
                    result = Common.Models.Assessment.Enums.AssessmentType.Unknown;
                    break;
            }

            return result;
        }
    }


}
