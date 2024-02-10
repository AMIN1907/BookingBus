

using BookingBus.models;
using BookingBus.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookingBus.Repository.Repository
{
    public interface ImangeprofileRepository : IRepository<ApplicationUser>
    {
        public Task<ApplicationUser> updatat(ApplicationUser entity);
      public  Task<object> ChangePassword (ApplicationUser entity , ChangePassword  changePassword);
       
    }
}
