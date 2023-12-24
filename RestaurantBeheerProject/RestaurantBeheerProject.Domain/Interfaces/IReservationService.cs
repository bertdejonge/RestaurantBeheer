using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IReservationService {
        // GET
        Task<Reservation> GetReservationByIDAsync(int reservationID);
        Task<List<Reservation>> GetReservationsRestaurantForDateAsync(int restaurantID, DateOnly date, DateOnly? optionalDate = null);
         
        // POST
        Task CreateReservationAsync(Reservation reservation);

        // PUT
        Task UpdateReservationAsync(Reservation updatedReservation);

        // DELETE
        Task CancelReservationAsync(int reservationID);
    }
}
