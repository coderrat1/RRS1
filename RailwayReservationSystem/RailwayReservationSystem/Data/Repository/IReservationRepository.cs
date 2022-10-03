using RailwayReservationSystem.Models;
using System.Collections.Generic;

namespace RailwayReservationSystem.Data.Repository
{
    public interface IReservationRepository
    {
        public Ticket AddReservation(ReservationDto reservation, int userId, List<Passenger> passengers);
        
        public Ticket GetTicket(int pnr);
        public Ticket CancelTicket(int pnr);
    }
}
