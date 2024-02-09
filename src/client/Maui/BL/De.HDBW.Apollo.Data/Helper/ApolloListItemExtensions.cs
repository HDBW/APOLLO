// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Lists;

namespace De.HDBW.Apollo.Data.Helper
{
    public static class ApolloListItemExtensions
    {
        public static TU? AsEnum<TU>(this ApolloListItem? item)
            where TU : Enum
        {
            if (item == null)
            {
                return default;
            }

            if (!Enum.IsDefined(typeof(TU), item.ListItemId))
            {
                return default;
            }

            return (TU)Enum.ToObject(typeof(TU), item.ListItemId);
        }

        public static ApolloListItem? ToApolloListItem<TU>(this TU? enumValue)
            where TU : Enum
        {
            if (enumValue == null)
            {
                return null;
            }

            return new ApolloListItem() { ListItemId = (int)(object)enumValue, Value = enumValue.ToString() };
        }
    }
}
