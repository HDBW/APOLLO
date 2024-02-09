// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// A Language Skill is defined as ISO 639-2.
    /// Thankfully C# has a CultureInfo Class that implements ISO 639-2 can be used for this.
    /// </summary>
    public class LanguageSkill
    {
        public string Name { get; set; }

        public LanguageNiveau Niveau { get; set; }

        // CultureInfo get Culture ISO639-2 
        public string Code { get; set; }
    }
}
