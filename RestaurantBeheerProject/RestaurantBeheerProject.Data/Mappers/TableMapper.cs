using RestaurantProject.Datalayer.Data;
using RestaurantProject.Datalayer.Models;
using RestaurantProject.Domain.Models;

namespace RestaurantProject.Datalayer.Mappers {
    public class TableMapper {
        public static Table MapToDomain(TableEF dataTable, RestaurantDbContext context) {
            Table domain = new(dataTable.TableNumber, dataTable.Seats);

            domain.TableID = dataTable.TableID;

            // Now the dictionary of all available standard dates and times is set, 
            // remove all the timeslots that are reserved 
            // First get all the reservations for the table
            List<ReservationEF> reservationsForTable = context.Reservations
                                                    .Where(r => r.Restaurant.Tables.Any(t => t.TableID == domain.TableID))
                                                    .ToList();

            // Then make a dictionary to get all the different reservation dates with their timeslots
            Dictionary<DateOnly, List<TimeOnly>> reservationDateToStarttimes = new();

            // Get the dates of the reservations and their respective startimes 
            foreach (DateOnly date in reservationsForTable.Select(r => DateOnly.FromDateTime(r.DateAndStartTime.Date))) {
                if (!reservationDateToStarttimes.ContainsKey(date)) {
                    List<TimeOnly> startTimesForDate = reservationsForTable.Where(r => DateOnly.FromDateTime(r.DateAndStartTime.Date) == date)
                                                       .Select(r =>TimeOnly.FromTimeSpan(r.DateAndStartTime.TimeOfDay)).ToList();

                    reservationDateToStarttimes.Add(date, startTimesForDate);
                }
            }

            // Now, remove all the starttimes from our populated dictionary of the table
            foreach (DateOnly date in reservationDateToStarttimes.Keys) {
                foreach (TimeOnly timeslot in reservationDateToStarttimes[date]) {
                    domain.RemoveTakenTimeForDate(date, timeslot);
                }
            }

            return domain;
        }
        
    }
}