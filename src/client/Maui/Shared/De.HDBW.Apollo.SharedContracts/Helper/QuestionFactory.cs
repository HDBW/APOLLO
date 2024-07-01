using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.SharedContracts.Helper
{
    public static class QuestionFactory
    {
        public static TU Create<TU>(RawData rawData, CultureInfo culture)
            where TU : AbstractQuestion
        {
            var type = rawData.type;
            switch (type)
            {
                case QuestionType.EACONDITIONS:
                    return new Eacondition(rawData, culture) as TU;
                case QuestionType.CHOICE:
                    return new Choice(rawData, culture) as TU;
                case QuestionType.SORT:
                    return new Sort(rawData, culture) as TU;
                case QuestionType.ASSOCIATE:
                    return new Associate(rawData, culture) as TU;
                case QuestionType.IMAGEMAP:
                    return new Imagemap(rawData, culture) as TU;
                case QuestionType.EAFREQUENCY:
                    return new Eafrequency(rawData, culture) as TU;
                case QuestionType.RATING:
                    return new Rating(rawData, culture) as TU;
                case QuestionType.BINARY:
                    return new Binary(rawData, culture) as TU;
                case QuestionType.CLOZE:
                    return new Cloze(rawData, culture) as TU;
                default:
                    throw new NotSupportedException($"QuestionType {rawData?.type} not supported.");
            }
        }
    }
}
