using BankManagementSystem.BackendWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankManagementSystem.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDBContext _db;

        public AccountController()
        {
            _db = new AppDBContext();
        }

        [HttpGet]
        public IActionResult GetAccounts()
        {
            List<AccountModel> accounts = _db.Accounts.ToList();

            return Ok(accounts);
        }


        [HttpGet("{id}")]
        public IActionResult GetAccounts(int id)
        {
            AccountModel account = _db.Accounts.FirstOrDefault(item => item.AccountId == id);
            if (account == null)
            {
                return NotFound("No Data Found");
            }
            return Ok(account);
        }

        [HttpPost]
        public IActionResult CreateAccount(AccountModel account)
        {
            _db.Accounts.Add(account);
            int result = _db.SaveChanges();
            string message = result > 0 ? "Saving Successful" : "Saving fail";
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, AccountModel accountUpdate)
        {
            var account = _db.Accounts.FirstOrDefault(item => item.AccountId == id);
            if (account == null)
            {
                return NotFound("No Data Found");
            }

            account.AccountNo = accountUpdate.AccountNo;
            account.CustomerName = accountUpdate.CustomerName;


            if (accountUpdate.TransactionStatus == 0)
            {
                account.Balance = accountUpdate.Balance;
            }
            else if (accountUpdate.TransactionStatus == 1)
            {
                //Plus preivous amount with current amount 
                account.Balance = account.Balance + accountUpdate.Balance;
            }
            else if (accountUpdate.TransactionStatus == 2)
            {
                if (account.Balance == 0 || account.Balance < accountUpdate.Balance)
                {
                    return NotFound("Balance is not enough.");
                }

                // preivous amount -current amount
                account.Balance = account.Balance - accountUpdate.Balance;
            }


            int result = _db.SaveChanges();
            string message = result > 0 ? "Update Successful" : "Update fail";
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            var account = _db.Accounts.FirstOrDefault(item => item.AccountId == id);
            if (account == null)
            {
                return NotFound("No Data Found");
            }

            _db.Remove(account);
            int result = _db.SaveChanges();
            string message = result > 0 ? "Delete Successful" : "Delete fail";
            return Ok(message);
        }

        [Route("Deposit")]
        [HttpPost]
        public IActionResult Deposit()
        {
            return Ok();
        }

        [Route("Withdraw")]
        [HttpPost]
        public IActionResult Withdraw()
        {
            return Ok();
        }
    }
}
