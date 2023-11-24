// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Api
{
    internal static class ErrorCodes
    {
        internal static class TrainingErrors
        {
            internal const int TrainingCodeBase = 100;
            internal const int GetTrainingError = TrainingCodeBase + 1;
            internal const int QueryTrainingsError = TrainingCodeBase + 10;
            internal const int InsertTrainingErr = TrainingCodeBase + 30;
            internal const int DeleteTrainingErr = TrainingCodeBase + 40; 
            internal const int CreateOrUpdateTrainingErr = TrainingCodeBase + 50;
            internal const int GetTotalTrainingCountErr = TrainingCodeBase + 60;
            internal const int QueryTrainingsWithCustomFieldsErr = TrainingCodeBase + 70;
            internal const int QueryTrainingsPaginatedErr = TrainingCodeBase + 80;
            internal const int CountTrainingsByProviderErr = TrainingCodeBase + 90;
            internal const int QueryTrainingsByDateRangeErr = TrainingCodeBase + 100;
            internal const int SearchTrainingsByKeywordErr = TrainingCodeBase + 110;

            internal static void HandleException(Exception ex)
            {
                throw new ApolloApiException(TrainingCodeBase, "Training Error occurred", ex);
            }
        }


        internal static class UserErrors
        {
            internal const int TrainingCodeBase = 200;
            internal const int GetUserError = TrainingCodeBase + 1;
            internal const int QueryUsersError = TrainingCodeBase + 10;
            internal const int QueryUsersByGoalError = TrainingCodeBase + 30;
            internal const int QueryUsersByKeywordError = TrainingCodeBase + 40;
            internal const int QueryUsersByMultipleCriteriaError = TrainingCodeBase + 50;
            internal const int QueryUsersWithPaginationError = TrainingCodeBase + 60;
            internal const int QueryUsersByDateRangeError = TrainingCodeBase + 70;
            internal const int QueryUsersByFirstNameError = TrainingCodeBase + 80;
            internal const int QueryUsersByLastNameError = TrainingCodeBase + 90;
            internal const int InsertUserError = TrainingCodeBase + 100;
            internal const int CreateOrUpdateUserError = TrainingCodeBase + 110;
            internal const int DeleteUserError = TrainingCodeBase + 120;

            internal static void HandleException(Exception ex)
            {
                throw new ApolloApiException(TrainingCodeBase, "User Error occurred", ex);
            }
        }
    }
}
