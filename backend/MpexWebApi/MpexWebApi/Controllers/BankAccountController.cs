using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MpexTestApi.Extensions;
using MpexWebApi.Core.Services;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels.BankAccount;

namespace MpexWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BankAccountController : BaseController
    {
        private readonly IBankAccountService bankAccountService;
        public BankAccountController(IBankAccountService bankAccountService) 
        { 
            this.bankAccountService = bankAccountService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> BankAccount(string? id)
        {
            Guid bankAccountGuidId = Guid.Empty;
            bool isIdValid = IsGuidValid(id, ref bankAccountGuidId);
            if (!isIdValid)
            {
                return BadRequest();
            }

            BankAccountViewModel? bankAccount = await bankAccountService
                .GetBankAccountAsync(bankAccountGuidId);

            if(bankAccount == null)
            {
                return NotFound();
            }

            var userId = User.GetId();
            if(userId == null || bankAccount.UserId.ToLower() != userId.ToString().ToLower())
            {
                return Forbid();
            }

            return Ok(bankAccount);
        }
    }
}
