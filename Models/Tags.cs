using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyWallet.Models;

public class Tags
{
    [BsonId]
    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
}
