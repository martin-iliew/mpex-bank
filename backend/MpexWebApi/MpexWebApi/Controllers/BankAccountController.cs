using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MpexTestApi.Extensions;
using MpexWebApi.Core.Services;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels.BankAccount;
using MpexWebApi.Core.ViewModels.Card;
using MpexWebApi.Infrastructure.Data.Models;

namespace MpexWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService bankAccountService;
        public BankAccountController(IBankAccountService bankAccountService) 
        { 
            this.bankAccountService = bankAccountService;
        }

        [HttpGet("bankAccount/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> BankAccount(string? id)
        {

            if (!Guid.TryParse(id, out Guid bankAccountGuidId))
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

        [HttpGet("card/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Card(string? id)
        {
            if (!Guid.TryParse(id, out Guid cardId))
            {
                return BadRequest();
            }

            DebitCardViewModel? card = await bankAccountService
                .GetCardAsync(cardId);

            if (card == null)
            {
                return NotFound();
            }

            var userId = User.GetId();
            if (userId == null || card.UserId.ToLower() != userId.ToString().ToLower())
            {
                return Forbid();
            }

            return Ok(card);
        }


        [HttpPost("{bankAccountId}/create-card")]
        public async Task<IActionResult> CreateCard(string? bankAccountId)
        {
            if (!Guid.TryParse(bankAccountId, out Guid bankAccountGuidId))
            {
                return BadRequest();
            }
            var bankAccount = await bankAccountService
                .GetBankAccountAsync(bankAccountGuidId);
            
            if (bankAccount == null)
            {
                return BadRequest();
            }

            var userId = User.GetId();
            if (userId == null || bankAccount.UserId.ToString().ToLower() !=
                userId.ToString().ToLower())
            {
                return Forbid();
            }

            bool result = await bankAccountService.CreateCardAsync(bankAccountGuidId);

            if (result == false)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
