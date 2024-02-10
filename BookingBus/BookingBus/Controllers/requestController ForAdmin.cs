
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
    
    [Authorize(Roles = "Admin")]
    public class requestControllerForAdmin : ControllerBase
    {
    

        

        private readonly iRequestRepository _RequestRepository;
        public requestControllerForAdmin(iRequestRepository RequestRepository)
        {

            _RequestRepository = RequestRepository;
            
             
        }
      

        
        [HttpGet("get info request/int:id ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
      
        public async Task<ActionResult<request>> Get(int id ) 
        {
            if (id == null)
                return BadRequest();
            var Result = await _RequestRepository.GetSpecialEntity<request>(e => e.id == id);
            {
                if (Result == null) return NotFound();
                else return Ok(Result);
                
            }

        }


        
        [HttpGet("GetAllRequestsForAppointment/int:id ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<request>> GetAllRequestsForAppointment(int id)
        {
            if (id == null)
                return BadRequest();
            var Result = await _RequestRepository.GetAllTEntity<request>(e => e.AppointmentID == id);
            {
                if (Result == null) return NotFound();
                else return Ok(Result);

            }

        }

    
        [HttpGet("PendingRequest ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<request>> GetPendingRequest()
        {
           
            var Result = await _RequestRepository.GetAllTEntity<request>(e => e.status == "Pending");
            {
                if (Result == null) return NotFound();
                else return Ok(Result);
            }
        }


     


        [HttpGet("AcceptRequest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<request>> GetAcceptRequest()
        {
            var Result = await _RequestRepository.GetAllTEntity<request>(e => e.status == "Accept");
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
           
            var Result = await _RequestRepository.GetAllTEntity<request>(e => e.status == "decline" );



            if (Result == null) return NotFound();
            return Ok(Result);

        }



        [HttpGet("history For Traveler/string:id ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<request>> HistoryForOneUSER(string userId)
        {
            

            var Result = await _RequestRepository.GetAllTEntity<request>(e => e.userid == userId);
            {
                if (Result == null) return NotFound();
                else return Ok(Result);
            }
        }


   
        [HttpGet("history Traveler username/ string:username")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<request>> historyusernaem (string username)
        {
            var Result = await _RequestRepository.GetSpecialEntity<ApplicationUser>(e => e.UserName == username);

            var requests = await _RequestRepository.GetAllTEntity<request>(e => e.userid == Result.Id);
            {
                if (requests == null) return NotFound();
                else return Ok(requests);
            }
        }



   






        [HttpDelete("UnRequest/ int:id ")]
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<request>> Delete(int id )

        {
            var Result = await _RequestRepository.GetSpecialEntity<request>(e => e.id == id);

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




        [HttpPut("Accept/ int: id ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<request>> Accept(int requestid)
        {
            var Result = await _RequestRepository.GetSpecialEntity<request>(e => e.id == requestid && e.status== "Pending");
            if (Result == null) return NotFound();

            var appointment = await _RequestRepository.GetSpecialEntity<appointment>(e => e.id == Result.AppointmentID);

            if (appointment.NumOfTicketIsAvalbel <= 0) return NotFound(" Tickets  not Avalbel");

            if (appointment.NumOfTicketIsAvalbel == 1) 
            {
                Result.status = "Accept";
                appointment.IsAvailable = false;
                appointment.NumOfTicketIsAvalbel--;

                await _RequestRepository.save();
                return Ok("Accept");

            }
            Result.status = "Accept";
            appointment.NumOfTicketIsAvalbel--;
            await _RequestRepository.save();
            return Ok("Accept");

        }

        [HttpPut("Reject/ int: id ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<request>> Reject(int requestid)
        {
            var Result = await _RequestRepository.GetSpecialEntity<request>(e => e.id == requestid);
            var appointment = await _RequestRepository.GetSpecialEntity<appointment>(e => e.id == Result.AppointmentID);

          

            Result.status = "Reject";
           
            await _RequestRepository.save();
            return Ok("Accept");

        }










    }
}
