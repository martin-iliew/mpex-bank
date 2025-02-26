using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MpexTestApi.Extensions;
using MpexWebApi.Core.Services;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels.BankAccount;
using MpexWebApi.Core.ViewModels.Card;
using MpexWebApi.Infrastructure.Data.Models;
using System.Security.Claims;

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

        [HttpGet("{id}")]
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AllBankAccounts()
        {

            var userId = User.GetId();
            if (userId == null)
            {
                return Unauthorized();
            }

            if (!Guid.TryParse(userId, out Guid userIdGuid))
            {
                return BadRequest();
            }

            IEnumerable<AllBankAccountViewModel?> allBankAccounts = await bankAccountService
                .GetAllBankAccountAsync(userIdGuid);

            return Ok(allBankAccounts);
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

            return Created();
        }

        [HttpPost("{bankAccountId}/deposit")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Deposit(string bankAccountId, [FromBody] DepositRequest request)
        {
            if (!Guid.TryParse(bankAccountId, out var accountId))
            {
                return BadRequest();
            }

            var userId = User.GetId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var bankAccount = await bankAccountService.GetBankAccountAsync(accountId);
            if (bankAccount == null || bankAccount.UserId.ToLower().ToString() != userId.ToLower())
            {
                return Forbid();
            }

            if (request.Amount < 10)
            {
                return BadRequest("Deposit amount must be at least 10€.");
            }

            var success = await bankAccountService.Deposit(accountId, request.Amount);
            if (!success)
            {
                return NotFound("Bank account not found.");
            }

            return Ok("Deposit successful.");
        }
    }
}
