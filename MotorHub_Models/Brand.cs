using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MotorHub_Models
{
    public class Brand
    {
        [Key]
        public int Brandid { get; set; }

        [Required]
        public string BrandName { get; set; }

        [Required]
        public int BrandLevel { get; set; }

        public string? BrandLogo { get; set; }

        // === Navigation Property ===
        public ICollection<Car>? Cars { get; set; }
    }
}
