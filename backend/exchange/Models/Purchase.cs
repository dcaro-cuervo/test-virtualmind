using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace exchange.Models
{
    public class Purchase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        [Required(ErrorMessage ="User is required")]
        [StringLength(15, ErrorMessage ="User can't be longer that 15 characters")]
        public string UserId { get; set; }

        [BsonElement("Amount")]
        [Required(ErrorMessage = "Amount is required")]
        [StringLength(6, ErrorMessage = "Amount can't be longer that 6 characters")]
        public double Amount { get; set; }

        [BsonElement("Currency")]
        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; }

        [BsonElement("Month")]
        [Required]
        public int Month { get; set; }

        [BsonElement("Year")]
        [Required]
        public int Year { get; set; }
    }
}
