using AutoMapper;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class AnswerTypeResolver: IValueResolver<Answer, AnswerItem, AnswerType>
    {
        public AnswerType Resolve(Answer source, AnswerItem destination, AnswerType destMember, ResolutionContext context)
        {
            AnswerType type = new();
            switch (source.Question.QuestionType)
            {
                case QuestionType.Unknown:
                    type = AnswerType.Unknown;
                    break;
                case QuestionType.Choice:
                    type = AnswerType.Boolean;
                    break;
                case QuestionType.Sort:
                    type = AnswerType.Long;
                    break;
                case QuestionType.Associate:
                    type = AnswerType.Long;
                    break;
                case QuestionType.Binary:
                    type = AnswerType.Boolean;
                    break;
                case QuestionType.Eafrequency:
                    type = AnswerType.Integer;
                    break;
                case QuestionType.Eaconditions:
                    type = AnswerType.Integer;
                    break;
                case QuestionType.Imagemap:
                    type = AnswerType.Location;
                    break;
                case QuestionType.Rating:
                    type = AnswerType.Integer;
                    break;
                case QuestionType.Cloze:
                    type = AnswerType.String;
                    break;
                case QuestionType.Survey:
                    type = AnswerType.String;
                    break;
                default:
                    type = AnswerType.Unknown;
                    break;
            }

            return type;
        }
    }

    public class QuestionLayoutResolver : IValueResolver<Question, QuestionItem, LayoutType>
    {
        public LayoutType Resolve(Question source, QuestionItem destination, LayoutType destMember, ResolutionContext context)
        {
            LayoutType type = new();
            switch (source.QuestionType)
            {
                case QuestionType.Unknown:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Choice:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Sort:
                    type = LayoutType.UniformGrid;
                    break;
                case QuestionType.Associate:
                    type = LayoutType.UniformGrid;
                    break;
                case QuestionType.Binary:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Eafrequency:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Eaconditions:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Imagemap:
                    type = LayoutType.Overlay;
                    break;
                case QuestionType.Rating:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Cloze:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Survey:
                    type = LayoutType.Default;
                    break;
                default:
                    type = LayoutType.Default;
                    break;
            }

            return type;
        }
    }

    public class AnswerLayoutResolver : IValueResolver<Question, QuestionItem, LayoutType>
    {
        public LayoutType Resolve(Question source, QuestionItem destination, LayoutType destMember, ResolutionContext context)
        {
            LayoutType type = new();
            switch (source.QuestionType)
            {
                case QuestionType.Unknown:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Choice:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Sort:
                    type = LayoutType.UniformGrid;
                    break;
                case QuestionType.Associate:
                    type = LayoutType.UniformGrid;
                    break;
                case QuestionType.Binary:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Eafrequency:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Eaconditions:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Imagemap:
                    type = LayoutType.Overlay;
                    break;
                case QuestionType.Rating:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Cloze:
                    type = LayoutType.Default;
                    break;
                case QuestionType.Survey:
                    type = LayoutType.Default;
                    break;
                default:
                    type = LayoutType.Default;
                    break;
            }

            return type;
        }
    }

    public class InteractionTypeResolver : IValueResolver<Question, QuestionItem, InteractionType>
    {
        public InteractionType Resolve(Question source, QuestionItem destination, InteractionType destMember, ResolutionContext context)
        {
            InteractionType type = new();
            switch (source.QuestionType)
            {
                case QuestionType.Unknown:
                    type = InteractionType.Unknown;
                    break;
                case QuestionType.Choice:
                    type = InteractionType.MultiSelect;
                    break;
                case QuestionType.Sort:
                    type = InteractionType.Associate;
                    break;
                case QuestionType.Associate:
                    type = InteractionType.Associate;
                    break;
                case QuestionType.Binary:
                    type = InteractionType.SingleSelect;
                    break;
                case QuestionType.Eafrequency:
                    type = InteractionType.Input;
                    break;
                case QuestionType.Eaconditions:
                    type = InteractionType.Input;
                    break;
                case QuestionType.Imagemap:
                    type = InteractionType.SingleSelect;
                    break;
                case QuestionType.Rating:
                    type = InteractionType.Input;
                    break;
                case QuestionType.Cloze:
                    type = InteractionType.Input;
                    break;
                case QuestionType.Survey:
                    type = InteractionType.Input;
                    break;
                default:
                    type = InteractionType.Unknown;
                    break;
            }
            return type;
        }
    }

}
