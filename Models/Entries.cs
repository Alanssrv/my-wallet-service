using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyWallet.Models;

public class Entry
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    [BsonElement("date")]
    public DateTime Date { get; set; }
    
    [BsonElement("value")]
    public decimal Value { get; set; }
    
    [BsonElement("category")]
    public ObjectId Category { get; set; }
    
    [BsonElement("description")]
    public string? Description { get; set; }
    
    [BsonElement("origin")]
    public OriginType Origin { get; set; }
    
    [BsonElement("tags")]
    public List<ObjectId>? Tags { get; set; }
}
