using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IReservationService {

        Task<bool> ExistingReservation(Reservation reservation);

        // GET
        Task<Reservation> GetReservationByIDAsync(int reservationID);
        Task<List<Reservation>> GetReservationsUserForDateOrRangeAsync(int userID, DateOnly date, DateOnly optionalDate);
        Task<List<Reservation>> GetReservationsForRestaurantForDateOrRangeAsync(int restaurantID, DateOnly date, DateOnly optionalDate);

        // POST
        Task<Reservation> CreateReservationAsync(Reservation reservation);

        // PUT
        Task<Reservation> UpdateReservationAsync(int reservationID, Reservation updatedReservation);

        // DELETE
        Task CancelReservationAsync(int reservationID);

        
    }
}
