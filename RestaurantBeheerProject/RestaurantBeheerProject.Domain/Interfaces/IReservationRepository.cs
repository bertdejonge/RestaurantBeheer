using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IReservationRepository {

        Task<Reservation> GetReservationByIDAsync(int reservationID);

        // Takes 2 or 3 arguments
        // If 2, search for date
        // If 3, search in range
        Task<List<Reservation>> GetReservationsRestaurantForDateAsync(int restaurantID, DateOnly date, DateOnly? optionalDate = null);

        Task CreateReservationAsync(Reservation reservation);            

        // Uur, Datum, aantal plaatsen
        Task UpdateReservationAsync(Reservation updatedReservation);

        // Enkel voor aankomende reservaties
        Task CancelReservationAsync(int reservationID);
    }
}
