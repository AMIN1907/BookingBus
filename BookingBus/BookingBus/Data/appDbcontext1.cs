
using BookingBus.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BookingBus.models;


namespace BookingBus.Controllers
{
    public class appDbcontext1 : IdentityDbContext<ApplicationUser>
    {
        public appDbcontext1(DbContextOptions<appDbcontext1> options) : base(options)
        { }
           public DbSet<appointment> appointments { get; set; }
           public DbSet<request> requests { get; set; }
           public DbSet<FilterHistory> FilterHistorys { get; set; }

    }


}
   

