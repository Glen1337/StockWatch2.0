using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DegenApp.Enums;

namespace DegenApp.Models
{
    public class WatchItem
    {
        [Key]
        public long WatchItemId { get; set; }

        [Required]
        public Outlook Outlook { get; set; }

        [Column(TypeName = "decimal(12,4)")]
        public decimal PriceChange { get; set; }

        [Column(TypeName = "decimal(12,4)")]
        public decimal PercentChange { get; set; }

        // Ticker
        [Required]
        [MaxLength(Constants._Medium, ErrorMessage = Constants._MediumMessage)]
        public string Symbol { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; }
    }
}
