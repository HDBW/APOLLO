// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.SemanticSearchWorker;

namespace Apollo.SemanticSearchExporter
{
    /// <summary>
    /// Converts combined profile and training instances into the set of strings for CSV output.
    /// </summary>
    //internal class CombinedProfileTrainingFormatter : IEntityFormatter
    //{
    //    private string _cDelimiter = ";";

    //    /// <summary>
    //    /// Returns a list of strings where each string represents a combined data row formatted for CSV.
    //    /// </summary>
    //    /// <param name="instance">An instance of combined profile and training data.</param>
    //    /// <returns>A list of formatted strings.</returns>
    //    public IList<string> FormatObject(object instance)
    //    {
    //        var combined = instance as ProfileTrainingCombined; // assuming we are gonna have a seperate class where profile and training will be combined
    //        if (combined == null) throw new ArgumentNullException(nameof(combined));

    //        List<string> lines = new List<string>();
    //        StringBuilder sb = new StringBuilder();

    //        // Append each property into the StringBuilder with delimiter handling
    //        sb.Append($"{combined.ProfileId}{_cDelimiter}");
    //        sb.Append($"{combined.TrainingId}{_cDelimiter}");
    //        sb.Append($"{combined.ProfileName.Replace(_cDelimiter, " ")}{_cDelimiter}");
    //        sb.Append($"{combined.TrainingName.Replace(_cDelimiter, " ")}{_cDelimiter}");
    //        sb.Append($"{combined.Role.Replace(_cDelimiter, " ")}{_cDelimiter}");
    //        sb.Append($"{combined.Description.Replace(_cDelimiter, " ").Trim()}");

    //        // Each line in the CSV will contain data of a combined profile and training entity
    //        lines.Add(sb.ToString());

    //        return lines;
    //    }
    //}

}
