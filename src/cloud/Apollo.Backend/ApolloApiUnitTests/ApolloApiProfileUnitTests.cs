// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using System.Collections;
using Apollo.Api;
using Apollo.Api.UnitTests;
using Apollo.Common.Entities;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace apolloapiunittests
{

    /// <summary>
    /// Unit tests for the ApolloApi class.
    /// </summary>
    [TestClass]
    public class ApolloApiProfileUnitTests
    {

        //[TestMethod]
        //[TestCategory("Prod")]
        //public async Task GetProfileTest()
        //{
        //    // Arrange
        //    var api = Helpers.GetApolloApi();

        //    // Create a new Profile instance
        //    var newProfile = new Profile
        //    {
                

        //        CareerInfos = new List<CareerInfo>
        //{
        //    new CareerInfo
        //    {
        //        // Sample career info data
        //        Start = new DateTime(2020, 01, 01),
        //        End = new DateTime(2022, 01, 01),
        //        NameOfInstitution = "Sample Company",
        //        Country = "ASD"
                
        //    }
        //},
              
        //    };

        //    // Assume that you have a valid userId for which the profile is being created
        //    string userId = "User-9BC5C338E6F847F5A2CF91250455470B";

        //    // Create or update the profile
        //    var profileIds = await api.CreateOrUpdateProfile(userId, newProfile);

        //    // Act
        //    // Retrieve the profile using the GetProfile method
        //    var profile = await api.GetProfile(profileIds.FirstOrDefault());

        //    // Assert
        //    // Ensure that the profile was retrieved and matches the created/updated profile
        //    Assert.IsNotNull(profile);
        //    Assert.IsNotNull(profile.CareerInfos);
        //    Assert.AreEqual(newProfile.CareerInfos.First().NameOfInstitution, profile.CareerInfos.First().NameOfInstitution);

        //    // Cleanup
        //    await api.DeleteProfiles(new string[] { profile.Id });
        //}
    }
}
