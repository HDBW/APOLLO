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
            //TODO
        }

        internal static class UserErrors
        {
            internal const int TrainingCodeBase = 200;
          
            //TODO
        }
    }
}
