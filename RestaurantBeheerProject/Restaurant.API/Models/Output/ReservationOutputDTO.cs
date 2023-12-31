namespace RestaurantProject.API.Models.Output {
    public class ReservationOutputDTO {
        public int ReservationID { get; set; }
        public string RestaurantInfo { get; set; }
        public string UserInfo { get; set; }
        public int PartySize { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public int TableNumber { get; set; }

        public override bool Equals(object? obj) {
            return obj is ReservationOutputDTO dTO &&
                   ReservationID == dTO.ReservationID &&
                   RestaurantInfo == dTO.RestaurantInfo &&
                   UserInfo == dTO.UserInfo &&
                   PartySize == dTO.PartySize &&
                   Date.Equals(dTO.Date) &&
                   StartTime.Equals(dTO.StartTime) &&
                   TableNumber == dTO.TableNumber;
        }
    }
}
