using System;
using System.Security.Cryptography;
using ServiceStack.DataAnnotations;

namespace PocoDynamoDemo.Models
{
    public class Employee
    {
        [HashKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public string DepartmentId { get; set; }
        
        public Employee() {}

        public Employee(string name, string departmentId)
        {
            Id = Guid.NewGuid().ToString("D");
            Name = name;
            DepartmentId = departmentId;
        }
    }
}