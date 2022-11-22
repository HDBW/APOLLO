using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.Execution;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class AnswerTypeResolver: IValueResolver<Answer, AnswerItem, AnswerType>
    {
        public AnswerType Resolve(Answer source, AnswerItem destination, AnswerType destMember, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class QuestionLayoutResolver : IValueResolver<Question, QuestionItem, LayoutType>
    {
        public LayoutType Resolve(Question source, QuestionItem destination, LayoutType destMember, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class AnswerLayoutResolver : IValueResolver<Question, QuestionItem, LayoutType>
    {
        public LayoutType Resolve(Question source, QuestionItem destination, LayoutType destMember, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class InteractionTypeResolver : IValueResolver<Question, QuestionItem, InteractionType>
    {
        public InteractionType Resolve(Question source, QuestionItem destination, InteractionType destMember, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }

}
