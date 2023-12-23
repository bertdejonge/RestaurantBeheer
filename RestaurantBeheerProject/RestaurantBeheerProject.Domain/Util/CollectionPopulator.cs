using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Util
{
    public class CollectionPopulator
    {
        public static void PopulateDateAndHours(Dictionary<DateOnly, List<TimeOnly>> dateToAvailableTimes) {
            DateOnly curDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly maxDate = curDate.AddDays(90); // Max three months in advance

            // Populate all the dates with a set of available times (every 30 minutes)
            while (curDate < maxDate) {
                // Make a new list to hold the values
                List<TimeOnly> times = new();

                // Make the start value
                TimeOnly startTime = new TimeOnly(17, 30);
                TimeOnly endTime = new TimeOnly(22, 01);

                // Add the time to the list and increment by 30 minutes
                while (startTime <= endTime) {
                    times.Add(startTime);
                    startTime = startTime.AddMinutes(30);
                }

                // Finally, add the key-value pair to the dictionary and increment the date
                // until the desired future date is reached
                dateToAvailableTimes.Add(curDate, times);
                curDate = curDate.AddDays(1);
            }
        }
    }
}
