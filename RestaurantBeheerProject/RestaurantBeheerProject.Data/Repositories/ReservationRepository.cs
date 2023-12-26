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

        public async Task<List<Reservation>> GetReservationsUserForDateOrRangeAsync(int userID, DateOnly date, DateOnly? optionalDate = null) {
            try {
                List<Reservation> reservations = new();

                // If no second date given, search for only the first date
                // Get all the reservations, include restaurant and user navigation props, 
                // select only the ones for the specified date and map them to domain
                if(optionalDate == null || optionalDate <= date) {
                    reservations = await _context.Reservations.Include(r => r.Restaurant)
                                         .Include(r => r.User).Where(r => r.UserID == userID && DateOnly.FromDateTime(r.DateAndStartTime.Date) == date)
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

        public async Task<Reservation> CreateReservationAsync(Reservation reservation) {
            try {
                if (await ExistingReservation(reservation.Restaurant.RestaurantID, reservation.User.UserID, reservation.Date)) {
                    throw new ReservationRepositoryException($"Reservation in {reservation.Restaurant} by {reservation.User} at {reservation.Date} already exists.");
                }

                // Check if we can book a new reservation
                Table table = reservation.Restaurant.ChooseBestTable(reservation.PartySize, reservation.Date, reservation.StartTime);
                reservation.TableNumber = table.TableNumber;

                _context.Add(ReservationMapper.MapToData(reservation, _context));
                await SaveAndClearAsync();

                DateTime dateAndStartTime = reservation.Date.ToDateTime(reservation.StartTime);
                return ReservationMapper.MapToDomain(await _context.Reservations.FirstOrDefaultAsync(r => r.RestaurantID == reservation.Restaurant.RestaurantID 
                                                                                                     && r.UserID == reservation.User.UserID && r.DateAndStartTime == dateAndStartTime), _context);
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
        
        public async Task<Reservation> UpdateReservationAsync(int reservationID, Reservation input) {
            try {
                // Check if exists
                var existingReservation = await _context.Reservations.FirstOrDefaultAsync(r => r.ReservationID == reservationID);
                               
                if (existingReservation == null) {
                    throw new ReservationRepositoryException("No existing reservation found to update. ");
                }

                var existingReservationDomain = ReservationMapper.MapToDomain(existingReservation, _context);

                // Equals overridden
                if (existingReservationDomain.Equals(input)) {
                    throw new ReservationRepositoryException("Reservations are the same. No update required.");
                }

                // Time and date is checked when mapping, so the values are ok     
                // The cancelled reservation time has to be added back to the table so it can be used by another user
                // We add them back first in case the user wants to update the starttime to one of the slots that is 
                // currently blocked by the 1,5h reservation time
                Table oldTable = existingReservationDomain.Restaurant.Tables.Find(t => t.TableNumber == existingReservation.TableNumber);
                oldTable.AddCancelledReservationTimeForDate(input.Date, input.StartTime);
                           
                try {
                    // If a table is available for the date, check if it corresponds with the desired starttime
                    if (existingReservationDomain.Restaurant.IsAnyTableAvailableForDate(input.Date, input.PartySize)) {                       

                        Table updatedTable = existingReservationDomain.Restaurant.ChooseBestTable(input.PartySize, input.Date, input.StartTime);
                        existingReservationDomain.TableNumber = updatedTable.TableNumber;
                        existingReservationDomain.Date = input.Date;
                        existingReservationDomain.StartTime = input.StartTime;
                        existingReservationDomain.PartySize = input.PartySize;
                    } else {
                        throw new ReservationRepositoryException("No timeslots available for this date");
                    }
                } catch (Exception ex) {
                    // If something failed in updating the reservation, set the reservationtimes again
                    oldTable.RemoveTakenTimeForDate(existingReservationDomain.Date, existingReservationDomain.StartTime);
                    throw;
                }         
                
                existingReservation = await ReservationMapper.MapToData(existingReservationDomain, _context);

                _context.Reservations.Update(existingReservation);
                await SaveAndClearAsync();
                return ReservationMapper.MapToDomain(existingReservation, _context);

            } catch (Exception) {

                throw;
            }

        }

        public async Task<List<Reservation>> GetReservationsForRestaurantAsync(int restaurantID) {
            try {
                List<Reservation> reservations = await _context.Reservations.Include(r => r.Restaurant)
                                                .Where(r => r.RestaurantID == restaurantID)
                                                .Select(r => ReservationMapper.MapToDomain(r, _context)).ToListAsync();

                if(reservations.Count == 0 ) {
                    throw new ReservationRepositoryException("No reservations found for this restaurant.");
                }

                return reservations;
            } catch (Exception) {

                throw;
            }
        }
    }
}
