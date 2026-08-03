using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyWallet.Models;

public class Limit
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("category")]
    public ObjectId Category { get; set; }

    [BsonElement("type")]
    public LimitType Type { get; set; }

    [BsonElement("warningValue")]
    public decimal WarningValue { get; set; }

    [BsonElement("cautionValue")]
    public decimal CautionValue { get; set; }

    [BsonElement("criticalValue")]
    public decimal CriticalValue { get; set; }
}
