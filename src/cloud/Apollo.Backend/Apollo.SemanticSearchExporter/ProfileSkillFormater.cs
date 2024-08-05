// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Apollo.SemanticSearchExporter;

namespace Apollo.SemanticSearchExporter
{
    /// <summary>
    /// Converts the skill instance into the set of strings.
    /// </summary>
    internal class ProfileSkillFormatter : IEntityFormatter
    {
        private string _cDelimiter = "|";

        /// <summary>
        /// Returns:
        /// Skills: Id, UserId/ProfileId, Skill.Title in Default Language (only Language or English), All titles, descriptions and alternative labels from all languages.
        /// The expected format: Id|Url|Title|Text
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="profileId">The ID of the profile that the skill belongs to.</param>
        /// <returns></returns>
        public IList<string> FormatObject(object instance, string profileId)
        {
            Skill? skill = instance as Skill;
            if (skill == null)
                throw new ArgumentNullException(nameof(skill));

            List<string> list = new List<string>();
            StringBuilder sb = new StringBuilder();

            // Extract titles, descriptions, and alternative labels
            var defaultTitle = skill.Title?.Items?.FirstOrDefault(item => item.Lng == "en" || string.IsNullOrEmpty(item.Lng))?.Value;
            var allTitles = string.Join(' ', skill.Title?.Items?.Select(item => item.Value.Replace(_cDelimiter, " ").Trim()) ?? Enumerable.Empty<string>());
            var allDescriptions = string.Join(' ', skill.Description?.Items?.Select(item => item.Value.Replace(_cDelimiter, " ").Trim()) ?? Enumerable.Empty<string>());
            var allAlternativeLabels = string.Join(' ', skill.AlternativeLabels?.Items?.Select(item => item.Value.Replace(_cDelimiter, " ").Trim()) ?? Enumerable.Empty<string>());

            sb.AppendLine(allTitles);
            sb.AppendLine(allDescriptions);
            sb.AppendLine(allAlternativeLabels);

            list.Add($"{skill?.Id}{_cDelimiter}{profileId}{_cDelimiter}{defaultTitle}{_cDelimiter}{sb.ToString().Replace("\r", " ").Replace("\n", " ")}");

            return list;
        }

        public IList<string> FormatObject(object entityInstance) => throw new NotImplementedException();
    }
}
