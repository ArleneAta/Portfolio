using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models
{
    public class SalesHistoryVM
    {
        [Key]
        public string Email { get; set; }
        public int PurchasedId { get; set; }
        public string Role { get; set; } 
        public string Title { get; set; }
        public int EventId { get; set; }
        [Display(Name = "Purchase Amount")]
        public decimal PurchaseAmount { get; set; }

    }
}
