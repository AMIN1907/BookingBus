
using BookingBus.Controllers;

using BookingBus.models;

namespace BookingBus.Repository.Repository
{
    public class RequestRepository : RepositoryGeneric<request>, iRequestRepository
    {
        private readonly appDbcontext1 _db;

        public RequestRepository(appDbcontext1 db) : base(db)
        {
            _db = db;

        }

      
        public async Task<request> updatat(request entity)
        {   
            _db.requests.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}


