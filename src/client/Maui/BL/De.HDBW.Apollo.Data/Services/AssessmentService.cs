// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text.Json.Nodes;
using De.HDBW.Apollo.Data.SampleData.Strings;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessments;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly string _defaultLanguage = "de-DE";
        private SampleDataContext _data;
        private string? _lastDeletedSessionId;

        public AssessmentService(
            ILocalAssessmentSessionRepository sessionRepository,
            IRawDataCacheRepository rawDataCacheRepository,
            ILogger<AssessmentService> logger)
        {
            ArgumentNullException.ThrowIfNull(sessionRepository);
            ArgumentNullException.ThrowIfNull(rawDataCacheRepository);
            ArgumentNullException.ThrowIfNull(logger);
            Logger = logger;
            SessionRepository = sessionRepository;
            RawDataCacheRepository = rawDataCacheRepository;
            _data = new SampleDataContext();
        }

        private ILogger Logger { get; }

        private ILocalAssessmentSessionRepository SessionRepository { get; }

        private IRawDataCacheRepository RawDataCacheRepository { get; }

        public Task<IEnumerable<AssessmentTile>> GetAssessmentTilesAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.So) !;
            var modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
            var so = Create(assessment, modules, string.Empty);
            so.ModuleScores.Add(new ModuleScore()
            {
                AssessmentId = so.AssessmentId,
                ModuleId = so.ModuleIds[0],
                Result = 50,
                Quantity = AssessmentScoreQuantity.Median,
            });

            so.ModuleScores.Add(new ModuleScore()
            {
                AssessmentId = so.AssessmentId,
                ModuleId = so.ModuleIds[1],
                Result = 10,
                Quantity = AssessmentScoreQuantity.Median,
            });
            so.ModuleScores.Add(new ModuleScore()
            {
                AssessmentId = so.AssessmentId,
                ModuleId = so.ModuleIds[2],
                Result = 30,
                Quantity = AssessmentScoreQuantity.Median,
            });
            so.ModuleScores.Add(new ModuleScore()
            {
                AssessmentId = so.AssessmentId,
                ModuleId = so.ModuleIds[3],
                Result = 40,
                Quantity = AssessmentScoreQuantity.Median,
            });
            so.ModuleScores.Add(new ModuleScore()
            {
                AssessmentId = so.AssessmentId,
                ModuleId = so.ModuleIds[4],
                Result = 80,
                Quantity = AssessmentScoreQuantity.Median,
            });

            assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.Gl) !;
            modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
            AssessmentTile gl = Create(assessment, modules, string.Empty);

            assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.Be) !;
            modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
            var be = Create(assessment, modules, string.Empty);

            assessment = _data.Assessments.Skip(1).FirstOrDefault(x => x.Type == AssessmentType.Sk) !;
            modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
            var sa = Create(assessment, modules, "Teste dein Wissen");

            assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.Ea) !;
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

        public async Task<Module> GetModuleAsync(string moduleId, string? language, CancellationToken token)
        {
            var lang = language ?? _defaultLanguage;
            var modules = _data.Modules.Where(x => moduleId == x.ModuleId);
            var requestedModule = modules.First(x => x.Language == lang);
            var localSession = await SessionRepository.GetItemByAssessmentIdAndModuleIdAsync(requestedModule.AssessmentId, requestedModule.ModuleId, token).ConfigureAwait(false);
            var rawDatas = _data.RawDatas.Where(x => x.ModuleId == moduleId && x.Language == language).ToList();
            var rawDataIds = localSession?.RawDataOrder?.Split(";").ToList() ?? new List<string>();
            var offset = localSession?.CurrentRawDataId != null ? rawDataIds.IndexOf(localSession.CurrentRawDataId) : 0;
            var waslastCanceledModule = _lastDeletedSessionId == moduleId && requestedModule.Type != AssessmentType.Be;
            _lastDeletedSessionId = null;
            var rawData = rawDatas.FirstOrDefault();
            string localizedJobName = string.Empty;
            string moduleName = string.Empty;
            if (rawData != null)
            {
                var node = JsonObject.Parse(rawDatas.First().Data);
                moduleName = node?[nameof(SharedContracts.RawData.module)]?.GetValue<string>()?.Trim() ?? string.Empty;
                var job = node?[nameof(SharedContracts.RawData.job)]?.GetValue<string>()?.Trim() ?? string.Empty;
                var schwerPunkt = node?[nameof(SharedContracts.RawData.schwerpunkt)]?.GetValue<string>()?.Trim() ?? string.Empty;
                if (schwerPunkt == "-")
                {
                    schwerPunkt = string.Empty;
                }

                var parts = new List<string>() { job, schwerPunkt }.Where(x => !string.IsNullOrWhiteSpace(x));
                localizedJobName = string.Join(" - ", parts);
            }

            var module = new Module()
            {
                Title = requestedModule.Title,
                JobId = requestedModule.JobId,
                LocalizedJobName = localizedJobName,
                Language = requestedModule.Language,
                Type = requestedModule.Type,
                EstimateDuration = requestedModule.EstimateDuration,
                ModuleId = moduleId,
                AssessmentId = requestedModule.AssessmentId,
                SessionId = localSession?.SessionId,
                RawDataCount = rawDatas.Count,
                AnswerCount = offset,
                Repeatable = waslastCanceledModule ? 1 : 0,
            };
            var escoId = string.Empty;
            var quantity = string.Empty;
            switch (module.Type)
            {
                case AssessmentType.Ea:
                case AssessmentType.Sk:
                    module.Subtitle = string.Format(GetText($"{module.Type}_{escoId}_{quantity}_{nameof(Module.Subtitle)}_{language}"), localizedJobName);
                    module.Description = GetText($"{module.Type}_{escoId}_{quantity}_{nameof(Module.Description)}_{language}");
                    break;
                case AssessmentType.So:
                    module.Subtitle = moduleName;
                    module.Description = string.Format(GetText($"{module.Type}_{escoId}_{quantity}_{nameof(Module.Description)}_{language}"), module.Title);
                    break;
                default:
                    module.Subtitle = GetText($"{module.Type}_{escoId}_{quantity}_{nameof(Module.Subtitle)}_{language}");
                    module.Description = GetText($"{module.Type}_{escoId}_{quantity}_{nameof(Module.Description)}_{language}");
                    break;
            }

            module.Languages.AddRange(modules.Select(x => x.Language).Distinct());
            await GenerateScoresAsync(module, rawDatas, string.Empty, lang).ConfigureAwait(false);
            return module;
        }

        public async Task<IEnumerable<ModuleTile>> GetModuleTilesAsync(IEnumerable<string> moduleIds, CancellationToken token)
        {
            var result = new List<ModuleTile>();
            var modules = _data.Modules.Where(x => moduleIds.Contains(x.ModuleId) && x.Language == _defaultLanguage);

            foreach (var module in modules)
            {
                var localSession = await SessionRepository.GetItemByAssessmentIdAndModuleIdAsync(module.AssessmentId, module.ModuleId, token).ConfigureAwait(false);
                var rawDataIds = localSession?.RawDataOrder?.Split(";").ToList() ?? new List<string>();
                var offset = localSession?.CurrentRawDataId != null ? rawDataIds.IndexOf(localSession.CurrentRawDataId) : 0;
                result.Add(new ModuleTile()
                {
                    Deleted = module.Deleted,
                    Type = module.Type,
                    ModuleId = module.ModuleId,
                    Title = module.Title,
                    SessionId = localSession?.SessionId,
                    RawDataCount = rawDataIds.Count,
                    AnswerCount = offset,
                });
            }

            return result;
        }

        public async Task<LocalAssessmentSession?> CreateSessionAsync(string moduleId, string assessmentId, string? language, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            LocalAssessmentSession? localSession;
            try
            {
                language = language ?? "de-DE";
                var session = new AssessmentSession();
                session.SessionId = Guid.NewGuid().ToString();
                session.ModuleId = moduleId;
                session.AssessmentId = assessmentId;
                session.RawDatas.AddRange(_data.RawDatas.Where(x => x.ModuleId == moduleId && x.Language == language));
                session.CurrentRawDataId = session.RawDatas.First().RawDataId;

                var items = new List<CachedRawData>();
                for (var i = 1; i <= session.RawDatas.Count; i++)
                {
                    var data = session.RawDatas[i - 1];
                    items.Add(new CachedRawData() { Id = i, RawDataId = data.RawDataId, SessionId = session.SessionId, AssesmentId = data.AssesmentId, ModuleId = data.ModuleId, Data = data.Data });
                }

                await RawDataCacheRepository.ResetItemsAsync(items, CancellationToken.None).ConfigureAwait(false);

                localSession = new LocalAssessmentSession()
                {
                    SessionId = session.SessionId,
                    AssessmentId = session.AssessmentId,
                    ModuleId = moduleId,
                    RawDataOrder = string.Join(";", session.RawDatas.Select(x => x.RawDataId)),
                    CurrentRawDataId = session.CurrentRawDataId,
                };

                if (!await RawDataCacheRepository.ResetItemsAsync(items, CancellationToken.None).ConfigureAwait(false))
                {
                    Logger.LogError($"Unable to cache raw data in {nameof(CreateSessionAsync)} in {GetType().Name}.");
                    throw new NotSupportedException();
                }

                if (!await SessionRepository.AddItemAsync(localSession, CancellationToken.None).ConfigureAwait(false))
                {
                    Logger.LogError($"Unable to store local session in {nameof(CreateSessionAsync)} in {GetType().Name}.");
                    throw new NotSupportedException();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error in {nameof(CreateSessionAsync)} in {GetType().Name}.");
                localSession = null;
            }

            return localSession;
        }

        public async Task<LocalAssessmentSession?> GetSessionAsync(string sessionId, string? language, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            LocalAssessmentSession? localSession;
            try
            {
                localSession = await SessionRepository.GetItemBySessionIdAsync(sessionId, token).ConfigureAwait(false);
                if (localSession == null)
                {
                    return null;
                }

                language = language ?? "de-DE";
                var session = new AssessmentSession()
                {
                    AssessmentId = localSession.AssessmentId,
                    CurrentRawDataId = localSession.CurrentRawDataId ?? string.Empty,
                    SessionId = localSession.SessionId ?? string.Empty,
                    ModuleId = localSession.ModuleId,
                };

                session.RawDatas.AddRange(_data.RawDatas.Where(x => x.ModuleId == localSession.ModuleId && x.Language == language));
                var items = new List<CachedRawData>();
                for (var i = 1; i <= session.RawDatas.Count; i++)
                {
                    var data = session.RawDatas[i - 1];
                    items.Add(new CachedRawData() { Id = i, RawDataId = data.RawDataId, SessionId = session.SessionId, AssesmentId = data.AssesmentId, ModuleId = data.ModuleId, Data = data.Data });
                }

                if (!await RawDataCacheRepository.ResetItemsAsync(items, CancellationToken.None).ConfigureAwait(false))
                {
                    Logger.LogError($"Unable to cache raw data in {nameof(GetSessionAsync)} in {GetType().Name}.");
                    throw new NotSupportedException();
                }

                localSession.RawDataOrder = string.Join(";", session.RawDatas.Select(x => x.RawDataId));
                if (!await SessionRepository.UpdateItemAsync(localSession, token).ConfigureAwait(false))
                {
                    Logger.LogError($"Unable to store local session in {nameof(GetSessionAsync)} in {GetType().Name}.");
                    throw new NotSupportedException();
                }

                return localSession;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error in {nameof(GetSessionAsync)} in {GetType().Name}.");
                localSession = null;
            }

            return localSession;
        }

        public async Task<RawData?> AnswerAsync(string sessionId, string rawDataId, double? score, CancellationToken token)
        {
            RawData? nextRawData = null;
            token.ThrowIfCancellationRequested();
            try
            {
                var answer = new Answer();
                answer.SessionId = sessionId;
                answer.RawDataId = rawDataId;
                answer.Score = score ?? -1d;
                var localSession = await SessionRepository.GetItemBySessionIdAsync(sessionId, token).ConfigureAwait(false);
                if (localSession?.RawDataOrder == null || localSession?.CurrentRawDataId == null)
                {
                    return null;
                }

                var rawDataIds = localSession.RawDataOrder.Split(";").ToList();
                var nextOffset = rawDataIds.IndexOf(localSession.CurrentRawDataId) + 1;

                if (nextOffset >= rawDataIds.Count)
                {
                    return null;
                }

                var nextRawDataId = rawDataIds[nextOffset];
                nextRawData = _data.RawDatas.FirstOrDefault(x => x.RawDataId == nextRawDataId);
                if (nextRawData != null)
                {
                    localSession.CurrentRawDataId = nextRawDataId;
                    await SessionRepository.UpdateItemAsync(localSession, CancellationToken.None).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error in {nameof(AnswerAsync)} in {GetType().Name}.");
                nextRawData = null;
            }

            return nextRawData;
        }

        public async Task<bool> UpdateSessionAsync(LocalAssessmentSession session, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await SessionRepository.UpdateItemAsync(session, token).ConfigureAwait(false);
        }

        public async Task<bool> CancelSessionAsync(string sessionId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var session = await GetSessionAsync(sessionId, null, token).ConfigureAwait(false);
            _lastDeletedSessionId = session?.ModuleId;
            return await SessionRepository.RemoveItemBySessionIdAsync(sessionId, token).ConfigureAwait(false);
        }

        public Task<bool> FinishSessionAsync(string sessionId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return SessionRepository.RemoveItemBySessionIdAsync(sessionId, token);
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

        private Task GenerateScoresAsync(Module module, List<RawData> rawDatas, string escoId, string language, AssessmentScoreQuantity quantity = AssessmentScoreQuantity.Over, double result = 1)
        {
            ModuleScore? score = null;
            string pattern = $"{module.Type}_{escoId}_{quantity}_{nameof(ModuleScore.ResultDescription)}_{language}";
            var segments = new List<string>();
            switch (module.Type)
            {
                case AssessmentType.Gl:
                    score = new ModuleScore();
                    score.Quantity = quantity;
                    int correctAnswerCount = 0, questionCount = 0;
                    var text = string.Format(GetText(pattern), correctAnswerCount, questionCount);
                    score.ResultDescription = string.IsNullOrWhiteSpace(text) ? pattern : text;
                    score.AssessmentId = module.AssessmentId;
                    score.ModuleId = module.ModuleId;
                    score.Result = result;
                    score.Segment = string.Empty;
                    module.SegmentScores.Add(score);
                    module.ModuleScore = score;
                    break;
                case AssessmentType.Sk:
                    foreach (var rawData in rawDatas)
                    {
                        var node = JsonObject.Parse(rawData.Data);
                        var segment = node?[nameof(SharedContracts.RawData.handlungsfeld)]?.GetValue<string>();
                        if (string.IsNullOrWhiteSpace(segment))
                        {
                            continue;
                        }

                        segments.Add(segment);
                    }

                    segments = segments.Distinct().ToList();
                    foreach (var segment in segments)
                    {
                        var name = segment.Split("-").Last().Trim();
                        score = new ModuleScore();
                        score.Quantity = quantity;
                        score.ResultDescription = GetText(pattern);
                        score.AssessmentId = module.AssessmentId;
                        score.ModuleId = module.ModuleId;
                        score.Result = result;
                        score.Segment = name;
                        module.SegmentScores.Add(score);
                        module.ModuleScore = new ModuleScore()
                        {
                            Quantity = AssessmentScoreQuantity.Median,
                            Result = 0.5,
                        };
                    }

                    break;
                case AssessmentType.Ea:
                    foreach (var rawData in rawDatas)
                    {
                        var node = JsonObject.Parse(rawData.Data);
                        var segment = node?[nameof(SharedContracts.RawData.handlungsfeld)]?.GetValue<string>();
                        if (string.IsNullOrWhiteSpace(segment))
                        {
                            continue;
                        }

                        segments.Add(segment);
                    }

                    segments = segments.Distinct().ToList();
                    foreach (var segment in segments)
                    {
                        var name = segment.Split("-").Last().Trim();
                        score = new ModuleScore();
                        score.Quantity = quantity;
                        score.ResultDescription = GetText(pattern);
                        score.AssessmentId = module.AssessmentId;
                        score.ModuleId = module.ModuleId;
                        score.Result = result;
                        score.Segment = name;
                        module.SegmentScores.Add(score);
                        module.ModuleScore = new ModuleScore()
                        {
                            Quantity = AssessmentScoreQuantity.Median,
                            Result = 0.5,
                        };
                    }

                    break;
                case AssessmentType.So:
                    foreach (var rawData in rawDatas)
                    {
                        var node = JsonObject.Parse(rawData.Data);
                        var segment = node?[nameof(SharedContracts.RawData.esco)]?.GetValue<string>();
                        if (string.IsNullOrWhiteSpace(segment))
                        {
                            continue;
                        }

                        segments.Add(segment);
                    }

                    segments = segments.Distinct().ToList();
                    foreach (var segment in segments)
                    {
                        var name = segment.Split("-").Last().Trim();
                        score = new ModuleScore();
                        score.Quantity = quantity;
                        score.ResultDescription = GetText(pattern);
                        score.AssessmentId = module.AssessmentId;
                        score.ModuleId = module.ModuleId;
                        score.Result = result;
                        score.Segment = name;
                        module.SegmentScores.Add(score);
                        module.ModuleScore = new ModuleScore()
                        {
                            Quantity = AssessmentScoreQuantity.Median,
                            Result = 0.5,
                        };
                    }

                    break;
            }

            return Task.CompletedTask;
        }

        private string GetText(string name)
        {
            return Resources.ResourceManager.GetString(name) ?? name;
        }
    }
}
