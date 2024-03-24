using BankManagementSystem.BackendWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankManagementSystem.BackendWebAPI;

namespace BankManagementSystem.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly AppDBContext _db;

        public StateController()
        {
            _db = new AppDBContext();
        }

        [HttpGet]
        public IActionResult GetStates()
        {
            List<StateModel> states = _db.States.ToList();
            return Ok(states);
        }


        [HttpGet("{id}")]
        public IActionResult GetState(int id)
        {
            StateModel state = _db.States.FirstOrDefault(item => item.StateId == id)!;
            if (state == null)
            {
                return NotFound("No Data Found");
            }
            return Ok(state);
        }

        [HttpPost]
        public IActionResult CreateBlogs(StateModel state)
        {
            _db.States.Add(state);
            int result = _db.SaveChanges();
            string message = result > 0 ? "Saving Successful" : "Saving fail";
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateState(int id, StateModel stateUpdate)
        {
            var state = _db.States.FirstOrDefault(item => item.StateId == id);
            if (state == null)
            {
                return NotFound("No Data Found");
            }

            state.StateCode = stateUpdate.StateCode;
            state.StateName = stateUpdate.StateName;
          

            int result = _db.SaveChanges();
            string message = result > 0 ? "Update Successful" : "Update fail";
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteState(int id)
        {
            var state = _db.States.FirstOrDefault(item => item.StateId == id);
            if (state == null)
            {
                return NotFound("No Data Found");
            }

            _db.Remove(state);
            int result = _db.SaveChanges();
            string message = result > 0 ? "Delete Successful" : "Delete fail";
            return Ok(message);
        }
    }
}
