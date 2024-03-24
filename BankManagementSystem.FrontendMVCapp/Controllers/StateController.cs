using BankManagementSystem.FrontendMVCapp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace BankManagementSystem.FrontendMVCapp.Controllers
{
    public class StateController : Controller
    {

        private readonly string _api_url = "https://localhost:7112/api/";

        public async Task<List<StateModel>> GetStateAPI()
        {
            List<StateModel> list = new List<StateModel>();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_api_url + "State");
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonStr);


                list = JsonConvert.DeserializeObject<List<StateModel>>(jsonStr);

            }

            return list;

        }


        public async Task<StateModel> GetStateByIDAPI(int id)
        {
            StateModel list = new StateModel();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_api_url + "State/" + id);
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonStr);


                list = JsonConvert.DeserializeObject<StateModel>(jsonStr);

            }

            return list;

        }


        public async Task<bool> SaveStateAPI(StateModel state)
        {


            string josnState = JsonConvert.SerializeObject(state);

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(josnState, Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage response = await client.PostAsync(_api_url + "State", content);
            if (response.IsSuccessStatusCode)
            {
                return true;

            }

            return false;

        }


        public async Task<bool> UpdateStateAPI(int id, StateModel state)
        {


            string josnState = JsonConvert.SerializeObject(state);

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(josnState, Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage response = await client.PutAsync(_api_url + "State/" + id, content);
            if (response.IsSuccessStatusCode)
            {
                return true;

            }

            return false;

        }


        public async Task<bool> DeleteStateAPI(int id)
        {

            HttpClient client = new HttpClient();


            HttpResponseMessage response = await client.DeleteAsync(_api_url + "State/" + id);
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
            List<StateModel> states = await GetStateAPI();
            return View("Index", states);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            //   var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetStateByIDAPI(id);
            if (item is null)
            {
                return Redirect("/State");
            }


            return View("Edit", item);
        }

        [ActionName("Create")]
        public IActionResult StateCreate()
        {

            return View("Create");
        }

        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> StateSave(StateModel state)
        {
            //   _db.Blogs.Add(blog);
            //   _db.SaveChanges();
            bool result = await SaveStateAPI(state);

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
        public async Task<IActionResult> StateUpdate(int id, StateModel state)
        {
            MessageResponseModel model = new MessageResponseModel();

            //var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetStateByIDAPI(id);
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

            item.StateCode = state.StateCode;
            item.StateName = state.StateName;

            bool result = await UpdateStateAPI(id, state);
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
        public async Task<IActionResult> Delete(StateModel state)
        {
            MessageResponseModel model = new MessageResponseModel();

            // var item = _db.Blogs.FirstOrDefault(x => x.BlogId == id);
            var item = await GetStateByIDAPI(state.StateId);
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
            bool result = await DeleteStateAPI(state.StateId);
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
