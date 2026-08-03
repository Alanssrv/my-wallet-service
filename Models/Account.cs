using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MyWallet.Models;

public class Account
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement("reference")]
    public DateTime? Reference { get; set; } // if is null is general account

    [BsonElement("financialSummaries")]
    public required List<FinancialSummary> FinancialSummaries { get; set; }

    public class FinancialSummary
    {
        [BsonElement("origin")]
        public OriginType? Origin { get; set; } // if is null is general account for the reference

        [BsonElement("income")]
        public decimal Income { get; set; }

        [BsonElement("expense")]
        public decimal Expense { get; set; }

        [BsonElement("balance")]
        public decimal Balance { get; set; }
    }
}
