using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Models;
using Wallet.Services.Contract;

namespace Wallet.Controllers
{
    public class TransactionListController : Controller
    {
        private readonly ITransactionService _transactionService;
        private IMapper _mapper;

        public TransactionListController(ITransactionService transactionService, IMapper mapper)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }

        [Authorize]
        [HttpGet("transactionlist/model")]
        public async Task<ActionResult<TransactionListModel>> GetTransactionListModel()
        {
            try
            {
                var user = User.Identity.Name;

                var transactionListModel = await _transactionService.GetTransactionListModel(user);

                if (transactionListModel != null)
                {
                    return Json(Ok(transactionListModel));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
