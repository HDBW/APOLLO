// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;

public class Mobility
{
    // Mobility_WillingToTravel_filtered.txt
    // Auswahlliste
    public Willing? WillingToTravel { get; set; }

    // Mobility_DriverLicenses_filtered.txt
    // Mehrfachselection
    public List<DriversLicense>? DriverLicenses { get; set; }

    public bool? HasVehicle { get; set; }
}
