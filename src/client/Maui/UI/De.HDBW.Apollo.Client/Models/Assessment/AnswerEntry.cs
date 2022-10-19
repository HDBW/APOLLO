// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class AnswerEntry : ObservableObject
    {
        private AnswerItem _answerItem;
        private IEnumerable<MetaDataItem> _answerMetaDataItems;

        private AnswerEntry(AnswerItem answerItem, IEnumerable<MetaDataItem> answerMetaDataItems)
        {
            ArgumentNullException.ThrowIfNull(answerItem);
            ArgumentNullException.ThrowIfNull(answerMetaDataItems);
            _answerItem = answerItem;
            _answerMetaDataItems = answerMetaDataItems;
        }

        public static AnswerEntry Import(AnswerItem answerItem, IEnumerable<MetaDataItem> answerMetaDataItems)
        {
            return new AnswerEntry(answerItem, answerMetaDataItems);
        }
    }
}
