// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace GrpcClient.Service
{
    public class AssessmentService : De.HDBW.Apollo.SharedContracts.Services.IAssessmentService
    {
        private SampleDataContext _data;
        private readonly string? _defaultLanguage = "de-DE";

        public AssessmentService()
        {
            _data = new SampleDataContext();
        }

        public Task<IEnumerable<AssessmentTile>> GetAssessmentTilesAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.So)!;
            var modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
            var so = Create(assessment, modules, string.Empty);

            so.AssessmentScores.Add(new AssessmentScore()
            {
                AssessmentId = so.AssessmentId,
                ModuleId = so.ModuleIds[0],
                Result = 50,
                Quantity = AssessmentScoreQuantity.Median,
            });

            so.AssessmentScores.Add(new AssessmentScore()
            {
                AssessmentId = so.AssessmentId,
                ModuleId = so.ModuleIds[1],
                Result = 10,
                Quantity = AssessmentScoreQuantity.Median,
            });
            so.AssessmentScores.Add(new AssessmentScore()
            {
                AssessmentId = so.AssessmentId,
                ModuleId = so.ModuleIds[2],
                Result = 30,
                Quantity = AssessmentScoreQuantity.Median,
            });
            so.AssessmentScores.Add(new AssessmentScore()
            {
                AssessmentId = so.AssessmentId,
                ModuleId = so.ModuleIds[3],
                Result = 40,
                Quantity = AssessmentScoreQuantity.Median,
            });
            so.AssessmentScores.Add(new AssessmentScore()
            {
                AssessmentId = so.AssessmentId,
                ModuleId = so.ModuleIds[4],
                Result = 80,
                Quantity = AssessmentScoreQuantity.Median,
            });

            assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.Gl)!;
            modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
            AssessmentTile gl = Create(assessment, modules, string.Empty);

            assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.Be)!;
            modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
            var be = Create(assessment, modules, string.Empty);

            assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.Sk)!;
            modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
            var sa = Create(assessment, modules, "Teste dein Wissen");

            assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.Ea)!;
            modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
            var sa1 = Create(assessment, modules, "Teste dein Wissen");

            return Task.FromResult<IEnumerable<AssessmentTile>>(new List<AssessmentTile>()
            {
                so,
                gl,
                be,
                sa,
                sa1,
            });
        }

        public Task<Module> GetModuleAsync(string moduleId, string? language, CancellationToken token)
        {
            var lang = language ?? _defaultLanguage;
            var modules = _data.Modules.Where(x => moduleId == x.ModuleId);
            var requestedModule = modules.First(x => x.Language == language);
            var module = new Module()
            {
                Title = requestedModule.Title,
                Subtitle = requestedModule.Subtitle,
                Description = requestedModule.Description,
                Language = requestedModule.Language,
                Type = requestedModule.Type,
                EstimateDuration = requestedModule.EstimateDuration,
                ModuleId = moduleId,
            };

            module.Languages.AddRange(modules.Select(x => x.Language).Distinct());
            return Task.FromResult(module);
        }

        public Task<IEnumerable<ModuleTile>> GetModuleTilesAsync(IEnumerable<string> moduleIds, CancellationToken token)
        {
            var result = new List<ModuleTile>();
            var modules = _data.Modules.Where(x => moduleIds.Contains(x.ModuleId) && x.Language == _defaultLanguage);

            foreach (var module in modules)
            {
                result.Add(new ModuleTile()
                {
                    Deleted = module.Deleted,
                    Type = module.Type,
                    ModuleId = module.ModuleId,
                    Title = module.Title,
                });
            }

            return Task.FromResult<IEnumerable<ModuleTile>>(result);
        }

        private static AssessmentTile Create(Assessment assessment, IEnumerable<Module> modules, string grouping)
        {
            var tile = new AssessmentTile()
            {
                Deleted = assessment.Deleted,
                Type = assessment.Type,
                Title = assessment.Title,
                Grouping = grouping,
            };

            foreach (var module in modules)
            {
                tile.ModuleIds.Add(module.ModuleId);
            }

            return tile;
        }

        public Task<AssessmentSession> CreateSessionAsync(string? moduleId, CancellationToken token) => throw new NotImplementedException();

        public Task<AssessmentSession> GetSessionAsync(object sessionId, string? language, CancellationToken token) => throw new NotImplementedException();
    }
}
