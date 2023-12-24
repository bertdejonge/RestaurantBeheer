using Microsoft.EntityFrameworkCore;
using RestaurantProject.Datalayer.Data;
using RestaurantProject.Datalayer.Exceptions;
using RestaurantProject.Datalayer.Mappers;
using RestaurantProject.Datalayer.Models;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Datalayer.Repositories {
    public class ReservationRepository : IReservationRepository {

        private readonly RestaurantDbContext _context;

        public ReservationRepository(RestaurantDbContext context) {
            _context = context ?? throw new ContextException(nameof(context));
        }

        // Reusable method as to not write double code
        private async Task SaveAndClearAsync() {
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        public async Task<bool> ExistingReservation(int restaurantID, int userID, DateOnly date) {
            return await _context.Reservations.AnyAsync(r => r.RestaurantID == restaurantID && r.UserID == userID 
                            && DateOnly.FromDateTime(r.DateAndStartTime.Date) == date);
        }

        public async Task<Reservation> GetReservationByIDAsync(int reservationID) {
            try {
                var dataReservation = await _context.Reservations.Include(r => r.Restaurant)
                                            .Include(r => r.User)
                                            .FirstOrDefaultAsync(r => r.ReservationID == reservationID);

                if (dataReservation == null) {
                    throw new ReservationRepositoryException($"No reservation found with id {reservationID}");
                }

                return ReservationMapper.MapToDomain(dataReservation, _context);

            } catch (Exception) {

                throw;
            }
        }

        public async Task<List<Reservation>> GetReservationsRestaurantForDateAsync(int restaurantID, DateOnly date, DateOnly? optionalDate = null) {
            try {
                List<Reservation> reservations = new();

                // If no second date given, search for only the first date
                // Get all the reservations, include restaurant and user navigation props, 
                // select only the ones for the specified date and map them to domain
                if(optionalDate == null || optionalDate <= date) {
                    reservations = await _context.Reservations.Include(r => r.Restaurant)
                                         .Include(r => r.User).Where(r => DateOnly.FromDateTime(r.DateAndStartTime.Date) == date)
                                         .Select(r => ReservationMapper.MapToDomain(r, _context))
                                         .ToListAsync();
                } else {
                    // Same except date must be between the 2 values 
                    reservations = await _context.Reservations.Include(r => r.Restaurant)
                                         .Include(r => r.User)
                                         .Where(r => DateOnly.FromDateTime(r.DateAndStartTime.Date) >= date && DateOnly.FromDateTime(r.DateAndStartTime.Date) <= optionalDate)
                                         .Select(r => ReservationMapper.MapToDomain(r, _context))
                                         .ToListAsync();
                }
                return reservations;
            } catch (Exception) {

                throw;
            }
        }

        public async Task CreateReservationAsync(Reservation reservation) {
            try {
                if (await ExistingReservation(reservation.Restaurant.RestaurantID, reservation.User.UserID, reservation.Date)) {
                    throw new ReservationRepositoryException($"Reservation in {reservation.Restaurant} by {reservation.User} at {reservation.Date} already exists.");
                }

                _context.Add(ReservationMapper.MapToData(reservation, _context));
                await SaveAndClearAsync();
            } catch (Exception) {

                throw;
            }
        }

        public async Task CancelReservationAsync(int reservationID) {
            try {
                var reservation = await _context.Reservations.Include(r => r.Restaurant)
                                            .Include(r => r.User)
                                            .FirstOrDefaultAsync(r => r.ReservationID == reservationID);

                if (reservation == null) {
                    throw new ReservationRepositoryException($"No reservation found with id {reservationID}");
                }

                _context.Remove(reservation);
                await SaveAndClearAsync();
            } catch (Exception) {

                throw;
            }           
        }
        
        public async Task UpdateReservationAsync(Reservation domainReservation) {
            try {
                ReservationEF updatedReservation = await ReservationMapper.MapToData(domainReservation, _context);

                if (!await ExistingReservation(updatedReservation.Restaurant.RestaurantID, updatedReservation.User.UserID, DateOnly.FromDateTime(updatedReservation.DateAndStartTime.Date))) {
                    throw new ReservationRepositoryException("Reservation doesn't exist and thus can't be updated");
                }

                // Change tracker of the the current restaurant and detach it to avoid conflicts
                // EF might track the existing and update restaurant, which will result in a conflict 
                var exisitingReservation = _context.ChangeTracker.Entries<ReservationEF>()
                                           .FirstOrDefault(r => r.Entity.ReservationID == updatedReservation.ReservationID);

                if (exisitingReservation != null) {
                    exisitingReservation.State = EntityState.Detached;
                }

                // Equals overridden
                if (updatedReservation.Equals(exisitingReservation)) {
                    throw new ReservationRepositoryException("Reservations are the same. No update required.");
                }

                _context.Reservations.Update(updatedReservation);
                await SaveAndClearAsync();

            } catch (Exception) {

                throw;
            }

        }
    }
}
