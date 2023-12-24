namespace RestaurantProject.API.Models.Output {
    public class ReservationOutputDTO {
        public int ReservationID { get; set; }
        public string RestaurantInfo { get; set; }
        public string UserInfo { get; set; }
        public int partySize { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public int TableNumber { get; set; }
    }
}
