using BankManagementSystem.FrontendMVCapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Mime;

namespace BankManagementSystem.FrontendMVCapp.Controllers
{
    public class TransactionHistoryController : Controller
    {
        private readonly string _api_url = "https://localhost:7112/api/";
        private AccountController _accountController = new AccountController();

        public async Task<List<TransactionHistoryModel>> GetTransactionAPI()
        {
            List<TransactionHistoryModel> list = new List<TransactionHistoryModel>();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_api_url + "TransactionHistory");
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonStr);


                list = JsonConvert.DeserializeObject<List<TransactionHistoryModel>>(jsonStr);

            }

            return list;

        }


        public async Task<TransactionHistoryModel> GetTransactionByIDAPI(int id)
        {
            TransactionHistoryModel list = new TransactionHistoryModel();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_api_url + "TransactionHistory/" + id);
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonStr);


                list = JsonConvert.DeserializeObject<TransactionHistoryModel>(jsonStr);

            }

            return list;

        }


        public async Task<bool> SaveTransactionAPI(TransactionHistoryModel transactionHistory)
        {


            string jsonTransactionHistory = JsonConvert.SerializeObject(transactionHistory);

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(jsonTransactionHistory, System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage response = await client.PostAsync(_api_url + "TransactionHistory", content);
            if (response.IsSuccessStatusCode)
            {
                return true;

            }

            return false;

        }


        public async Task<bool> UpdateTransactionAPI(int id, TransactionHistoryModel transactionHistory)
        {


            string josnTransactionHistory = JsonConvert.SerializeObject(transactionHistory);

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(josnTransactionHistory, System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage response = await client.PutAsync(_api_url + "TransactionHistory/" + id, content);
            if (response.IsSuccessStatusCode)
            {
                return true;

            }

            return false;

        }


        public async Task<bool> DeleteTransactionAPI(int id)
        {

            HttpClient client = new HttpClient();


            HttpResponseMessage response = await client.DeleteAsync(_api_url + "TransactionHistory/" + id);
            if (response.IsSuccessStatusCode)
            {
                return true;

            }

            return false;

        }


        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            //List<BlogModel> blogs = _db.Blogs.ToList();
            //get from API
            List<TransactionHistoryModel> transactions = await GetTransactionAPI();
            return View("Index", transactions);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            //   var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetTransactionByIDAPI(id);
            if (item is null)
            {
                return Redirect("/TransactionHistory");
            }

            List<AccountModel> accountList = await _accountController.GetAccountAPI();

            List<SelectListItem> fromItems = new List<SelectListItem>();
            List<SelectListItem> toItems = new List<SelectListItem>();

            fromItems.Add(new SelectListItem { Text = "Please select account code.", Value = "0" });
            toItems.Add(new SelectListItem { Text = "Please select account code.", Value = "0" });


            foreach (var i in accountList)
            {
                if (i.AccountNo != item.FromAccountNo)
                {
                    fromItems.Add(new SelectListItem { Text = i.AccountNo, Value = i.AccountId.ToString() });
                }
                else
                {
                    fromItems.Add(new SelectListItem { Text = i.AccountNo, Value = i.AccountId.ToString(), Selected = true });
                }


                if (i.AccountNo != item.ToAccountNo)
                {
                    toItems.Add(new SelectListItem { Text = i.AccountNo, Value = i.AccountId.ToString() });
                }
                else
                {
                    toItems.Add(new SelectListItem { Text = i.AccountNo, Value = i.AccountId.ToString(), Selected = true });
                }

            }



            ViewBag.FromAccountCode = fromItems;
            ViewBag.ToAccountCode = toItems;


            item.TransactionDate = Convert.ToDateTime(item.TransactionDate.ToShortDateString());
            return View("Edit", item);
        }

        [ActionName("Create")]
        public async Task<IActionResult> StateCreate()
        {
            List<AccountModel> accountList = await _accountController.GetAccountAPI();

            ViewBag.Accounts = accountList;
            return View("Create");
        }

        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> TransactionSave(TransactionHistoryModel transaction)
        {
            //   _db.Blogs.Add(blog);
            //   _db.SaveChanges();
            bool result = await SaveTransactionAPI(transaction);

            //return Redirect("/State");

            string message = result == true ? "Saving Successful." : "Saving Failed.";
            MessageResponseModel model = new MessageResponseModel()
            {
                IsSuccess = result == true,
                Message = message
            };
            return Json(model);
        }



        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> TransactionUpdate(int id, TransactionHistoryModel transaction)
        {
            MessageResponseModel model = new MessageResponseModel();

            //var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetTransactionByIDAPI(id);
            if (item is null)
            {
                // return Redirect("/State");

                model = new MessageResponseModel()
                {
                    IsSuccess = false,
                    Message = "No Data Found"
                };
                return Json(model);
            }

            item.FromAccountNo = transaction.FromAccountNo;
            item.ToAccountNo = transaction.ToAccountNo;
            item.TransactionDate = transaction.TransactionDate;
            item.Amount = transaction.Amount;

            bool result = await UpdateTransactionAPI(id, transaction);
            string message = result == true ? "Updating Successful." : "Updating Failed.";
            // _db.SaveChanges();

            model = new MessageResponseModel()
            {
                IsSuccess = result == true,
                Message = message
            };
            return Json(model);

            // return Redirect("/State");
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(TransactionHistoryModel transaction)
        {
            MessageResponseModel model = new MessageResponseModel();

            // var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetTransactionByIDAPI(transaction.TransactionHistoryId);
            if (item is null)
            {
                //  return Redirect("/State");
                model = new MessageResponseModel()
                {
                    IsSuccess = false,
                    Message = "No Data Found"
                };
                return Json(model);
            }

            //_db.Remove(item);
            //_db.SaveChanges();
            bool result = await DeleteTransactionAPI(transaction.TransactionHistoryId);
            string message = result == true ? "Deleting Successful." : "Deleting Failed.";

            model = new MessageResponseModel()
            {
                IsSuccess = result == true,
                Message = message
            };
            return Json(model);

            // return Redirect("/State");
        }


        [ActionName("Transfer")]
        public async Task<IActionResult> Transfer()
        {
            //   var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            //var item = await GetAccountByIDAPI(id);
            //if (item is null)
            //{
            //    return Redirect("/Account");
            //}

            List<AccountModel> accountList = await _accountController.GetAccountAPI();

            ViewBag.Accounts = accountList;

            return View("Transfer");
        }


        [HttpPost]
        [ActionName("TransferAccount")]
        public async Task<IActionResult> TransferAccount(TransactionHistoryModel transaction,
            AccountModel fromAccount, AccountModel toAccount)
        {
            MessageResponseModel model = new MessageResponseModel();



            //From Account Code
            var fromAccountItem = await _accountController.GetAccountByIDAPI(fromAccount.AccountId);
            if (fromAccountItem is null)
            {
                // return Redirect("/State");

                model = new MessageResponseModel()
                {
                    IsSuccess = false,
                    Message = "No Data Found"
                };
                return Json(model);
            }

            if (fromAccountItem.Balance < transaction.Amount)
            {
                model = new MessageResponseModel()
                {
                    IsSuccess = false,
                    Message = "Not enough amount in from account no." + fromAccountItem.AccountNo
                };
                return Json(model);
            }

            fromAccountItem.Balance = fromAccountItem.Balance - transaction.Amount;


            bool result2 = await _accountController.UpdateAccountAPI(fromAccountItem.AccountId, fromAccountItem);



            //To Account Code
            var toAccountItem = await _accountController.GetAccountByIDAPI(toAccount.AccountId);
            if (toAccountItem is null)
            {
                // return Redirect("/State");

                model = new MessageResponseModel()
                {
                    IsSuccess = false,
                    Message = "No Data Found"
                };
                return Json(model);
            }




            toAccountItem.Balance = toAccountItem.Balance + transaction.Amount;
            bool result3 = await _accountController.UpdateAccountAPI(toAccountItem.AccountId, toAccountItem);


            TransactionHistoryModel transactionItem = new TransactionHistoryModel()
            {
                FromAccountNo = fromAccountItem.AccountNo,
                ToAccountNo = toAccountItem.AccountNo,
                TransactionDate = transaction.TransactionDate,
                Amount = transaction.Amount
            };





            bool result = await SaveTransactionAPI(transactionItem);




            string message = result == true ? "Updating Successful." : "Updating Failed.";







            model = new MessageResponseModel()
            {
                IsSuccess = result == true,
                Message = message
            };
            return Json(model);

            // return Redirect("/State");
        }


    }
}
