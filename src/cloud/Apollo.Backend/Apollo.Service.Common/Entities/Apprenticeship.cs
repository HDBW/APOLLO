// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    /// <summary>
    /// This is part of the original BA Dataset for Machine Learning.
    /// It is not relevant for the client.
    /// </summary>
    public class Apprenticeship
    {
        // Apprenticeship_Kind_filtered.txt
        // Freitext oder Vorschlagsliste
        public Occupation Kind { get; set; }

        public int Years { get; set; }
    }
}
