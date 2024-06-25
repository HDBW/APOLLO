// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace GrpcClient.Service
{
    public class AssessmentService : De.HDBW.Apollo.SharedContracts.Services.IAssessmentService
    {
        private readonly Dictionary<string, (string Title, AssessmentType Type)> _titles = new Dictionary<string, (string Title, AssessmentType Type)>()
        {
            { "fbffa07b-7c3a-4c94-9e41-55579b6e2d4a", ("Selbsteinschätzung zu typischem kognitiven Verhalten", AssessmentType.So) },
            { "ec0d7bb5-b8aa-49f9-91b0-7e3e3edf8ca9", ("Selbstmanagementkompetenzen", AssessmentType.So) },
            { "0c6d1762-1709-49db-bb21-754770e4ef13", ("Soziale und Kommunikative Kompetenzen", AssessmentType.So) },
            { "2b86e91f-66f1-4fb0-a9ac-924e4131370d", ("Umsetzungskompetenzen", AssessmentType.So) },
            { "f72d0d89-b2fb-453a-b0f1-0bd0187d1f05", ("Digitale Grundkompetenzen", AssessmentType.So) },
            { "bc5b65f5-5c00-4345-a781-211b69534239", ("Deutsch", AssessmentType.Gl) },
            { "f03d6c0f-af50-4582-aeb2-9bddbbf8a8db", ("Hochbaufacharbeiter:in", AssessmentType.Sk) },
            { "25324ec2-1f36-485b-ae69-56e985ccbb7b", ("Fachkraft im Gastgewerbe", AssessmentType.Sk) },
            { "26894d0d-c670-4eed-a57d-ac95033966d5", ("Koch / Köchin", AssessmentType.Sk) },
            { "42495073-3715-4d80-8b00-cd761956d1c4", ("Berufe entdecken", AssessmentType.Be) },
        };

        public async Task<IEnumerable<AssessmentTile>> GetAssessmentTilesAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var so = new AssessmentTile()
            {
                Deleted = false,
                Type = AssessmentType.So,
                Grouping = string.Empty,
            };
            so.AssessmentId = Guid.Empty.ToString();
            so.ModuleIds.Add("fbffa07b-7c3a-4c94-9e41-55579b6e2d4a");
            so.ModuleIds.Add("ec0d7bb5-b8aa-49f9-91b0-7e3e3edf8ca9");
            so.ModuleIds.Add("0c6d1762-1709-49db-bb21-754770e4ef13");
            so.ModuleIds.Add("2b86e91f-66f1-4fb0-a9ac-924e4131370d");
            so.ModuleIds.Add("f72d0d89-b2fb-453a-b0f1-0bd0187d1f05");
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

            var data = _titles["bc5b65f5-5c00-4345-a781-211b69534239"];
            var gl = new AssessmentTile()
            {
                Type = data.Type,
                Title = data.Title,
                Grouping = string.Empty,
            };
            gl.ModuleIds.Add("bc5b65f5-5c00-4345-a781-211b69534239");

            data = _titles["42495073-3715-4d80-8b00-cd761956d1c4"];
            var be = new AssessmentTile()
            {
                Type = data.Type,
                Title = data.Title,
                Grouping = string.Empty,
            };

            be.ModuleIds.Add("42495073-3715-4d80-8b00-cd761956d1c4");

            data = _titles["f03d6c0f-af50-4582-aeb2-9bddbbf8a8db"];
            var sa = new AssessmentTile()
            {
                Type = data.Type,
                Title = data.Title,
                Grouping = "Teste dein Wissen",
                MemberOnly = true,
            };
            sa.ModuleIds.Add("f03d6c0f-af50-4582-aeb2-9bddbbf8a8db");

            data = _titles["25324ec2-1f36-485b-ae69-56e985ccbb7b"];
            var sa1 = new AssessmentTile()
            {
                Type = data.Type,
                Title = data.Title,
                Grouping = "Teste dein Wissen",
                MemberOnly = false,
            };
            sa1.ModuleIds.Add("25324ec2-1f36-485b-ae69-56e985ccbb7b");

            data = _titles["26894d0d-c670-4eed-a57d-ac95033966d5"];
            var sa2 = new AssessmentTile()
            {
                Type = data.Type,
                Title = data.Title,
                Grouping = "Teste dein Wissen",
                MemberOnly = true,
            };
            sa2.ModuleIds.Add("26894d0d-c670-4eed-a57d-ac95033966d5");

            return new List<AssessmentTile>()
            {
                so,
                gl,
                be,
                sa,
                sa1,
                sa2,
            };
        }

        public async Task<object> GetModuleInstructionAsync(string moduleId, CancellationToken token)
        {
            return null;
        }

        public async Task<IEnumerable<ModuleTile>> GetModuleTilesAsync(IEnumerable<string> moduleIds, CancellationToken token)
        {
            var result = new List<ModuleTile>();
            foreach (var moduleId in moduleIds)
            {
                result.Add(new ModuleTile()
                {
                    Deleted = false,
                    Type = _titles[moduleId].Type,
                    ModuleId = moduleId,
                    Title = _titles[moduleId].Title,
                });
            }

            return result;
        }
    }
}
