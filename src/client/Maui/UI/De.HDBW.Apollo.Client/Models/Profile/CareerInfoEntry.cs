// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Helper;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.Client.Models.Profile
{
    public partial class CareerInfoEntry : AbstractProfileEntry<CareerInfo>
    {
        private CareerInfoEntry(
            CareerInfo data,
            Func<AbstractProfileEntry<CareerInfo>, Task> editHandle,
            Func<AbstractProfileEntry<CareerInfo>, bool> canEditHandle,
            Func<AbstractProfileEntry<CareerInfo>, Task> deleteHandle,
            Func<AbstractProfileEntry<CareerInfo>, bool> canDeleteHandle)
            : base(data, editHandle, canEditHandle, deleteHandle, canDeleteHandle)
        {
        }

        public static CareerInfoEntry Import(CareerInfo data, Func<AbstractProfileEntry<CareerInfo>, Task> editHandle, Func<AbstractProfileEntry<CareerInfo>, bool> canEditHandle, Func<AbstractProfileEntry<CareerInfo>, Task> deleteHandle, Func<AbstractProfileEntry<CareerInfo>, bool> canDeleteHandle)
        {
            return new CareerInfoEntry(data, editHandle, canEditHandle,  deleteHandle, canDeleteHandle);
        }

        protected override ObservableCollection<string> GetAdditionalLines(CareerInfo data)
        {
            var items = new List<string?>();
            items.Add(data.Description);
            items.Add(data.Job?.PreferedTerm?.FirstOrDefault());
            items.Add(data.Start.ToUIDate().ToShortDateString());
            items.Add(data.End.ToUIDate()?.ToShortDateString());
            items.Add(data.NameOfInstitution);
            items.Add(data.City);
            items.Add(data.Country);
            return new ObservableCollection<string>(items.Where(x => !string.IsNullOrWhiteSpace(x) && x != FirstLine).OfType<string>());
        }

        protected override string GetFristLine(CareerInfo data)
        {
            var items = new List<string?>();
            items.Add(data.Description);
            items.Add(data.Job?.PreferedTerm?.FirstOrDefault());
            items.Add(data.Start.ToUIDate().ToShortDateString());
            items.Add(data.End.ToUIDate()?.ToShortDateString());
            items.Add(data.NameOfInstitution);
            items.Add(data.City);
            items.Add(data.Country);
            return items.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? string.Empty;
        }
    }
}
