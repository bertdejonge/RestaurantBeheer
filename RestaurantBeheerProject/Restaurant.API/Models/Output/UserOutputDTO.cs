namespace RestaurantProject.API.Models.Output {
    public class UserOutputDTO {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Municipality { get; set; }
        public int ZipCode { get; set; }
        public string StreetName { get; set; }
        public string HouseNumberLabel { get; set; }
    }
}