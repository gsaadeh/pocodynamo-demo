using System;
using ServiceStack.DataAnnotations;

namespace PocoDynamoDemo.Models
{
    public class Customer : IRecord
    {
        [HashKey]
        public string HashKey { get; set; }
        [RangeKey]
        public string RangeKey { get; set; }
        public string Type { get; set; } 
        public string CustomerId { get; private set; }
         public string Name { get; private set; }
         
         protected Customer() {}

         public Customer(string name)
         {
             CustomerId = Guid.NewGuid().ToString("D");
             Name = name;
             HashKey = $"CUSTOMER#{CustomerId}";
             RangeKey = $"CUSTOMER#{CustomerId}";
             Type = "CUSTOMER";
         }
    }
}