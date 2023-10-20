using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Wallet.Application.Models;
using Wallet.Data;
using Wallet.Models.Requests;
using Wallet.Models.Response;

namespace Wallet.Controllers.AdminPanel.CRUD
{
    public class TransactionCRUDController : Controller
    {
        private readonly ITransactionRepository _repo;
        private readonly IMapper _mapper;

        public TransactionCRUDController(ITransactionRepository repo, IMapper mapper)
        {
            _repo = repo;

            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/transaction/crud/list")]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetTransactionList()
        {
            try
            {
                var list = await _repo.GetTransactionListAsync();

                if ((list != null) && list.Any())
                {
                    var result = new List<TransactionResponse>();

                    foreach (var transaction in list)
                    {
                        var model = _mapper.Map<TransactionResponse>(transaction);

                        using (MemoryStream stream = new MemoryStream())
                        {
                            transaction.Icon.Save(stream);
                            model.Icon = stream.ToArray();
                        }
                        result.Add(model);
                    }
                    
                    return Json(Ok(result.AsEnumerable()));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/transaction/crud/{id}")]
        public async Task<ActionResult<TransactionResponse>> GetTransactionById(int id)
        {
            try
            {
                var transaction = await _repo.GetTransactionByIdAsync(id);

                if ((transaction != null))
                {
                    var model = _mapper.Map<TransactionResponse>(transaction);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        transaction.Icon.Save(stream);
                        model.Icon = stream.ToArray();
                    }

                    return Json(Ok(model));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/transaction/crud/getbyuserid/{userId}")]
        public async Task<ActionResult<List<TransactionResponse>>> GetTransactionByUserId(int userId)
        {
            try
            {
                var transactions = await _repo.GetTransactionByUserIdAsync(userId);

                if ((transactions != null) && transactions.Any())
                {
                    var result = new List<TransactionResponse>();

                    foreach (var transaction in transactions)
                    {
                        var model = _mapper.Map<TransactionResponse>(transaction);

                        using (MemoryStream stream = new MemoryStream())
                        {
                            transaction.Icon.Save(stream);
                            model.Icon = stream.ToArray();
                        }
                        result.Add(model);
                    }

                    return Json(Ok(result));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/transaction/crud/add")]
        public async Task<ActionResult> CreateTransaction(TransactionRequest request)
        {
            try
            {
                var result = _mapper.Map<Transaction>(request);

                var check = await _repo.Create(result);

                return Json(check);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("user/transaction/crud/update")]
        public async Task<ActionResult> UpdateTransaction(TransactionUpdateRequest request)
        {
            try
            {
                var result = _mapper.Map<Transaction>(request);

                await _repo.UpdateAsync(result);

                return Json(Ok());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("user/transaction/crud/{id}")]
        public async Task<ActionResult> DeleteTransactionByID(int id)
        {
            try
            {
                await _repo.DeleteAsync(id);

                return Json(Ok());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
