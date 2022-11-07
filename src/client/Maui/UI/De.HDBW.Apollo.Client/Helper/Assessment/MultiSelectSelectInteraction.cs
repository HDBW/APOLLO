// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Helper.Assessment
{
    public class MultiSelectSelectInteraction : AbstractSelectInteraction, IAnswerItemInteraction
    {
        public MultiSelectSelectInteraction(QuestionEntry question, ILogger logger)
            : base(question, logger)
        {
        }
    }
}
