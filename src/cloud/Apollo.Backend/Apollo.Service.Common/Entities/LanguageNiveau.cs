// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    /// <summary>
    /// This is defined by the Common European Framework of Reference for Languages (CEFR).
    /// It will most likely not change that frequently.
    /// </summary>
    public class LanguageNiveau : ApolloListItem
    {
        //Unknown = 0,
        //A1 = 1,
        //A2 = 2,
        //B1 = 3,
        //B2 = 4,
        //C1 = 5,
        //C2 = 6,
    }


    // Write the method which calculates the sun of elements in the list
    public class LanguageNiveauService
    {
        public int SumOfElementsInList(List<LanguageNiveau> list)
        {
            int sum = 0;
            foreach (var item in list)
            {
                sum += item.ListItemId;
            }
            return sum;
        }
    }
}
