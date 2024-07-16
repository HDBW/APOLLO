using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.SharedContracts.Helper
{
    public static class QuestionFactory
    {
        public static TU Create<TU>(RawData rawData, string rawDataId, string modulId, string assessmentId, CultureInfo culture)
            where TU : AbstractQuestion
        {
            var type = rawData.type;

            #pragma warning disable CS8603 // Possible null reference return.
            switch (type)
            {
                case QuestionType.EACONDITIONS:
                    return new Eacondition(rawData, rawDataId, modulId, assessmentId, culture) as TU;
                case QuestionType.CHOICE:
                    return new Choice(rawData, rawDataId, modulId, assessmentId, culture) as TU;
                case QuestionType.SORT:
                    return new Sort(rawData, rawDataId, modulId, assessmentId, culture) as TU;
                case QuestionType.ASSOCIATE:
                    return new Associate(rawData, rawDataId, modulId, assessmentId, culture) as TU;
                case QuestionType.IMAGEMAP:
                    return new Imagemap(rawData, rawDataId, modulId, assessmentId, culture) as TU;
                case QuestionType.EAFREQUENCY:
                    return new Eafrequency(rawData, rawDataId, modulId, assessmentId, culture) as TU;
                case QuestionType.RATING:
                    return new Rating(rawData, rawDataId, modulId, assessmentId, culture) as TU;
                case QuestionType.BINARY:
                    return new Binary(rawData, rawDataId, modulId, assessmentId, culture) as TU;
                case QuestionType.CLOZE:
                    return new Cloze(rawData, rawDataId, modulId, assessmentId, culture) as TU;
                default:
                    throw new NotSupportedException($"QuestionType {rawData?.type} not supported.");
            }
            #pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
