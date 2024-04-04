using BankManagementSystem.BackendWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;


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
            TransactionHistoryModel transactionHistory = _db.TransactionHistories.FirstOrDefault(item => item.TransactionHistoryId == id);
            if (transactionHistory == null)
            {
                return NotFound("No Data Found");
            }
            return Ok(transactionHistory);
        }

        [HttpPost]
        public IActionResult CreateTransactionHistory(TransactionHistoryViewModel transactionHistory
            )
        {
            TransactionHistoryModel transactionHistoryModel = new TransactionHistoryModel()
            {
                TransactionHistoryId = transactionHistory.TransactionHistoryId,
                TransactionDate = transactionHistory.TransactionDate,
                FromAccountNo = transactionHistory.FromAccountNo,
                ToAccountNo = transactionHistory.ToAccountNo,
                Amount = transactionHistory.Amount
            };

            _db.TransactionHistories.Add(transactionHistoryModel);
            int result = _db.SaveChanges();

            if(result==0)
            {
                 //return Ok("Transfer Failed. Transsaction Save Failed");
                
            }

            AccountModel fromAccount = _db.Accounts.FirstOrDefault(item => item.AccountId == transactionHistory.FromAccountId);

            if (fromAccount==null)
            {
                return NotFound("Transfer Failed. From Account Code is not found");
            }

            if(fromAccount.Balance < transactionHistory.Amount || fromAccount.Balance==0)
            {
                return Ok("Transfer Failed. From Account Code is not enough");
            }
            fromAccount.Balance = fromAccount.Balance-transactionHistory.Amount;
            int result2 = _db.SaveChanges();
            if(result2==0)
            {
                return NotFound("Transfer Failed. To Account Code is not found");
            }

            AccountModel toAccount = _db.Accounts.FirstOrDefault(item => item.AccountId == transactionHistory.ToAccountId);
            if (toAccount == null)
            {                
                return NotFound("Transfer Failed. To Account Code is not found");
            }
            toAccount.Balance = toAccount.Balance + transactionHistory.Amount;
            int result3 = _db.SaveChanges();

            return Ok("Transfer Successful.");
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
