using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DegenApp.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DegenApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBalanceController : ControllerBase
    {
        private readonly ILogger<PortfoliosController> _logger;
        private readonly Data.ApplicationDbContext _context;

        public UserBalanceController(Data.ApplicationDbContext context, ILogger<PortfoliosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/UserBalance
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Decimal>> Get()
        {
            string accountID = await GetCurrentUserIdAsync();

            User currUser = await _context.Users.FindAsync(accountID);

            return currUser.Balance;
        }

        private async Task<string> GetCurrentUserIdAsync()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)this.User.Identity;

            Claim userClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            User foundUser = await _context.Users.FindAsync(userClaim.Value);

            if (foundUser == null && userClaim != null)
            {
                var newId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _context.AddAsync<User>(new Models.User { UserId = newId, Balance = 10000m });
                await _context.SaveChangesAsync();
            }

            return userClaim.Value;
        }

        // GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

    }
}
