namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class Category : BaseItem
    {

        public string Title { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Threshold
        /// TODO: Remove before DataBase Creation
        /// </summary>
        public int ResultLimit { get; set; }
        
        public string Description { get; set; } = string.Empty;

        //Moved to CategoryRecomendation
        public long CourseId { get; set; }

        public List<Question> Questions { get; set; }

        public string EscoId { get; set; }

        public string Minimum { get; set; } = string.Empty;
        public string Maximum { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public List<CategoryRecomendation> CategoryRecomendations { get; set; }
    }
}
