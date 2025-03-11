using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MpexTestApi.Extensions;
using MpexWebApi.Core.Services;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels.BankAccount;
using MpexWebApi.Core.ViewModels.Card;
using MpexWebApi.Core.ViewModels.Cards;
using MpexWebApi.Infrastructure.Constants.Enums;
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
        [HttpGet("/api/BankAccounts")]
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

        [HttpGet("cards")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AllCards()
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

            IEnumerable<AllCardsViewModel?> allBankAccounts = await bankAccountService
                .GetAllCardsAsync(userIdGuid);

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
            if(userId == null)
            {
                return Unauthorized();
            }

            if (bankAccount.UserId.ToString().ToLower() != userId.ToString().ToLower())
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

        [HttpPost("create-bank-account")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateBankAccount(int accountPlan, int accountType)
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

            if(!Enum.IsDefined(typeof(AccountPlans), accountPlan) ||
                !Enum.IsDefined(typeof(AccountPlans), accountType))
            {
                return BadRequest();
            }

            await bankAccountService.CreateBankAccountAsync(userIdGuid, accountPlan, accountType);

            return Ok();
        }

        [HttpPost("{bankAccountId}/deposit")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Deposit(string bankAccountId, [FromBody] TransactionRequest request)
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
                return BadRequest();
            }

            var success = await bankAccountService.Deposit(accountId, request.Amount);
            if (!success)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost("{bankAccountId}/withdraw")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Withdraw(string bankAccountId, [FromBody] TransactionRequest request)
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

            if (request.Amount <= 0)
            {
                return BadRequest();
            }

            var success = await bankAccountService.WithdrawAsync(accountId, request.Amount);
            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("{senderBankAccountId}/transfer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> TransferBetweenOwnAccounts(string senderBankAccountId, [FromBody]TransferRequest request)
        {
            if (!Guid.TryParse(senderBankAccountId, out var bankAccountIdGuid))
            {
                return BadRequest();
            }

            var userId = User.GetId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var bankAccount = await bankAccountService.GetBankAccountAsync(bankAccountIdGuid);
            if (bankAccount == null || bankAccount.UserId.ToLower() != userId.ToLower())
            {
                return Forbid();
            }

            if (String.IsNullOrEmpty(request.ReceiverIBAN) || request.Amount <= 0)
            {
                return BadRequest();
            }

            var success = await bankAccountService
                .TransferBetweenOwnAccounts(bankAccountIdGuid, request.ReceiverIBAN, request.Amount);
            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("{senderBankAccountId}/iban-transfer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> TransferToIBAN(string senderBankAccountId, [FromBody] TransferRequest request)
        {
            if (!Guid.TryParse(senderBankAccountId, out var senderBankAccountIdGuid))
            {
                return BadRequest();
            }

            var userId = User.GetId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var bankAccount = await bankAccountService.GetBankAccountAsync(senderBankAccountIdGuid);
            if (bankAccount == null || bankAccount.UserId.ToLower() != userId.ToLower())
            {
                return Forbid();
            }

            if (String.IsNullOrEmpty(request.ReceiverIBAN) || request.Amount <= 0)
            {
                return BadRequest();
            }

            var success = await bankAccountService
                .TransferToIBAN(senderBankAccountIdGuid, request.ReceiverIBAN, request.Amount);
            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
