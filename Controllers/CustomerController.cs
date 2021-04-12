using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PocoDynamoDemo.Models;
using ServiceStack.Aws.DynamoDb;

namespace PocoDynamoDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IPocoDynamo _db;

        public CustomerController(IPocoDynamo db)
        {
            _db = db;
        }
        
        [HttpPost]
        public IActionResult Create(string name)
        {
            var customer = new Customer(name);
            
            _db.PutItem(customer);

            return CreatedAtAction(nameof(GetById), new {id = customer.CustomerId}, customer);
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(_db
                .FromQuery<Order>()
                .KeyCondition($"HashKey = :customerId  and RangeKey = :orderId", new Dictionary<string, string>()
                {
                    {"customerId", $"CUSTOMER#{id}"},
                    {"orderId", $"CUSTOMER#{id}"}
                })
                .Exec().SingleOrDefault());
        }
        
        [HttpPost]
        [Route("{id}/orders")]
        public IActionResult CreateOrder(string id, double total)
        {
            var order = new Order(id, total);
            
            _db.PutItem(order);

            return CreatedAtAction(nameof(GetOrder), new {id = id, orderId = order.OrderId}, order);
        }
        
        [HttpGet]
        [Route("{id}/orders")]
        public IActionResult GetOrders(string id)
        {
            return Ok(_db
                .FromQuery<Order>()
                .KeyCondition($"HashKey = :customerId  and begins_with(RangeKey, :orderId)", new Dictionary<string, string>()
                {
                    {"customerId", $"CUSTOMER#{id}"},
                    {"orderId", $"ORDER"}
                })
                .Exec().ToList());
        }
        
        [HttpGet]
        [Route("{id}/orders/{orderId}")]
        public IActionResult GetOrder(string id, string orderId)
        {
            return Ok(_db
                .FromQuery<Order>()
                .KeyCondition($"HashKey = :customerId  and RangeKey = :orderId", new Dictionary<string, string>()
                {
                    {"customerId", $"CUSTOMER#{id}"},
                    {"orderId", $"ORDER#{orderId}"}
                })
                .Exec().SingleOrDefault());
        }
    }
}