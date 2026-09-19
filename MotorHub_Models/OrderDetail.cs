using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorHub_Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderHeaderId { get; set; }

        [ForeignKey("OrderHeaderId")]
        public OrderHeader OrderHeader { get; set; }

        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }
    }
}