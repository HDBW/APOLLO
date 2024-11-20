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
        private Random _random = new Random(Guid.NewGuid().GetHashCode());

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

        public Task<IEnumerable<AssessmentTile>> GetAssessmentTilesAsync(long? jobId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            IEnumerable<Invite.Apollo.App.Graph.Common.Models.Assessments.Module> modules = new List<Invite.Apollo.App.Graph.Common.Models.Assessments.Module>();
            if (jobId == null)
            {
                var assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.So)!;
                modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
                var so = Create(assessment, modules, string.Empty);
                so.ModuleScores.Add(new ModuleScore()
                {
                    AssessmentId = so.AssessmentId,
                    ModuleId = so.ModuleIds[0],
                    Result = 0.50,
                    Quantity = AssessmentScoreQuantity.Median,
                });

                so.ModuleScores.Add(new ModuleScore()
                {
                    AssessmentId = so.AssessmentId,
                    ModuleId = so.ModuleIds[1],
                    Result = 0.10,
                    Quantity = AssessmentScoreQuantity.Median,
                });
                so.ModuleScores.Add(new ModuleScore()
                {
                    AssessmentId = so.AssessmentId,
                    ModuleId = so.ModuleIds[2],
                    Result = 0.30,
                    Quantity = AssessmentScoreQuantity.Median,
                });
                so.ModuleScores.Add(new ModuleScore()
                {
                    AssessmentId = so.AssessmentId,
                    ModuleId = so.ModuleIds[3],
                    Result = 0.40,
                    Quantity = AssessmentScoreQuantity.Median,
                });
                so.ModuleScores.Add(new ModuleScore()
                {
                    AssessmentId = so.AssessmentId,
                    ModuleId = so.ModuleIds[4],
                    Result = 0.80,
                    Quantity = AssessmentScoreQuantity.Median,
                });

                assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.Gl)!;
                modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
                AssessmentTile gl = Create(assessment, modules, string.Empty);

                assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.Be)!;
                modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
                var be = Create(assessment, modules, string.Empty);

                assessment = _data.Assessments.Skip(1).FirstOrDefault(x => x.Type == AssessmentType.Sk)!;
                modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
                var sa = Create(assessment, modules, Resources.TestYourKnowledge);

                assessment = _data.Assessments.FirstOrDefault(x => x.Type == AssessmentType.Ea)!;
                modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
                var sa1 = Create(assessment, modules, Resources.TestYourKnowledge);

                return Task.FromResult<IEnumerable<AssessmentTile>>(new List<AssessmentTile>()
                {
                    so,
                    gl,
                    be,
                    sa,
                    sa1,
                });
            }

            var assessments = _data.Assessments.Where(x => x.JobId == jobId);
            var tiles = new List<AssessmentTile>();
            foreach (var assessment in assessments)
            {
                modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
                tiles.Add(Create(assessment, modules, Resources.TestYourKnowledge));
            }

            return Task.FromResult<IEnumerable<AssessmentTile>>(tiles);
        }

        public Task<AssessmentTile?> GetAssessmentTileAsync(string? assessmentId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var assessment = _data.Assessments.FirstOrDefault(x => x.AssessmentId == assessmentId)!;
            var modules = _data.Modules.Where(x => x.AssessmentId == assessment.AssessmentId && x.Language == _defaultLanguage);
            var tile = Create(assessment, modules, string.Empty);
            foreach (var module in modules)
            {
                tile.ModuleScores.Add(new ModuleScore()
                {
                    AssessmentId = tile.AssessmentId,
                    ModuleId = tile.ModuleIds[0],
                    Result = _random.Next(100) * 0.01,
                    Quantity = AssessmentScoreQuantity.Median,
                });
            }

            return Task.FromResult<AssessmentTile?>(tile);
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
                Title = moduleName ?? requestedModule.Title,
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
                MemberOnly = requestedModule.MemberOnly,
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
            await GenerateScoresAsync(module, rawDatas, lang).ConfigureAwait(false);
            return module;
        }

        public async Task<IEnumerable<ModuleTile>> GetModuleTilesAsync(IEnumerable<string> moduleIds, CancellationToken token)
        {
            var result = new List<ModuleTile>();
            var modules = _data.Modules.Where(x => moduleIds.Contains(x.ModuleId) && x.Language == _defaultLanguage);
            var language = "de-DE";
            foreach (var module in modules)
            {
                var moduleId = module.ModuleId;
                var localSession = await SessionRepository.GetItemByAssessmentIdAndModuleIdAsync(module.AssessmentId, module.ModuleId, token).ConfigureAwait(false);
                var rawDataIds = localSession?.RawDataOrder?.Split(";").ToList() ?? new List<string>();
                var offset = localSession?.CurrentRawDataId != null ? rawDataIds.IndexOf(localSession.CurrentRawDataId) : 0;
                var rawDatas = _data.RawDatas.Where(x => x.ModuleId == moduleId && x.Language == language).ToList();
                await GenerateScoresAsync(module, rawDatas, language).ConfigureAwait(false);
                result.Add(new ModuleTile()
                {
                    Deleted = module.Deleted,
                    Type = module.Type,
                    AssessmentId = module.AssessmentId,
                    ModuleId = module.ModuleId,
                    Title = module.Title,
                    SessionId = localSession?.SessionId,
                    RawDataCount = rawDataIds.Count,
                    AnswerCount = offset,
                    ModuleScore = module.ModuleScore,
                    MemberOnly = module.MemberOnly,
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

        public Task<RawData?> AnswerAsync(string sessionId, string rawDataId, double? score, CancellationToken token)
        {
            return AnswerAsync(sessionId, rawDataId, new List<double?>() { score }, token);
        }

        public async Task<RawData?> AnswerAsync(string sessionId, string rawDataId, IEnumerable<double?> scores, CancellationToken token)
        {
            RawData? nextRawData = null;
            token.ThrowIfCancellationRequested();
            try
            {
                foreach (var score in scores)
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

                    rawDataId = nextRawDataId;
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

        public Task<IEnumerable<Job>> GetJobsAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult<IEnumerable<Job>>(_data.Jobs);
        }

        private static AssessmentTile Create(Assessment assessment, IEnumerable<Module> modules, string grouping)
        {
            var tile = new AssessmentTile()
            {
                Deleted = assessment.Deleted,
                Type = assessment.Type,
                Title = assessment.Title,
                Grouping = grouping,
                MemberOnly = assessment.MemberOnly,
            };

            foreach (var module in modules)
            {
                tile.ModuleIds.Add(module.ModuleId);
            }

            return tile;
        }

        private Task GenerateScoresAsync(Module module, List<RawData> rawDatas, string language, AssessmentScoreQuantity quantity = AssessmentScoreQuantity.Over, double result = 1)
        {
            SegmentScore? segmentScore = null;
            string pattern = $"{module.Type}_{string.Empty}_{quantity}_{nameof(ModuleScore.ResultDescription)}_{language}";
            var segments = new List<(string SegmentName, string EscoId)>();
            switch (module.Type)
            {
                case AssessmentType.Gl:
                    segmentScore = new SegmentScore();
                    segmentScore.Quantity = quantity;
                    int correctAnswerCount = module.AnswerCount ?? 0, questionCount = module.RawDataCount ?? 0;
                    var text = string.Format(GetText(pattern), correctAnswerCount, questionCount);
                    segmentScore.ResultDescription = string.IsNullOrWhiteSpace(text) ? pattern : text;
                    segmentScore.AssessmentId = module.AssessmentId;
                    segmentScore.ModuleId = module.ModuleId;
                    segmentScore.Result = result;
                    segmentScore.Segment = module.Title;
                    module.SegmentScores.Add(segmentScore);
                    module.ModuleScore = new ModuleScore()
                    {
                        Segment = segmentScore.Segment,
                        AssessmentId = segmentScore.AssessmentId,
                        ModuleId = segmentScore.ModuleId,
                        ProfileId = segmentScore.ProfileId,
                        Quantity = segmentScore.Quantity,
                        Result = segmentScore.Result,
                        ResultDescription = segmentScore.ResultDescription,
                    };
                    break;
                case AssessmentType.Sk:
                    foreach (var rawData in rawDatas)
                    {
                        var node = JsonObject.Parse(rawData.Data);
                        var segment = GetSegmentName(node, nameof(SharedContracts.RawData.handlungsfeld));
                        var escoId = GetEscoId(node);
                        if (string.IsNullOrWhiteSpace(segment))
                        {
                            continue;
                        }

                        segments.Add((segment, escoId));
                    }

                    segments = segments.DistinctBy(x => x.SegmentName).ToList();
                    foreach (var segment in segments)
                    {
                        var name = segment.SegmentName;
                        segmentScore = new SegmentScore();
                        segmentScore.Quantity = quantity;
                        segmentScore.ResultDescription = name;
                        segmentScore.ResultDetail = name;
                        segmentScore.AssessmentId = module.AssessmentId;
                        segmentScore.ModuleId = module.ModuleId;
                        segmentScore.Result = result;
                        segmentScore.Segment = name;
                        module.SegmentScores.Add(segmentScore);
                    }

                    module.ModuleScore = new ModuleScore
                    {
                        ProfileId = string.Empty,
                        Segment = module.LocalizedJobName ?? string.Empty,
                        AssessmentId = module.AssessmentId,
                        ModuleId = module.ModuleId,
                        ResultDescription = string.Empty,
                        Result = module.SegmentScores.Sum(x => x.Result) / Math.Max(module.SegmentScores.Count, 1),
                    };

                    break;
                case AssessmentType.Ea:
                    foreach (var rawData in rawDatas)
                    {
                        var node = JsonObject.Parse(rawData.Data);
                        var segment = GetSegmentName(node, nameof(SharedContracts.RawData.handlungsfeld));
                        var escoId = GetEscoId(node);
                        if (string.IsNullOrWhiteSpace(segment))
                        {
                            continue;
                        }

                        segments.Add((segment, escoId));
                    }

                    segments = segments = segments.DistinctBy(x => x.SegmentName).ToList();
                    foreach (var segment in segments)
                    {
                        var name = segment.SegmentName;
                        segmentScore = new SegmentScore();
                        segmentScore.Quantity = quantity;
                        segmentScore.ResultDescription = GetText(pattern);
                        segmentScore.AssessmentId = module.AssessmentId;
                        segmentScore.ModuleId = module.ModuleId;
                        segmentScore.Result = result;
                        segmentScore.Segment = name;
                        module.SegmentScores.Add(segmentScore);
                        module.ModuleScore = new ModuleScore()
                        {
                            Segment = module.Title,
                            Quantity = AssessmentScoreQuantity.Median,
                            Result = 0.5,
                        };
                    }

                    break;
                case AssessmentType.So:
                    foreach (var rawData in rawDatas)
                    {
                        var node = JsonObject.Parse(rawData.Data);
                        var segment = GetSegmentName(node, nameof(SharedContracts.RawData.esco));
                        var escoId = GetEscoId(node);
                        if (string.IsNullOrWhiteSpace(segment))
                        {
                            continue;
                        }

                        segments.Add((segment, escoId));
                    }

                    segments = segments.DistinctBy(x => x.SegmentName).ToList();
                    foreach (var segment in segments)
                    {
                        pattern = $"{module.Type}_{segment.EscoId}_{string.Empty}_{nameof(ModuleScore.ResultDescription)}_{language}";
                        var name = segment.SegmentName;
                        segmentScore = new SegmentScore();
                        segmentScore.Quantity = quantity;
                        segmentScore.ResultDescription = GetText(pattern);
                        segmentScore.AssessmentId = module.AssessmentId;
                        segmentScore.ModuleId = module.ModuleId;
                        segmentScore.Result = result;
                        segmentScore.Segment = name;
                        var overPatter = $"{module.Type}_{segment.EscoId}_{AssessmentScoreQuantity.Over}_{nameof(ModuleScore.ResultDescription)}_{language}";
                        var underPatter = $"{module.Type}_{segment.EscoId}_{AssessmentScoreQuantity.Under}_{nameof(ModuleScore.ResultDescription)}_{language}";
                        segmentScore.ResultDetail = $"<p>{GetText(overPatter)}</p><br/><p>{GetText(underPatter)}</p>";
                        module.SegmentScores.Add(segmentScore);
                    }

                    module.ModuleScore = new ModuleScore()
                    {
                        Segment = module.Title,
                        Quantity = AssessmentScoreQuantity.Median,
                        Result = (double)_random.Next(100) * 0.01,
                    };
                    break;
            }

            return Task.CompletedTask;
        }

        private string? GetSegmentName(JsonNode? node, string nodeName)
        {
            var name = node?[nodeName]?.GetValue<string>() ?? string.Empty;
            name = name.Split("-").Last().Trim();
            return name.Trim();
        }

        private string GetEscoId(JsonNode? node)
        {
            var escoId = node?[nameof(SharedContracts.RawData.idesco)]?.GetValue<string>() ?? string.Empty;
            escoId = escoId.Replace("(", string.Empty);
            escoId = escoId.Replace(")", string.Empty);
            escoId = escoId.Replace(".", string.Empty);
            return escoId.Trim();
        }

        private string GetText(string name)
        {
            return Resources.ResourceManager.GetString(name) ?? name;
        }
    }
}
