
using BookingBus.models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookingBus.models;
using BookingBus.Repository.Repository;
using BookingBus.models;



namespace BookingBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class profileController : ControllerBase
    {


        private readonly ImangeprofileRepository _ProfileRepository;

        public profileController(ImangeprofileRepository ProfileRepository)
        {

            _ProfileRepository = ProfileRepository;


        }
        private async Task<List<ProfileDto>> mapprofile(List<ApplicationUser> users)
        {
            List<ProfileDto> Profile = new List<ProfileDto>();
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


            return Profile;
        }

        [HttpGet("get info")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationUser>> Get()

        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;
            if (userId == null)
                return BadRequest();
            var Result = await _ProfileRepository.GetAllTEntity<ApplicationUser>(e => e.Id == userId);

            {
                if (Result == null) return NotFound();
                return Ok(await mapprofile(Result));

            }

        }

        [HttpGet("get info for anther user/string username ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]




        public async Task<ActionResult<ApplicationUser>> Getuser(string username)

        {

            if (username == null)
                return BadRequest();
            var Result = await _ProfileRepository.GetAllTEntity<ApplicationUser>(e => e.UserName == username);

            {
                if (Result == null) return NotFound();
                else return Ok(await mapprofile(Result));

            }

        }

        [HttpDelete("Delete My Account")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationUser>> Delete()

        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var id = userIdClaim.Value;
            var Result = await _ProfileRepository.GetSpecialEntity<ApplicationUser>(e => e.Id == id);

            if (id == null)
                return BadRequest();
            else
            {
                if (Result == null) return NotFound();
                else await _ProfileRepository.Remove(Result);
            }

            await _ProfileRepository.save();
            return Ok(" Your Account are Deleted successfully ");






        }

        [HttpPut("updatedProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationUser>> UpdateUserProfile(ProfileDto updatedProfile)
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var id = userIdClaim.Value;
            var existingUser = await _ProfileRepository.GetSpecialEntity<ApplicationUser>(e => e.Id == id);

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
                existingUser.ginder != updatedProfile.ginder ||
                existingUser.PhoneNumber != updatedProfile.PhoneNumber ||
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








        [HttpPut("changepassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePassword changePassword)
        {
            var userIdClaim = (HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == "uid");
            var userId = userIdClaim.Value;
            var user = await _ProfileRepository.GetSpecialEntity<ApplicationUser>(e => e.Id == userId);

            if (user == null)
            { return NotFound("User not found"); }

            var errors = new List<string>();
            // Check if any required fields are null
            if (changePassword.NewPassword == null || changePassword.ConfirmPassword == null || changePassword.CurrentPassword == null)
            {
                errors.Add("New Password, Confirm Password, and Current Password are required");
            }

            // Check if New Password matches Confirm Password
            if (changePassword.NewPassword != changePassword.ConfirmPassword)
            {
                errors.Add("New Password and Confirm Password do not match");
            }

            // Check if New Password is the same as Current Password
            if (changePassword.NewPassword == changePassword.CurrentPassword)
            {
                errors.Add("New Password and Current Password are the same");
            }
            // If there are any errors, return them
            if (errors.Any())
            {
                return BadRequest(errors);
            }
            var result = await _ProfileRepository.ChangePassword(user, changePassword);


            if (result is not ApplicationUser)
                return BadRequest(result);
            return Ok(result);
        }





    }
}
