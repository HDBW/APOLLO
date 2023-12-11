// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using MongoDB.Bson.Serialization.Attributes;

public interface IContent
{
    
    public DateTime PublishingDate { get; set; }
    
    public DateTime UnpublishingDate { get; set; }
    
    public string? Successor { get; set; }
    
    public string? Predecessor { get; set; }
}
