// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.SharedContracts.Models
{
    public class Favorite
    {
        public Favorite(string id, string type)
        {
            Id = id;
            Type = type;
        }

        public string Id { get; set; }

        public string Type { get; set; }
    }
}
