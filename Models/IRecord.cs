using System;
using ServiceStack.DataAnnotations;

namespace PocoDynamoDemo.Models
{
    public interface IRecord
    {
        public string HashKey { get; set; }
        public string RangeKey { get; set; }
        public string Type { get; set; }
    }
}