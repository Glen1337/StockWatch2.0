using DegenApp.Attributes;
using DegenApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DegenApp.Models
{
    public class Order
    {

        [Key]
        public long OrderId { get; set; }

        [Required]
        public string Symbol { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Column(TypeName = "Money")]
        public decimal Price { get; set; }

        public OrderType OrderType { get; set; }

        public SecurityType SecurityType { get; set; }

        [Required]
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; }

        public long PortfolioId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Portfolio Portfolio { get; set; }
    }
}
