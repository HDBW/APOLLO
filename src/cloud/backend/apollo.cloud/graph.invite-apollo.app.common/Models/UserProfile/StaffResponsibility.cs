namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public enum StaffResponsibility
    {
        Unknown = 0,
        // bis 9 Mitarbeiter/innen
        LessThan10,
        // 10 - 49 Mitarbeiter/innen
        Between10And49,
        // 50 - 499 Mitarbeiter/innen
        Between50And499,
        // 500 und mehr Mitarbeiter/innen
        MoreThan499,
    }
}
