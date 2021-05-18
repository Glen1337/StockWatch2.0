using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DegenApp.Data;
using DegenApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace DegenApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ApplicationDbContext context, ILogger<OrdersController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/Orders
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            _logger.Log(LogLevel.Information, "In Orders Get");

            string accountID = await GetCurrentUserIdAsync();

            var orders = await _context.Orders
                .Where(order => String.Equals(order.UserId, accountID))
                .OrderByDescending(order => order.TransactionDate)
                .ToListAsync();
            return orders;
        }

        private async Task<string> GetCurrentUserIdAsync()
        {
            String userId = "";

            ClaimsPrincipal currUser = this.User;
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)currUser.Identity;

            Claim userClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            User foundUser = await _context.Users.FindAsync(userClaim.Value);
            if (foundUser == null && userClaim != null)
            {
                var newId = currUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _context.AddAsync<User>(new Models.User { UserId = newId, Balance = 10000m });
                await _context.SaveChangesAsync();
            }

            if (userClaim != null)
            {
                userId = userClaim.Value;
            }

            return userId;
        }

        #region unused code - these other action methods should not be called outside of API
        //GET: api/Orders/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Order>> GetOrder(long id)
        //{
        //    var order = await _context.Orders.FindAsync(id);

        //    if (order == null) { return NotFound(); }

        //    return order;
        //}

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754


        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOrder(long id, Order order)
        //{
        //    if (id != order.OrderId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Order>> PostOrder(Order order)
        //{
        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        //}


        // DELETE: api/Orders/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrder(long id)
        //{
        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool OrderExists(long id)
        //{
        //    return _context.Orders.Any(e => e.OrderId == id);
        //}
        #endregion
    }
}
