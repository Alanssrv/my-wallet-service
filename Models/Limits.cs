using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyWallet.Models;

public class Limits
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement("type")]
    public LimitType Type { get; set; }
    [BsonElement("alertValue")]
    public decimal AlertValue { get; set; }
    [BsonElement("warningValue")]
    public decimal WarningValue { get; set; }
    [BsonElement("maxValue")]
    public decimal MaxValue { get; set; }
}
