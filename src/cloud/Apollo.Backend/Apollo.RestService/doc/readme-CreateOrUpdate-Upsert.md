This file contains useful information about the APOLLO project.


https://github.com/HDBW/APOLLO/tree/development/src/cloud/invite-apollo.app

https://github.com/HDBW/APOLLO/blob/development/src/cloud/invite-apollo.app/Apollo.API.Public.Training/Models/Contact.cs


# Auth


AppId: 6a17b692-2ce3-4c94-9d07-3b54a2d2662d

TenantId: 857332e7-9da9-46a2-ba04-5616116258e1

apolloappb2c.onmicrosoft.com


# REST API Postman Collection

https://dark-resonance-7673.postman.co/workspace/Apollo-Workspace~062f247f-5ff6-4e49-91d9-ac4fde051a85/overview


# Where is the data?

## 1000000 user profiles
https://portal.azure.com/#@student.hdbw-hochschule.de/resource/subscriptions/f83b64c1-d950-44e7-9004-80d6c393dafa/resourceGroups/rg-apollo-dac/overview

https://portal.azure.com/#view/Microsoft_Azure_Storage/ContainerMenuBlade/~/overview/storageAccountId/%2Fsubscriptions%2Ff83b64c1-d950-44e7-9004-80d6c393dafa%2FresourceGroups%2Frg-apollo-dac%2Fproviders%2FMicrosoft.Storage%2FstorageAccounts%2Fapollotxtanalytics/path/ingressbaprofile/etag/%220x8DB4249DC95079C%22/defaultEncryptionScope/%24account-encryption-key/denyEncryptionScopeOverride~/false/defaultId//publicAccessVal/None

## Trainings Examples

https://portal.azure.com/#view/Microsoft_Azure_Storage/ContainerMenuBlade/~/overview/storageAccountId/%2Fsubscriptions%2Ff83b64c1-d950-44e7-9004-80d6c393dafa%2FresourceGroups%2Frg-apollo-dac%2Fproviders%2FMicrosoft.Storage%2FstorageAccounts%2Fapollotxtanalytics/path/ingressbbw/etag/%220x8DAFEDC26850AF9%22/defaultEncryptionScope/%24account-encryption-key/denyEncryptionScopeOverride~/false/defaultId//publicAccessVal/None


# Upsert and CreateOrUpdate functions

This guide provides an overview of how to create and update entities within our system, focusing on operations such as Upsert and CreateOrUpdate within different contexts like training sessions, user profiles, and more.

### How it works

Upsert Operation

The Upsert operation is designed to either insert a new document into the database or update an existing one, based on the presence of an Id or ObjectId.

-	*Insert* :  If neither Id nor ObjectId(for User Entity) is specified, the system will insert a new document.
- *Update* : If an Id or ObjectId is provided, the system will search for the document and update it accordingly.

#### Example of Upsert Operation

```sh
 public async Task<UpsertResult> UpsertAsync(string collectionName, ICollection<ExpandoObject> documents)
        {
            if (documents == null)
                throw new ArgumentNullException($"Argument {nameof(documents)} cannot be nulL!");

            var coll = GetCollection(collectionName);

            try
            {
                int inserted = 0;
                int updated = 0;

                foreach (IDictionary<string, object> item in documents)
                {
                    if (item == null)
                        throw new ArgumentNullException(nameof(item));

                    FilterDefinition<BsonDocument> filter =
                    Builders<BsonDocument>.Filter.Eq("_id", item["_id"]);

                    //FilterDefinition<BsonDocument> filter = ShredKey == null ?
                    //    Builders<BsonDocument>.Filter.Eq("_id", item["_id"]) :
                    //    Builders<BsonDocument>.Filter.Eq("_id", item["_id"]) &
                    //    Builders<BsonDocument>.Filter.Eq("ShredKey", item[ShredKey]);

                    BsonDocument? doc =
                        await coll.FindOneAndUpdateAsync<BsonDocument>(filter, BuildUpdate(item as ExpandoObject),
                        new FindOneAndUpdateOptions<BsonDocument>
                        {
                            IsUpsert = true,
                            ReturnDocument = ReturnDocument.Before
                        });

                    if (doc == null)
                    {
                        // inserted
                        inserted++;
                    }
                    else
                    {
                        // updated
                        updated++;
                    }
                }

                return new UpsertResult
                {
                    Inserted = inserted,
                    Updated = updated
                };
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex.Message, $"{nameof(UpsertAsync)} has failed");
                throw;

            }
        }

```

This function iterates through each document in the documents collection. If a **document** doesn't have an **_id**, it's inserted. Otherwise, it's updated.

#### GetUserAsync Extension

The **GetUserAsync** method has been extended to query by Id first and then by **ObjectId** if the **Id** is not specified.


#### CreateOrUpdate Operation

The CreateOrUpdate operation allows for the modification or addition of entities like trainings, profiles, or users based on specified criteria.

Example of CreateOrUpdate Operation for Training:


```sh
   public async Task<CreateOrUpdateTrainingResponse> CreateOrUpdateTrainingAsync([FromBody] CreateOrUpdateTrainingRequest req)
        {
            try
            {
                _logger?.LogTrace($"{nameof(CreateOrUpdateTrainingAsync)} entered.");

                // Assuming req contains the Training object to create or update.
                var result = await _api.CreateOrUpdateTrainingAsync(new List<Training> { req.Training });

                _logger?.LogTrace($"{nameof(CreateOrUpdateTrainingAsync)} completed.");

                // Return the result of the create/update operation as a response.
                return new CreateOrUpdateTrainingResponse { Training = req.Training };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError($"{nameof(CreateOrUpdateTrainingAsync)} failed: {ex.Message}");
                throw;
            }
        }
```

This endpoint either creates a new training session or updates an existing one based on the presence of an Id in the request object.


#### Testing CreateOrUpdate Operation

It's essential to verify that both creating and updating functionalities work as expected through unit tests.

```sh
 public async Task CreateOrUpdateTrainingTest()
        {
            var api = Helpers.GetApolloApi();

            // Create a new training instance with Id and ProviderId
            var newTraining = new Training
            {
                Id = "T23", 
                ProviderId = "provider123", // Sample provider Id
                TrainingName = "New Test Training",
                TrainingType = "Type1",
                Description = "New training description"
            };

            // Insert the new training
            var insertedTrainingIds = await api.CreateOrUpdateTrainingAsync(new List<Training> { newTraining });

            // Ensure that the new training was inserted correctly
            Assert.IsNotNull(insertedTrainingIds);
            Assert.AreEqual(1, insertedTrainingIds.Count);
            Assert.AreEqual(newTraining.Id, insertedTrainingIds[0]);

            // Update the created training with new data
            newTraining.TrainingName = "Updated Test Training";
            newTraining.Description = "Updated Description";
            newTraining.TrainingType = "Type2";

            // Update the training using the same ID
            var updatedTrainingIds = await api.CreateOrUpdateTrainingAsync(new List<Training> { newTraining });

            // Ensure that the training was updated correctly
            Assert.IsNotNull(updatedTrainingIds);
            Assert.AreEqual(1, updatedTrainingIds.Count);
            Assert.AreEqual(newTraining.Id, updatedTrainingIds[0]); // The ID should remain the same

            // Clean up: Delete the created or updated training
            await api.DeleteTrainingsAsync(updatedTrainingIds.ToArray());
        }
```

#### Example Request Body for CreateOrUpdateTraining

Then the following request body of the Training entity with dummy data should be of the following:

```sh
{
  "training": {
    "id": "UT01",
    "providerId": "unittest01",
    "trainingName": "Dummy Training",
    "description": "This is a dummy training description.",
    "shortDescription": "Short description of the dummy training.",
    "content": ["Content 1", "Content 2"],
    "benefitList": ["Benefit 1", "Benefit 2"],
    "certificate": ["Certificate 1", "Certificate 2"],
    "prerequisites": ["Prerequisite 1", "Prerequisite 2"],
    "loans": [
      {
        "id": "L01",
        "name": "Dummy Loan 1",
        "description": "Description of Dummy Loan 1",
        "url": "http://dummyloan1.com",
        "loanContact": {
          "id": "LC01",
          "surname": "Loan Contact 1",
          "mail": "loancontact1@example.com",
          "phone": "1234567890",
          "organization": "Loan Org 1",
          "address": "123 Loan St",
          "city": "Loan City",
          "zipCode": "12345",
          "eAppointmentUrl": "http://loancontact1.example.com"
        }
      },
      {
        "id": "L02",
        "name": "Dummy Loan 2",
        "description": "Description of Dummy Loan 2",
        "url": "http://dummyloan2.com",
        "loanContact": {
          "id": "LC02",
          "surname": "Loan Contact 2",
          "mail": "loancontact2@example.com",
          "phone": "9876543210",
          "organization": "Loan Org 2",
          "address": "456 Loan St",
          "city": "Loan City",
          "zipCode": "67890",
          "eAppointmentUrl": "http://loancontact2.example.com"
        }
      }
    ],
    "trainingProvider": {
      "id": "TP01",
      "name": "Training Provider 1",
      "description": "Description of Training Provider 1",
      "url": "http://trainingprovider1.com",
      "contact": {
        "id": "TPC01",
        "surname": "Training Provider Contact 1",
        "mail": "trainingprovidercontact1@example.com",
        "phone": "1234567890",
        "organization": "Training Provider Org 1",
        "address": "123 Training Provider St",
        "city": "Training Provider City",
        "zipCode": "12345",
        "eAppointmentUrl": "http://trainingprovidercontact1.example.com"
      },
      "image": "http://trainingprovider1.com/image"
    },
    "courseProvider": {
      "id": "CP01",
      "name": "Course Provider 1",
      "description": "Description of Course Provider 1",
      "url": "http://courseprovider1.com",
      "contact": {
        "id": "CPC01",
        "surname": "Course Provider Contact 1",
        "mail": "courseprovidercontact1@example.com",
        "phone": "1234567890",
        "organization": "Course Provider Org 1",
        "address": "123 Course Provider St",
        "city": "Course Provider City",
        "zipCode": "12345",
        "eAppointmentUrl": "http://courseprovidercontact1.example.com"
      },
      "image": "http://courseprovider1.com/image"
    },
    "appointments": {
      "id": "AP01",
      "appointment": "http://appointment1.com",
      "appointmentType": "Type 1",
      "appointmentDescription": "Description of Appointment 1",
      "appointmentLocation": {
        "id": "AL01",
        "surname": "Appointment Location 1",
        "mail": "appointmentlocation1@example.com",
        "phone": "1234567890",
        "organization": "Appointment Org 1",
        "address": "123 Appointment St",
        "city": "Appointment City",
        "zipCode": "12345",
        "eAppointmentUrl": "http://appointmentlocation1.example.com"
      },
      "startDate": "2024-03-06T09:00:00",
      "endDate": "2024-03-06T17:00:00",
      "durationDescription": "Full Day",
      "duration": {
        "ticks": 0,
        "days": 1,
        "hours": 8,
        "milliseconds": 0,
        "microseconds": 0,
        "nanoseconds": 0,
        "minutes": 480,
        "seconds": 0,
        "totalDays": 1,
        "totalHours": 8,
        "totalMilliseconds": 28800000,
        "totalMicroseconds": 28800000000,
        "totalNanoseconds": 28800000000000,
        "totalMinutes": 480,
        "totalSeconds": 28800
      },
      "occurences": [
        {
          "correlationId": "C01",
          "id": "O01",
          "startDate": "2024-03-06T09:00:00",
          "endDate": "2024-03-06T17:00:00",
          "duration": {
            "ticks": 0,
            "days": 1,
            "hours": 8,
            "milliseconds": 0,
            "microseconds": 0,
            "nanoseconds": 0,
            "minutes": 480,
            "seconds": 0,
            "totalDays": 1,
            "totalHours": 8,
            "totalMilliseconds": 28800000,
            "totalMicroseconds": 28800000000,
            "totalNanoseconds": 28800000000000,
            "totalMinutes": 480,
            "totalSeconds": 28800
          },
          "description": "Occurrence 1",
          "location": {
            "id": "LO01",
            "surname": "Occurrence Location 1",
            "mail": "occurrencelocation1@example.com",
            "phone": "1234567890",
            "organization": "Occurrence Org 1",
            "address": "123 Occurrence St",
            "city": "Occurrence City",
            "zipCode": "12345",
            "eAppointmentUrl": "http://occurrencelocation1.example.com"
          }
        }
      ],
      "isGuaranteed": true,
      "trainingType": {},
      "timeInvestAttendee": {
        "ticks": 0,
        "days": 0,
        "hours": 0,
        "milliseconds": 0,
        "microseconds": 0,
        "nanoseconds": 0,
        "minutes": 0,
        "seconds": 0,
        "totalDays": 0,
        "totalHours": 0,
        "totalMilliseconds": 0,
        "totalMicroseconds": 0,
        "totalNanoseconds": 0,
        "totalMinutes": 0,
        "totalSeconds": 0
      },
      "timeModel": "Model 1"
    },
    "productUrl": "http://dummyproducturl.com",
    "contacts": {
      "sed_6": {
        "id": "SED01",
        "surname": "SED Contact",
        "mail": "sedcontact@example.com",
        "phone": "1234567890",
        "organization": "SED Org",
        "address": "123 SED St",
        "city": "SED City",
        "zipCode": "12345",
        "eAppointmentUrl": "http://sedcontact.example.com"
      },
      "ut_0": {
        "id": "UT01",
        "surname": "UT Contact",
        "mail": "utcontact@example.com",
        "phone": "1234567890",
        "organization": "UT Org",
        "address": "123 UT St",
        "city": "UT City",
        "zipCode": "12345",
        "eAppointmentUrl": "http://utcontact.example.com"
      },
      "exercitation_9": {
        "id": "EXE01",
        "surname": "Exercitation Contact",
        "mail": "exercitationcontact@example.com",
        "phone": "1234567890",
        "organization": "Exercitation Org",
        "address": "123 Exercitation St",
        "city": "Exercitation City",
        "zipCode": "12345",
        "eAppointmentUrl": "http://exercitationcontact.example.com"
      }
    },
    "trainingType": 0,
    "individualStartDate": "2024-03-06T09:00:00",
    "price": 42.0,
    "priceDescription": "EUR",
    "accessibilityAvailable": true,
    "tags": ["Tag 1", "Tag 2"],
    "categories": ["Category 1", "Category 2"],
    "publishingDate": "2024-03-04T09:00:00",
    "unpublishingDate": "2024-03-11T09:00:00",
    "successor": "Successor Training",
    "predecessor": "Predecessor Training"
  }
}
```

This test method sends requests to either create or update training sessions based on whether an Id is present, asserting the success of each operation. So if Training with **id** UT01 already exists in the database the CreateOrUpdate function will update the given fileds with the new values. If Training with that id doesnt exist i will create a new training document with specified values.

#### General Functionality Across Controllers

The pattern for create-or-update functionality is consistent across various controllers, including but not limited to training, profile, and user controllers. This ensures a uniform approach to handling entity creation and updates throughout the system.


### User Entity


This section outlines various test cases to demonstrate the creation, updating, and deletion of User objects within an application and should give you extra conext on how it works. These test cases also help verify the functionality of the CreateOrUpdate and Delete operations. 

#### Test Case 1: Create or Update Single User

**Objective**
To test the creation of a new User object, update its properties, verify the updates, and clean up by deleting the user.

**Steps**
Create a new user with predefined properties.
Insert the user using the CreateOrUpdateUserAsync method.
Retrieve the inserted user to verify insertion.
Update the user's properties.
Update the user using the CreateOrUpdateUserAsync method.
Retrieve the updated user to verify updates.
Delete the user to clean up.

**Expected Results**
The user is successfully created and inserted.
The user's updated properties are correctly saved and retrieved.
The user is successfully deleted after the test.

**Example Request Body for Creating a User**
```sh
{
  "Name": "John Doe",
  "Email": "johndoe@example.com"
}
```

**Response Body for Created User**
```sh
{
  "Id": "generatedUserId1",
  "Name": "John Doe",
  "Email": "johndoe@example.com"
}
```

**Request Body for Updating the User**
```sh
{
  "Id": "generatedUserId1",
  "Name": "Jane Doe",
  "Email": "johndoe@example.com"
}
```


**Response Body for Updated User**
```sh
{
  "Id": "generatedUserId1",
  "Name": "Jane Doe",
  "Email": "johndoe@example.com"
}
```

#### Test Case 2: Create or Update Single User with ID

**Objective**
To create and update a user with a specific ID, verifying that the user is successfully created, updated, and matches the provided ID.

**Steps**
Create a new user and insert it using CreateOrUpdateUserAsync to generate an ID.
Update the user's information.
Update the user using the CreateOrUpdateUserAsync method with the predefined ID.
Clean up by deleting the user.

**Expected Results**
A user ID is generated upon creation.
The user is updated, retaining the same ID.
The user is deleted successfully at the end of the test.


**Initial Request to Create User**
```sh
{
  "Name": "Test User",
  "Email": "tstuser@example.com"
}
```

**Response with Generated ID**
```sh
{
  "Id": "generatedUserId2"
}
```

**Request to Update User with Predefined ID**
```sh
{
  "Id": "generatedUserId2",
  "Name": "Updated User",
  "Email": "tstuser@example.com"
}
```


**Response After Update**
```sh
{
  "Id": "generatedUserId2",
  "Name": "Updated User",
  "Email": "tstuser@example.com"
}
```


#### Test Case 3: Create or Update Single User with ObjectId

**Objective**
To create and update a user with a specific ObjectId, verifying that the user is successfully inserted, updated, and the changes are persisted.

**Steps**
Create a user with an ObjectId and other properties.
Insert the user using CreateOrUpdateUserAsync.
Retrieve and verify the inserted user.
Update the user's properties.
Update the user using CreateOrUpdateUserAsync.
Verify the updated user's properties.
Clean up by deleting the user.

**Expected Results**
The user with ObjectId is correctly inserted and updated.
The updates on the user are correctly persisted.
The user is deleted successfully at the end of the test.

**Request to Create User with ObjectId**
```sh
{
  "ObjectId": "someObjectId",
  "Name": "User with ObjectId",
  "Email": "objectiduser@example.com"
}

```

**Response After Creating User with ObjectId**
```sh
{
  "Id": "generatedUserId3",
  "ObjectId": "someObjectId",
  "Name": "User with ObjectId",
  "Email": "objectiduser@example.com"
}
```

**Request to Update User with ObjectId**
```sh
{
  "Id": "generatedUserId3",
  "ObjectId": "someObjectId1",
  "Name": "Updated User with ObjectId",
  "Email": "objectiduser@example.com"
}
```


**Response After Update**
```sh
{
  "Id": "generatedUserId3",
  "ObjectId": "someObjectId1",
  "Name": "Updated User with ObjectId",
  "Email": "objectiduser@example.com"
}

```


#### Test Case 4: Create or Update Multiple Users with Mixed IDs

**Objective**
To test creating or updating multiple users with a mix of predefined IDs, ObjectIds, and users without any ID, ensuring all are inserted or updated correctly.

**Steps**
Create multiple users, some with predefined IDs, some with ObjectIds, and some without any IDs.
Insert the users using CreateOrUpdateUserAsync.
Update each user's properties.
Update the users using CreateOrUpdateUserAsync.
Verify each updated user's properties.
Clean up by deleting all users.

**Expected Results**
All users, regardless of having predefined IDs, ObjectIds, or no IDs, are inserted or updated correctly.
The updates on each user are correctly persisted and verified.
All users are deleted successfully at the end of the test.

**Request to Create/Update Multiple Users**
```sh
[
  {
    "Id": "User1",
    "Name": "User One",
    "Email": "user1@example.com"
  },
  {
    "ObjectId": "ObjectID2",
    "Name": "User Two",
    "Email": "user2@example.com"
  },
  {
    "Name": "User Three",
    "Email": "user3@example.com"
  }
]
```

**Response After Creating/Updating Multiple Users**
```sh
[
  {
    "Id": "User1",
    "Name": "Updated User One",
    "Email": "user1@example.com"
  },
  {
    "Id": "ObjectID2",
    "ObjectId": "someGeneratedId2",
    "Name": "Updated User Two",
    "Email": "user2@example.com"
  },
  {
    "Id": "someGeneratedId3",
    "Name": "Updated User Three",
    "Email": "user3@example.com"
  }
]

```

These request and response bodies illustrate the data flow in each test case, providing clarity on what is being tested and the expected outcomes.
