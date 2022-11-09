using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Common.Test
{
    public class AssessmentDictonary
    {
        private Dictionary<int, AssessmentItem> _assessmentItems = new();
        private Dictionary<int, MetaDataItem> _metaDataItems = new();
        private Dictionary<int, AnswerItem> _answerItems = new();
        private Dictionary<int, QuestionItem> _questionItems = new();
        private Dictionary<int, MetaDataMetaDataRelation> _metaDataMetaDataRelations = new();
        private Dictionary<int, QuestionMetaDataRelation> _questionMetaDataRelation = new();
        private Dictionary<int, AnswerMetaDataRelation> _answerMetaDataRelation = new();

        public AssessmentItem AddAssessmentItem(AssessmentItem item)
        {
            item.Id = AssessmentIndex;
            _assessmentItems.Add(AssessmentIndex,item);
            return item;
        }

        public int AssessmentIndex => _assessmentItems.Count;

        public MetaDataItem AddMetaDataItem(MetaDataItem meta)
        {
            meta.Id = MetaDataIndex;
            _metaDataItems.Add(MetaDataIndex,meta);
            return meta;
        }

        public int MetaDataIndex => _metaDataItems.Count;

        public QuestionItem AddQuestionItem(QuestionItem question)
        {
            question.Id = QuestionIndex;
            _questionItems.Add(QuestionIndex,question);
            return question;
        }

        public int QuestionIndex => _questionItems.Count;

        public AnswerItem AddAnswerItem(AnswerItem answer)
        {
            answer.Id = AnswerIndex;
            _answerItems.Add(AnswerIndex,answer);
            return answer;
        }

        public int AnswerIndex => _answerItems.Count;

        public QuestionMetaDataRelation AddMetaDataToQuestion(QuestionItem question, MetaDataItem meta)
        {
            QuestionMetaDataRelation relation = new QuestionMetaDataRelation
                {
                    Id = QuestionMetaDataIndex, BackendId = QuestionMetaDataIndex,
                    MetaDataId = meta.Id,
                    QuestionId = question.Id,
                    Schema = new($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks
                };
            _questionMetaDataRelation.Add(QuestionMetaDataIndex, relation);
            return relation;
        }

        public int QuestionMetaDataIndex => _questionMetaDataRelation.Count;

        public AnswerMetaDataRelation AddMetaDataToAnswer(AnswerItem answer, MetaDataItem meta)
        {
            AnswerMetaDataRelation relation = new AnswerMetaDataRelation
            {
                Id = AnswerMetaIndex,
                BackendId = AnswerMetaIndex,
                MetaDataId = meta.Id,
                AnswerId = answer.Id,
                Schema = new($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Ticks = DateTime.Now.Ticks
            };
            _answerMetaDataRelation.Add(AnswerMetaIndex, relation);
            return relation;
        }

        public int AnswerMetaIndex => _answerMetaDataRelation.Count;

        public MetaDataMetaDataRelation AddLabel(MetaDataItem source, MetaDataItem target)
        {
            MetaDataMetaDataRelation relation = new MetaDataMetaDataRelation
            {
                Id = AnswerMetaIndex,
                BackendId = AnswerMetaIndex,
                SourceId = source.Id,
                TargetId = target.Id,
                Schema = new($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Ticks = DateTime.Now.Ticks
            };
            return relation;
        }

        public int MetaDataMetaDataIndex => _metaDataMetaDataRelations.Count;

        public Collection<AssessmentItem> GetAssessmentItems()
        {
            Collection<AssessmentItem> assessment = new(_assessmentItems.Values.ToList());
            return assessment;
        }

        public Collection<MetaDataItem> GetMetaDataItems()
        {
            Collection<MetaDataItem> metaDataItems = new(_metaDataItems.Values.ToList());
            return metaDataItems;
        }

        public Collection<AnswerItem> GetAnswerItems()
        {
            Collection<AnswerItem> answerItems = new(_answerItems.Values.ToList());
            return answerItems;
        }

        public Collection<QuestionItem> GetQuestionItems()
        {
            Collection<QuestionItem> questionItems = new(_questionItems.Values.ToList());
            return questionItems;
        }

        public Collection<MetaDataMetaDataRelation> GetMetaDataMetaDataRelations()
        {
            Collection<MetaDataMetaDataRelation> metaDataRelations = new(_metaDataMetaDataRelations.Values.ToList());
            return metaDataRelations;
        }

        public Collection<QuestionMetaDataRelation> GetQuestionMetaDataRelations()
        {
            Collection<QuestionMetaDataRelation> questionMetaDataRelations =
                new(_questionMetaDataRelation.Values.ToList());
            return questionMetaDataRelations;
        }

        public Collection<AnswerMetaDataRelation> GetAnswerMetaDataRelations()
        {
            Collection<AnswerMetaDataRelation> answerMetaDataRelations = new(_answerMetaDataRelation.Values.ToList());
            return answerMetaDataRelations;
        }

    }
}
