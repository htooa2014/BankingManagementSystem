using BankManagementSystem.FrontendMVCapp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace BankManagementSystem.FrontendMVCapp.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _api_url = "https://localhost:7112/api/";

        public async Task<List<AccountModel>> GetAccountAPI()
        {
            List<AccountModel> list = new List<AccountModel>();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_api_url + "Account");
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonStr);


                list = JsonConvert.DeserializeObject<List<AccountModel>>(jsonStr);

            }

            return list;

        }


        public async Task<AccountModel> GetAccountByIDAPI(int id)
        {
            AccountModel list = new AccountModel();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_api_url + "Account/" + id);
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonStr);


                list = JsonConvert.DeserializeObject<AccountModel>(jsonStr);

            }

            return list;

        }

      

       


        public async Task<bool> SaveAccountAPI(AccountModel account)
        {


            string josnAccoount = JsonConvert.SerializeObject(account);

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(josnAccoount, Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage response = await client.PostAsync(_api_url + "Account", content);
            if (response.IsSuccessStatusCode)
            {
                return true;

            }

            return false;

        }


        public async Task<bool> UpdateAccountAPI(int id, AccountModel account)
        {


            string josnAccount = JsonConvert.SerializeObject(account);

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(josnAccount, Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage response = await client.PutAsync(_api_url + "Account/" + id, content);
            if (response.IsSuccessStatusCode)
            {
                return true;

            }

            return false;

        }


        public async Task<bool> DeleteAccountAPI(int id)
        {

            HttpClient client = new HttpClient();


            HttpResponseMessage response = await client.DeleteAsync(_api_url + "Account/" + id);
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
            List<AccountModel> accounts = await GetAccountAPI();
            return View("Index", accounts);
        }

        

        [ActionName("Create")]
        public IActionResult AccountCreate()
        {

            return View("Create");
        }

        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> AccountSave(AccountModel account)
        {
            //   _db.Blogs.Add(blog);
            //   _db.SaveChanges();
            account.CustomerCode = "";
            bool result = await SaveAccountAPI(account);

            //return Redirect("/State");

            string message = result == true ? "Saving Successful." : "Saving Failed.";
            MessageResponseModel model = new MessageResponseModel()
            {
                IsSuccess = result == true,
                Message = message
            };
            return Json(model);
        }



        [ActionName("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            //   var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetAccountByIDAPI(id);
            if (item is null)
            {
                return Redirect("/Account");
            }


            return View("Edit", item);
        }


        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> AccountUpdate(int id, AccountModel account)
        {
            MessageResponseModel model = new MessageResponseModel();

            //var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetAccountByIDAPI(id);
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
             
            item.AccountNo = account.AccountNo;
            item.CustomerCode = account.CustomerCode;
            item.CustomerName = account.CustomerName;
            item.Balance = account.Balance;
            item.TransactionStatus = account.TransactionStatus;

            bool result = await UpdateAccountAPI(id, item);
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



        [ActionName("Deposit")]
        public async Task<IActionResult> Deposit()
        {
            //   var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            //var item = await GetAccountByIDAPI(id);
            //if (item is null)
            //{
            //    return Redirect("/Account");
            //}

            List<AccountModel> accountList = await GetAccountAPI();

            ViewBag.Accounts = accountList;

            return View("Deposit");
        }


        [HttpPost]
        [ActionName("DepositAccount")]
        public async Task<IActionResult> DepositAccount(int id, AccountModel account)
        {
            MessageResponseModel model = new MessageResponseModel();

            //var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetAccountByIDAPI(id);
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

            item.AccountNo = account.AccountNo;
           item.Balance = account.Balance;
            item.TransactionStatus = account.TransactionStatus;

            bool result = await UpdateAccountAPI(id, item);
            string message = result == true ? "Deposit Successful." : "Deposit Failed.";
            // _db.SaveChanges();

            model = new MessageResponseModel()
            {
                IsSuccess = result == true,
                Message = message
            };
            return Json(model);
        }

        [ActionName("Withdraw")]
        public async Task<IActionResult> Withdraw()
        {
            //   var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            //var item = await GetAccountByIDAPI(id);
            //if (item is null)
            //{
            //   return Redirect("/Account");
            //}

            List<AccountModel> accountList = await GetAccountAPI();

            ViewBag.Accounts = accountList;
            return View("Withdraw");
        }


        [HttpPost]
        [ActionName("WithdrawAccount")]
        public async Task<IActionResult> WithdrawAccount(int id, AccountModel account)
        {
            MessageResponseModel model = new MessageResponseModel();

            //var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetAccountByIDAPI(id);
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

            item.AccountNo = account.AccountNo;
            item.Balance = account.Balance;
            item.TransactionStatus = account.TransactionStatus;

            bool result = await UpdateAccountAPI(id, item);
            string message = result == true ? "Withdraw Successful." : "Withdraw Failed.";
            // _db.SaveChanges();

            model = new MessageResponseModel()
            {
                IsSuccess = result == true,
                Message = message
            };
            return Json(model);
        }


        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(AccountModel account)
        {
            MessageResponseModel model = new MessageResponseModel();

            // var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetAccountByIDAPI(account.AccountId);
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
            bool result = await DeleteAccountAPI(account.AccountId);
            string message = result == true ? "Deleting Successful." : "Deleting Failed.";

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
