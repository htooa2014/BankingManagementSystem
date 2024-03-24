using BankManagementSystem.BackendWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankManagementSystem.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownshipController : ControllerBase
    {
        private readonly AppDBContext _db;

        public TownshipController()
        {
            _db = new AppDBContext();
        }

        [HttpGet]
        public IActionResult GetTownships()
        {
            List<TownshipModel> townships = _db.Townships.ToList();

            return Ok(townships);
        }

        [HttpGet("{id}")]
        public IActionResult GetTownships(int id)
        {
            TownshipModel township = _db.Townships.FirstOrDefault(item => item.TownshipId == id);
            if (township == null)
            {
                return NotFound("No Data Found");
            }
            return Ok(township);
        }

        [HttpPost]
        public IActionResult CreateTownship(TownshipModel township)
        {
            _db.Townships.Add(township);
            int result = _db.SaveChanges();
            string message = result > 0 ? "Saving Successful" : "Saving fail";
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTownship(int id, TownshipModel townshipUpdate)
        {
            var township = _db.Townships.FirstOrDefault(item => item.TownshipId == id);
            if (township == null)
            {
                return NotFound("No Data Found");
            }

            township.TownshipCode = townshipUpdate.TownshipCode;
            township.TownshipName = townshipUpdate.TownshipName;
            township.StateCode = townshipUpdate.StateCode;

            int result = _db.SaveChanges();
            string message = result > 0 ? "Update Successful" : "Update fail";
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletTownship(int id)
        {
            var township = _db.Townships.FirstOrDefault(item => item.TownshipId == id);
            if (township == null)
            {
                return NotFound("No Data Found");
            }


            _db.Remove(township);
            int result = _db.SaveChanges();
            string message = result > 0 ? "Delete Successful" : "Delete fail";
            return Ok(message);
        }
    }
}
