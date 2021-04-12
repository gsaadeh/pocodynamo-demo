using System;
using ServiceStack.DataAnnotations;

namespace PocoDynamoDemo.Models
{
    public class Order : IRecord
    {
        [HashKey]
        public string HashKey { get; set; }
        [RangeKey]
        public string RangeKey { get; set; }
        public string Type { get; set; }
        public string OrderId { get; private set; }
        public double OrderTotal { get; private set; }
        public DateTime CreatedDate { get; private set; }
        
        protected Order() {}

        public Order(string customerId, double orderTotal)
        {
            OrderId = Guid.NewGuid().ToString("D");
            OrderTotal = orderTotal;
            CreatedDate = DateTime.Now;
            HashKey = $"CUSTOMER#{customerId}";
            RangeKey = $"ORDER#{OrderId}";
            Type = "ORDER";
        }
    }
}