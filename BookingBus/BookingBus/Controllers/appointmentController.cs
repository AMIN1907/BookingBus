
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
using System.Buffers;
using System;



namespace BookingBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]


    public class appointmentController : ControllerBase
    {
    

        private readonly IappointmentRepository _Repository;

        
        public appointmentController( IappointmentRepository Repository)
        {
            
            _Repository = Repository;
            

        }

        [HttpPatch("Filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<appointment>>> Filter(string searchField = null, string searchValue = null)
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;
            var appointmentList = await _Repository.GetALLEntityByDynamicQuery<appointment>(searchField, searchValue);
            if(appointmentList == null) { return NotFound(); }


            List<appointment> result = new List<appointment>();
            foreach (var appointment in appointmentList)
            {

                if (appointment.IsAvailable == false || appointment.NumOfTicketIsAvalbel == 0) { continue; }

                result.Add(appointment);
            }



            FilterHistory model = new FilterHistory
            {userid = userId,
            searchField= searchField,
            searchValue= searchValue

            };
            await _Repository.AddEntityAsync(model);
            return Ok(result);
        }


        [HttpPatch("DateFilter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<appointment>>> DateFilter(string searchField = null, int? year = null, int? month = null, int? day = null)
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;
            List<appointment> appointmentList = await _Repository.DateFilter<appointment>(searchField, year, month,day);
            List<appointment> result = new List<appointment>();
            foreach (var appointment in appointmentList)
            {
               
                if (appointment.IsAvailable == false || appointment.NumOfTicketIsAvalbel==0) { continue;}

                 result.Add(appointment);
            }
            if (appointmentList == null)
            {
                return NotFound();
            }
            FilterHistory model = new FilterHistory
            {
                userid = userId,
                searchField = searchField,
                searchValue = string.Format("{0}-{1}-{2}", year, month, day)


        };
            await _Repository.AddEntityAsync(model);
            return Ok(result);
        }



        [HttpGet("GetAvailableAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<appointment>> GetAvailableAppointment()
        {
            var Result = await _Repository.GetAllTEntity<appointment>(e=>e.IsAvailable==true&&e.NumOfTicketIsAvalbel>0);
            return Ok(Result);
        }

      
        [HttpGet("get info appointment/int:id ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<appointment>> Get(int id)
        {
            if (id == 0)
                return BadRequest();

            var result = await _Repository.GetSpecialEntity<appointment>(e => e.id == id);

            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }













    }
}
