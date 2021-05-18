using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DegenApp.Models
{
    public class Portfolio
    {
        [Key]
        public long PortfolioId { get; set; }

        [Required]
        public string Title { get; set; }

        [Column(TypeName = "Money")]
        public decimal TotalMarketValue { get; set; }

        public DateTime CreationDate { get; set; }

        public string Type { get; set; }

        [Column(TypeName = "Money")]
        public decimal GainLoss { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Holding> Holdings { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; }
    }
}
