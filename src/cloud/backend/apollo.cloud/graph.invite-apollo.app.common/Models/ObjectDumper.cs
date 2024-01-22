// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text;
using Newtonsoft.Json;


namespace Invite.Apollo.App.Graph.Common.Models
{
    public static class ObjectDumper
    {
        public static string Dump(this object obj)
        {
            StringBuilder sb = new StringBuilder();

            if (obj==null)
            {
                sb.AppendLine("Object is null");
            }

            sb.AppendLine($"Hash: {obj.GetHashCode()}");

            sb.AppendLine($"Type: {obj.GetType()}");
            sb.AppendLine("__________________________");
            sb.AppendLine(JsonConvert.SerializeObject(obj));
            //foreach (var property in obj.GetType().GetProperties())
            //{
            //    writer.WriteLine($"{property.Name} = {property.GetValue(obj)}");
            //}
            return sb.ToString();
        }
    }
}
