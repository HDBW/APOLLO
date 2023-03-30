using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.VisualBasic;
using NUnit.Framework;

namespace Invite.Apollo.App.Graph.Common.Test
{
    //[TestFixture]
    //public class AssessmentDictonaryTest
    //{

    //    public List<AssessmentItem> assessemts = new List<AssessmentItem>(){
    //        new AssessmentItem()
    //        {
    //            AssessmentType = AssessmentType.SkillAssessment,
    //            Description = "Schwarze Schafe zählen doppelt",
    //            Disclaimer = "Zu viele Schafe verderben den Schlaf",
    //            Duration = TimeSpan.Zero,
    //            EscoOccupationId = String.Empty,
    //            Id = 1,
    //            Kldb = "Klo loch durch boden",
    //            Profession = "Sandmann",
    //            Publisher = "DC Comics",
    //            Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
    //            Ticks = DateTime.Now.Ticks,
    //            Title = "Schafe zählen SK Assessment"
    //        },
    //        new AssessmentItem()
    //        {
    //            AssessmentType = AssessmentType.SkillAssessment,
    //            Description = "Scha(r)fe Pixel lizenfrei anschauen",
    //            Disclaimer = "Anfassen kostet extra",
    //            Duration = TimeSpan.Zero,
    //            EscoOccupationId = String.Empty,
    //            Id = 2,
    //            Kldb = "Klo loch durch boden",
    //            Profession = "Punisher",
    //            Publisher = "DC Comics",
    //            Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
    //            Ticks = DateTime.Now.Ticks,
    //            Title = "Lizenzfreie Schafe für Developer"
    //        },
    //        new AssessmentItem()
    //        {
    //            AssessmentType = AssessmentType.SoftSkillAssessment,
    //            Description = "Sofort soft",
    //            Disclaimer = "Vorsicht vor Erbsen",
    //            Duration = TimeSpan.Zero,
    //            EscoOccupationId = String.Empty,
    //            Id = 4,
    //            Kldb = "Klo loch durch boden",
    //            Profession = "Punisher",
    //            Publisher = "DC Comics",
    //            Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
    //            Ticks = DateTime.Now.Ticks,
    //            Title = "Schlafen auf soften schafwollekissen für Harte Kerle"
    //        }
  

    //    };

    //    public List<AssessmentItem> BoeseAssessmentItems = new List<AssessmentItem>(){
    //        new AssessmentItem()
    //        {
    //            AssessmentType = AssessmentType.SkillAssessment,
    //            Description = "Doppelt",
    //            Disclaimer = "Doppelt",
    //            Duration = TimeSpan.Zero,
    //            EscoOccupationId = String.Empty,
    //            Id = 5,
    //            Kldb = "Klo loch durch boden",
    //            Profession = "Punisher",
    //            Publisher = "DC Comics",
    //            Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
    //            Ticks = DateTime.Now.Ticks,
    //            Title = "Doppelt hält besser"
    //        },
    //        new AssessmentItem()
    //        {
    //        AssessmentType = AssessmentType.SkillAssessment,
    //        Description = "Negativ",
    //        Disclaimer = "Keine postive Stimmung",
    //        Duration = TimeSpan.Zero,
    //        EscoOccupationId = String.Empty,
    //        Id = -1,
    //        Kldb = "Klo loch durch boden",
    //        Profession = "Punisher",
    //        Publisher = "DC Comics",
    //        Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
    //        Ticks = DateTime.Now.Ticks,
    //        Title = "Negativ geladen"
    //    }

    //    };


    //    /// <summary>
    //    /// Test
    //    /// </summary>
    //    [Test]
    //    public void AddAssessment()
    //    {
    //        AssessmentDictonary ad = new AssessmentDictonary();

    //        ad.GetAnswerItems();
    //        int index = 0;
    //        foreach (AssessmentItem assessemt in assessemts)
    //        {

    //            var result = ad.AddAssessmentItem(assessemt);
               
    //            index++;

    //        }

    //        foreach (AssessmentItem assessemt in BoeseAssessmentItems)
    //        {
    //            //Assert.That(ad.AddAssessmentItem(assessemt));

    //        }

    //    }


    //    /// <summary>
    //    /// Test
    //    /// </summary>
    //    [Test]
    //    public void AddQuestion()
    //    {

    //    }

    //    /// <summary>
    //    /// Usecase Choice
    //    /// </summary>
    //    [Test]
    //    public void AddMetaDataItem()
    //    {

    //    }

    //}
}
