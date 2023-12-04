using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Models {
    public class CollectionPopulator {
        private static void PopulateAvailableHours(List<TimeOnly> list) {
            TimeOnly startTime = new TimeOnly(9, 30);
            TimeOnly endTime = new TimeOnly(23, 31);

            TimeOnly currentTime = startTime;

            while (currentTime <= endTime) {
                list.Add(currentTime);
                currentTime = currentTime.AddMinutes(30); // Increment by 30 minutes
            }
        }
    }
}
