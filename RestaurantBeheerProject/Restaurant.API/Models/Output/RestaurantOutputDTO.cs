namespace RestaurantProject.API.Models.Output {
    public class RestaurantOutputDTO {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Cuisine { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Municipality { get; set; }
        public int ZipCode { get; set; }
        public List<TableOutputDTO> Tables { get; set; }

    }
}