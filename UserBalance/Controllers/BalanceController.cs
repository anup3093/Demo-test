using Microsoft.AspNetCore.Mvc;
using UserBalance.Domain;
using UserBalance.Repositories;

namespace UserBalance.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly IUserBalanceService _userBalanceService;

        public BalanceController(IUserBalanceService userBalanceService)
        {
            _userBalanceService = userBalanceService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<int>> GetBalance(int userId)
        {
            var balance = await _userBalanceService.GetUserBalanceAsync(userId);
            return Ok(balance);
        }

        [HttpPost("{userId}/debit")]
        public async Task<ActionResult> DebitBalance(int userId, int amount)
        {
            await _userBalanceService.DebitUserBalanceAsync(userId, amount);
            return Ok();
        }

        [HttpPost("{userId}/credit")]
        public async Task<ActionResult> CreditBalance(int userId, int amount)
        {
            await _userBalanceService.CreditUserBalanceAsync(userId, amount);
            return Ok();
        }
        [HttpPost("AddUsers")]
        public async Task<IActionResult> AddUsers([FromBody] User request)
        {
            await _userBalanceService.AddUserAsync(request);
            return Ok();
        }
    }
}
