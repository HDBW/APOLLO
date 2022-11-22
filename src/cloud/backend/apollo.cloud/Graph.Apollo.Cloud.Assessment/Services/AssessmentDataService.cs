using AutoMapper;
using Invite.Apollo.App.Graph.Assessment.Models;
using Invite.Apollo.App.Graph.Assessment.Repository;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.EntityFrameworkCore;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public class AssessmentDataService :
        IDataService
    {
        //Implement logger
        private readonly ILogger<AssessmentDataService> _logger;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IMetaDataRepository _metaDataRepository;
        private readonly IAnswerHasMetaDataRepository _answerHasMetaDataRepository;
        private readonly IQuestionHasMetaDataRepository _questionHasMetaDataRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMetaDataHasMetaDataRepository _metaDataHasMetaDataRepository;
        

        public AssessmentDataService(ILogger<AssessmentDataService> logger, IAssessmentRepository assessmentRepository, IQuestionRepository questionRepository, IAnswerRepository answerRepository, IMetaDataRepository metaDataRepository, IAnswerHasMetaDataRepository answerHasMetaDataRepository, IQuestionHasMetaDataRepository questionHasMetaDataRepository, ICategoryRepository categoryRepository, IMetaDataHasMetaDataRepository metaDataHasMetaDataRepository)
        {
            _logger = logger;
            _assessmentRepository = assessmentRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _metaDataRepository = metaDataRepository;
            _answerHasMetaDataRepository = answerHasMetaDataRepository;
            _questionHasMetaDataRepository = questionHasMetaDataRepository;
            _categoryRepository = categoryRepository;
            _metaDataHasMetaDataRepository = metaDataHasMetaDataRepository;
        }

        
        //TODO: https://victorakpan.com/blog/ef-core-inmemory-and-dependency-injection-in-console-app

        #region Implementation of IAssessmentDataService

        public async Task<IEnumerable<Models.Assessment>> GetAllAssessmentsAsync() =>
            await _assessmentRepository.GetAllAsync();

        public async Task<Models.Assessment> GetAssessmentByIdAsync(long assessmentId)
        {
            return await _assessmentRepository.GetSingleAsync(assessmentId);
        }


        public async Task<IEnumerable<Models.Assessment>> GetAssessmentsByOccupation(string occupation) =>
            await _assessmentRepository.FindByAsync(assessment => assessment.EscoOccupationId.Equals(occupation));

        //TODO: Implement return Assessment
        public void CreateAssessment(Models.Assessment assessment) =>
            _assessmentRepository.Add(assessment);

        public void EditAssessmentAsync(Models.Assessment assessment)
        {
            _assessmentRepository.Edit(assessment);
            _assessmentRepository.Commit();
        }

        public void DeleteAssessmentAsync(Models.Assessment assessment)
        {
            _assessmentRepository.Delete(assessment);
            //TODO: Configure Delete 
            _assessmentRepository.Commit();
        }

        #endregion

        #region Implementation of IAssessmentDtoDataService

        async Task<IEnumerable<AssessmentItem>> IAssessmentDtoDataService.GetAssessmentsByOccupation(string occupation)
        {
            var assessments = await GetAssessmentsByOccupation(occupation);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Assessment, AssessmentItem>());
            Mapper mapper = new(config);
            List<AssessmentItem> assessmentItems = new();
            foreach (var assessment in assessments)
            {
                assessmentItems.Add(mapper.Map<AssessmentItem>(assessment));
            }

            return assessmentItems;
        }

        public void CreateAssessmentItem(AssessmentItem assessment) => throw new NotImplementedException();

        public void EditAssessmentItemAsync(AssessmentItem assessment) => throw new NotImplementedException();

        public void DeleteAssessmentItemAsync(AssessmentItem assessment) => throw new NotImplementedException();

        public async Task<AssessmentItem> GetAssessmentItemByIdAsync(long assessmentId)
        {
            var assessment = await _assessmentRepository.GetSingleAsync(assessmentId);
            //var assessment = await _assessmentRepository.FindByAsync(a => a.Id.Equals(assessmentId));
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Models.Assessment, AssessmentItem>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<AssessmentItem>(assessment);

        }

        public async Task<IEnumerable<AssessmentItem>> GetAllAssessmentItemsAsync()
        {
            var assessments = await _assessmentRepository.GetAllAsync();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Models.Assessment, AssessmentItem>()
            );
            var mapper = new Mapper(config);
            List<AssessmentItem> assessmentItems = new List<AssessmentItem>();
            foreach (var assessment in assessments)
            {
                assessmentItems.Add(mapper.Map<AssessmentItem>(assessment));
            }

            return assessmentItems;
        }

        #endregion

        #region Implementation of IQuestionDataService

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync() => await _questionRepository.GetAllAsync();

        public async Task<Question> GetQuestionByIdAsync(long assessmentId) => await _questionRepository.GetSingleAsync(assessmentId);

        public void CreateQuestion(Question question) => _questionRepository.Add(question);

        public void EditQuestionAsync(Question question)
        {
            _questionRepository.Edit(question);
            _questionRepository.Commit();
        }

        public void DeleteQuestionAsync(Question question)
        {
            _questionRepository.Delete(question);
            _questionRepository.Commit();
        }

        #endregion

        #region Implementation of IQuestionDtoDataService

        public async Task<IEnumerable<QuestionItem>> GetAllQuestionItemsAsync()
        {
            var questions = await GetAllQuestionsAsync();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Models.Question, QuestionItem>()
                    .ForMember(dest => dest.AnswerLayout, opt => opt.MapFrom<AnswerLayoutResolver>())
                    .ForMember(dest => dest.QuestionLayout, opt => opt.MapFrom<QuestionLayoutResolver>())
                    .ForMember(dest => dest.Interaction, opt => opt.MapFrom<InteractionTypeResolver>())
                );

            var mapper = new Mapper(config);
            List<QuestionItem> questionItems = new();
            foreach (var item in questions)
            {
                questionItems.Add(mapper.Map<QuestionItem>(item));
            }
            return questionItems;
        }

        public async Task<QuestionItem> GetQuestionItemByIdAsync(long assessmentId)
        {
            var question = await GetQuestionByIdAsync(assessmentId);
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Models.Question, QuestionItem>()
                    .ForMember(dest => dest.AnswerLayout, opt => opt.MapFrom<AnswerLayoutResolver>())
                    .ForMember(dest => dest.QuestionLayout, opt => opt.MapFrom<QuestionLayoutResolver>())
                    .ForMember(dest => dest.Interaction, opt => opt.MapFrom<InteractionTypeResolver>())
            );
            var mapper = new Mapper(config);
            return mapper.Map<QuestionItem>(question);
        }

        public void CreateQuestionItem(QuestionItem question) => throw new NotImplementedException();

        public void EditQuestionItemAsync(QuestionItem question) => throw new NotImplementedException();

        public void DeleteQuestionItemAsync(QuestionItem question) => throw new NotImplementedException();

        #endregion

        #region Implementation of IAnswerDataService

        public async Task<IEnumerable<Answer>> GetAllAnswersAsync() => await _answerRepository.GetAllAsync();

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(long questionId) =>
            await _answerRepository.FindByAsync(a => a.QuestionId == questionId);

        public void CreateAnswer(Answer answer) => _answerRepository.Add(answer);

        public void EditAnswerAsync(Answer answer)
        {
            _answerRepository.Edit(answer);
            _answerRepository.Commit();
        }

        public void DeleteAnswerAsync(Answer answer)
        {
            _answerRepository.Delete(answer);
            _answerRepository.Commit();
        }

        #endregion

        #region Implementation of IAnswerDtoDataService

        public async Task<IEnumerable<AnswerItem>> GetAllAnswerItemsAsync()
        {
            var answers = await GetAllAnswersAsync();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Answer, AnswerItem>()
                .ForMember(dest => dest.AnswerType, opt => opt.MapFrom<AnswerTypeResolver>()));

            var mapper = new Mapper(config);
            List<AnswerItem> answerItems = new();
            foreach (Answer answer in answers)
            {
                answerItems.Add(mapper.Map<AnswerItem>(answer));
            }
            return answerItems;
        }

        public async Task<AnswerItem> GetAnswerItemByIdAsync(long question)
        {
            var answer = await GetAnswersByQuestionIdAsync(question);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Answer, AnswerItem>()
                .ForMember(dest => dest.AnswerType, opt => opt.MapFrom<AnswerTypeResolver>()));

            var mapper = new Mapper(config);
            return mapper.Map<AnswerItem>(answer);
        }

        public void CreateAnswerItem(AnswerItem answer) => throw new NotImplementedException();

        public void EditAnswerItemAsync(AnswerItem answer) => throw new NotImplementedException();

        public void DeleteAnswerItemAsync(AnswerItem answer) => throw new NotImplementedException();

        #endregion

        #region Implementation of IMetaDataDataService

        public async Task<IEnumerable<MetaData>> GetAllMetaDataAsync() => await _metaDataRepository.GetAllAsync();

        public async Task<MetaData> GetMetaDataByIdAsync(long metadataId) => await _metaDataRepository.GetSingleAsync(metadataId);

        public void CreateMetaData(MetaData metaData) => _metaDataRepository.Add(metaData);

        public void EditMetaDataAsync(MetaData metaData)
        {
            _metaDataRepository.Edit(metaData);
            _metaDataRepository.Commit();
        }

        public void DeleteMetaDataAsync(MetaData metaData)
        {
            _metaDataRepository.Edit(metaData);
            _metaDataRepository.Commit();
        }

        #endregion

        #region Implementation of IMetaDataDtoDataService

        public async Task<IEnumerable<MetaDataItem>> GetAllMetaDataItemsAsync()
        {
            var metaData = await GetAllMetaDataAsync();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.MetaData, MetaDataItem>());
            var mapper = new Mapper(config);
            List<MetaDataItem> metaDataItems = new();
            foreach (MetaData data in metaData)
            {
                metaDataItems.Add(mapper.Map<MetaDataItem>(data));
            }
            return metaDataItems;
        }

        public async Task<MetaDataItem> GetMetaDataItemByIdAsync(long metadataId)
        {
            var metaData = await GetMetaDataByIdAsync(metadataId);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.MetaData, MetaDataItem>());
            var mapper = new Mapper(config);
            return mapper.Map<MetaDataItem>(metaData);
        }

        public void CreateMetaDataItem(MetaDataItem metaData) => throw new NotImplementedException();

        public void EditMetaDataItemAsync(MetaDataItem metaData) => throw new NotImplementedException();

        public void DeleteMetaDataItemAsync(MetaDataItem metaData) => throw new NotImplementedException();

        #endregion

        #region Implementation of IAnswerHasMetaDataDataService

        public async Task<IEnumerable<AnswerHasMetaData>> GetAllAnswerHasMetaDataAsync() => await _answerHasMetaDataRepository.GetAllAsync();

        public async Task<IEnumerable<AnswerHasMetaData>> GetAnswerHasMetaDataByAnswerIdAsync(long answerId) => await _answerHasMetaDataRepository.FindByAsync(m=>m.AnswerId==answerId);

        public void CreateAnswerHasMetaData(AnswerHasMetaData answerHasMetaData) =>
            _answerHasMetaDataRepository.Add(answerHasMetaData);

        public void EditAnswerHasMetaData(AnswerHasMetaData answerHasMetaData)
        {
            _answerHasMetaDataRepository.Edit(answerHasMetaData);
            _answerHasMetaDataRepository.Commit();
        }

        public void DeleteAnswerHasMetaData(AnswerHasMetaData answerHasMetaData)
        {
            _answerHasMetaDataRepository.Edit(answerHasMetaData);
            _answerHasMetaDataRepository.Commit();
        }

        #endregion

        #region Implementation of IAnswerHasMetaDataDtoDataService

        public async Task<IEnumerable<AnswerMetaDataRelation>> GetAllAnswerMetaDataRelationsAsync()
        {
            var answerHasMetaData = await GetAllAnswerHasMetaDataAsync();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.AnswerHasMetaData, AnswerMetaDataRelation>());
            var mapper = new Mapper(config);
            List<AnswerMetaDataRelation> metaDataItems = new();
            foreach (AnswerHasMetaData data in answerHasMetaData)
            {
                metaDataItems.Add(mapper.Map<AnswerMetaDataRelation>(data));
            }
            return metaDataItems;
        }

        public async Task<IEnumerable<AnswerMetaDataRelation>> GetAnswerMetaDataRelationByAnswerIdAsync(long answerId)
        {
            var answerHasMetaData = await GetAnswerHasMetaDataByAnswerIdAsync(answerId);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.AnswerHasMetaData, AnswerMetaDataRelation>());
            var mapper = new Mapper(config);
            List<AnswerMetaDataRelation> metaDataItems = new();
            foreach (AnswerHasMetaData data in answerHasMetaData)
            {
                metaDataItems.Add(mapper.Map<AnswerMetaDataRelation>(data));
            }
            return metaDataItems;
        }

        public void CreateAnswerMetaDataRelation(AnswerMetaDataRelation answerMetaDataRelation) => throw new NotImplementedException();

        public void EditAnswerMetaDataRelation(AnswerMetaDataRelation answerMetaDataRelation) => throw new NotImplementedException();

        public void DeleteAnswerMetaDataRelation(AnswerMetaDataRelation answerMetaDataRelation) => throw new NotImplementedException();

        #endregion

        #region Implementation of IQuestionHasMetaDataDataService

        public async Task<IEnumerable<QuestionHasMetaData>> GetAllQuestionHasMetaDataAsync() => await _questionHasMetaDataRepository.GetAllAsync();

        public async Task<IEnumerable<QuestionHasMetaData>> GetQuestionHasMetaDataByQuestionIdAsync(long questionId) =>
            await _questionHasMetaDataRepository.FindByAsync(m => m.QuestionId == questionId);

        public void CreateQuestionHasMetaData(QuestionHasMetaData questionHasMeta) => _questionHasMetaDataRepository.Add(questionHasMeta);

        public void EditQuestionHasMetaData(QuestionHasMetaData questionHasMeta)
        {
            _questionHasMetaDataRepository.Edit(questionHasMeta);
            _questionHasMetaDataRepository.Commit();
        }

        public void DeleteQuestionHasMetaData(QuestionHasMetaData questionHasMeta)
        {
            _questionHasMetaDataRepository.Delete(questionHasMeta);
            _questionHasMetaDataRepository.Commit();
        }

        #endregion

        #region Implementation of IQuestionHasMetaDataDtoDataService

        public async Task<IEnumerable<QuestionMetaDataRelation>> GetAllQuestionMetaDataRelationsAsync()
        {
            var questionHasMetaData = await GetAllQuestionHasMetaDataAsync();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.QuestionHasMetaData, QuestionMetaDataRelation>());
            var mapper = new Mapper(config);
            List<QuestionMetaDataRelation> metaDataItems = new();
            foreach (QuestionHasMetaData data in questionHasMetaData)
            {
                metaDataItems.Add(mapper.Map<QuestionMetaDataRelation>(data));
            }
            return metaDataItems;
        }

        public async Task<IEnumerable<QuestionMetaDataRelation>> GetQuestionMetaDataRelationByQuestionIdAsync(long questionId)
        {
            var questionHasMetaData = await GetQuestionHasMetaDataByQuestionIdAsync(questionId);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.QuestionHasMetaData, QuestionMetaDataRelation>());
            var mapper = new Mapper(config);
            List<QuestionMetaDataRelation> metaDataItems = new();
            foreach (QuestionHasMetaData data in questionHasMetaData)
            {
                metaDataItems.Add(mapper.Map<QuestionMetaDataRelation>(data));
            }
            return metaDataItems;
        }

        public void CreateQuestionMetaDataRelation(QuestionMetaDataRelation questionMetaDataRelation) => throw new NotImplementedException();

        public void EditQuestionMetaDataRelation(QuestionMetaDataRelation questionMetaDataRelation) => throw new NotImplementedException();

        public void DeleteQuestionMetaDataRelation(QuestionMetaDataRelation questionMetaDataRelation) => throw new NotImplementedException();

        #endregion

        #region Implementation of IMetaDataHasMetaDataDataService

        public async Task<IEnumerable<MetaDataHasMetaData>> GetAllMetaDataHasMetaDataAsync() => await _metaDataHasMetaDataRepository.GetAllAsync();

        public void CreateMetaDataHasMetaData(MetaDataHasMetaData metaDataHasMetaData) => _metaDataHasMetaDataRepository.Add(metaDataHasMetaData);

        public void EditMetaDataHasMetaData(MetaDataHasMetaData metaDataHasMetaData)
        {
            _metaDataHasMetaDataRepository.Edit(metaDataHasMetaData);
            _metaDataHasMetaDataRepository.Commit();
        }

        public void DeleteMetaDataHasMetaData(MetaDataHasMetaData metaDataHasMetaData)
        {
            _metaDataHasMetaDataRepository.Delete(metaDataHasMetaData);
            _metaDataHasMetaDataRepository.Commit();
        }

        #endregion

        #region Implementation of IMetaDataHasMetaDataDtoDataService

        public async Task<IEnumerable<MetaDataMetaDataRelation>> GetAllMetaDataMetaDataRelationsAsync()
        {
            var metaDataHasMetaData = await GetAllMetaDataHasMetaDataAsync();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.MetaDataHasMetaData, MetaDataMetaDataRelation>());
            var mapper = new Mapper(config);
            List<MetaDataMetaDataRelation> metaDataItems = new();
            foreach (Models.MetaDataHasMetaData data in metaDataHasMetaData)
            {
                metaDataItems.Add(mapper.Map<MetaDataMetaDataRelation>(data));
            }
            return metaDataItems;
        }

        public void CreateMetaDataMetaDataRelation(MetaDataMetaDataRelation metaDataHasMetaData) => throw new NotImplementedException();

        public void EditMetaDataMetaDataRelation(MetaDataMetaDataRelation metaDataHasMetaData) => throw new NotImplementedException();

        public void DeleteMetaDataMetaDataRelation(MetaDataMetaDataRelation metaDataHasMetaData) => throw new NotImplementedException();

        #endregion

        #region Implementation of ICategoryDataService

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync() => await _categoryRepository.GetAllAsync();

        public async Task<Category> GetCategoryByIdAsync(long categoryId) =>
            await _categoryRepository.GetSingleAsync(categoryId);

        public void CreateCategory(Category category) => _categoryRepository.Add(category);

        public void EditCategory(Category category)
        {
            _categoryRepository.Edit(category);
            _categoryRepository.Commit();
        }

        public void DeleteCategory(Category category)
        {
            _categoryRepository.Delete(category);
            _categoryRepository.Commit();
        }

        #endregion

        #region Implementation of ICategoryDtoDataService

        public async Task<IEnumerable<AssessmentCategory>> GetAllAssessmentCategoriesAsync()
        {
            var categories = await GetAllCategoriesAsync();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Category, AssessmentCategory>());
            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            List<AssessmentCategory> assessmentCategories = new();
            foreach (var categoryItem in categories)
            {
                assessmentCategories.Add(mapper.Map<AssessmentCategory>(categoryItem));
            }
            return assessmentCategories;
        }

        public async Task<AssessmentCategory> GetAssessmentCategoryByIdAsync(long categoryId)
        {
            var cat = GetCategoryByIdAsync(categoryId);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Category, AssessmentCategory>());
            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            return mapper.Map<AssessmentCategory>(cat);
        }

        public void CreateAssessmentCategory(AssessmentCategory category) => throw new NotImplementedException();

        public void EditAssessmentCategory(AssessmentCategory category) => throw new NotImplementedException();

        public void DeleteAssessmentCategory(AssessmentCategory category) => throw new NotImplementedException();

        #endregion
    }
}
