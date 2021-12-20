using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisWebApp8.Contracts;
using TennisWebApp8.Data;


namespace TennisWebApp8.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private  IGenericRepositoryBase<Booking> _bookings;
        private  IGenericRepositoryBase<Timeslot> _timeslots;
        private  IGenericRepositoryBase<Dayslot> _dayslots;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IGenericRepositoryBase<Booking> Bookings 
            => _bookings ??= new GenericRepository<Booking>(_context);             
        
        public IGenericRepositoryBase<Timeslot> Timeslots 
            => _timeslots ??= new GenericRepository<Timeslot>(_context);

        public IGenericRepositoryBase<Dayslot> Dayslots
            => _dayslots ??= new GenericRepository<Dayslot>(_context);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {
            if (dispose)
            { _context.Dispose(); 
            };
            
        }

        public async  Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
