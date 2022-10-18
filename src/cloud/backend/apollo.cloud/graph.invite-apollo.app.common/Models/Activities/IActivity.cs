using System;
using System.Collections.Generic;
using System.Text;

namespace Invite.Apollo.App.Graph.Common.Models.Activities
{
    public interface IActivity
    {
        public abstract string ActivityText();

        public abstract string ActivityIcon();

        public DateTime DueDate { get; set; }

        public DateTime Reminder { get; set; }

        public ActivityReminderType ActivityReminderType { get; set; }

        public ActivityType ActivityType { get; set; }
    }

    public enum ActivityReminderType
    {
        None = 0,
        Snooze = 1,
        Reminder = 2
    }
}
