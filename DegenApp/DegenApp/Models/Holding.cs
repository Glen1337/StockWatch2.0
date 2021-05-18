using DegenApp.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DegenApp.Enums;
using System.Text.Json.Serialization;

namespace DegenApp.Models
{
    public class Holding
    {
        const int SHORT_LENGTH = 4;
        const int MEDIUM_LENGTH = 8;
        const int LONG_LENGTH = 16;

        [Key]
        public long HoldingId { get; set; }

        // Cost per share
        [Required]
        [Column(TypeName = "Money")]
        public decimal CostBasis { get; set; }

        [Required]
        public double Quantity { get; set; }

        // Ticker
        [Required]
        [MaxLength(Constants._Long, ErrorMessage = Constants._MediumMessage)]
        public string Symbol { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        public SecurityType SecurityType { get; set; }

        [Required]
        public OrderType OrderType { get; set; }

        [DefaultValue(false)]
        public bool ReinvestDivs { get; set; }

        public bool IsOpen { get; set; }

        [Column(TypeName = "Money")]
        public decimal CurrentPrice { get; set; }

        // FK
        public long PortfolioId { get; set; }

        [Column(TypeName = "Money")]
        public decimal StrikePrice { get; set; }

        public string ContractName { get; set; }

        public DateTime ExpirationDate { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Portfolio Portfolio { get; set; }
    }
}
