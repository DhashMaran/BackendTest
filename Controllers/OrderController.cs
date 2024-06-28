using Microsoft.AspNetCore.Mvc;
using BackendTest.Models;
using BackendTest.Services;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace BackendTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly string _connectionString;

       
        public OrderController(IConfiguration configuration)
        {
            _orderService = new OrderService();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET api to fetch items from the database
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var items = connection.Query<Item>("SELECT * FROM Items").ToList();
                return Ok(items); 
            }
        }

        // POST api to place an order and to split
        [HttpPost("placeorder")]
        public IActionResult PlaceOrder([FromBody] List<int> itemIds)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var items = connection.Query<Item>("SELECT * FROM Items WHERE Id IN @Ids", new { Ids = itemIds }).ToList();
                var packages = _orderService.SplitOrderIntoPackages(items); // Split the order into packages
                return Ok(packages); 
            }
        }
    }
}
