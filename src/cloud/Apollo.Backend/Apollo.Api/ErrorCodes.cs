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

            public static void HandleException(Exception ex)
            {
                throw new ApolloApiException(TrainingCodeBase, "Training Error occurred", ex);
            }
        }


        public static class UserErrors
        {
            public const int TrainingCodeBase = 200;
            public const int GetUserError = TrainingCodeBase + 1;
            public const int QueryUsersError = TrainingCodeBase + 10;
            public const int QueryUsersByGoalError = TrainingCodeBase + 30;
            public const int QueryUsersByKeywordError = TrainingCodeBase + 40;
            public const int QueryUsersByMultipleCriteriaError = TrainingCodeBase + 50;
            public const int QueryUsersWithPaginationError = TrainingCodeBase + 60;
            public const int QueryUsersByDateRangeError = TrainingCodeBase + 70;
            public const int QueryUsersByFirstNameError = TrainingCodeBase + 80;
            public const int QueryUsersByLastNameError = TrainingCodeBase + 90;
            public const int InsertUserError = TrainingCodeBase + 100;
            public const int CreateOrUpdateUserError = TrainingCodeBase + 110;
            public const int DeleteUserError = TrainingCodeBase + 120;
            public const int UserNotFound = TrainingCodeBase + 130;
            public const int NoUsersFoundByGoal = TrainingCodeBase + 140;
            public const int NoUsersFoundByKeyword = TrainingCodeBase + 150;
            public const int UserAlreadyExists = TrainingCodeBase + 160;
            public const int NoUsersToDelete = TrainingCodeBase + 170;
            public const int UserIdNotNeeded = TrainingCodeBase + 171;

            public static void HandleException(Exception ex)
            {
                throw new ApolloApiException(TrainingCodeBase, "User Error occurred", ex);
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
