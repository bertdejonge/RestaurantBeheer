using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Util
{
    public class CollectionPopulator
    {
        public static void PopulateAvailableHours(List<TimeOnly> list)
        {
            TimeOnly startTime = new TimeOnly(17, 30);
            TimeOnly endTime = new TimeOnly(22, 01);

            TimeOnly currentTime = startTime;

            while (currentTime <= endTime)
            {
                list.Add(currentTime);
                currentTime = currentTime.AddMinutes(30);
            }
        }
    }
}
