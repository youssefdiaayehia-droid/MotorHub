using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MotorHub_Models
{
    public class Car
    {
        [Key]
        public int Carid { get; set; }

        [Required]
        public string CarName { get; set; }

        [Required]
        public string CarMadein { get; set; }

        public string? CarImage { get; set; }

        [Required]
        public decimal CarPrice { get; set; }

        [Required]
        public string CarType { get; set; }

        public int? Brandid { get; set; }

        [ForeignKey("Brandid")]
        public Brand? Brand { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}