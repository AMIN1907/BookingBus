
using BookingBus.models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookingBus.models;
using BookingBus.Repository.Repository;
using BookingBus.Repository;
using NuGet.Protocol.Core.Types;
using Microsoft.Extensions.Hosting;
using Humanizer;



namespace BookingBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class requestController : ControllerBase
    {
    

        

        private readonly iRequestRepository _RequestRepository;
        public requestController(iRequestRepository RequestRepository)
        {

            _RequestRepository = RequestRepository;
            
             
        }
      

       

     
        

        [HttpGet("AcceptRequest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<request>> GetAcceptRequest()
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;
            var Result = await _RequestRepository.GetAllTEntity<request>(e => e.status == "Accept" && e.userid == userId);

            {
                if (Result == null) return NotFound();
                else return Ok(Result);
            }
        }





        [HttpGet("declineRequest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<request>> GetRejectedRequest()
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;
            var Result = await _RequestRepository.GetAllTEntity<request>(e => e.status == "decline" && e.userid == userId);



            if (Result == null) return NotFound();
            return Ok(Result);

        }







        [HttpGet("PendingRequest ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<request>> GetPendingRequest()
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;
            var Result = await _RequestRepository.GetAllTEntity<request>(e => e.status == "Pending" && e.userid == userId);
            {
                if (Result == null) return NotFound();
                else return Ok(Result);
            }
        }









        [HttpGet(" history of traveler requests in all appointments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<request>> HistoryForMy()
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;
            var requests = await _RequestRepository.GetAllTEntity<request>(e => e.userid == userId);
            {
                if (requests == null) return NotFound();
                else return Ok(requests);
            }
        }


        [HttpGet("history of appointment Filter related to his account")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<request>> FilterHistory()
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;
            var requests = await _RequestRepository.GetAllTEntity<FilterHistory>(e => e.userid == userId);
            {
                if (requests == null) return NotFound();
                else return Ok(requests);
            }
        }



        [HttpPost("int:id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<request>> sendrequest(int id )
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");

            var Result = await _RequestRepository.GetSpecialEntity<appointment>(e =>e.id==id && e.IsAvailable == true);
            if (Result == null) return NotFound("this appointment are not avalbel new ");

            var request = await _RequestRepository.GetSpecialEntity<request>(e => e.AppointmentID == id && e.userid== userIdClaim.Value);
            if (request != null) return BadRequest("the request are sended before");
            request model = new()
            {
                userid = userIdClaim.Value,
                AppointmentID= id,
                status = "Pending"

            };

            await _RequestRepository.Create(model);
            await _RequestRepository.save();

            return Ok(model);
        }




        [HttpDelete("UnRequest/ int:id ")]
       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<request>> Delete(int id )

        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");

            var Result = await _RequestRepository.GetSpecialEntity<request>(e => e.id == id&&e.userid== userIdClaim.Value);
            if (Result == null) return NotFound();

            if (id == null)
                return BadRequest();
            else
            {
                if (Result == null) return NotFound();
                else await _RequestRepository.Remove(Result);
            }
            
            await _RequestRepository.save();
            return Ok(" the request are Deleted successfully ");

        }


        






    }
}
