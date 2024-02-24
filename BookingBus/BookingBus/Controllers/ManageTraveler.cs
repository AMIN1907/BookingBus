
using BookingBus.models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookingBus.models;
using BookingBus.Repository.Repository;
using System.Collections.Generic;
//using BookingBus.models.dto;


namespace BookingBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "Admin")]
    public class ManageTraveler : ControllerBase
    {
       

        private readonly ImangeprofileRepository _ProfileRepository;

       
        public ManageTraveler(  ImangeprofileRepository ProfileRepository)
        {
          
            _ProfileRepository = ProfileRepository;
            

        }

        private async Task<List<ProfileDto>> mapprofile(List<ApplicationUser> users) {
            List< ProfileDto > Profile = new List< ProfileDto >();
            foreach (var user in users) 
            {
                ProfileDto userdto = new ProfileDto()
                {
                    id = user.Id,
                    fname = user.fname,
                    lname = user.lname,
                    UserName = user.UserName,
                    Email = user.Email,
                    barthday = user.Birthdate,
                    PhoneNumber = user.PhoneNumber,
                    ginder = user.ginder
                };
                Profile.Add(userdto);
            }
            
            
            return Profile; }

        [HttpGet("Get all  Traveler")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProfileDto>> Getalluser()
        {
            var Result = await _ProfileRepository.GetAllTEntity<ApplicationUser>();
           
            return Ok(await mapprofile(Result)) ;
        }

        [HttpGet("Get Info For Any Traveler /string username  ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProfileDto>> Get(string username)
        {
            if (username == null)
                return NotFound();

            var Result = await _ProfileRepository.GetAllTEntity<ApplicationUser>(e => e.UserName == username);

            if (Result == null)
                return NotFound();
          
            return Ok(await mapprofile(Result));

        }


        [HttpDelete("Delete Traveler  Account / string username ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationUser>> Delete(string username)
        {var Result = await _ProfileRepository.GetAllTEntity<ApplicationUser>(e => e.UserName == username);

            if (username == null)
                return BadRequest();
            else
            {
                if (Result == null) return NotFound();
                else await _ProfileRepository.Remove(Result);
            }
            
            await _ProfileRepository.save();
            return Ok(" the Account are Deleted successfully ");

        }

        [HttpPut("updatedProfile/string username")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationUser>> UpdateUserProfile(string username  ,ProfileDto updatedProfile)
        {
            
            var existingUser = await _ProfileRepository.GetSpecialEntity<ApplicationUser>(e => e.UserName == username);

            if (existingUser == null)
            {
                return NotFound();
            }

            var existingUsername = await _ProfileRepository.GetSpecialEntity<ApplicationUser>(e => e.UserName == updatedProfile.UserName);
            if (existingUsername != null && existingUsername.Id != existingUser.Id)
            {
                return BadRequest("Username is already taken");
            }

            var existingEmail = await _ProfileRepository.GetSpecialEntity<ApplicationUser>(e => e.Email == updatedProfile.Email);
            if (existingEmail != null && existingEmail.Id != existingUser.Id)
            {
                return BadRequest("Email is already taken");
            }

            // Check if any field is updated
            bool anyFieldUpdated =
                existingUser.fname != updatedProfile.fname ||
                existingUser.lname != updatedProfile.lname ||
                existingUser.UserName != updatedProfile.UserName ||
                existingUser.Email != updatedProfile.Email ||
                existingUser.PhoneNumber != updatedProfile.PhoneNumber || 
                existingUser.ginder != updatedProfile.ginder ||
                existingUser.Birthdate != updatedProfile.barthday;

            if (!anyFieldUpdated)
            {
                return BadRequest("No changes detected. Please provide updated information.");
            }

            // Update the user profile
            existingUser.fname = updatedProfile.fname ?? existingUser.fname;
            existingUser.lname = updatedProfile.lname ?? existingUser.lname;
            existingUser.UserName = updatedProfile.UserName ?? existingUser.UserName;
            existingUser.Email = updatedProfile.Email ?? existingUser.Email;
            existingUser.ginder = updatedProfile.ginder ?? existingUser.ginder;
            existingUser.NormalizedUserName = (updatedProfile.UserName ?? existingUser.UserName).ToUpper();
            existingUser.NormalizedEmail = (updatedProfile.Email ?? existingUser.Email).ToUpper();
            existingUser.PhoneNumber = updatedProfile.PhoneNumber ?? existingUser.PhoneNumber;
            existingUser.Birthdate = updatedProfile.barthday != DateTime.MinValue ? updatedProfile.barthday : existingUser.Birthdate;

            await _ProfileRepository.updatat(existingUser);

            return Ok("The data has been updated successfully");
        }





    }
}
