// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    /// <summary>
    /// This is part of the original BA Dataset for Machine Learning.
    /// I would argue Skill is a better System to store knowledge then string.
    /// However, maybe easier for the client to have a list of strings.
    /// We can try to match them later in the backend.
    /// Or we get rid of the string and try to merch directly?
    /// It is not a priority for now.
    /// </summary>
    public class Knowledge
    {
        // Knowledge_Advanced_filtered.txt
        // Freitext
        public List<string> Advanced { get; set; }
        public List<Skill> AdvancedSkills { get; set; }

        // Knowledge_Basic_filtered.txt
        // Freitext
        public List<string> Basic { get; set; }
        public List<Skill> BasicSkills { get; set; }

        // Knowledge_Expert_filtered.txt
        // Freitext
        public List<string> Expert { get; set; }
        public List<Skill> ExpertSkills { get; set; }
    }
}
