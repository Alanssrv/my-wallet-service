using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyWallet.Models;

public class Entries
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement("date")]
    public DateTime Date { get; set; }
    [BsonElement("value")]
    public decimal Value { get; set; }
    [BsonElement("categoryId")]
    public int CategoryId { get; set; }
    [BsonElement("description")]
    public string? Description { get; set; }
    [BsonElement("origin")]
    public OriginType Origin { get; set; }
    [BsonElement("tagIds")]
    public List<int>? TagIds { get; set; }
}
