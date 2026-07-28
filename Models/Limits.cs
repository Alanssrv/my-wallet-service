using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyWallet.Models;

public class Limits
{
    [BsonId]
    public ObjectId Id { get; set; }
    public LimitType Type { get; set; }
    public decimal AlertValue { get; set; }
    public decimal WarningValue { get; set; }
    public decimal MaxValue { get; set; }
}
