
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
    public class appointmentControllerADMIN : ControllerBase
    {
    

        private readonly IappointmentRepository _Repository;

        
        public appointmentControllerADMIN( IappointmentRepository Repository)
        {
            
            _Repository = Repository;
            

        }


       

        [HttpGet("Get all appointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<appointment>> Getallappointment()
        {
            var Result = await _Repository.GetAllTEntity<appointment>();
            return Ok(Result);
        }

        
        [HttpGet("GetAvailableAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<appointment>> GetAvailableAppointment()
        {
            var Result = await _Repository.GetAllTEntity<appointment>(e=>e.IsAvailable==true);
            return Ok(Result);
        }

        
        [HttpGet("get info appointment/int:id ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
      
        public async Task<ActionResult<appointment>> Get(int id ) 

        {
            
            if (id == null)
                return BadRequest();
            var Result = await _Repository.GetSpecialEntity<appointment>(e => e.id == id);

            {
                if (Result == null) return NotFound();
                else return Ok(Result);
                
            }

        }



     

        [HttpPost]


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<appointment>> CreatAppointment([FromBody] appointment appointment )
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;
            if (appointment == null)
            {
                return BadRequest();
            }
            appointment model = new()
            {
                DataToBack= appointment.DataToBack,
                DataToGo= appointment.DataToGo,
                NumOfTicketIsAvalbel= appointment.NumOfTicketIsAvalbel,
                description= appointment.description,
                TO= appointment.TO,
                FROM= appointment.FROM,
                DeadLineToBook = appointment.DeadLineToBook,

              };

            await _Repository.Create(model);
            await _Repository.save();

            return Ok(model);
        }




        [HttpDelete("Delete appointment/ int:id ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<appointment>> Delete(int id )

        {
            var Result = await _Repository.GetSpecialEntity<appointment>(e => e.id == id);

            if (id == null)
                return BadRequest();
            else
            {
                if (Result == null) return NotFound();
                else await _Repository.Remove(Result);
            }
            
            await _Repository.save();
            return Ok(" the appointment are Deleted successfully ");






        }

        [HttpPut("id:int ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<appointment>> update(int id, [FromBody] appointment appointment)
        {
            var Result = await _Repository.GetSpecialEntity<appointment>(e => e.id == id);

            if (appointment == null) return BadRequest();

                
            Result.DataToBack = appointment.DataToBack;
            Result.DataToGo = appointment.DataToGo;
            Result.NumOfTicketIsAvalbel = appointment.NumOfTicketIsAvalbel;
            Result.description = appointment.description;
            Result.TO = appointment.TO;
            Result.FROM = appointment.FROM;
            Result.DeadLineToBook = appointment.DeadLineToBook;

            await _Repository.save();
            return Ok(" the appointment are update successfully ");

        }









    }
}
