// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public interface ICalculateScore
    {
        double CalculateScore(object selection);
    }

    public interface ICalculateScore<TIn> : ICalculateScore
    {
        double ICalculateScore.CalculateScore(object selection) => CalculateScore((TIn)selection);

        double CalculateScore(TIn selection);
    }
}
