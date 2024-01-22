// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Api
{
    public static class ErrorCodes
    {
        public static class TrainingErrors
        {
            public const int TrainingCodeBase = 100;
            public const int GetTrainingError = TrainingCodeBase + 1;
            public const int QueryTrainingsError = TrainingCodeBase + 10;
            public const int InsertTrainingErr = TrainingCodeBase + 30;
            public const int DeleteTrainingErr = TrainingCodeBase + 40; 
            public const int CreateOrUpdateTrainingErr = TrainingCodeBase + 50;
            public const int GetTotalTrainingCountErr = TrainingCodeBase + 60;
            public const int QueryTrainingsWithCustomFieldsErr = TrainingCodeBase + 70;
            public const int QueryTrainingsPaginatedErr = TrainingCodeBase + 80;
            public const int CountTrainingsByProviderErr = TrainingCodeBase + 90;
            public const int QueryTrainingsByDateRangeErr = TrainingCodeBase + 100;
            public const int SearchTrainingsByKeywordErr = TrainingCodeBase + 110;
            public const int UpdateTrainingErr = TrainingCodeBase + 120;
            public const int CheckTrainingExistenceErr = TrainingCodeBase + 130; 

            public static void HandleException(Exception ex)
            {
                throw new ApolloApiException(TrainingCodeBase, "Training Error occurred", ex);
            }
        }


        public static class UserErrors
        {
            public const int UserCodeBase = 200;
            public const int GetUserError = UserCodeBase + 1;
            public const int QueryUsersError = UserCodeBase + 10;
            public const int QueryUsersByGoalError = UserCodeBase + 30;
            public const int QueryUsersByKeywordError = UserCodeBase + 40;
            public const int QueryUsersByMultipleCriteriaError = UserCodeBase + 50;
            public const int QueryUsersWithPaginationError = UserCodeBase + 60;
            public const int QueryUsersByDateRangeError = UserCodeBase + 70;
            public const int QueryUsersByFirstNameError = UserCodeBase + 80;
            public const int QueryUsersByLastNameError = UserCodeBase + 90;
            public const int InsertUserError = UserCodeBase + 100;
            public const int CreateOrUpdateUserError = UserCodeBase + 110;
            public const int DeleteUserError = UserCodeBase + 120;
            public const int UserNotFound = UserCodeBase + 130;
            public const int NoUsersFoundByGoal = UserCodeBase + 140;
            public const int NoUsersFoundByKeyword = UserCodeBase + 150;
            public const int UserAlreadyExists = UserCodeBase + 160;
            public const int NoUsersToDelete = UserCodeBase + 170;
            public const int UserIdNotNeeded = UserCodeBase + 171;

            public static void HandleException(Exception ex)
            {
                throw new ApolloApiException(UserCodeBase, "User Error occurred", ex);
            }
        }

        public static class ProfileErrors
        {
            public const int TrainingCodeBase = 300;
            public const int GetProfileError = TrainingCodeBase + 1;
            public const int QueryProfilesError = TrainingCodeBase + 10;
            //Will update error code in future for other types of Query
            //public const int QueryProfilesByGoalError = TrainingCodeBase + 30;
            //public const int QueryProfilesByKeywordError = TrainingCodeBase + 40;
            //public const int QueryProfilesByMultipleCriteriaError = TrainingCodeBase + 50;
            //public const int QueryProfilesWithPaginationError = TrainingCodeBase + 60;
            //public const int QueryProfilesByDateRangeError = TrainingCodeBase + 70;
            //public const int QueryProfilesByFirstNameError = TrainingCodeBase + 80;
            //public const int QueryProfilesByLastNameError = TrainingCodeBase + 90;
            public const int InsertProfileError = TrainingCodeBase + 100;
            public const int CreateOrUpdateProfileError = TrainingCodeBase + 110;
            public const int CreateOrUpdateProfileUserDoesNotExistError = TrainingCodeBase + 111;
            public const int DeleteProfileError = TrainingCodeBase + 120;
            public const int ProfileNotFound = TrainingCodeBase + 130;
            public const int ProfileAlreadyExists = TrainingCodeBase + 140;
            public const int NoProfilesToDelete = TrainingCodeBase + 150;

            public static void HandleException(Exception ex)
            {
                throw new ApolloApiException(TrainingCodeBase, "Profile Error occurred", ex);
            }
        }


        public static class GeneralErrors
        {
            public const int OperationFailed = 1000;
            
            public const int InvalidId = 1001;
            // Add more general error codes as needed...
        }

       
    }
}
