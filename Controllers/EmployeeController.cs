using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PocoDynamoDemo.Models;
using ServiceStack.Aws.DynamoDb;

namespace PocoDynamoDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IPocoDynamo _db;

        public EmployeeController(IPocoDynamo db)
        {
            _db = db;
        }
        
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create(string name, string departmentId)
        {
            var employee = new Employee(name, departmentId);
            
            _db.PutItem(employee);

            return CreatedAtAction(nameof(GetById), new {id = employee.Id}, employee);
        }
        
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetById(string id)
        {
            var employee = _db
                .GetItem<Employee>(id);

            return Ok(employee);
        }
    }
}