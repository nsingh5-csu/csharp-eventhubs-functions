using System;

namespace Company.Function.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public int Cost { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
