using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserTopUpService.Domain;
using UserTopUpService.Repositories;
using FluentValidation;
using UserTopUpService.Validation;
namespace UserTopUpService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopUpController : ControllerBase
    {
        private readonly ITopUpService _topUpService;

        public TopUpController(ITopUpService topUpService)
        {
            _topUpService = topUpService;
        }

        [HttpPost("beneficiaries")]
        public async Task<IActionResult> AddBeneficiary([FromBody] Beneficiary request)
        {
            var validator = new BeneficiaryValidator();
            validator.ValidateAndThrow(request);
            var result = await _topUpService.AddBeneficiaryAsync(request);
            return Ok(result);
        }

        [HttpGet("beneficiaries")]
        public async Task<IActionResult> GetBeneficiaries(int UserID)
        {
            var beneficiaries = await _topUpService.GetBeneficiariesAsync(UserID);
            return Ok(beneficiaries);
        }

        [HttpGet("topup-options")]
        public IActionResult GetTopUpOptions()
        {
            var options = _topUpService.GetTopUpOptions();
            return Ok(options);
        }

        [HttpPost("topup")]
        public async Task<IActionResult> TopUp([FromBody] TopUpRequest request)
        {
            var result = await _topUpService.TopUpAsync(request);
            return Ok(result);
        }

        [HttpPost("AddUserss")]
        public async Task<IActionResult> AddUsers([FromBody] User request)
        {
            var result = await _topUpService.AddUserAsync(request);
            return Ok(result);
        }
    }

}
