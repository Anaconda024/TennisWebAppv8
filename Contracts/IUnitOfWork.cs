using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisWebApp8.Data;

namespace TennisWebApp8.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepositoryBase<Booking> Bookings { get; }
        IGenericRepositoryBase<Timeslot> Timeslots { get; }
        IGenericRepositoryBase<Dayslot> Dayslots { get; }
        Task Save();
    }
}
