
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
        {
            var errors = new List<string>();
            var result = await _userManager.ChangePasswordAsync(entity, changePassword.CurrentPassword, changePassword.NewPassword);
            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(e => e.Description));
                return errors;
            }
            await save();
            return entity;
        }


        public async Task<ApplicationUser> updatat(ApplicationUser entity)
        {   
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}


