

using BookingBus.models;

namespace BookingBus.Repository.Repository
{
    public interface iRequestRepository : IRepository<request>
    {

       public Task<request> updatat(request entity);

    }
}
