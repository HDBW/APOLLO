﻿// (c) Licensed to the HDBW under one or more agreements.
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
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.ServiceType.GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.VoluntaryServiceType.GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.WorkingTimeModel.GetLocalizedString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.Description));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.Job?.PreferedTerm?.FirstOrDefault()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.Start.ToUIDate().ToShortDateString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.End.ToUIDate()?.ToShortDateString()));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.NameOfInstitution));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.City));
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_ContactType, data.Country));
            return new ObservableCollection<StringValue>(items.Where(x => !string.IsNullOrWhiteSpace(x.Data)));
        }

        private string? GetLocalizedString(string? v) => throw new NotImplementedException();
        private string? GetLocalizedString(CareerType careerType) => throw new NotImplementedException();
    }
}