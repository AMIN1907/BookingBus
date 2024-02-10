

using BookingBus.models;

namespace BookingBus.Repository.Repository
{
    public interface IappointmentRepository : IRepository<appointment>
    {

       public Task<appointment> updatat(appointment entity);

    }
}
