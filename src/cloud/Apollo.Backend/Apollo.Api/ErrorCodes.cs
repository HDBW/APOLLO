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

            public static int CreateOrUpdateList { get; internal set; }

            public static void HandleException(Exception ex)
            {
                throw new ApolloApiException(UserCodeBase, "User Error occurred", ex);
            }
        }

        public static class ProfileErrors
        {
            public const int ProfileCodeBase = 300;
            public const int GetProfileError = ProfileCodeBase + 1;
            public const int QueryProfilesError = ProfileCodeBase + 10;
            //Will update error code in future for other types of Query
            //public const int QueryProfilesByGoalError = ProfileCodeBase + 30;
            //public const int QueryProfilesByKeywordError = ProfileCodeBase + 40;
            //public const int QueryProfilesByMultipleCriteriaError = ProfileCodeBase + 50;
            //public const int QueryProfilesWithPaginationError = ProfileCodeBase + 60;
            //public const int QueryProfilesByDateRangeError = ProfileCodeBase + 70;
            //public const int QueryProfilesByFirstNameError = ProfileCodeBase + 80;
            //public const int QueryProfilesByLastNameError = ProfileCodeBase + 90;
            public const int InsertProfileError = ProfileCodeBase + 100;
            public const int CreateOrUpdateProfileError = ProfileCodeBase + 110;
            public const int CreateOrUpdateProfileUserDoesNotExistError = ProfileCodeBase + 111;
            public const int DeleteProfileError = ProfileCodeBase + 120;
            public const int ProfileNotFound = ProfileCodeBase + 130;
            public const int ProfileIsNullOrEmpty = ProfileCodeBase + 131;
            public const int ProfileAlreadyExists = ProfileCodeBase + 140;
            public const int NoProfilesToDelete = ProfileCodeBase + 150;
            public const int ListItemNotfound = ProfileCodeBase + 160;

            public static void HandleException(Exception ex)
            {
                throw new ApolloApiException(ProfileCodeBase, "Profile Error occurred", ex);
            }
        }

        public static class QualificationErrors
        {
            public const int QualificationCodeBase = 500;

            public const int CreateOrUpdateQualificationError = QualificationCodeBase + 1;

            public static void HandleException(Exception ex)
            {
                throw new ApolloApiException(QualificationCodeBase, "Qualification Error occurred", ex);
            }
        }

        public static class ListErrors
        {
            public const int ListCodeBase = 400;

            public const int GetListError = ListCodeBase + 1;

            public const int QueryListError = ListCodeBase + 5;

            public const int CreateOrUpdateListError = ListCodeBase + 10;

            public const int DeleteListError = ListCodeBase + 15;

            public static int ListNotFoundError = ListCodeBase + 16;
        }

            public static class GeneralErrors
        {
            public const int OperationFailed = 1000;
            
            public const int InvalidId = 1001;

            public static int InvalidQuery = 1002;
            // Add more general error codes as needed...
        }

       
    }
}
