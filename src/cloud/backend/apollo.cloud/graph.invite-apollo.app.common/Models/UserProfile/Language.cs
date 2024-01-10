using System.Globalization;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class Language
    {
        public string Name { get; set; }

        public LanguageNiveau? Niveau { get; set; }

        // CultureInfo get Culture ISO639-2 
        public CultureInfo Code { get; set; }
    }
}
