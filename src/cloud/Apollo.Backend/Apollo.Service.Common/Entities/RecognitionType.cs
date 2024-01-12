﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    public enum RecognitionType
    {
        Unknown = 0,
        // Nicht reglementierter, nicht anerkannter Abschluss
        UnregulatedNotRecognized = 1,
        // Reglementierter und nicht anerkannter Abschluss
        RegulatedNotRecognized = 2,
        // Anerkannter Abschluss
        Recognized = 3,
        // Anerkennung des Abschlusses wird geprüft
        Pending = 4,
        // Teilweise anerkannter Abschluss
        PatialRecognized = 5,
    }
}
