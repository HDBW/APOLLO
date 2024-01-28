// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models.Generic;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

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

        protected override ObservableCollection<StringValue> GetAllLines(CareerInfo data)
        {
            var items = new List<StringValue>();
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.CareerType.GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ServiceType, data.ServiceType.GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ServiceType, data.VoluntaryServiceType.GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_WorkTimeModel, data.WorkingTimeModel.GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Description, data.Description));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Occupation, data.Job?.PreferedTerm?.FirstOrDefault()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_DateRange, GetDateRangeText(data.Start, data.End)));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_NameOfInstitution, data.NameOfInstitution));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_City, data.City));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Country, data.Country));
            return new ObservableCollection<StringValue>(items.Where(x => !string.IsNullOrWhiteSpace(x.Data)));
        }
    }
}
