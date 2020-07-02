using System;
using System.ComponentModel.DataAnnotations;

namespace exchange.Models
{
    public class PurchaseDto
    {
        [Required(ErrorMessage = "User is required")]
        [StringLength(15, ErrorMessage = "User can't be longer that 15 characters")]
        public string userId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [StringLength(6, ErrorMessage = "Amount can't be longer that 6 characters")]
        public string amount { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public string currency { get; set; }
    }
}
