using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DegenApp.Models
{
    public class User
    {
        [Key]
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; }

        [Column(TypeName = "Money")]
        public decimal Balance { get; set; }
    }
}
