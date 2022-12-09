using System.Runtime.Serialization;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Esco
{

    [DataContract]
    public class Skill : BaseItem
    {
       public string Value { get; set; }
    }
}
