// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;

namespace De.HDBW.Apollo.Client.Helper
{
    public static class QuestionTypeExtensions
    {
        public static string? ToRoute(this QuestionType type)
        {
            switch (type)
            {
                case QuestionType.EACONDITIONS:
                    return Routes.EaconditionView;
                case QuestionType.CHOICE:
                    return Routes.ChoiceView;
                case QuestionType.SORT:
                    return Routes.SortView;
                case QuestionType.ASSOCIATE:
                    return Routes.AssociateView;
                case QuestionType.IMAGEMAP:
                    return Routes.ImagemapView;
                case QuestionType.EAFREQUENCY:
                    return Routes.EafrequencyView;
                case QuestionType.RATING:
                    return Routes.RatingView;
                case QuestionType.BINARY:
                    return Routes.BinaryView;
                case QuestionType.CLOZE:
                    return Routes.ClozeView;
                default:
                    return null;
            }
        }
    }
}
