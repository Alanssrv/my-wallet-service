using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyWallet.Models;

public class Categories
{
    [BsonId]
    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public EntryType Type { get; set; }
}
