using BankManagementSystem.FrontendMVCapp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace BankManagementSystem.FrontendMVCapp.Controllers
{
    public class TownshipController : Controller
    {
        private readonly string _api_url = "https://localhost:7112/api/";
        public async Task<List<TownshipModel>> GetTownshipAPI()
        {
            List<TownshipModel> list = new List<TownshipModel>();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_api_url + "Township");
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonStr);


                list = JsonConvert.DeserializeObject<List<TownshipModel>>(jsonStr);

            }

            return list;

        }


        public async Task<TownshipModel> GetTownshipByIDAPI(int id)
        {
            TownshipModel list = new TownshipModel();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_api_url + "Township/" + id);
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonStr);


                list = JsonConvert.DeserializeObject<TownshipModel>(jsonStr);

            }

            return list;

        }


        public async Task<bool> SaveTownshipAPI(TownshipModel township)
        {


            string josnTownship = JsonConvert.SerializeObject(township);

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(josnTownship, Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage response = await client.PostAsync(_api_url + "Township", content);
            if (response.IsSuccessStatusCode)
            {
                return true;

            }

            return false;

        }


        public async Task<bool> UpdateTownshipAPI(int id, TownshipModel township)
        {


            string josnTownship = JsonConvert.SerializeObject(township);

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(josnTownship, Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage response = await client.PutAsync(_api_url + "Township/" + id, content);
            if (response.IsSuccessStatusCode)
            {
                return true;

            }

            return false;

        }


        public async Task<bool> DeleteTownshipAPI(int id)
        {

            HttpClient client = new HttpClient();


            HttpResponseMessage response = await client.DeleteAsync(_api_url + "Township/" + id);
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
            List<TownshipModel> townships = await GetTownshipAPI();
            return View("Index", townships);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            //   var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetTownshipByIDAPI(id);
            if (item is null)
            {
                return Redirect("/State");
            }


            return View("Edit", item);
        }

        [ActionName("Create")]
        public IActionResult TownshipCreate()
        {

            return View("Create");
        }

        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> TownshipSave(TownshipModel township)
        {
            //   _db.Blogs.Add(blog);
            //   _db.SaveChanges();
            bool result = await SaveTownshipAPI(township);

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
        public async Task<IActionResult> TownshipUpdate(int id, TownshipModel township)
        {
            MessageResponseModel model = new MessageResponseModel();

            //var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetTownshipByIDAPI(id);
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

            item.TownshipCode = township.TownshipCode;
            item.TownshipName = township.TownshipName;
            item.StateCode = township.StateCode;


            bool result = await UpdateTownshipAPI(id, township);
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
        public async Task<IActionResult> Delete(TownshipModel township)
        {
            MessageResponseModel model = new MessageResponseModel();

            // var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetTownshipByIDAPI(township.TownshipId);
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
            bool result = await DeleteTownshipAPI(township.TownshipId);
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

