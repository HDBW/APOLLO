namespace Invite.Apollo.App.Graph.Assessment.Data
{
    internal class BstAssessment
    {
        public string HTMLDistractorPrimary_1;
        public string HTMLDistractorPrimary_2;
        public string HTMLDistractorPrimary_3;
        public string HTMLDistractorPrimary_4;
        public string ScoringOption_1;
        public string HTMLDistractorSecondary_1;
        public string HTMLDistractorSecondary_2;
        public string HTMLDistractorSecondary_3;
        public string HTMLDistractorSecondary_4;
        public int Credit_ScoringOption_1;


        /// <summary>
        /// Unique Identifier of the Skill Assessment
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Identifier of the itemType
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// ItemStem Text
        /// </summary>
        public string ItemStem { get; set; }

        /// <summary>
        /// The Image of the Resource
        /// </summary>
        public string ImageResourceName1 { get; set; }

        /// <summary>
        /// Instruction map to MetaDataType.Hint on Apollo
        /// </summary>
        public string Instruction { get; set; }

        public string NumberOfPrimaryDisctrators { get; set; }
        public int NumberSelectable { get; set; }
        public string DescriptionOfProfession { get; set; }
        public string Kldb { get; set; }
        public string DescriptionOfPartialQualification { get; set; }
        public string DescriptionOfWorkingProcess { get; set; }

        public string Description { get; set; }
        public string AssessmentType { get; set; }
        public string Disclaimer { get; set; }
        public string Duration { get; set; }
        public string EscoOccupationId { get; set; }
        public string EscoSkills { get; set; }
        public string Publisher { get; set; }
        public string Title { get; set; }
    }
}
