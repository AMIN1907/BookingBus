
using BookingBus.Controllers;

using BookingBus.models;
using BookingBus.models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static BookingBus.Controllers.profileController;

namespace BookingBus.Repository.Repository
{
    public class mangeprofilerepository : RepositoryGeneric<ApplicationUser>, ImangeprofileRepository
    {
        private readonly appDbcontext1 _db;
        private readonly UserManager<BookingBus.models.ApplicationUser> _userManager;
        public mangeprofilerepository(appDbcontext1 db, UserManager<ApplicationUser> userManager) : base(db)
        {
            _db = db;
            _userManager = userManager;
        }
        

        
        public async Task<object> ChangePassword(ApplicationUser entity, ChangePassword changePassword)
        { var errors = "";
            var result = await _userManager.ChangePasswordAsync(entity, changePassword.CurrentPassword, changePassword.NewPassword);
            if (changePassword.NewPassword == null || changePassword.ConfirmPassword == null || changePassword.CurrentPassword == null)
            {
                errors = "New Password and Confirm Password  are required";
                return errors;
            }
            if (changePassword.NewPassword != changePassword.ConfirmPassword)
            {
                errors = ("New Password and Confirm Password not match");
                return errors;
            }
            if (changePassword.NewPassword == changePassword.CurrentPassword)
            {
                 errors = ("New Password and Current Password are matching ");
                    return errors;
            }
            if (!result.Succeeded)
            {
                 errors = string.Join(", ", result.Errors.Select(e => e.Description));
                  return errors;
            }
            if (result.Succeeded)
            {
                    await _db.SaveChangesAsync();
                    return entity;
            }
            return null;
        

    }

        public async Task<ApplicationUser> updatat(ApplicationUser entity)
        {   
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}


