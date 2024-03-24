using BankManagementSystem.BackendWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankManagementSystem.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        private readonly AppDBContext _db;

        public TransactionHistoryController()
        {
            _db = new AppDBContext();
        }

        [HttpGet]
        public IActionResult GetTransactionHistories()
        {
            List<TransactionHistoryModel> transactionHistories = _db.TransactionHistories.ToList();

            return Ok(transactionHistories);
        }

        [HttpGet("{id}")]
        public IActionResult GetTransactionHistory(int id)
        {
            TransactionHistoryModel transactionHistory = _db.TransactionHistories.FirstOrDefault(item => item.TransactionHistoryId == id)!;
            if (transactionHistory == null)
            {
                return NotFound("No Data Found");
            }
            return Ok(transactionHistory);
        }

        [HttpPost]
        public IActionResult CreateTransactionHistory(TransactionHistoryModel transactionHistory)
        {
            _db.TransactionHistories.Add(transactionHistory);
            int result = _db.SaveChanges();
            string message = result > 0 ? "Saving Successful" : "Saving fail";
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTransactionHistory(int id, TransactionHistoryModel transactionHistoryUpdate)
        {
            var transactionHistory = _db.TransactionHistories.FirstOrDefault(item => item.TransactionHistoryId == id);
            if (transactionHistory == null)
            {
                return NotFound("No Data Found");
            }

            transactionHistory.FromAccountNo = transactionHistoryUpdate.FromAccountNo;
            transactionHistory.ToAccountNo = transactionHistoryUpdate.ToAccountNo;
            transactionHistory.TransactionDate = transactionHistoryUpdate.TransactionDate;
            transactionHistory.Amount = transactionHistoryUpdate.Amount;

            int result = _db.SaveChanges();
            string message = result > 0 ? "Update Successful" : "Update fail";
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransactionHistory(int id)
        {
            var transactionHistory = _db.TransactionHistories.FirstOrDefault(item => item.TransactionHistoryId == id);
            if (transactionHistory == null)
            {
                return NotFound("No Data Found");
            }

            _db.Remove(transactionHistory);
            int result = _db.SaveChanges();
            string message = result > 0 ? "Delete Successful" : "Delete fail";
            return Ok(message);
        }
    }
}
