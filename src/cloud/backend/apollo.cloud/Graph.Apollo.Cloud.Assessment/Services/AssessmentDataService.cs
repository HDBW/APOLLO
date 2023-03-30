using System.Collections.Generic;
using AutoMapper;
using Invite.Apollo.App.Graph.Assessment.Models;
using Invite.Apollo.App.Graph.Assessment.Repository;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public class AssessmentDataService : IDataService
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
        private readonly ICategoryRecomendationRepository _categoryRecomendationRepository;


        public AssessmentDataService(ILogger<AssessmentDataService> logger, IAssessmentRepository assessmentRepository, IQuestionRepository questionRepository, IAnswerRepository answerRepository, IMetaDataRepository metaDataRepository, IAnswerHasMetaDataRepository answerHasMetaDataRepository, IQuestionHasMetaDataRepository questionHasMetaDataRepository, ICategoryRepository categoryRepository, IMetaDataHasMetaDataRepository metaDataHasMetaDataRepository, ICategoryRecomendationRepository categoryRecomendationRepository)
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
            _categoryRecomendationRepository = categoryRecomendationRepository;
        }

        
        //TODO: https://victorakpan.com/blog/ef-core-inmemory-and-dependency-injection-in-console-app

        #region Implementation of IAssessmentDataService

        public async Task<IEnumerable<Models.Assessment>> GetAllAssessmentsAsync() =>
            await _assessmentRepository.GetAllAsync();

        public async Task<Models.Assessment> GetAssessmentByIdAsync(long assessmentId)
        {
            return await _assessmentRepository.GetSingleAsync(assessmentId);
        }

        public async Task<IEnumerable<Models.Assessment>> GetAssessmentByType(AssessmentType type)
        {
            return await _assessmentRepository.FindByAsync(a => a.AssessmentType.Equals(type));

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
            foreach (var assessment in assessments ?? Enumerable.Empty<Models.Assessment>())
            {
                assessmentItems.Add(mapper.Map<AssessmentItem>(assessment));
            }

            return assessmentItems;
        }
        public async Task<IEnumerable<AssessmentItem>> GetAssessmentItemsByType(AssessmentType type)
        {
            var assessments = await GetAssessmentByType(type);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Assessment, AssessmentItem>());
            Mapper mapper = new(config);
            List<AssessmentItem> assessmentItems = new();
            foreach (var assessment in assessments ?? Enumerable.Empty<Models.Assessment>())
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
            foreach (var assessment in assessments ?? Enumerable.Empty<Models.Assessment>())
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
            foreach (var item in questions ?? Enumerable.Empty<Models.Question>())
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
            foreach (Answer answer in answers ?? Enumerable.Empty<Models.Answer>())
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
            foreach (MetaData data in metaData ?? Enumerable.Empty<Models.MetaData>())
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
            foreach (AnswerHasMetaData data in answerHasMetaData ?? Enumerable.Empty<Models.AnswerHasMetaData>())
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
            foreach (AnswerHasMetaData data in answerHasMetaData ?? Enumerable.Empty<Models.AnswerHasMetaData>())
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
            foreach (QuestionHasMetaData data in questionHasMetaData ?? Enumerable.Empty<Models.QuestionHasMetaData>())
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
            foreach (QuestionHasMetaData data in questionHasMetaData ?? Enumerable.Empty<Models.QuestionHasMetaData>())
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
            foreach (Models.MetaDataHasMetaData data in metaDataHasMetaData ?? Enumerable.Empty<Models.MetaDataHasMetaData>())
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
            foreach (var categoryItem in categories ?? Enumerable.Empty<Models.Category>())
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

        #region Implementation of ICategoryRecomendation
        public async Task<IEnumerable<CategoryRecomendation>> GetAllCategoryRecomendationsAsync()
        {
            return await _categoryRecomendationRepository.GetAllAsync();
        }

        public async Task<CategoryRecomendation> GetCategoryRecomendationByIdAsync(long categoryRecomendationId) =>
            await _categoryRecomendationRepository.GetSingleAsync(categoryRecomendationId);

        public async Task<CategoryRecomendation> GetCategoryRecomendationByCategoryIdAsync(long categoryId) =>
            await _categoryRecomendationRepository.GetSingleAsync(categoryId);

        public void CreateCategoryRecomendation(CategoryRecomendation categoryRecomendation) => throw new NotImplementedException();

        public void EditCategoryRecomendation(CategoryRecomendation categoryRecomendation) => throw new NotImplementedException();

        public void DeleteCategoryRecomendation(CategoryRecomendation categoryRecomendation) => throw new NotImplementedException();

        public async Task<IEnumerable<CategoryRecomendationItem>> GetAllCategoryRecomendationItemAsync()
        {
            var list = await GetAllCategoryRecomendationsAsync();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.CategoryRecomendation, CategoryRecomendationItem>());
            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            List<CategoryRecomendationItem> result = new();
            foreach (var item in list ?? Enumerable.Empty<Models.CategoryRecomendation>())
            {
                result.Add(mapper.Map<CategoryRecomendationItem>(item));
            }
            return result;
        }

        public async Task<CategoryRecomendationItem> GetCategoryRecomendationItemByIdAsync(long categoryRecomendationId)
        {
            var item = GetCategoryByIdAsync(categoryRecomendationId);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.CategoryRecomendation, CategoryRecomendationItem>());
            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            return mapper.Map<CategoryRecomendationItem>(item);
        }

        public void CreateCategoryRecomendationItem(CategoryRecomendationItem categoryRecomendationItem) => throw new NotImplementedException();

        public void EditCategoryRecomendationItem(CategoryRecomendationItem categoryRecomendationItem) => throw new NotImplementedException();

        public void DeleteCategoryRecomendationItem(CategoryRecomendationItem categoryRecomendationItem) => throw new NotImplementedException();
        #endregion


        #region December UseCase


        public IEnumerable<Models.Assessment> GetAssessmentsByUseCase(int useCase)
        {
            var list  = GetAllAssessmentsAsync().Result;
            var result =  list.Where(u => u.UseCaseId.Equals(useCase));
            return result;
        }

        public IEnumerable<Answer> GetAnswersByUseCase(int useCase)
        {
            var questions = GetQuestionsByUseCase(useCase);
            List<Answer> result = new();
            foreach (var question in questions ?? Enumerable.Empty<Models.Question>())
            {
                foreach (Answer answer in question.Answers)
                {
                    result.Add(answer);
                }
            }

            return result;
        }

        public IEnumerable<Question> GetQuestionsByUseCase(int useCase)
        {
            var assessments = GetAssessmentsByUseCase(useCase);
            List<Question> result = new();
            foreach (var assessment in assessments ?? Enumerable.Empty<Models.Assessment>())
            {
                foreach (Question assessmentQuestion in assessment.Questions ?? Enumerable.Empty<Models.Question>())
                {
                    result.Add(assessmentQuestion);
                }
            }
            return result;
        }

        public IEnumerable<Category> GetCategoryByUseCase(int useCase)
        {
            var questions = GetQuestionsByUseCase(useCase);
            List<Category> result = new();
            foreach (var question in questions ?? Enumerable.Empty<Question>())
            {
                result.Add(question.Category);
            }
            return result.Distinct();


        }

        public IEnumerable<CategoryRecomendation> GetCategoryRecomendationsByUseCase(int useCase)
        {
            var categories = GetCategoryByUseCase(useCase);
            List<CategoryRecomendation> result = new();
            foreach (var category in categories ?? Enumerable.Empty<Category>())
            {
                result.AddRange(_categoryRecomendationRepository.FindBy(a => a.CategoryId.Equals(category.Id)));
                
            }

            return result;
        }

        public IEnumerable<MetaData> GetMetaDataByUseCase(int useCase)
        {
            List<MetaData> result = new();
            var answers = GetAnswersByUseCase(useCase);
            var questions = GetQuestionsByUseCase(useCase);

            foreach (Answer answer in answers ?? Enumerable.Empty<Models.Answer>())
            {
                foreach (AnswerHasMetaData answerAnswerHasMetaData in answer.AnswerHasMetaDatas ?? Enumerable.Empty<AnswerHasMetaData>())
                {
                    result.Add(answerAnswerHasMetaData.MetaData);

                }
            }
            foreach (Question question in questions ?? Enumerable.Empty<Models.Question>())
            {
                foreach (QuestionHasMetaData questionHasMetaData in question.QuestionHasMetaDatas ?? Enumerable.Empty<QuestionHasMetaData>())
                {
                    result.Add(questionHasMetaData.MetaData);

                }
            }

            return result;

        }

        public IEnumerable<MetaDataHasMetaData> GetMetaDataHasMetaDataByUseCase(int useCase)
        {
            if (_metaDataHasMetaDataRepository.GetAll().Count() == 0)
                return new List<MetaDataHasMetaData>();


            List<MetaDataHasMetaData> result = new();
            var questionHasMetaData = GetQuestionHasMetaDataByUseCase(useCase);
            var answerHasMetaData = GetAnswerHasMetaDataByUseCase(useCase);

            foreach (QuestionHasMetaData qhmd in questionHasMetaData ?? Enumerable.Empty<Models.QuestionHasMetaData>())
            {
                if (qhmd.MetaData.TargetMetaDataHasMetaDatas != null)
                {
                    foreach (var target in qhmd.MetaData.TargetMetaDataHasMetaDatas)
                    {
                        result.Add(target);
                    }
                }

                if (qhmd.MetaData.SourceQuestionHasMetaDatas != null)
                {
                    foreach (var source in qhmd.MetaData.SourceQuestionHasMetaDatas)
                    {
                        result.Add(source);
                    }
                }

            }

            foreach (AnswerHasMetaData ahmd in answerHasMetaData ?? Enumerable.Empty<Models.AnswerHasMetaData>())
            {
                foreach (var target in ahmd.MetaData.TargetMetaDataHasMetaDatas ?? Enumerable.Empty<Models.MetaDataHasMetaData>())
                {
                    result.Add(target);
                }

                foreach (var source in ahmd.MetaData.SourceQuestionHasMetaDatas ?? Enumerable.Empty<Models.MetaDataHasMetaData>())
                {
                    result.Add(source);
                }
            }

            return result.Distinct();
        }

        public IEnumerable<QuestionHasMetaData> GetQuestionHasMetaDataByUseCase(int useCase)
        {
            List<QuestionHasMetaData> result = new();
            
            var questions = GetQuestionsByUseCase(useCase);

            

            foreach (Question question in questions ?? Enumerable.Empty<Models.Question>())
            {
                foreach (QuestionHasMetaData questionHasMetaData in question.QuestionHasMetaDatas ?? Enumerable.Empty<Models.QuestionHasMetaData>())
                {
                    result.Add(questionHasMetaData);

                }
            }

            return result;

        }

        public IEnumerable<AnswerHasMetaData> GetAnswerHasMetaDataByUseCase(int useCase)
        {

            List<AnswerHasMetaData> result = new();
            var answers = GetAnswersByUseCase(useCase);
            foreach (Answer answer in answers ?? Enumerable.Empty<Models.Answer>())
            {
                foreach (var answerAnswerHasMetaData in answer.AnswerHasMetaDatas ?? Enumerable.Empty<AnswerHasMetaData>())
                {
                    result.Add(answerAnswerHasMetaData);
                }
            }

            return result;
        }

        public IEnumerable<AssessmentItem> GetAssessmentItemsByUseCase(int useCase)
        {
            var list = GetAssessmentsByUseCase(useCase);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Assessment, AssessmentItem>());
            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            List<AssessmentItem> result = new();
            foreach (var item in list ?? Enumerable.Empty<Models.Assessment>())
            {
                result.Add(mapper.Map<AssessmentItem>(item));
            }
            return result;
        }

        public IEnumerable<AnswerItem> GetAnswersItemsByUseCase(int useCase)
        {
            var list = GetAnswersByUseCase(useCase);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Answer, AnswerItem>()
                .ForMember(dest => dest.AnswerType, opt => opt.MapFrom<AnswerTypeResolver>()));

            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            List<AnswerItem> result = new();
            foreach (var item in list ?? Enumerable.Empty<Models.Answer>())
            {
                result.Add(mapper.Map<AnswerItem>(item));
            }
            return result;
        }

        public IEnumerable<QuestionItem> GetQuestionItemsByUseCase(int useCase)
        {
            var list = GetQuestionsByUseCase(useCase);
            //var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Question, QuestionItem>());
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Models.Question, QuestionItem>()
                    .ForMember(dest => dest.AnswerLayout, opt => opt.MapFrom<AnswerLayoutResolver>())
                    .ForMember(dest => dest.QuestionLayout, opt => opt.MapFrom<QuestionLayoutResolver>())
                    .ForMember(dest => dest.Interaction, opt => opt.MapFrom<InteractionTypeResolver>())
            );

            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            List<QuestionItem> result = new();
            foreach (var item in list ?? Enumerable.Empty<Models.Question>())
            {
                result.Add(mapper.Map<QuestionItem>(item));
            }
            return result; 
        }

        public IEnumerable<AssessmentCategory> GetAssessmentCategoriesByUseCase(int useCase)
        {
            var list = GetCategoryByUseCase(useCase);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Category, AssessmentCategory>());
            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            List<AssessmentCategory> result = new();
            foreach (var item in list ?? Enumerable.Empty<Models.Category>())
            {
                result.Add(mapper.Map<AssessmentCategory>(item));
            }
            return result;
        }

        public IEnumerable<CategoryRecomendationItem> GetCategoryRecomendationItemsByUseCase(int useCase)
        {
            var list = GetCategoryRecomendationsByUseCase(useCase);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.CategoryRecomendation, CategoryRecomendationItem>());
            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            List<CategoryRecomendationItem> result = new();
            foreach (var item in list ?? Enumerable.Empty<Models.CategoryRecomendation>())
            {
                result.Add(mapper.Map<CategoryRecomendationItem>(item));
            }
            return result;
        }

        public IEnumerable<MetaDataItem> GetMetaDataItemsByUseCase(int useCase)
        {
            var list = GetMetaDataByUseCase(useCase);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.MetaData, MetaDataItem>());
            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            List<MetaDataItem> result = new();
            foreach (var item in list ?? Enumerable.Empty<Models.MetaData>())
            {
                result.Add(mapper.Map<MetaDataItem>(item));
            }
            return result;
        }

        public IEnumerable<MetaDataMetaDataRelation> GetMetaDataMetaDataRelationsByUseCase(int useCase)
        {
            var list = GetMetaDataHasMetaDataByUseCase(useCase);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.MetaDataHasMetaData, MetaDataMetaDataRelation>());
            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            List<MetaDataMetaDataRelation> result = new();
            foreach (var item in list ?? Enumerable.Empty<Models.MetaDataHasMetaData>())
            {
                result.Add(mapper.Map<MetaDataMetaDataRelation>(item));
            }
            return result;
        }

        public IEnumerable<QuestionMetaDataRelation> GetQuestionMetaDataRelationByUseCase(int useCase)
        {
            var list = GetQuestionHasMetaDataByUseCase(useCase);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.QuestionHasMetaData, QuestionMetaDataRelation>());
            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            List<QuestionMetaDataRelation> result = new();
            foreach (var item in list ?? Enumerable.Empty<Models.QuestionHasMetaData>())
            {
                result.Add(mapper.Map<QuestionMetaDataRelation>(item));
            }
            return result;
        }

        public IEnumerable<AnswerMetaDataRelation> GetAnswerMetaDataRelationByUseCase(int useCase)
        {
            var list = GetAnswerHasMetaDataByUseCase(useCase);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.AnswerHasMetaData, AnswerMetaDataRelation>());
            //TODO: Overwrite shit
            var mapper = new Mapper(config);
            List<AnswerMetaDataRelation> result = new();
            foreach (var item in list ?? Enumerable.Empty<Models.AnswerHasMetaData>())
            {
                result.Add(mapper.Map<AnswerMetaDataRelation>(item));
            }
            return result;
        }

        #endregion


    }
}
