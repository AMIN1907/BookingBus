
using BookingBus.Controllers;

using BookingBus.models;

namespace BookingBus.Repository.Repository
{
    public class appointmentRepository : RepositoryGeneric<appointment>, IappointmentRepository
    {
        private readonly appDbcontext1 _db;

        public appointmentRepository(appDbcontext1 db) : base(db)
        {
            _db = db;

        }


        public async Task<appointment> updatat(appointment entity)
        {   
            _db.appointments.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}


