using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MotorHub_Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Count { get; set; }
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
